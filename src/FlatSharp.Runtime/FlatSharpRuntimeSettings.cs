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

namespace FlatSharp
{
    /// <summary>
    /// Defines a property bag of global runtime settings for FlatSharp.
    /// </summary>
    public static class FlatSharpRuntimeSettings
    {
        /// <summary>
        /// A flag that causes recycled tables and structs to collect their stack traces at allocation and release time. It
        /// also disables recycling, to allow catching use-after-recycle bugs. This is a diagnostic mode that should only
        /// be used to investigate issues.
        /// </summary>
        public static bool EnableRecyclingDiagnostics { get; set; }

        /// <summary>
        /// An object pool factory for Flatsharp-generated code to use to acquire object pools for object recycling. 
        /// This must be set before the serialized code is invoked as the object pools produced by this factory
        /// are referenced statically. It is recommended to do so at application startup time.
        /// </summary>
        public static IFlatSharpObjectPoolFactory ObjectPoolFactory { get; set; } = new ChannelBasedObjectPoolFactory();
    }
}
