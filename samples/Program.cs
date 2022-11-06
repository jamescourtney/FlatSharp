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

namespace Samples;

public static class Program
{
    public static void Main(string[] args)
    {
        List<IFlatSharpSample> samples = new()
        {
            new Basics.Basics(),
            new DeserializationModes.DeserializationModes(),
            new Vectors.VectorsSample(),
            new GrpcExample.GrpcExample(),
            new CopyConstructorsExample.CopyConstructorsExample(),
            new IncludesExample.IncludesExample(),
            new SortedVectors.SortedVectorsExample(),
            new Unions.UnionsExample(),
            new SharedStrings.SharedStringsExample(),
            new IndexedVectors.IndexedVectorsExample(),
            new StructVectors.StructVectorsSample(),
            new WriteThrough.WriteThroughSample(),
            new ValueStructs.ValueStructsSample(),
        };

        foreach (var sample in samples)
        {
            if (sample.HasConsoleOutput)
            {
                Console.WriteLine();
                Console.WriteLine($"Beginning Sample: {sample.GetType().Name}:");
                Console.WriteLine();

                sample.Run();

                Console.WriteLine();
                Console.WriteLine($"Done with Sample: {sample.GetType().Name}");
                Console.WriteLine();
            }
            else
            {
                sample.Run();
            }
        }
    }
}
