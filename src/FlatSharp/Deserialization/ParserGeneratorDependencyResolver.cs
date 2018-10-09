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

namespace FlatSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using FlatSharp.TypeModel;

    /// <summary>
    /// A dependency resolver for generating a parse tree. Methods and type are registered and tracked. 
    /// </summary>
    internal class ParserGeneratorDependencyResolver
    {
        private readonly Dictionary<Type, MethodInfo> typeParserMap = ReflectedMethods.InputBufferReaders.ToDictionary(x => x.Key, x => x.Value);
        private readonly List<TypeBuilder> typesToBuild = new List<TypeBuilder>();
        private readonly StructParserGenerator structParserGenerator;
        private readonly TableParserGenerator tableParserGenerator;
        private readonly ListVectorParserGenerator listVectorParserGenerator;

        internal ParserGeneratorDependencyResolver(bool implementIDeserializedObject, bool cacheListVectorData)
        {
            this.structParserGenerator = new StructParserGenerator(implementIDeserializedObject);
            this.tableParserGenerator = new TableParserGenerator(implementIDeserializedObject);
            this.listVectorParserGenerator = new ListVectorParserGenerator(cacheListVectorData);
        }

        /// <summary>
        /// Gets or creates a method that parses the given type.
        /// </summary>
        public MethodInfo GetOrCreateParser(Type type)
        {
            RuntimeTypeModel typeModel = RuntimeTypeModel.CreateFrom(type);

            if (this.typeParserMap.TryGetValue(type, out var method))
            {
                return method;
            }

            if (typeModel is VectorTypeModel vectorModel)
            {
                if (vectorModel.IsList)
                {
                    this.listVectorParserGenerator.Generate(vectorModel, this);
                }
                else if (vectorModel.IsArray)
                {
                    ArrayVectorParserGenerator.Generate(vectorModel, this);
                }
                else if (vectorModel.IsMemoryVector)
                {
                    MemoryVectorParserGenerator.Generate(vectorModel, this);
                }
                else
                {
                    throw new InvalidOperationException("Unexpected vector type.");
                }
            }
            else if (typeModel is TableTypeModel tableModel)
            {
                this.tableParserGenerator.Generate(tableModel, this);
            }
            else if (typeModel is StructTypeModel structModel)
            {
                this.structParserGenerator.Generate(structModel, this);
            }

            if (!this.typeParserMap.TryGetValue(type, out method))
            {
                throw new InvalidOperationException("Unable to find method to parse type: " + type);
            }

            return method;
        }

        /// <summary>
        /// Registers the given method as being the parser for the given type model.
        /// </summary>
        public void Register(RuntimeTypeModel typeModel, MethodInfo methodInfo)
        {
            this.typeParserMap.Add(typeModel.ClrType, methodInfo);
        }

        /// <summary>
        /// Registers the type to be closed once the current action tree has completed.
        /// </summary>
        /// <param name="typeBuilder"></param>
        public void RegisterType(TypeBuilder typeBuilder)
        {
            this.typesToBuild.Add(typeBuilder);
        }

        public void CloseAllTypes()
        {
            foreach (var item in this.typesToBuild)
            {
                item.CreateTypeInfo();
            }
        }
    }
}
