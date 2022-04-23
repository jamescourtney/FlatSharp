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

using System.Linq;
using FlatSharp.CodeGen;
using FlatSharp.TypeModel;

namespace FlatSharpTests;

public class DepthLimitTests
{
    [Fact]
    public void Table_References_Self()
    {
        var typeModel = RuntimeTypeModel.CreateFrom(typeof(TableReferencesSelf));
        Assert.True(typeModel is TableTypeModel);
        Assert.True(typeModel.ContainsCycle());

        typeModel = RuntimeTypeModel.CreateFrom(typeof(TableReferenceDangling));
        Assert.True(typeModel is TableTypeModel);
        Assert.False(typeModel.ContainsCycle());
    }

    [FlatBufferTable]
    public class TableReferencesSelf
    {
        [FlatBufferItem(0)]
        public virtual TableReferencesSelf? Item { get; set; }

        [FlatBufferItem(1)]
        public virtual TableReferenceDangling? Something { get; set; }
    }

    [FlatBufferTable]
    public class TableReferenceDangling
    { 
        [FlatBufferItem(0)]
        public virtual int Value { get; set; }
    }

    [Fact]
    public void Table_References_Self_Through_List()
    {
        var typeModel = RuntimeTypeModel.CreateFrom(typeof(TableReferencesSelfThroughList_Container));
        Assert.True(typeModel is TableTypeModel);
        Assert.True(typeModel.ContainsCycle());

        typeModel = RuntimeTypeModel.CreateFrom(typeof(TableReferencesSelfThroughList));
        Assert.True(typeModel is TableTypeModel);
        Assert.True(typeModel.ContainsCycle());

        typeModel = RuntimeTypeModel.CreateFrom(typeof(TableReferencesSelfThroughList_Inner));
        Assert.True(typeModel is TableTypeModel);
        Assert.True(typeModel.ContainsCycle());

        typeModel = RuntimeTypeModel.CreateFrom(typeof(IList<TableReferencesSelfThroughList_Inner>));
        Assert.True(typeModel is ListVectorTypeModel);
        Assert.True(typeModel.ContainsCycle());
    }

    [FlatBufferTable]
    public class TableReferencesSelfThroughList_Container
    {
        [FlatBufferItem(0)]
        public TableReferencesSelfThroughList? Item { get; set; }
    }

    [FlatBufferTable]
    public class TableReferencesSelfThroughList
    {
        [FlatBufferItem(0)]
        public virtual IList<TableReferencesSelfThroughList_Inner>? Items { get; set; }
    }

    [FlatBufferTable]
    public class TableReferencesSelfThroughList_Inner
    {
        [FlatBufferItem(0)]
        public virtual TableReferencesSelfThroughList? Item { get; set; }
    }
}
