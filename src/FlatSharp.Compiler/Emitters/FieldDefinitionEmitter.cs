namespace FlatSharp.Compiler
{
    using FlatSharp.Attributes;
    using FlatSharp.TypeModel;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Diagnostics.CodeAnalysis;

    internal class FieldDefinitionEmitter : IFieldDefinitionEmitter
    {
        public void EmitBasicDefinition(CodeWriter writer, FieldDefinition fieldDefinition, CompileContext context)
        {
            // Indexed Vectors require two generic parameters:
            // TKeyType, TableType
            // We have a chicken-egg problem here, because we need the compiled version
            // to discover the key type, but we need the key type to build the compiled version.
            // In the first pass, where we don't have any type model information, we can break
            // this circular loop by changing indexed vectors to arrays. Following that,
            // we can introspect on the key type and build the proper indexed vector definition
            // in pass #2.
            VectorType type = fieldDefinition.VectorType;
            if (type == VectorType.IIndexedVector)
            {
                type = VectorType.Array;
            }

            string clrType = this.GetClrVectorTypeName(
                type,
                this.GetBasicClrTypeName(fieldDefinition, context), 
                string.Empty);

            writer.AppendLine(this.FormatAttribute(fieldDefinition, null));
            writer.AppendLine(this.FormatPropertyDeclaration(fieldDefinition, null, clrType));
        }

        public bool EmitDefinition(CodeWriter writer, FieldDefinition fieldDefinition, CompileContext context)
        {
            if (context.CompilePass == CodeWritingPass.Initialization)
            {
                this.EmitBasicDefinition(writer, fieldDefinition, context);
                return true;
            }

            if (!this.TryGetTypeModel(fieldDefinition, context, out var model))
            {
                return false;
            }

            string clrType = this.GetBasicClrTypeName(fieldDefinition, context);
            string vectorKeyType = string.Empty;

            if (model.TryGetUnderlyingVectorType(out ITypeModel? vectorItemModel) &&
                vectorItemModel.TryGetTableKeyMember(out TableMemberModel? memberModel))
            {
                vectorKeyType = CSharpHelpers.GetCompilableTypeName(memberModel.ItemTypeModel.ClrType);
            }

            clrType = this.GetClrVectorTypeName(fieldDefinition.VectorType, clrType, vectorKeyType);

            writer.AppendLine(this.FormatAttribute(fieldDefinition, model));
            writer.AppendLine(this.FormatPropertyDeclaration(fieldDefinition, model, clrType));

            return false;
        }

        public void EmitCloneLine(CodeWriter writer, string variableName, FieldDefinition fieldDefinition, CompileContext context)
        {
            if (context.CompilePass <= CodeWritingPass.PropertyModeling)
            {
                return;
            }

            if (!fieldDefinition.Parent.TryResolveTypeModelWithError(fieldDefinition.FbsFieldType, context, out _))
            {
                return;
            }

            writer.AppendLine($"this.{fieldDefinition.Name} = {context.FullyQualifiedCloneMethodName}({variableName}.{fieldDefinition.Name});");
        }

        public void EmitDefaultInitializationLine(CodeWriter writer, FieldDefinition fieldDefinition, CompileContext compileContext)
        {
            if (compileContext.CompilePass <= CodeWritingPass.PropertyModeling)
            {
                return;
            }

            if (!this.TryGetTypeModel(fieldDefinition, compileContext, out var model))
            {
                return;
            }

            string? assignment = null;
            if (this.TryGetDefaultValueLiteral(fieldDefinition, model, out var defaultValueLiteral))
            {
                assignment = defaultValueLiteral;
            }
            else if (model.ClassifyContextually(fieldDefinition.Parent.SchemaType).IsRequiredReference())
            {
                var cSharpTypeName = CSharpHelpers.GetCompilableTypeName(model.ClrType);
                assignment = $"new {cSharpTypeName}()";
            }

            if (!string.IsNullOrEmpty(assignment))
            {
                writer.AppendLine($"this.{fieldDefinition.Name} = {assignment};");
            }
        }

        private bool TryGetTypeModel(
            FieldDefinition fieldDefinition, 
            CompileContext context, 
            [NotNullWhen(true)] out ITypeModel? typeModel)
        {
            typeModel = null;
            Type? propertyType = context.PreviousAssembly?.GetType(fieldDefinition.Parent.FullName)?.GetProperty(fieldDefinition.Name)?.PropertyType;

            if (propertyType is null)
            {
                ErrorContext.Current.RegisterError($"Unable to find property '{fieldDefinition.Name}' from parent '{fieldDefinition.Parent.Name}'.");
                return false;
            }

            if (!context.TypeModelContainer.TryCreateTypeModel(propertyType, out typeModel))
            {
                ErrorContext.Current.RegisterError($"Type model container failed to create type model for type '{propertyType}'.");
                return false;
            }

            return true;
        }

        private string FormatPropertyDeclaration(
            FieldDefinition definition,
            ITypeModel? thisTypeModel,
            string clrTypeName)
        {
            string @virtual = "virtual";

            if (definition.NonVirtual ?? definition.Parent.NonVirtual == true)
            {
                @virtual = string.Empty;
            }

            string setter = definition.SetterKind switch
            {
                SetterKind.Public => "set;", // setter has same access as getter.
                SetterKind.PublicInit => "init;",
                SetterKind.Protected => "protected set;",
                SetterKind.ProtectedInit => "protected init;",
                SetterKind.ProtectedInternal => "protected internal set;",
                SetterKind.ProtectedInternalInit => "protected internal init;",
                SetterKind.None => string.Empty,
                _ => string.Empty,
            };

            if (thisTypeModel?.ClassifyContextually(definition.Parent.SchemaType).IsOptionalReference() == true)
            {
                clrTypeName += "?";
            }

            return $"public {@virtual} {clrTypeName} {definition.Name} {{ get; {setter} }}";
        }

        private string FormatAttribute(
            FieldDefinition definition,
            ITypeModel? thisTypeModel)
        {
            string isKey = string.Empty;
            string sortedVector = string.Empty;
            string defaultValue = string.Empty;
            string isDeprecated = string.Empty;

            if (definition.IsKey)
            {
                isKey = $", {nameof(FlatBufferItemAttribute.Key)} = true";
            }

            if (definition.SortedVector)
            {
                sortedVector = $", {nameof(FlatBufferItemAttribute.SortedVector)} = true";
            }

            if (definition.Deprecated)
            {
                isDeprecated = $", {nameof(FlatBufferItemAttribute.Deprecated)} = true";
            }

            if (this.TryGetDefaultValueLiteral(definition, thisTypeModel, out var literal))
            {
                defaultValue = $", {nameof(FlatBufferItemAttribute.DefaultValue)} = {literal}";
            }

            return $"[{nameof(FlatBufferItemAttribute)}({definition.Index}{defaultValue}{isDeprecated}{sortedVector}{isKey})]";
        }

        private bool TryGetDefaultValueLiteral(FieldDefinition definition, ITypeModel? typeModel, [NotNullWhen(true)] out string? literal)
        {
            if (typeModel is not null && !string.IsNullOrEmpty(definition.DefaultValue))
            {
                if (typeModel.TryFormatStringAsLiteral(definition.DefaultValue, out literal))
                {
                    return true;
                }
                else
                {
                    ErrorContext.Current.RegisterError($"Unable to format default value '{definition.DefaultValue}' as a literal.");
                }
            }

            literal = null;
            return false;
        }

        /// <summary>
        /// Returns the basic CLR type. Non-vectorized.
        /// </summary>
        private string GetBasicClrTypeName(
            FieldDefinition definition, 
            CompileContext context)
        {
            if (definition.SharedString)
            {
                return typeof(SharedString).FullName ?? throw new InvalidOperationException("Full name was null");
            }

            string clrType = definition.FbsFieldType;

            if (context.TypeModelContainer.TryResolveFbsAlias(definition.FbsFieldType, out ITypeModel? resolvedTypeModel))
            {
                clrType = resolvedTypeModel.ClrType.FullName ?? throw new InvalidOperationException("Full name was null");
            }
            else if (definition.Parent.TryResolveName(definition.FbsFieldType, out var node))
            {
                clrType = node.GlobalName;
            }

            if (definition.IsOptionalScalar)
            {
                clrType += "?";
            }

            return clrType;
        }

        private string GetClrVectorTypeName(VectorType vectorType, string clrType, string sortKeyType)
        {
            switch (vectorType)
            {
                case VectorType.Array:
                    return $"{clrType}[]";

                case VectorType.IList:
                    return $"IList<{clrType}>";

                case VectorType.IReadOnlyList:
                    return $"IReadOnlyList<{clrType}>";

                case VectorType.Memory:
                    return $"Memory<{clrType}>?";

                case VectorType.ReadOnlyMemory:
                    return $"ReadOnlyMemory<{clrType}>?";

                case VectorType.IIndexedVector:
                    if (string.IsNullOrWhiteSpace(sortKeyType))
                    {
                        ErrorContext.Current.RegisterError($"Unable to determine key type for table {clrType}. Please make sure a property has the 'Key' metadata.");
                    }

                    return $"IIndexedVector<{sortKeyType}, {clrType}>";

                case VectorType.None:
                    return clrType;

                default:
                    throw new InvalidOperationException($"Unexpected value for vectortype: '{vectorType}'");
            }
        }
    }
}
