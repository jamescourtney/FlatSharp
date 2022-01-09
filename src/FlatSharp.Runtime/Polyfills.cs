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

#if NETSTANDARD2_0

global using NotNullWhenAttribute = FlatSharp.Polyfills.NotNullWhenAttribute;
global using DoesNotReturnIfAttribute = FlatSharp.Polyfills.DoesNotReturnIfAttribute;
global using NotNullIfNotNullAttribute = FlatSharp.Polyfills.NotNullIfNotNullAttribute;

namespace FlatSharp.Polyfills
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    internal class NotNullWhenAttribute : Attribute
    {
        public NotNullWhenAttribute(bool returnValue)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    internal class DoesNotReturnIfAttribute : Attribute
    {
        public DoesNotReturnIfAttribute(bool value)
        {
        }
    }


    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
    internal class NotNullIfNotNullAttribute : Attribute
    {
        public NotNullIfNotNullAttribute(string parameterName)
        {
        }
    }
}
#endif

#if !NET5_0_OR_GREATER

namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}

#endif