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

namespace FlatSharp
{
    using System.Reflection.Emit;

    /// <summary>
    /// A shared lock for use when doing compilation work.
    /// </summary>
    internal static class CompilerLock
    {
        public static object Instance { get; } = new object();

        static CompilerLock()
        {
        }

        public static AssemblyBuilder DynamicAssembly { get; } = AssemblyBuilder.DefineDynamicAssembly(
            new System.Reflection.AssemblyName("FlatSharpDynamicAssembly"),
#if NET47
            AssemblyBuilderAccess.RunAndSave
#else
            AssemblyBuilderAccess.Run
#endif
            );

        public static ModuleBuilder DynamicModule { get; } = CompilerLock.DynamicAssembly
            .DefineDynamicModule(
                "FlatSharpDynamicModule"
#if NET47
                , "FlatSharpDynamicModule.dll"
#endif
            );
    }
}
