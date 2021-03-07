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
    /// shows the usage for fixed-size struct vectors.
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
                Hash = new Sha256()
            };

            byte[] hash;
            using (var sha256 = SHA256Managed.Create())
            {
                byte[] data = new byte[1024];
                new Random().NextBytes(data);

                hash = sha256.ComputeHash(data);

                // Bulk-load the struct vector.
                transaction.Hash.Value.CopyFrom(hash.AsSpan());
            }

            byte[] serialized = new byte[1024];
            Transaction.Serializer.Write(serialized, transaction);
            var parsed = Transaction.Serializer.Parse(serialized);

            Debug.Assert(parsed.Hash is not null);

            for (int i = 0; i < parsed.Hash.Value.Count; ++i)
            {
                Debug.Assert(parsed.Hash.Value[i] == hash[i]);
            }
        }
    }
}
