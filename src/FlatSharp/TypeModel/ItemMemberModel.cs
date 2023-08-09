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
using FlatSharp.Attributes;

namespace FlatSharp.TypeModel;

/// <summary>
/// Describes a member of a FlatBuffer table or struct.
/// </summary>
public abstract class ItemMemberModel
{
    public enum SetMethodKind
    {
        Set = 0,
        Init = 1,
    }

    public string FriendlyName => $"{this.PropertyInfo.DeclaringType!.GetCompilableTypeName()}.{this.PropertyInfo.Name}";

    protected ItemMemberModel(
        ITypeModel propertyModel,
        PropertyInfo propertyInfo,
        FlatBufferItemAttribute attribute)
    {
        var getMethod = propertyInfo.GetMethod;
        var setMethod = propertyInfo.SetMethod;

        this.ItemTypeModel = propertyModel;
        this.PropertyInfo = propertyInfo;
        this.Index = attribute.Index;
        this.CustomAccessor = propertyInfo.GetFlatBufferMetadataOrNull(FlatBufferMetadataKind.Accessor);
        this.IsWriteThrough = attribute.WriteThrough;
        this.IsRequired = attribute.Required;
        this.Attribute = attribute;

        FlatSharpInternal.Assert(getMethod is not null, $"Property {this.DeclaringType} on did not declare a getter.");

        if (setMethod is not null)
        {
            this.SetterKind = SetMethodKind.Set;
            if (setMethod.ReturnParameter.GetRequiredCustomModifiers().Any(x => x.FullName == "System.Runtime.CompilerServices.IsExternalInit"))
            {
                this.SetterKind = SetMethodKind.Init;
            }
        }
    }

    internal virtual void Validate()
    {
        MethodInfo getMethod = this.PropertyInfo.GetMethod!; // validated in .ctor.
        bool validAccessor = getMethod.IsPublic || !string.IsNullOrEmpty(this.CustomAccessor);

        FlatSharpInternal.Assert(
            validAccessor,
            $"Property {this.DeclaringType} must declare a public getter.");

        FlatSharpInternal.Assert(
            ValidateVirtualPropertyMethod(getMethod, false),
            $"Property {this.DeclaringType} did not declare a public/protected virtual getter.");

        FlatSharpInternal.Assert(
            ValidateVirtualPropertyMethod(this.PropertyInfo.SetMethod, true),
            $"Property {this.DeclaringType} declared a set method, but it was not public/protected and virtual.");
    }

    protected string DeclaringType
    {
        get
        {
            string declaringType = "";
            if (this.PropertyInfo.DeclaringType is not null)
            {
                declaringType = CSharpHelpers.GetCompilableTypeName(this.PropertyInfo.DeclaringType);
            }

            return $"{declaringType}.{this.PropertyInfo.Name} (Index {this.Index})";
        }
    }

    private static bool CanBeOverridden(MethodInfo method)
    {
        // Note: !IsFinal is different than IsVirtual.
        // The difference is that interface implementations cause the "virtual" flag to be set,
        // so a method implementing an interface that is not virtual is both final and virtual.

        // Truth table:
        //                                     | IsVirtual | IsFinal  |  Overridable?
        // ------------------------------------|-----------|----------|--------------
        // NonVirtual Interface implementation |   True    |   True   |   False
        //    Virtual Interface Implementation |   True    |   False  |   True
        //    NonVirtual Method (no interface) |   False   |   False  |   False
        //       Virtual Method (no interface) |   True    |   False  |   True
        // NB: No confirmed examples of Final = True, Virtual = False.
        // Relationship appears to be: Overriddable = Virtual && !Final
        // https://docs.microsoft.com/en-us/dotnet/api/system.reflection.methodbase.isvirtual?view=net-5.0
        return method.IsVirtual && !method.IsFinal;
    }

    private static bool ValidateVirtualPropertyMethod(MethodInfo? method, bool allowNull)
    {
        if (method is null)
        {
            return allowNull;
        }

        FlatSharpInternal.Assert(CanBeOverridden(method), "virtual method expected");

        return method.IsPublic || method.IsFamilyOrAssembly || method.IsFamily;
    }

    /// <summary>
    /// The index of the table member.
    /// </summary>
    public ushort Index { get; }

    /// <summary>
    /// The property info of the table member.
    /// </summary>
    public PropertyInfo PropertyInfo { get; }

    /// <summary>
    /// The actual attribute.
    /// </summary>
    public FlatBufferItemAttribute Attribute { get; }

    /// <summary>
    /// The type model of the item.
    /// </summary>
    public ITypeModel ItemTypeModel { get; }

    /// <summary>
    /// Indicates if this member writes through to the underlying buffer.
    /// </summary>
    public bool IsWriteThrough { get; protected init; }

    /// <summary>
    /// Indicates if this member is required.
    /// </summary>
    public bool IsRequired { get; }

    /// <summary>
    /// A custom getter for this item.
    /// </summary>
    public string? CustomAccessor { get; set; }

    /// <summary>
    /// The semantics of the setter.
    /// </summary>
    public SetMethodKind? SetterKind { get; }

    /// <summary>
    /// Creates a method body to read the given property. This is contextual depending
    /// on whether this member is table/struct/etc.
    /// </summary>
    /// <param name="vtableVariableName">For tables, the <see cref="IVTable"/> variable name.</param>
    public abstract string CreateReadItemBody(
        ParserCodeGenContext context,
        string vtableVariableName);

    /// <summary>
    /// Creates a method body to write the given property back to the buffer. This is contextual depending
    /// on whether this member is table/struct/etc.
    /// </summary>
    /// <param name="vtableVariableName">For tables, the <see cref="IVTable"/> variable name.</param>
    public abstract string CreateWriteThroughBody(
        SerializationCodeGenContext context,
        string vtableVariableName);
}
