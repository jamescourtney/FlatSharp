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

namespace Samples.StructVectors
{
    using System;
    using System.Diagnostics;
    using System.Security.Cryptography;
    using FlatSharp;

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
    /// FlatSharp supports this syntax when using the FBS compiler. This example
    /// shows the usage for fixed-size struct vectors in both reference (class-based) 
    /// and value (struct-based) flatbuffer structs. For more information on value-type structs,
    /// see Example15-ValueStructs.
    /// </summary>
    public class StructVectorsSample
    {
        public static void Run()
        {
            Transaction transaction = new()
            {
                Amount = 1.2,
                Receiver = "0011223344",
                Sender = "5566779900",

                Hash_Reference = new Sha256_Reference(),
            };

            byte[] hash;
            using (var sha256 = SHA256Managed.Create())
            {
                byte[] data = new byte[1024];
                new Random().NextBytes(data);

                hash = sha256.ComputeHash(data);

                // Bulk-load the struct vector.
                transaction.Hash_Reference.Value.CopyFrom(hash.AsSpan());

                // Create value-type structs to represent the same.
                // These two types have the same API and behave the same,
                // but Sha256_FastValue is implemented with unsafe code to make access
                // to the indices of the struct vector more performant.
                Sha256_Value sha256Struct = default;
                Sha256_FastValue sha256FastStruct = default;

                for (int i = 0; i < hash.Length; ++i)
                {
                    Debug.Assert(i < sha256Struct.Value_Length);
                    Debug.Assert(i < sha256FastStruct.Value_Length);

                    // Value struct vectors supply an extension method that acts like an indexer.
                    sha256Struct.Value(i) = hash[i];
                    sha256FastStruct.Value(i) = hash[i];
                }

                transaction.Hash_Value = sha256Struct;
                transaction.Hash_FastValue = sha256FastStruct;
            }

            byte[] serialized = new byte[1024];
            Transaction.Serializer.Write(serialized, transaction);
            var parsed = Transaction.Serializer.Parse(serialized);

            Debug.Assert(parsed.Hash_Reference is not null);
            Debug.Assert(parsed.Hash_Value is not null);
            Debug.Assert(parsed.Hash_FastValue is not null);

            Debug.Assert(parsed.Hash_Reference.Value.Count == 32);
            Debug.Assert(default(Sha256_Value).Value_Length == 32);
            Debug.Assert(default(Sha256_FastValue).Value_Length == 32);

            Sha256_Value valueStruct = parsed.Hash_Value.Value;
            Sha256_FastValue fastValueStruct = parsed.Hash_FastValue.Value;

            for (int i = 0; i < 32; ++i)
            {
                Debug.Assert(parsed.Hash_Reference.Value[i] == hash[i]);
                Debug.Assert(valueStruct.Value(i) == hash[i]);
                Debug.Assert(fastValueStruct.Value(i) == hash[i]);
            }
        }
    }
}
