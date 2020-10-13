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
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class MetadataHelpers
    {
        public delegate bool TryParse<T>(string value, out T result);

        public static T ParseMetadata<T>(
            this IDictionary<string, string> metadata, 
            string key,
            TryParse<T> tryParse,
            T defaultValueIfPresent,
            T defaultIfNotPresent)
        {
            if (metadata.TryGetValue(key, out string value))
            {
                if (string.IsNullOrEmpty(value))
                {
                    return defaultValueIfPresent;
                }
                else if (tryParse(value, out T parsed))
                {
                    return parsed;
                }
                else
                {
                    ErrorContext.Current?.RegisterError($"Unable to parse attribute '{key}' with value '{value}' as as type '{typeof(T).Name}'. ");
                    return defaultValueIfPresent;
                }
            }
            else
            {
                return defaultIfNotPresent;
            }
        }

        public static bool ParseBooleanMetadata(this IDictionary<string, string> metadata, string key)
        {
            return ParseMetadata<bool>(
                metadata,
                key,
                bool.TryParse,
                true,
                false);
        }

        public static bool? ParseNullableBooleanMetadata(this IDictionary<string, string> metadata, string key)
        {
            return ParseMetadata<bool?>(
                metadata,
                key,
                TryParseNullableBool,
                true,
                null);
        }

        private static bool TryParseNullableBool(string value, out bool? result)
        {
            var returnValue = bool.TryParse(value, out var parsed);
            result = parsed;
            return returnValue;
        }
    }
}
