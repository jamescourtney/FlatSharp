/*
 * Copyright 2021 James Courtney
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Security.Cryptography;

namespace Samples.StructVectors;

/// <summary>
/// FlatBuffers allows declaring fixed-length vectors in structs using the syntax:
/// 
/// struct Struct 
/// {
///    Value:[ubyte:32];
/// }
/// 
/// This is syntax sugar for:
/// 
/// struct Struct
/// {
///    Value_0:ubyte;
///    Value_1:ubyte;
///    ...
///    Value_31:ubyte;
/// }
/// 
/// This example shows the usage for fixed-size struct vectors in both reference (class-based) 
/// and value (struct-based) flatbuffer structs. For more information on value-type structs,
/// see Example15-ValueStructs.
/// </summary>
public class StructVectorsSample : IFlatSharpSample
{
    public bool HasConsoleOutput => false;

    public void Run()
    {
        Transaction transaction = new()
        {
            Amount = 1.2,
            Receiver = "0011223344",
            Sender = "5566779900",

            HashReference = new Sha256_Reference(),
            HashValue = new Sha256_Value(),
            HashFastValue = new Sha256_FastValue(),
        };

        byte[] originalHash;
        using (var sha256 = SHA256.Create())
        {
            byte[] data = new byte[1024];
            new Random().NextBytes(data);

            originalHash = sha256.ComputeHash(data);

            // Bulk-load the reference struct vector.
            transaction.HashReference.Value.CopyFrom(originalHash.AsSpan());

            // Create value-type structs to represent the same.
            // These two types have the same API and behave the same,
            // but Sha256_FastValue is implemented with unsafe code to make access
            // to the indices of the struct vector more performant.
            Sha256_Value sha256Struct = default;
            Sha256_FastValue sha256FastStruct = default;
            Sha256_Reference refStruct = new();

            for (int i = 0; i < originalHash.Length; ++i)
            {
                Assert.True(i < sha256Struct.Value_Length, "i is in bounds.");
                Assert.True(i < sha256FastStruct.Value_Length, "i is in bounds.");

                // Value struct vectors supply an extension method that acts like an indexer.
                sha256Struct.Value(i) = originalHash[i];
                sha256FastStruct.Value(i) = originalHash[i];
            }

            // Assign these later due to value type semantics.
            transaction.HashValue = sha256Struct;
            transaction.HashFastValue = sha256FastStruct;
        }

        byte[] serialized = new byte[1024];
        Transaction.Serializer.Write(serialized, transaction);
        var parsed = Transaction.Serializer.Parse(serialized);

        Assert.True(parsed.HashReference!.Value.Count == 32, "Sha256_reference has expected number of items");
        Assert.True(parsed.HashValue.Value_Length == 32, "Sha256_Value has expected number of items");
        Assert.True(parsed.HashFastValue.Value_Length == 32, "Sha256_FastValue has expected number of items");

        Sha256_Value valueStruct = parsed.HashValue;
        Sha256_FastValue fastValueStruct = parsed.HashFastValue;

        for (int i = 0; i < 32; ++i)
        {
            Assert.True(parsed.HashReference.Value[i] == originalHash[i], "Reference hash matches at index: " + i);
            Assert.True(valueStruct.Value(i) == originalHash[i], "Value hash matches at index: " + i);
            Assert.True(fastValueStruct.Value(i) == originalHash[i], "FastValue hash matches at index: " + i);
        }
    }
}
