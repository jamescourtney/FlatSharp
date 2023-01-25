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

namespace FlatSharpTests;

internal static class Extensions
{
    internal static string? GetCSharp<T>(this ISerializer<T> serializer) where T : class
    {
        return ((GeneratedSerializerWrapper<T>)serializer).CSharp;
    }

    internal static string? GetCSharp(this ISerializer serializer)
    {
        var prop = typeof(GeneratedSerializerWrapper<>)
                    .MakeGenericType(serializer.RootType)
                    .GetProperty("CSharp");

        return (string?)prop.GetValue(serializer);
    }
}