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

namespace FlatSharp.Compiler.SchemaModel
{
    using FlatSharp.Attributes;
    using System;

    public class AttributeValidationResult
    {
        private const string ElementTypeSubs = "__ELEMENT_TYPE__";
        private const string AttributeNameSubs = "__ATTR_NAME__";
        private const string AttributeValueSubs = "__ATTR_VALUE__";

        public static readonly AttributeValidationResult Valid = new(null, true);

        public static readonly AttributeValidationResult NeverValid = new AttributeValidationResult($"The attribute {AttributeNameSubs} is never valid on {ElementTypeSubs} elements.", false);

        public static readonly AttributeValidationResult ValueInvalid = new AttributeValidationResult($"The attribute {AttributeNameSubs} value {AttributeValueSubs} is not valid on {ElementTypeSubs} elements.", false);


        private AttributeValidationResult(string? message, bool isValid)
        {
            this.Message = message ?? string.Empty;
            this.IsValid = isValid;
        }

        public string Message { get; }

        public bool IsValid { get; }

        public string ToString(FlatBufferSchemaElementType type, string attributeName, string attributeValue)
        {
            return this.Message
                .Replace(AttributeNameSubs, $"'{attributeName}'")
                .Replace(ElementTypeSubs, type.ToString())
                .Replace(AttributeValueSubs, attributeValue);
        }
    }
}
