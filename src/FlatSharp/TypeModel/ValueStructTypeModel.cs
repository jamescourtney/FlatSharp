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

using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using FlatSharp.Attributes;

namespace FlatSharp.TypeModel;

/// <summary>
/// Defines the type schema for a Flatbuffer struct. Structs, like C structs, are statically ordered sets of data
/// whose schema may not be changed.
/// </summary>
public class ValueStructTypeModel : RuntimeTypeModel
{
    private readonly List<(int offset, string accessor, ITypeModel model)> members = new List<(int, string, ITypeModel)>();

    private int inlineSize;
    private int maxAlignment = 1;
    private bool isExternal;

    internal ValueStructTypeModel(Type clrType, TypeModelContainer container) : base(clrType, container)
    {
    }

    /// <summary>
    /// Gets the schema type.
    /// </summary>
    public override FlatBufferSchemaType SchemaType => FlatBufferSchemaType.Struct;

    /// <summary>
    /// Layout of the vtable.
    /// </summary>
    public override ImmutableArray<PhysicalLayoutElement> PhysicalLayout =>
        new PhysicalLayoutElement[] { new PhysicalLayoutElement(this.inlineSize, this.maxAlignment) }.ToImmutableArray();

    /// <summary>
    /// Like scalars, value structs are not sensitive to deserialization mode.
    /// </summary>
    public override bool IsParsingInvariant => true;

    /// <summary>
    /// Structs are composed of scalars.
    /// </summary>
    public override bool IsFixedSize => true;

    /// <summary>
    /// Structs can be part of structs.
    /// </summary>
    public override bool IsValidStructMember => true;

    /// <summary>
    /// Structs can be part of tables.
    /// </summary>
    public override bool IsValidTableMember => true;

    /// <summary>
    /// Structs can be part of unions.
    /// </summary>
    public override bool IsValidUnionMember => true;

    /// <summary>
    /// Structs can be part of vectors.
    /// </summary>
    public override bool IsValidVectorMember => true;

    /// <summary>
    /// Structs are written inline.
    /// </summary>
    public override bool SerializesInline => true;

    public override bool SerializeMethodRequiresContext => false;

    public override IEnumerable<ITypeModel> Children => this.members.Select(x => x.model);

    public bool CanMarshalOnSerialize { get; private set; }

    public bool CanMarshalOnParse { get; private set; }

    public override CodeGeneratedMethod CreateCloneMethodBody(CloneCodeGenContext context)
    {
        // Value types are pretty easy to clone!
        return new CodeGeneratedMethod($"return {context.ItemVariableName};") { IsMethodInline = true };
    }

    public override CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
    {
        return new CodeGeneratedMethod($"return {this.MaxInlineSize};");
    }

    public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
    {
        if (this.isExternal)
        {
            return this.CreateExternalParseMethod(context);
        }

        var propertyStatements = new List<string>();
        for (int i = 0; i < this.members.Count; ++i)
        {
            var member = this.members[i];
            var offsetAdjustment = member.offset != 0 ? $" + {member.offset}" : string.Empty;

            var parts = DefaultMethodNameResolver.ResolveParse(context.Options.DeserializationOption, member.model);

            propertyStatements.Add($@"
                item.{member.accessor} = {parts.@namespace}.{parts.className}.{parts.methodName}<{context.InputBufferTypeName}>(
                    {context.InputBufferVariableName}, 
                    {context.OffsetVariableName}{offsetAdjustment},
                    {context.RemainingDepthVariableName});");
        }

        string nonMarshalBody = $@"
            var item = default({CSharpHelpers.GetGlobalCompilableTypeName(this.ClrType)});
            {string.Join("\r\n", propertyStatements)}
            return item;
        ";

        if (!this.CanMarshalOnParse || !context.Options.EnableValueStructMemoryMarshalDeserialization)
        {
            return new CodeGeneratedMethod(nonMarshalBody);
        }

        string globalName = this.ClrType.GetGlobalCompilableTypeName();

        // For little endian architectures, we can do the equivalent of a reinterpret_cast operation. This will be
        // generally faster than reading fields individually, since we will read entire words.
        string body = $@"
            {StrykerSuppressor.SuppressNextLine("boolean")}
            if ({StrykerSuppressor.BitConverterTypeName}.IsLittleEndian)
            {{
                var mem = {context.InputBufferVariableName}.{nameof(IInputBuffer.GetReadOnlySpan)}().Slice({context.OffsetVariableName}, {this.inlineSize});
                return {typeof(MemoryMarshal).GetGlobalCompilableTypeName()}.{nameof(MemoryMarshal.Read)}<{globalName}>(mem);
            }}
            else
            {{
                {nonMarshalBody}
            }}
        ";

        return new CodeGeneratedMethod(body);
    }

    public override CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
    {
        if (this.isExternal)
        {
            return this.CreateExternalSerializeMethod(context);
        }

        var propertyStatements = new List<string>();
        for (int i = 0; i < this.members.Count; ++i)
        {
            var member = this.members[i];
            var fieldContext = context with
            {
                SpanVariableName = "sizedSpan",
                OffsetVariableName = $"{member.offset}",
                ValueVariableName = $"{context.ValueVariableName}.{member.accessor}",
            };

            propertyStatements.Add(fieldContext.GetSerializeInvocation(member.model.ClrType) + ";");
        }
        
        string body;
        string slice = $"Span<byte> sizedSpan = {context.SpanVariableName}.Slice({context.OffsetVariableName}, {this.inlineSize});";
        if (this.CanMarshalOnSerialize && context.Options.EnableValueStructMemoryMarshalDeserialization)
        {
            body = $@"
                {slice}
                
                {StrykerSuppressor.SuppressNextLine("boolean")}
                if ({StrykerSuppressor.BitConverterTypeName}.IsLittleEndian)
                {{
#if {CSharpHelpers.Net8PreprocessorVariable}
                    {typeof(MemoryMarshal).GetGlobalCompilableTypeName()}.Write(sizedSpan, in {context.ValueVariableName});
#else
                    {typeof(MemoryMarshal).GetGlobalCompilableTypeName()}.Write(sizedSpan, ref {context.ValueVariableName});
#endif
                }}
                else
                {{
                    {string.Join("\r\n", propertyStatements)}
                }}
            ";
        }
        else
        {
            body = $@"
                {slice}
                {string.Join("\r\n", propertyStatements)}";
        }

        return new CodeGeneratedMethod(body);
    }

    private CodeGeneratedMethod CreateExternalSerializeMethod(SerializationCodeGenContext context)
    {
        string globalName = this.ClrType.GetGlobalCompilableTypeName();
        string body = $@"
            FlatSharpInternal.AssertLittleEndian();
            FlatSharpInternal.AssertSizeOf<{globalName}>({this.inlineSize});
            Span<byte> sizedSpan = {context.SpanVariableName}.Slice({context.OffsetVariableName}, {this.inlineSize});

#if NET8_0_OR_GREATER
            {typeof(MemoryMarshal).GetGlobalCompilableTypeName()}.Write(sizedSpan, in {context.ValueVariableName});
#else
            {typeof(MemoryMarshal).GetGlobalCompilableTypeName()}.Write(sizedSpan, ref {context.ValueVariableName});
#endif
        ";

        return new CodeGeneratedMethod(body) { IsMethodInline = true };
    }

    private CodeGeneratedMethod CreateExternalParseMethod(ParserCodeGenContext context)
    {
        string globalName = this.ClrType.GetGlobalCompilableTypeName();

        string body = $@"
            FlatSharpInternal.AssertLittleEndian();
            FlatSharpInternal.AssertSizeOf<{globalName}>({this.inlineSize});
            var slice = {context.InputBufferVariableName}.{nameof(IInputBuffer.GetReadOnlySpan)}().Slice({context.OffsetVariableName}, {this.inlineSize});
            return {typeof(MemoryMarshal).GetGlobalCompilableTypeName()}.Read<{globalName}>(slice);
        ";

        return new CodeGeneratedMethod(body) { IsMethodInline = true };
    }

    public override void Initialize()
    {
        var structAttribute = this.ClrType.GetCustomAttribute<FlatBufferStructAttribute>();

        FlatSharpInternal.Assert(structAttribute is not null, "Struct attribute was null");
        FlatSharpInternal.Assert(this.ClrType.IsValueType, "Struct was not a value type");

        {
            string msg = $"Value struct '{this.GetCompilableTypeName()}' must have [StructLayout(LayoutKind.Explicit)] specified.";
            FlatSharpInternal.Assert(this.ClrType.StructLayoutAttribute is not null, msg);
            FlatSharpInternal.Assert(this.ClrType.StructLayoutAttribute.Value == LayoutKind.Explicit, msg);
            FlatSharpInternal.Assert(this.ClrType.IsExplicitLayout, msg);
        }

        var fields = this.ClrType
            .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Select(x => new
            {
                Field = x,
                OffsetAttribute = x.GetCustomAttribute<FieldOffsetAttribute>(),
            })
            .Where(x => x.OffsetAttribute != null)
            .OrderBy(x => x.OffsetAttribute!.Value)
            .ToList();

        FlatSharpInternal.Assert(fields.Count > 0, $"Value struct '{this.GetCompilableTypeName()}' is empty or has no public fields.");

        this.inlineSize = 0;
        foreach (var item in fields)
        {
            var offsetAttribute = item.OffsetAttribute;
            var field = item.Field;
            string? accessor = item.Field.GetFlatBufferMetadataOrNull(FlatBufferMetadataKind.Accessor);

            ITypeModel propertyModel = this.typeModelContainer.CreateTypeModel(field.FieldType);

            bool validMember = propertyModel.IsValidStructMember && propertyModel.PhysicalLayout.Length == 1;
            FlatSharpInternal.Assert(
                validMember,
                $"Struct '{this.GetCompilableTypeName()}' field {field.Name} cannot be part of a flatbuffer struct. Structs may only contain scalars and other structs.");

            FlatSharpInternal.Assert(
                !string.IsNullOrEmpty(accessor),
                $"Struct '{this.GetCompilableTypeName()}' field {field.Name} is not public and does not declare a custom accessor. Fields must also specify a custom accessor.");

            FlatSharpInternal.Assert(
                propertyModel.ClrType.IsValueType,
                $"Struct '{this.GetCompilableTypeName()}' field {field.Name} must be a value type if the struct is a value type.");

            int propertySize = propertyModel.PhysicalLayout[0].InlineSize;
            int propertyAlignment = propertyModel.PhysicalLayout[0].Alignment;
            this.maxAlignment = Math.Max(propertyAlignment, this.maxAlignment);

            // Pad for alignment.
            this.inlineSize += SerializationHelpers.GetAlignmentError(this.inlineSize, propertyAlignment);

            this.members.Add((this.inlineSize, accessor, propertyModel));

            FlatSharpInternal.Assert(
                offsetAttribute is not null,
                $"Struct '{this.ClrType.GetCompilableTypeName()}' missing offset attribute.");
            FlatSharpInternal.Assert(
                offsetAttribute.Value == this.inlineSize,
                $"Struct '{this.ClrType.GetCompilableTypeName()}' property '{field.Name}' defines invalid [FieldOffset] attribute. Expected: [FieldOffset({this.inlineSize})].");

            this.inlineSize += propertyModel.PhysicalLayout[0].InlineSize;
        }

        FlatSharpInternal.Assert(
            this.ClrType.IsPublic || this.ClrType.IsNestedPublic,
            $"Can't create type model from type {this.ClrType.GetCompilableTypeName()} because it is not public.");

        this.isExternal = this.ClrType.GetCustomAttribute<ExternalDefinitionAttribute>() is not null;
        this.CanMarshalOnSerialize = false;
        this.CanMarshalOnParse = false;

        if (UnsafeSizeOf(this.ClrType) == this.inlineSize)
        {
            this.CanMarshalOnParse = structAttribute.MemoryMarshalBehavior switch
            {
                MemoryMarshalBehavior.Parse or MemoryMarshalBehavior.Always => true,
                MemoryMarshalBehavior.Default => this.IsComplexStruct(),
                _ => false,
            };

            this.CanMarshalOnSerialize = structAttribute.MemoryMarshalBehavior switch
            {
                MemoryMarshalBehavior.Serialize or MemoryMarshalBehavior.Always => true,
                MemoryMarshalBehavior.Default => this.IsComplexStruct(),
                _ => false,
            };
        }
    }

    /// <summary>
    /// A complex struct is defined as:
    /// Having nested structs OR having at least 4 members. Not rocket science, but 
    /// experimentally, performance of MemoryMarshal.Cast overtakes field-by-field serialization 
    /// at around the 4 element mark. This is a heurustic, and can be overridden.
    /// </summary>
    private bool IsComplexStruct()
    {
        return this.members.Count >= 4 || this.members.Any(x => x.model.SchemaType != FlatBufferSchemaType.Scalar);
    }

    private static int UnsafeSizeOf(Type t)
    {
        object? value = typeof(Unsafe).GetMethod("SizeOf", BindingFlags.Public | BindingFlags.Static)?
            .MakeGenericMethod(t)
            .Invoke(null, new object[0]);

        FlatSharpInternal.Assert(value is not null, "Unsafe.SizeOf returned null.");
        return (int)value;
    }

    public override string GetDeserializedTypeName(FlatBufferDeserializationOption option, string inputBufferTypeName)
    {
        return this.GetGlobalCompilableTypeName();
    }
}
