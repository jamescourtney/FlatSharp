namespace FlatSharp.Compiler.Schema
{
    using FlatSharp.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /*
    table Object {  // Used for both tables and structs.
        name:string (required, key);
        fields:[Field] (required);  // Sorted.
        is_struct:bool = false;
        minalign:int;
        bytesize:int;  // For structs.
        attributes:[KeyValue];
        documentation:[string];
        /// File that this Object is declared in.
        declaration_file: string;
    }
    */
    [FlatBufferTable]
    public class FlatBufferObject
    {
        [FlatBufferItem(0, Required = true, Key = true)]
        public virtual string Name { get; set; } = string.Empty;

        [FlatBufferItem(1, Required = true)]
        public virtual IIndexedVector<string, Field> Fields { get; set; } = new IndexedVector<string, Field>();

        [FlatBufferItem(2)]
        public virtual bool IsStruct { get; set; }

        [FlatBufferItem(3)]
        public virtual int MinAlign { get; set; }

        // For structs, the size.
        [FlatBufferItem(4)]
        public virtual int ByteSize { get; set; }

        [FlatBufferItem(5)]
        public virtual IIndexedVector<string, KeyValue>? Attributes { get; set; }

        [FlatBufferItem(6)]
        public virtual IList<string>? Documentation { get; set; }

        [FlatBufferItem(7)]
        public virtual string? DeclarationFile { get; set; }

        internal void WriteCode(CodeWriter writer, CompileContext context)
        {
            if (context.CompilePass >= CodeWritingPass.LastPass && context.RootFile != this.DeclarationFile)
            {
                return;
            }

            FlatSharpAttributeTarget containerKind = this.Classify();

            FlatSharpAttributes typeAttributes = new FlatSharpAttributes(this.Attributes);
            typeAttributes.Validate(containerKind);

            (string ns, _) = Helpers.ParseName(this.Name);

            writer.AppendLine($"namespace {ns}");
            using (writer.WithBlock())
            {
                if (containerKind != FlatSharpAttributeTarget.ValueStruct)
                {
                    this.WriteReferenceTableOrStruct(writer, context, containerKind, typeAttributes);
                }
            }
        }

        private void WriteReferenceTableOrStruct(
            CodeWriter writer,
            CompileContext context,
            FlatSharpAttributeTarget containerKind,
            FlatSharpAttributes attributes)
        {
            (_, string name) = Helpers.ParseName(this.Name);

            FlatSharpAttributeTarget fieldType = containerKind == FlatSharpAttributeTarget.Table ? FlatSharpAttributeTarget.TableField : FlatSharpAttributeTarget.StructField;

            writer.AppendLine("[System.Runtime.CompilerServices.CompilerGenerated]");
            writer.AppendLine("[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]");
            writer.AppendLine(this.GetAttribute(containerKind, context));
            writer.AppendLine($"public partial class {name} : object");
            using (writer.WithBlock())
            {
                ushort nextField = 0;
                foreach (var field in this.Fields.Select(x => x.Value).OrderBy(x => x.Id))
                {
                    var fieldAttributes = new FlatSharpAttributes(field.Attributes);
                    fieldAttributes.Validate(fieldType);

                    string @virtual = "virtual ";
                    if (fieldAttributes.NonVirtual ?? attributes.NonVirtual == true) 
                    {
                        @virtual = string.Empty;
                    }

                    string setter = fieldAttributes.SetterKind switch
                    {
                        SetterKind.None => string.Empty,
                        SetterKind.Private => "private set;",

                        SetterKind.Public => "set;",
                        SetterKind.PublicInit => "init;",

                        SetterKind.Protected => "protected set;",
                        SetterKind.ProtectedInit => "protected init;",

                        SetterKind.ProtectedInternal => "protected internal set;",
                        SetterKind.ProtectedInternalInit => "protected internal init;",

                        _ => "set;"
                    };

                    string typeName = field.Type.FormatTypeName(context.Root, out bool isVector, out bool isArray);

                    if (isArray)
                    {
                        // TODO
                        int length = field.Type.FixedLength;
                        for (int i = 0; i < length; ++i)
                        {
                            // Add virtual fields to represent the array.
                            var temp = new Field
                            {
                                Attributes = field.Attributes,
                                Type = new FlatBufferType { BaseType = field.Type.ElementType },
                                Name = $"__flatsharp__{field.Name}_{i}",
                            };

                            writer.AppendLine();
                            writer.AppendLine(this.GetPropertyAttribute(temp, fieldAttributes, attributes, nextField++, customGetter: $"{field.Name}[{i}]"));
                            writer.AppendLine($"protected {@virtual}{typeName} {temp.Name} {{ get; {setter} }}");
                        }

                        continue;
                    }
                    else if (isVector)
                    {
                        if (field.Type.ElementType != BaseType.UByte && 
                            (fieldAttributes.VectorKind == VectorType.Memory || fieldAttributes.VectorKind == VectorType.ReadOnlyMemory))
                        {
                            ErrorContext.Current.RegisterError("Memory and ReadOnlyMemory vectors may only contain unsigned bytes.");
                        }

                        string keyType = string.Empty;
                        if (fieldAttributes.VectorKind == VectorType.IIndexedVector)
                        {
                            Field? keyField = context.Root.Objects[field.Type.Index].Fields.Select(x => x.Value).SingleOrDefault(x => x.Key);
                            if (keyField is not null)
                            {
                                keyType = keyField.Type.FormatTypeName(context.Root, out _, out _);
                            }
                            else
                            {
                                keyType = "string";
                                ErrorContext.Current.RegisterError($"Field '{field.Name}' declares an indexed vector, but the inner table does not declare a '{MetadataKeys.Key}' field for sorting.");
                            }
                        }

                        typeName = fieldAttributes.VectorKind switch
                        {
                            VectorType.IList => $"IList<{typeName}>",
                            VectorType.IReadOnlyList => $"IReadOnlyList<{typeName}>",
                            VectorType.Array => $"{typeName}[]",
                            VectorType.Memory => $"Memory<{typeName}>",
                            VectorType.ReadOnlyMemory => $"ReadOnlyMemory<{typeName}>",
                            VectorType.IIndexedVector => $"IIndexedVector<{keyType}, {typeName}>",
                            _ => $"IList<{typeName}>", // TODO: error
                        };
                    }

                    // Structs are always required.
                    string optional = string.Empty;
                    if (containerKind == FlatSharpAttributeTarget.Table)
                    {
                        if (field.Type.BaseType.IsScalar())
                        {
                            if (field.Optional)
                            {
                                optional = "?";
                            }
                        }
                        else if (!field.Required)
                        {
                            optional = "?";
                        }
                    }

                    writer.AppendLine();
                    writer.AppendLine(this.GetPropertyAttribute(field, fieldAttributes, attributes, nextField++));
                    writer.AppendLine($"public {@virtual}{typeName}{optional} {field.Name} {{ get; {setter} }}");
                }
            }
        }

        private string GetPropertyAttribute(
            Field field, 
            FlatSharpAttributes fieldAttributes,
            FlatSharpAttributes containerAttributes,
            ushort fieldId,
            string? customGetter = null)
        {
            string isKey = string.Empty;
            string sortedVector = string.Empty;
            string defaultValue = string.Empty;
            string isDeprecated = string.Empty;
            string forceWrite = string.Empty;
            string customAccessor = string.Empty;
            string writeThrough = string.Empty;
            string required = string.Empty;

            if (field.Key)
            {
                isKey = $", {nameof(FlatBufferItemAttribute.Key)} = true";
            }

            if (fieldAttributes.SortedVector == true)
            {
                sortedVector = $", {nameof(FlatBufferItemAttribute.SortedVector)} = true";
            }

            if (field.Deprecated)
            {
                isDeprecated = $", {nameof(FlatBufferItemAttribute.Deprecated)} = true";
            }

            if (!string.IsNullOrEmpty(customGetter))
            {
                customAccessor = $"[{nameof(FlatBufferMetadataAttribute)}({nameof(FlatBufferMetadataKind)}.{nameof(FlatBufferMetadataKind.Accessor)}, \"{customGetter}\")]";
            }

            //if (this.TryGetDefaultValueLiteral(thisTypeModel, out var literal))
            //{
            //    defaultValue = $", {nameof(FlatBufferItemAttribute.DefaultValue)} = {literal}";
            //}

            if (field.Required)
            {
                required = $", {nameof(FlatBufferItemAttribute.Required)} = true";
            }

            if (field.Type.BaseType.IsScalar())
            {
                bool? fw = fieldAttributes.ForceWrite ?? containerAttributes.ForceWrite;
                if (fw == true)
                {
                    forceWrite = $", {nameof(FlatBufferItemAttribute.ForceWrite)} = true";
                }
            }

            if (fieldAttributes.WriteThrough ?? containerAttributes.WriteThrough == true)
            {
                writeThrough = $", {nameof(FlatBufferItemAttribute.WriteThrough)} = true";
            }

            return $"[{nameof(FlatBufferItemAttribute)}({fieldId}{defaultValue}{isDeprecated}{sortedVector}{isKey}{forceWrite}{writeThrough}{required})]{customAccessor}";
        }

        private void WriteValueStruct()
        {

        }

        private FlatSharpAttributeTarget Classify()
        {
            FlatSharpAttributeTarget containerKind = this.IsStruct ? FlatSharpAttributeTarget.Struct : FlatSharpAttributeTarget.Table;
            if (this.Attributes.DefaultIfNull().ContainsKey(MetadataKeys.ValueStruct))
            {
                if (this.IsStruct)
                {
                    containerKind = FlatSharpAttributeTarget.ValueStruct;
                }
                else
                {
                    ErrorContext.Current.RegisterError($"Table '{this.Name}' cannot have the '{MetadataKeys.ValueStruct}' attribute set. It is only valid on structs.");
                }
            }

            return containerKind;
        }

        private string GetAttribute(FlatSharpAttributeTarget element, CompileContext context)
        {
            if (element == FlatSharpAttributeTarget.Table)
            {
                return "[FlatBufferTable]";
            }
            else if (element == FlatSharpAttributeTarget.Struct)
            {
                return "[FlatBufferStruct]";
            }
            else if (element == FlatSharpAttributeTarget.ValueStruct)
            {
                string size = string.Empty;
                if (context.PreviousAssembly is not null)
                {
                    Type? previousType = context.PreviousAssembly.GetType(this.Name);
                    FlatSharpInternal.Assert(previousType is not null, "Previous type was null");

                    var model = context.TypeModelContainer.CreateTypeModel(previousType);
                    FlatSharpInternal.Assert(model.PhysicalLayout.Length == 1, "physical length was 1");

                    size = $", Size = {model.PhysicalLayout[0].InlineSize}";
                }

                return $"[FlatBufferStruct, StructLayout(LayoutKind.Explicit{size})]";
            }
            else
            {
                throw new InvalidOperationException("Unexpected classification: " + element);
            }
        }
    }
}
