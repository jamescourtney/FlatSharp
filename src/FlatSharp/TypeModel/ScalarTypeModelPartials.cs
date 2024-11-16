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


using System.Buffers.Binary;

namespace FlatSharp.TypeModel;

public partial class DoubleTypeModel
{
    public override string FormatDefaultValueAsLiteral(object? defaultValue)
    {
        if (defaultValue is double d)
        {
            return d switch
            {
                double.NegativeInfinity => "double.NegativeInfinity",
                double.PositiveInfinity => "double.PositiveInfinity",
                double.NaN => "double.NaN",
                _ => $"{d:G17}d",
            };
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
            return f switch
            {
                float.NegativeInfinity => "float.NegativeInfinity",
                float.PositiveInfinity => "float.PositiveInfinity",
                float.NaN => "float.NaN",
                _ => $"{f:G17}f",
            };
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
