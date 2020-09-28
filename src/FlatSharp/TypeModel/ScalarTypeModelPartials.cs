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
    public partial class DoubleTypeModel
    {
        public override bool TryFormatDefaultValueAsLiteral(object defaultValue, out string literal)
        {
            literal = null;

            if (defaultValue is double d)
            {
                literal = $"{d:G17}d";
                return true;
            }

            return false;
        }
    }

    public partial class FloatTypeModel
    {
        public override bool TryFormatDefaultValueAsLiteral(object defaultValue, out string literal)
        {
            literal = null;

            if (defaultValue is float f)
            {
                literal = $"{f:G17}f";
                return true;
            }

            return false;
        }
    }

    public partial class BoolTypeModel
    {
        public override bool TryFormatDefaultValueAsLiteral(object defaultValue, out string literal)
        {
            literal = null;

            if (defaultValue is bool b)
            {
                literal = b.ToString().ToLower();
                return true;
            }

            return false;
        }
    }
}
