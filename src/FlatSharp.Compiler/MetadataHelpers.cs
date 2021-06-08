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

namespace FlatSharp.Compiler
{
    using System.Collections.Generic;

    public static class MetadataHelpers
    {
        public delegate bool TryParse<T>(string value, out T result);

        public static T ParseMetadata<T>(
            this IDictionary<string, string?> metadata,
            string[] keys,
            TryParse<T> tryParse,
            T defaultValueIfPresent,
            T defaultValueIfNotPresent)
        {
            foreach (string key in keys)
            {
                if (!metadata.TryGetValue(key, out string? value))
                {
                    continue;
                }

                if (string.IsNullOrEmpty(value))
                {
                    return defaultValueIfPresent;
                }

                if (tryParse(value, out var parsed))
                {
                    return parsed;
                }

                ErrorContext.Current?.RegisterError(
                  $"Unable to parse attribute '{key}' with value '{value}' as as type '{typeof(T).GetCompilableTypeName()}'. ");

                return defaultValueIfPresent;
            }

            return defaultValueIfNotPresent;
        }

        public static bool ParseBooleanMetadata(this IDictionary<string, string?> metadata, params string[] keys)
        {
            return ParseMetadata(
                metadata,
                keys,
                bool.TryParse,
                true,
                false);
        }

        public static int? ParseNullableIntegerMetadata(
            this IDictionary<string, string?> metadata, 
            string[] keys,
            int? defaultValueIfPresent = null,
            int? defaultValueIfNotPresent = null)
        {
            return ParseMetadata<int?>(
                metadata,
                keys,
                TryParseNullableInt,
                defaultValueIfPresent,
                defaultValueIfNotPresent);
        }

        public static bool? ParseNullableBooleanMetadata(
            this IDictionary<string, string?> metadata, 
            params string[] keys)
        {
            return ParseMetadata<bool?>(
                metadata,
                keys,
                TryParseNullableBool,
                true,
                null);
        }

        private static bool TryParseNullableInt(string? value, out int? result)
        {
            result = null;
            if (int.TryParse(value, out var parsed))
            {
                result = parsed;
                return true;
            }

            return false;
        }

        private static bool TryParseNullableBool(string? value, out bool? result)
        {
            result = null;
            if (bool.TryParse(value, out var parsed))
            {
                result = parsed;
                return true;
            }

            return false;
        }
    }
}
