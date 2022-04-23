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

namespace FlatSharp.TypeModel;

[Flags]
public enum ContextualTypeModelClassification
{
    /// <summary>
    /// Default -- no flags.
    /// </summary>
    Undefined = 0,

    /// <summary>
    /// Indicates that the field is optional
    /// </summary>
    Optional = 1,

    /// <summary>
    /// Indicates that the field is required.
    /// </summary>
    Required = 2,

    /// <summary>
    /// Indicates that the field is a reference.
    /// </summary>
    ReferenceType = 4,

    /// <summary>
    /// Indicates that the field is a value.
    /// </summary>
    ValueType = 8,
}

/// <summary>
/// Extensions for <see cref="ITypeModel"/>.
/// </summary>
internal static class ITypeModelExtensions
{
    /// <summary>
    /// Indicates if the given type model is a nullable reference in the given context.
    /// </summary>
    /// <param name="typeModel">The type model.</param>
    /// <param name="context">The context (ie, table, struct, vector, etc)</param>
    public static ContextualTypeModelClassification ClassifyContextually(this ITypeModel typeModel, FlatBufferSchemaType context)
    {
        var flags = ContextualTypeModelClassification.Undefined;

        if (typeModel.ClrType.IsValueType)
        {
            flags |= ContextualTypeModelClassification.ValueType;

            if (Nullable.GetUnderlyingType(typeModel.ClrType) == null)
            {
                flags |= ContextualTypeModelClassification.Required;
            }
            else
            {
                flags |= ContextualTypeModelClassification.Optional;
            }
        }
        else
        {
            flags |= ContextualTypeModelClassification.ReferenceType;

            if (context == FlatBufferSchemaType.Table)
            {
                flags |= ContextualTypeModelClassification.Optional;
            }
            else
            {
                flags |= ContextualTypeModelClassification.Required;
            }
        }

        return flags;
    }

    [ExcludeFromCodeCoverage]
    public static bool IsRequiredReference(this ContextualTypeModelClassification flags)
        => flags.HasFlag(ContextualTypeModelClassification.ReferenceType | ContextualTypeModelClassification.Required);

    [ExcludeFromCodeCoverage]
    public static bool IsOptionalReference(this ContextualTypeModelClassification flags)
        => flags.HasFlag(ContextualTypeModelClassification.ReferenceType | ContextualTypeModelClassification.Optional);

    [ExcludeFromCodeCoverage]
    public static bool IsRequiredValue(this ContextualTypeModelClassification flags)
        => flags.HasFlag(ContextualTypeModelClassification.ValueType | ContextualTypeModelClassification.Required);

    [ExcludeFromCodeCoverage]
    public static bool IsOptionalValue(this ContextualTypeModelClassification flags)
        => flags.HasFlag(ContextualTypeModelClassification.ValueType | ContextualTypeModelClassification.Optional);

    [ExcludeFromCodeCoverage]
    public static bool IsOptional(this ContextualTypeModelClassification flags)
        => flags.HasFlag(ContextualTypeModelClassification.Optional);

    [ExcludeFromCodeCoverage]
    public static bool IsRequired(this ContextualTypeModelClassification flags)
        => flags.HasFlag(ContextualTypeModelClassification.Required);

    [ExcludeFromCodeCoverage]
    public static bool IsReference(this ContextualTypeModelClassification flags)
        => flags.HasFlag(ContextualTypeModelClassification.ReferenceType);

    [ExcludeFromCodeCoverage]
    public static bool IsValue(this ContextualTypeModelClassification flags)
        => flags.HasFlag(ContextualTypeModelClassification.ValueType);

    /// <summary>
    /// Shortcut for getting compilable type name.
    /// </summary>
    public static string GetCompilableTypeName(this ITypeModel typeModel)
        => CSharpHelpers.GetCompilableTypeName(typeModel.ClrType);

    /// <summary>
    /// Shortcut for getting compilable type name.
    /// </summary>
    public static string GetGlobalCompilableTypeName(this ITypeModel typeModel)
        => CSharpHelpers.GetGlobalCompilableTypeName(typeModel.ClrType);

    public static string GetNullableAnnotationTypeName(this ItemMemberModel memberModel, FlatBufferSchemaType context)
    {
        var typeName = memberModel.ItemTypeModel.GetCompilableTypeName();
        if (memberModel.ItemTypeModel.ClassifyContextually(context).IsOptionalReference() && !memberModel.IsRequired)
        {
            typeName += "?";
        }

        return typeName;
    }

    /// <summary>
    /// Gets a value indicating if the type of this type model is a non-nullable CLR value type.
    /// </summary>
    public static bool IsNonNullableClrValueType(this ITypeModel typeModel)
        => typeModel.ClrType.IsValueType && Nullable.GetUnderlyingType(typeModel.ClrType) is null;

    /// <summary>
    /// Returns a boolean expression that compares the given variable name to the given default value literal for inequality.
    /// </summary>
    public static string GetNotEqualToDefaultValueLiteralExpression(this ITypeModel typeModel, string variableName, string defaultValueLiteral)
    {
        if (typeModel.IsNonNullableClrValueType())
        {
            if (typeModel.ClrType.IsPrimitive || typeModel.ClrType.IsEnum)
            {
                // Let's hope that != is implemented rationally!
                return $"{variableName} != {defaultValueLiteral}";
            }
            else
            {
                // We assume non-primitive structs are not equal.
                return "true";
            }
        }
        else
        {
            // Nullable<T> and reference types are easy.
            return $"!({variableName} is {defaultValueLiteral})";
        }
    }

    /// <summary>
    /// Returns a compile-time constant default expression for the given type model.
    /// </summary>
    public static string GetTypeDefaultExpression(this ITypeModel typeModel)
    {
        if (typeModel.IsNonNullableClrValueType())
        {
            return $"default({typeModel.GetCompilableTypeName()})";
        }
        else
        {
            return "null";
        }
    }

    /// <summary>
    /// Returns true if the dependency graph of this type model contains a cycle. Implemented without recursion
    /// to accomodate complex graphs.
    /// </summary>
    public static bool ContainsCycle(this ITypeModel model)
    {
        // Bad implementation, but probably won't be a problem.
        HashSet<Type> visited = new();
        HashSet<Type> onStack = new();
        Stack<ITypeModel> stack = new();

        IList<ITypeModel> list = model.TraverseObjectGraph(out _);
        for (int i = 0; i < list.Count; ++i)
        {
            ITypeModel current = list[i];

            if (visited.Contains(current.ClrType))
            {
                continue;
            }

            stack.Push(current);

            while (stack.Count > 0)
            {
                ITypeModel top = stack.Peek();

                if (visited.Add(top.ClrType))
                {
                    onStack.Add(top.ClrType);
                }
                else
                {
                    onStack.Remove(top.ClrType);
                    stack.Pop();
                }

                foreach (ITypeModel child in top.Children)
                {
                    if (!visited.Contains(child.ClrType))
                    {
                        stack.Push(child);
                    }
                    else if (onStack.Contains(child.ClrType))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Recursively traverses the full object graph for the given type model.
    /// </summary>
    public static IList<ITypeModel> TraverseObjectGraph(this ITypeModel model, out HashSet<Type> seenTypes)
    {
        Queue<ITypeModel> discoveryQueue = new();
        List<ITypeModel> distinctModels = new();
        seenTypes = new();

        discoveryQueue.Enqueue(model);

        while (discoveryQueue.Count > 0)
        {
            ITypeModel next = discoveryQueue.Dequeue();
            if (seenTypes.Add(next.ClrType))
            {
                distinctModels.Add(next);

                foreach (var child in next.Children)
                {
                    discoveryQueue.Enqueue(child);
                }
            }
        }

        return distinctModels;
    }

    /// <summary>
    /// Gets all TableFieldContexts for the entire object graph.
    /// </summary>
    public static List<(ITypeModel, TableFieldContext)> GetAllTableFieldContexts(this ITypeModel rootType)
    {
        static void GetTableFieldContextsRecursive(ITypeModel model, List<(ITypeModel, TableFieldContext)> contexts, HashSet<Type> seenTypes)
        {
            if (seenTypes.Add(model.ClrType))
            {
                contexts.AddRange(model.GetFieldContexts());

                foreach (var child in model.Children)
                {
                    GetTableFieldContextsRecursive(child, contexts, seenTypes);
                }
            }
        }

        List<(ITypeModel, TableFieldContext)> result = new();
        GetTableFieldContextsRecursive(rootType, result, new());
        return result;
    }
}
