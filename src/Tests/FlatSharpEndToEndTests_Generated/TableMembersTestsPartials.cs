/*
 * Copyright 2023 James Courtney
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

using System.Collections.Generic;

namespace FlatSharpEndToEndTests.TableMembers;

public interface ITypedTable<T> where T : struct
{
    T ItemStandard { get; set; }

    T? ItemOptional { get; set; }

    T ItemWithDefault { get; set; }

    T ItemDeprecated { get; set; }

    IList<T>? ItemList { get; set; }

    IReadOnlyList<T>? ItemReadonlyList { get; set; }
}

public partial class BoolTable : ITypedTable<bool> { }
public partial class ByteTable : ITypedTable<byte> { }
public partial class SByteTable : ITypedTable<sbyte> { }
public partial class ShortTable : ITypedTable<short> { }
public partial class UShortTable : ITypedTable<ushort> { }
public partial class IntTable : ITypedTable<int> { }
public partial class UIntTable : ITypedTable<uint> { }
public partial class LongTable : ITypedTable<long> { }
public partial class ULongTable : ITypedTable<ulong> { }
public partial class FloatTable : ITypedTable<float> { }
public partial class DoubleTable : ITypedTable<double> { }