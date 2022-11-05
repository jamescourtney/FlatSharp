/*
 * Copyright 2020 James Courtney
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

namespace Samples.IncludesExample;

/// <summary>
/// This example shows how different flatbuffer files can include each other. In this example,
/// IncludesExample.fbs references A, which references B, which is in a subdirectory.
/// We can create a complete serializer package and gRPC service just from the declarations in IncludesExample.cs.
/// There is no code sample here -- just FBS files.
/// </summary>
public class IncludesExample : IFlatSharpSample
{
    public void Run()
    {
    }
}
