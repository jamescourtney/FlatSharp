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

using System.Linq;
using FlatSharp.Compiler.Schema;

namespace FlatSharp.Compiler.SchemaModel;

public class EnumSchemaModel : BaseSchemaModel
{
    private readonly bool isFlags;
    private readonly Dictionary<string, EnumVal> nameValueMap;
    private readonly string underlyingType;
    private readonly IEnumerable<string>? documentation;

    private EnumSchemaModel(Schema.Schema schema, FlatBufferEnum @enum) : base(schema, @enum.Name, new FlatSharpAttributes(@enum.Attributes))
    {
        FlatSharpInternal.Assert(!@enum.IsUnion, "Not expecting union");
        FlatSharpInternal.Assert(@enum.UnderlyingType.BaseType.IsInteger(), "Expected scalar base type");
        FlatSharpInternal.Assert(@enum.UnderlyingType.BaseType.TryGetBuiltInTypeName(out this.underlyingType!), "Couldn't get type name string");

        this.isFlags = @enum.Attributes?.ContainsKey(MetadataKeys.BitFlags) == true;
        this.nameValueMap = @enum.Values.ToDictionary(x => x.Value.Key, x => x.Value);
        this.DeclaringFile = @enum.DeclarationFile;
        this.documentation = @enum.Documentation;
    }

    public static bool TryCreate(Schema.Schema schema, FlatBufferEnum @enum, [NotNullWhen(true)] out EnumSchemaModel? model)
    {
        model = null;
        if (@enum.IsUnion || !@enum.UnderlyingType.BaseType.IsInteger())
        {
            return false;
        }

        model = new EnumSchemaModel(schema, @enum);
        return true;
    }

    public override FlatBufferSchemaElementType ElementType => FlatBufferSchemaElementType.Enum;

    public override string DeclaringFile { get; }

    protected override void OnWriteCode(CodeWriter writer, CompileContext context)
    {
        writer.AppendSummaryComment(this.documentation);

        if (this.isFlags)
        {
            writer.AppendLine("[Flags]");
        }

        writer.AppendLine($"[FlatBufferEnum(typeof({this.underlyingType}))]");
        writer.AppendLine($"public enum {this.Name} : {this.underlyingType}");
        using (writer.WithBlock())
        {
            foreach (var item in this.nameValueMap.OrderBy(x => x.Value.Value))
            {
                if (item.Value.Documentation?.Any() == true)
                {
                    writer.AppendSummaryComment(item.Value.Documentation);
                }

                writer.AppendLine($"{item.Key} = {item.Value.Value},");
            }
        }
    }
}
