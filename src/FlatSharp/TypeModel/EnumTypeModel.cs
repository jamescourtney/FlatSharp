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
 
 namespace FlatSharp.TypeModel
{
    using FlatSharp.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Defines an Enum FlatSharp type model, which derives from the scalar type model.
    /// </summary>
    public class EnumTypeModel : ScalarTypeModel
    {
        internal EnumTypeModel(Type type, int size) : base(type, size)
        {
        }

        protected override void Initialize()
        {
            base.Initialize();

            Type enumType = this.ClrType;
            var attribute = enumType.GetCustomAttribute<FlatBufferEnumAttribute>();
            if (attribute.DeclaredUnderlyingType != Enum.GetUnderlyingType(enumType))
            {
                throw new InvalidFlatBufferDefinitionException($"Enum '{enumType.Name}' declared underlying type '{attribute.DeclaredUnderlyingType}', but was actually '{Enum.GetUnderlyingType(enumType)}'");
            }
        }

        /// <summary>
        /// Enums are not built in, even though scalars are.
        /// </summary>
        public override bool IsBuiltInType => false;

        /// <summary>
        /// Enums can't be sorted vector keys.
        /// </summary>
        public override bool IsValidSortedVectorKey => false;

        public override CodeGeneratedMethod CreateGetMaxSizeMethodBody(GetMaxSizeCodeGenContext context)
        {
            Type underlyingType = Enum.GetUnderlyingType(this.ClrType);
            string underlyingTypeName = CSharpHelpers.GetCompilableTypeName(underlyingType);

            return new CodeGeneratedMethod 
            { 
                MethodBody = $"return {context.MethodNameMap[underlyingType]}(({underlyingTypeName}){context.ValueVariableName});" 
            };
        }

        public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
        {
            Type underlyingType = Enum.GetUnderlyingType(this.ClrType);
            string typeName = CSharpHelpers.GetCompilableTypeName(this.ClrType);

            return new CodeGeneratedMethod
            {
                MethodBody = $"return ({typeName}){context.MethodNameMap[underlyingType]}({context.InputBufferVariableName}, {context.OffsetVariableName});"
            };
        }

        public override CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
        {
            Type underlyingType = Enum.GetUnderlyingType(this.ClrType);
            string underlyingTypeName = CSharpHelpers.GetCompilableTypeName(underlyingType);

            return new CodeGeneratedMethod
            {
                MethodBody = $"{context.MethodNameMap[underlyingType]}({context.SpanWriterVariableName}, {context.SpanVariableName}, ({underlyingTypeName}){context.ValueVariableName}, {context.OffsetVariableName}, {context.SerializationContextVariableName});"
            };
        }

        public override void TraverseObjectGraph(HashSet<Type> seenTypes)
        {
            base.TraverseObjectGraph(seenTypes);
            seenTypes.Add(this.ClrType);
            seenTypes.Add(Enum.GetUnderlyingType(this.ClrType));
        }
    }
}
