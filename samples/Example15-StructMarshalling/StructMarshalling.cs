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

namespace Samples.StructMarshalling
{
    using FlatSharp;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Some scenarios, such as SIMD, need to convert FlatBuffer structs into proper C# structs. FlatSharp models all items as classes.
    /// This sample shows how to marshal data from a FlatBuffer struct into a proper CLR struct. This is an advanced technique that
    /// should be evaluated carefully and only used in conjunction with thorough unit testing, as it requires some unsafe techniques.
    /// </summary>
    public class StructMarshallingSample
    {
        public static void Run()
        {
            Table table = new Table
            {
                Vectors = new List<Vector3>(),
                IrregularStructs = new List<IrregularStruct>(),
            };

            for (int i = 0; i < 100; ++i)
            {
                table.Vectors.Add(new Vector3
                {
                    X = i,
                    Y = 2 * i,
                    Z = 3 * i,
                });

                table.IrregularStructs.Add(new IrregularStruct
                {
                    A = ulong.MaxValue / (ulong)(i + 1),
                    B = (sbyte)i,
                });
            }

            byte[] data = new byte[10240];
            Table.Serializer.Write(data, table);

            Table parsedTable = Table.Serializer.Parse(data);

            {
                System.Numerics.Vector3 expectedSum = default;

                // We are going to sum all of the elements of the 'Vectors' array in the parsed buffer.
                System.Numerics.Vector3 vec3 = System.Numerics.Vector3.Zero;
                Vector3 parsedVec3 = parsedTable.Vectors![0];

                if (parsedVec3 is IFlatBufferAddressableStruct addressableStruct)
                {
                    // All structs deserialized with non-greedy deserializers will implement this interface.
                    // Greedily-deserialized structs will not implement the interface.
                    int offset = addressableStruct.Offset;
                    int size = addressableStruct.Size;
                    int alignment = addressableStruct.Alignment;
                    int count = parsedTable.Vectors.Count;

                    // Note that we are traversing the entire array yet only asking FlatSharp to give us information about the first element.
                    // For this reason, this technique is incredibly efficient, but also dangerous. By using this method, you take responsibility
                    // for ensuring the correctness of your data, as FlatSharp can only tell you where the item is in the array.
                    for (int i = 0; i < count; ++i)
                    {
                        expectedSum += new System.Numerics.Vector3(i, 2 * i, 3 * i);
                        vec3 += MarshalAs<System.Numerics.Vector3>(data, size, ref offset, alignment);
                    }

                    Console.WriteLine($"Vec3 Sum = (X = {vec3.X}, Y = {vec3.Y}, Z = {vec3.Z})");
                    Console.WriteLine($"Exptected Sum = (X = {expectedSum.X}, Y = {expectedSum.Y}, Z = {expectedSum.Z})");
                }
            }

            {
                IrregularStruct parsedIrregularStruct = parsedTable.IrregularStructs![0];
                if (parsedIrregularStruct is IFlatBufferAddressableStruct addressableStruct)
                {
                    IrregularStructCLR lastValue = default;

                    int offset = addressableStruct.Offset;
                    int size = addressableStruct.Size;
                    int alignment = addressableStruct.Alignment;
                    int count = parsedTable.Vectors.Count;

                    for (int i = 0; i < count; ++i)
                    {
                        lastValue = MarshalAs<IrregularStructCLR>(data, size, ref offset, alignment);
                    }

                    Console.WriteLine($"LastValue = {{ A = {lastValue.A}, B = {lastValue.B} }}");
                    Console.WriteLine($" Expected = {{ A = {table.IrregularStructs[^1].A}, B = {table.IrregularStructs[^1].B} }}");
                }
            }

            static T MarshalAs<T>(byte[] buffer, int flatBufferSize, ref int flatBufferOffset, int flatBufferAlignment)
                where T : struct
            {
                int clrTypeSize = Marshal.SizeOf<T>();

                // CLR can have larger sizes than FlatSharp as it may pad them out for alignment purposes.
                // In our example, the FlatBuffer size of IrregularStruct is 9 bytes, but the CLR size is 16 bytes.
                Span<byte> span = buffer.AsSpan().Slice(flatBufferOffset, Math.Max(clrTypeSize, flatBufferSize));

                Span<T> typedSpan = MemoryMarshal.Cast<byte, T>(span);
                Debug.Assert(typedSpan.Length == 1, "Typed span length is 1.");

                // Increment the offset to move to the next item in the vector.
                flatBufferOffset += flatBufferSize;

                // For structs that do not end on alignment boundaries, we also need to compensate for padding. This
                // is especially important for structs that are not-self aligning, such as the Irregular Struct example
                flatBufferOffset += FlatSharp.SerializationHelpers.GetAlignmentError(flatBufferOffset, flatBufferAlignment);

                return typedSpan[0];
            }
        }
    }

    // Corresponds to the 'IrregularStruct' we defined in the FBS file.
    [StructLayout(LayoutKind.Explicit)]
    public struct IrregularStructCLR
    {
        [FieldOffset(0)]
        public ulong A;

        [FieldOffset(8)]
        public sbyte B;
    }
}
