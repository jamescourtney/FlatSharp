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

using System;
using System.Collections.Generic;
using System.Linq;

namespace FlatSharpEndToEndTests.ClassLib.NonScalarVectorTests;

public interface ITable<T> where T : IEquatable<T>
{
    IList<T> Vector { get; set; }
}


public interface IReadOnlyTable<T> where T : IEquatable<T>
{
    IReadOnlyList<T> Vector { get; set; }
}


public partial class Root : ITable<InnerTable>, ITable<InnerStruct>, ITable<string>
{
    IList<string> ITable<string>.Vector
    {
        get => this.StringVector;
        set => this.StringVector = value;
    }

    IList<InnerTable> ITable<InnerTable>.Vector
    {
        get => this.TableVector;
        set => this.TableVector = value;
    }

    IList<InnerStruct> ITable<InnerStruct>.Vector
    {
        get => this.StructVector;
        set => this.StructVector = value;
    }
}

public partial class RootReadOnly : IReadOnlyTable<InnerTable>, IReadOnlyTable<InnerStruct>, IReadOnlyTable<string>
{
    IReadOnlyList<string> IReadOnlyTable<string>.Vector
    {
        get => this.StringVector;
        set => this.StringVector = value;
    }

    IReadOnlyList<InnerTable> IReadOnlyTable<InnerTable>.Vector
    {
        get => this.TableVector;
        set => this.TableVector = value;
    }

    IReadOnlyList<InnerStruct> IReadOnlyTable<InnerStruct>.Vector
    {
        get => this.StructVector;
        set => this.StructVector = value;
    }
}

public partial class InnerTable : IEquatable<InnerTable>
{
    public bool Equals(InnerTable other)
    {
        return other?.Value == this.Value;
    }

    public override bool Equals(object obj)
    {
        return this.Equals(obj as InnerTable);
    }

    public override int GetHashCode()
    {
        return this.Value.GetHashCode();
    }
}

public partial class InnerStruct : IEquatable<InnerStruct>
{
    public bool Equals(InnerStruct other)
    {
        return other?.Value == this.Value;
    }

    public override bool Equals(object obj)
    {
        return this.Equals(obj as InnerStruct);
    }

    public override int GetHashCode()
    {
        return this.Value.GetHashCode();
    }
}