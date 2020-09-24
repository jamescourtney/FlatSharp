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
 
 namespace FlatSharp.TypeModel
{
    using System;
    using System.Collections.Generic;

    public partial class DoubleTypeModel
    {
        public override string FormatDefaultValueAsLiteral(object defaultValue)
        {
            return ((double)defaultValue).ToString("G17");
        }
    }

    public partial class FloatTypeModel
    {
        public override string FormatDefaultValueAsLiteral(object defaultValue)
        {
            return ((float)defaultValue).ToString("G17") + "f";
        }
    }

    public partial class BoolTypeModel
    {
        public override string FormatDefaultValueAsLiteral(object defaultValue)
        {
            return defaultValue.ToString().ToLower();
        }
    }
}
