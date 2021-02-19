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

namespace FlatSharp.TypeModel
{
    using System.Diagnostics.CodeAnalysis;

    public partial class DoubleTypeModel
    {
        public override string FormatDefaultValueAsLiteral(object? defaultValue)
        {
            if (defaultValue is double d)
            {
                return $"{d:G17}d";
            }

            return base.FormatDefaultValueAsLiteral(defaultValue);
        }
    }

    public partial class FloatTypeModel
    {
        public override string FormatDefaultValueAsLiteral(object? defaultValue)
        {
            if (defaultValue is float f)
            {
                return $"{f:G17}f";
            }

            return base.FormatDefaultValueAsLiteral(defaultValue);
        }
    }

    public partial class BoolTypeModel
    {
        public override string FormatDefaultValueAsLiteral(object? defaultValue)
        {
            if (defaultValue is bool b)
            {
                return b.ToString().ToLower();
            }

            return base.FormatDefaultValueAsLiteral(defaultValue);
        }
    }
}
