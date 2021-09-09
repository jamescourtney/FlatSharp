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

namespace FlatSharp.Compiler.Schema
{
    using FlatSharp.Attributes;
    using FlatSharp.Compiler.SchemaModel;
    using System.Collections.Generic;
    using System.Linq;

    /*
    table Schema {
        objects:[Object] (required);    // Sorted.
        enums:[Enum] (required);        // Sorted.
        file_ident:string;
        file_ext:string;
        root_table:Object;
        services:[Service];             // Sorted.
        advanced_features:AdvancedFeatures;
        /// All the files used in this compilation. Files are relative to where
        /// flatc was invoked.
        fbs_files:[SchemaFile];         // Sorted.
    }
    */
    [FlatBufferTable]
    public class Schema
    {
        [FlatBufferItem(0, Required = true, SortedVector = true)]
        public virtual IList<FlatBufferObject> Objects { get; set; } = new List<FlatBufferObject>();

        [FlatBufferItem(1, Required = true, SortedVector = true)]
        public virtual IList<FlatBufferEnum> Enums { get; set; } = new List<FlatBufferEnum>();

        [FlatBufferItem(2)]
        public virtual string? FileIdentifier { get; set; }

        [FlatBufferItem(3)]
        public virtual string? FileExtension { get; set; }

        [FlatBufferItem(4)]
        public virtual FlatBufferObject? RootTable { get; set; }

        [FlatBufferItem(5)]
        public virtual IList<RpcService>? Services { get; set; }

        [FlatBufferItem(6)]
        public virtual AdvancedFeatures AdvancedFeatures { get; set; }

        [FlatBufferItem(7)]
        public virtual IIndexedVector<string, SchemaFile>? FbsFiles { get; set; }

        public RootModel ToRootModel()
        {
            RootModel model = new RootModel(this.AdvancedFeatures);

            foreach (var @enum in this.Enums)
            {
                if (EnumSchemaModel.TryCreate(this, @enum, out var enumModel))
                {
                    model.AddElement(enumModel);
                }
                else if (UnionSchemaModel.TryCreate(this, @enum, out var unionModel))
                {
                    model.AddElement(unionModel);
                }
            }

            foreach (var obj in this.Objects)
            {
                if (TableSchemaModel.TryCreate(this, obj, out var tableModel))
                {
                    model.AddElement(tableModel);
                }
                else if (ReferenceStructSchemaModel.TryCreate(this, obj, out var refStructModel))
                {
                    model.AddElement(refStructModel);
                }
                else if (ValueStructSchemaModel.TryCreate(this, obj, out var valueStructModel))
                {
                    model.AddElement(valueStructModel);
                }
            }

            if (this.Services is not null)
            {
                foreach (var service in this.Services)
                {
                    model.AddElement(new RpcServiceSchemaModel(this, service));
                }
            }

            return model;
        }
    }
}
