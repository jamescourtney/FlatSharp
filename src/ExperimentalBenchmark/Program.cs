/*
 * Copyright 2018 James Courtney
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

namespace BenchmarkCore
{
    using System;
    using System.Diagnostics;

    public class Program
    {
        public static void Main(string[] args)
        {
            Google_FBBench b = new Google_FBBench();
            const int count = 1000000;
            b.TraversalCount = 5;
            b.VectorLength = 30;
            b.GlobalSetup();

            Func<int>[] items = new Func<int>[]
            {
                b.FlatSharp_GetMaxSize,
                b.FlatSharp_Serialize,
                // b.FlatSharp_ParseAndTraverse_List,
            };

            for (int loop = 0; loop < 2; ++loop)
            {
                foreach (var action in items)
                {
                    for (int i = 0; i < 10000; ++i)
                    {
                        action();
                    }
                    GC.Collect();

                    Stopwatch sw = Stopwatch.StartNew();
                    for (int i = 0; i < count; ++i)
                    {
                        action();
                    }
                    sw.Stop();
                    Console.WriteLine($"{action.Method.Name}: Took {sw.ElapsedMilliseconds} ({sw.ElapsedMilliseconds * 1000 / ((double)count)} us per op)");
                    System.Threading.Thread.Sleep(1000);
                    GC.Collect();
                }
            }

            b.GlobalCleanup();
        }
    }
}
