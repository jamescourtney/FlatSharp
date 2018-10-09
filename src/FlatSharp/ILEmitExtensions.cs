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
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>
    /// Extensions to IL.Emit. This is to allow us to emit smaller versions
    /// of common instructions that are contextual on the value.
    /// </summary>
    internal static class ILEmitExtensions
    {
        private const TypeAttributes DefaultClassAttributes = TypeAttributes.AutoLayout | TypeAttributes.AnsiClass | TypeAttributes.Class;
        private const TypeAttributes StaticClass = TypeAttributes.Abstract | TypeAttributes.Sealed;

        public static TypeBuilder CreatePublicStaticType(this ModuleBuilder builder, string name)
        {
            return builder.DefineType(
                name,
                DefaultClassAttributes | StaticClass | TypeAttributes.Public,
                null,
                0);
        }

        public static TypeBuilder CreatePublicSealedType(this ModuleBuilder builder, string name, Type baseClass)
        {
            return builder.DefineType(
                name,
                DefaultClassAttributes | TypeAttributes.Public | TypeAttributes.Sealed,
                baseClass,
                0);
        }

        public static MethodBuilder DefineMethodOverride(this TypeBuilder typeBuilder, MethodInfo toOverride)
        {
            var builder = typeBuilder.DefineMethod(
                toOverride.Name,
                toOverride.Attributes & ~MethodAttributes.NewSlot & ~MethodAttributes.Abstract,
                toOverride.ReturnType,
                toOverride.GetParameters().Select(x => x.ParameterType).ToArray());

            builder.AddAggressiveInlining();
            return builder;
        }

        public static void OverrideProperty(this TypeBuilder typeBuilder, PropertyInfo property, MethodBuilder getMethod, MethodBuilder setMethod)
        {
            var propertyBuilder = typeBuilder.DefineProperty(
                property.Name,
                property.Attributes,
                property.PropertyType,
                new Type[0]);

            propertyBuilder.SetGetMethod(getMethod);

            if (setMethod != null)
            {
                propertyBuilder.SetSetMethod(setMethod);
            }
        }

        public static void AddAggressiveInlining(this MethodBuilder methodBuilder)
        {
            methodBuilder.SetImplementationFlags(methodBuilder.GetMethodImplementationFlags() | MethodImplAttributes.AggressiveInlining);
        }

        public static void EmitInt32Constant(this ILGenerator generator, int value)
        {
            switch (value)
            {
                case -1:
                    generator.Emit(OpCodes.Ldc_I4_M1);
                    break;
                case 0:
                    generator.Emit(OpCodes.Ldc_I4_0);
                    break;
                case 1:
                    generator.Emit(OpCodes.Ldc_I4_1);
                    break;
                case 2:
                    generator.Emit(OpCodes.Ldc_I4_2);
                    break;
                case 3:
                    generator.Emit(OpCodes.Ldc_I4_3);
                    break;
                case 4:
                    generator.Emit(OpCodes.Ldc_I4_4);
                    break;
                case 5:
                    generator.Emit(OpCodes.Ldc_I4_5);
                    break;
                case 6:
                    generator.Emit(OpCodes.Ldc_I4_6);
                    break;
                case 7:
                    generator.Emit(OpCodes.Ldc_I4_7);
                    break;
                case 8:
                    generator.Emit(OpCodes.Ldc_I4_8);
                    break;
                default:
                    if (value <= sbyte.MaxValue && value >= sbyte.MinValue)
                    {
                        generator.Emit(OpCodes.Ldc_I4_S, (sbyte)value);
                    }
                    else
                    {
                        generator.Emit(OpCodes.Ldc_I4, value);
                    }
                    break;
            }
        }

        public static void EmitStLoc(this ILGenerator generator, LocalBuilder localBuilder)
        {
            switch (localBuilder.LocalIndex)
            {
                case 0:
                    generator.Emit(OpCodes.Stloc_0);
                    break;
                case 1:
                    generator.Emit(OpCodes.Stloc_1);
                    break;
                case 2:
                    generator.Emit(OpCodes.Stloc_2);
                    break;
                case 3:
                    generator.Emit(OpCodes.Stloc_3);
                    break;
                default:
                    if (localBuilder.LocalIndex <= byte.MaxValue)
                    {
                        generator.Emit(OpCodes.Stloc_S, (byte)localBuilder.LocalIndex);
                    }
                    else
                    {
                        generator.Emit(OpCodes.Stloc, localBuilder.LocalIndex);
                    }
                    break;
            }
        }

        public static void EmitLdLoc(this ILGenerator generator, LocalBuilder localBuilder)
        {
            switch (localBuilder.LocalIndex)
            {
                case 0:
                    generator.Emit(OpCodes.Ldloc_0);
                    break;
                case 1:
                    generator.Emit(OpCodes.Ldloc_1);
                    break;
                case 2:
                    generator.Emit(OpCodes.Ldloc_2);
                    break;
                case 3:
                    generator.Emit(OpCodes.Ldloc_3);
                    break;
                default:
                    if (localBuilder.LocalIndex <= byte.MaxValue)
                    {
                        generator.Emit(OpCodes.Ldloc_S, (byte)localBuilder.LocalIndex);
                    }
                    else
                    {
                        generator.Emit(OpCodes.Ldloc, localBuilder.LocalIndex);
                    }
                    break;
            }
        }

        public static void EmitLdArg(this ILGenerator generator, int index)
        {
            switch (index)
            {
                case 0:
                    generator.Emit(OpCodes.Ldarg_0);
                    break;
                case 1:
                    generator.Emit(OpCodes.Ldarg_1);
                    break;
                case 2:
                    generator.Emit(OpCodes.Ldarg_2);
                    break;
                case 3:
                    generator.Emit(OpCodes.Ldarg_3);
                    break;
                default:
                    if (index <= byte.MaxValue)
                    {
                        generator.Emit(OpCodes.Ldarg_S, (byte)index);
                    }
                    else
                    {
                        generator.Emit(OpCodes.Ldarg, index);
                    }
                    break;
            }
        }

        public static void EmitLdLoca(this ILGenerator generator, LocalBuilder localBuilder)
        {
            if (localBuilder.LocalIndex <= byte.MaxValue)
            {
                generator.Emit(OpCodes.Ldloca_S, localBuilder.LocalIndex);
            }
            else
            {
                generator.Emit(OpCodes.Ldloca, localBuilder.LocalIndex);
            }
        }

        public static void EmitMethodCall(this ILGenerator generator, MethodInfo method)
        {
            if (method.IsStatic)
            {
                generator.Emit(OpCodes.Call, method);
            }
            else
            {
                generator.Emit(OpCodes.Callvirt, method);
            }
        }
    }
}
