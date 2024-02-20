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

using System.Collections.Immutable;
using System.Linq;
using FlatSharp.Attributes;

namespace FlatSharp.TypeModel;

/// <summary>
/// Defines the type schema for a Flatbuffer struct. Structs, like C structs, are statically ordered sets of data
/// whose schema may not be changed.
/// </summary>
public class StructTypeModel : RuntimeTypeModel
{
    private readonly List<StructMemberModel> memberTypes = new List<StructMemberModel>();
    private int inlineSize;
    private int maxAlignment = 1;
    private ConstructorInfo? preferredConstructor;
    private MethodInfo? onDeserializeMethod;
    private readonly Guid guid = Guid.NewGuid();

    internal StructTypeModel(Type clrType, TypeModelContainer container) : base(clrType, container)
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
    /// We only need context if one of our children needs it.
    /// </summary>
    public override bool SerializeMethodRequiresContext => this.Members.Any(x => x.ItemTypeModel.SerializeMethodRequiresContext);

    /// <summary>
    /// Structs are written inline.
    /// </summary>
    public override bool SerializesInline => true;

    public override IEnumerable<ITypeModel> Children => this.memberTypes.Select(x => x.ItemTypeModel);

    public override ConstructorInfo? PreferredSubclassConstructor => this.preferredConstructor;

    /// <summary>
    /// Gets the members of this struct.
    /// </summary>
    public IReadOnlyList<StructMemberModel> Members => this.memberTypes;

    public override CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
    {
        return new CodeGeneratedMethod($"return {this.MaxInlineSize};")
        {
            IsMethodInline = true
        };
    }

    public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
    {
        // We have to implement two items: The table class and the overall "read" method.
        // Let's start with the read method.
        string className = this.GetDeserializedClassName(context.Options.DeserializationOption);
        DeserializeClassDefinition classDef = DeserializeClassDefinition.Create(
            className,
            this.onDeserializeMethod,
            this,
            -1,
            context.Options);

        // Build up a list of property overrides.
        for (int index = 0; index < this.Members.Count; ++index)
        {
            var value = this.Members[index];
            classDef.AddProperty(value, context);
        }

        return new CodeGeneratedMethod($"return {className}<{context.InputBufferTypeName}>.GetOrCreate({context.InputBufferVariableName}, {context.OffsetVariableName}, {context.RemainingDepthVariableName});")
        {
            ClassDefinition = classDef.ToString(),
            IsMethodInline = true,
        };
    }

    public override CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
    {
        List<string> body = new List<string>();
        body.Add($"Span<byte> scopedSpan = {context.SpanVariableName}.Slice({context.OffsetVariableName}, {this.PhysicalLayout[0].InlineSize});");

        FlatSharpInternal.Assert(!this.ClrType.IsValueType, "Value-type struct is unexpected");

        body.Add(
            $@"
                if ({context.ValueVariableName} is null)
                {{
                    scopedSpan.Clear();
                    return;
                }}
            ");

        for (int i = 0; i < this.Members.Count; ++i)
        {
            var memberInfo = this.Members[i];

            string propertyAccessor = $"{context.ValueVariableName}.{memberInfo.PropertyInfo.Name}";
            if (memberInfo.CustomAccessor is not null)
            {
                propertyAccessor = $"{context.ValueVariableName}.{memberInfo.CustomAccessor}";
            }

            var propContext = context with
            {
                SpanVariableName = "scopedSpan",
                OffsetVariableName = $"{memberInfo.Offset}",
                ValueVariableName = $"{propertyAccessor}"
            };

            string invocation = propContext.GetSerializeInvocation(memberInfo.ItemTypeModel.ClrType) + ";";
            body.Add(invocation);
        }

        return new CodeGeneratedMethod(string.Join("\r\n", body));
    }

    public override CodeGeneratedMethod CreateCloneMethodBody(CloneCodeGenContext context)
    {
        var typeName = this.GetCompilableTypeName();
        string body = $"return {context.ItemVariableName} is null ? null : new {typeName}({context.ItemVariableName});";
        return new CodeGeneratedMethod(body)
        {
            IsMethodInline = true,
        };
    }

    public override void Initialize()
    {
        base.Initialize();

        foreach (var propertyTuple in this.GetProperties())
        {
            // Create downstream dependencies.
            this.typeModelContainer.CreateTypeModel(propertyTuple.Property.PropertyType);
        }
    }

    public override void Validate()
    {
        base.Validate();

        {
            FlatBufferStructAttribute? attribute = this.ClrType.GetCustomAttribute<FlatBufferStructAttribute>();
            FlatSharpInternal.Assert(attribute != null, "Missing attribute.");
        }
        
        // Reset in case validation is invoked multiple times.
        this.inlineSize = 0;
        this.memberTypes.Clear();

        TableTypeModel.EnsureClassCanBeInheritedByOutsideAssembly(this.ClrType, out this.preferredConstructor);
        this.onDeserializeMethod = TableTypeModel.ValidateOnDeserializedMethod(this);

        var properties = this.GetProperties();

        FlatSharpInternal.Assert(
            properties.Any(),
            $"Can't create struct type model from type {this.GetCompilableTypeName()} because it does not have any non-static [FlatBufferItem] properties. Structs cannot be empty.");

        ushort expectedIndex = 0;

        foreach (var item in properties)
        {
            var propertyAttribute = item.Attribute;
            var property = item.Property;

            FlatSharpInternal.Assert(
                !propertyAttribute.Deprecated,
                $"FlatBuffer struct {this.GetCompilableTypeName()} may not have deprecated properties");

            FlatSharpInternal.Assert(
                !propertyAttribute.ForceWrite,
                $"FlatBuffer struct {this.GetCompilableTypeName()} may not have properties with the ForceWrite option set to true.");

            ushort index = propertyAttribute.Index;
            FlatSharpInternal.Assert(
                index == expectedIndex,
                $"FlatBuffer struct {this.GetCompilableTypeName()} does not declare an item with index {expectedIndex}. Structs must have sequenential indexes starting at 0.");

            FlatSharpInternal.Assert(
                propertyAttribute.DefaultValue is null,
                $"FlatBuffer struct {this.GetCompilableTypeName()} declares default value on index {expectedIndex}. Structs may not have default values.");

            expectedIndex++;
            ITypeModel propertyModel = this.typeModelContainer.CreateTypeModel(property.PropertyType);

            int propertySize = propertyModel.PhysicalLayout[0].InlineSize;
            int propertyAlignment = propertyModel.PhysicalLayout[0].Alignment;
            this.maxAlignment = Math.Max(propertyAlignment, this.maxAlignment);

            // Pad for alignment.
            this.inlineSize += SerializationHelpers.GetAlignmentError(this.inlineSize, propertyAlignment);
            int length = propertyModel.PhysicalLayout[0].InlineSize;

            StructMemberModel model = new StructMemberModel(
                propertyModel,
                property,
                item.Attribute,
                this.inlineSize,
                length);

            this.memberTypes.Add(model);
            this.inlineSize += length;
        }

        foreach (StructMemberModel member in this.memberTypes)
        {
            ITypeModel memberModel = member.ItemTypeModel;

            bool validMember = memberModel.IsValidStructMember && memberModel.PhysicalLayout.Length == 1;

            FlatSharpInternal.Assert(
                validMember,
                $"Struct '{this.GetCompilableTypeName()}' property {member.PropertyInfo.Name} (Index {member.Index}) with type {CSharpHelpers.GetCompilableTypeName(member.PropertyInfo.PropertyType)} cannot be part of a flatbuffer struct.");

            member.Validate();
        }
    }

    public override string GetDeserializedTypeName(FlatBufferDeserializationOption option, string inputBufferTypeName)
    {
        var (ns, name) = DefaultMethodNameResolver.ResolveHelperClassName(this);
        return $"{ns}.{name}.{this.GetDeserializedClassName(option)}<{inputBufferTypeName}>";
    }

    private IEnumerable<(PropertyInfo Property, FlatBufferItemAttribute Attribute)> GetProperties()
    {
        return this.ClrType
            .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Select(x => (Property: x, Attribute: x.GetCustomAttribute<FlatBufferItemAttribute>()!))
            .Where(x => x.Attribute is not null)
            .OrderBy(x => x.Attribute.Index);
    }

    private string GetDeserializedClassName(FlatBufferDeserializationOption option)
    {
        return $"structReader_{this.guid:n}_{option}";
    }
}
