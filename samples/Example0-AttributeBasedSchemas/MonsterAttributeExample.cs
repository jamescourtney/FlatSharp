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

namespace Samples.MonsterAttributeExample
{
    using System;
    using System.Collections.Generic;
    using FlatSharp;
    using FlatSharp.Attributes;

    /// <summary>
    /// This sample shows the usage of FlatSharp when declaring types in C#.
    /// </summary>
    /// <remarks>
    /// Based on the monster sample here:
    /// https://google.github.io/flatbuffers/flatbuffers_guide_tutorial.html
    /// 
    /// For FlatSharp to work with your schema, you need to obey the following set of rules:
    /// - All types must be public and externally visible.
    /// - All types must be unsealed.
    /// - All properties decorated by [FlatBufferItem] must be virtual and public. Setters may be omitted, but Getters are required.
    /// - All FlatBufferItem indexes must be unique within the given data type.
    /// - Struct/Table vectors must be defined as IList{T}, IReadOnlyList{T}, or T[].
    /// - Scalar vectors must be defined as either IList{T}, IReadOnlyList{T}, ReadOnlyMemory{T}, or T[]. Scalar vectors of bytes may be defined as Memory{byte}/ReadOnlyMemory{byte}.
    /// - All types must be serializable in FlatBuffers(that is -- you can't throw in an arbitrary C# type).
    /// 
    /// When versioning your schema, you'll need to keep these rules in mind: https://google.github.io/flatbuffers/flatbuffers_guide_writing_schema.html
    /// </remarks>
    public class MonsterAttributeExample
    {
        public static void Run()
        {
            // Make a monster!
            Monster monster = new Monster
            {
                Color = Color.Green,
                Equipped = new FlatBufferUnion<Weapon, string>(new Weapon { Damage = 100, Name = "Master sword" }),
                Friendly = true,
                HP = 932,
                Inventory = new byte[] { 1, 2, 3, 4, 5, },
                Mana = 32,
                Name = "Link",
                Pos = new Vec3 { X = 1.1f, Y = 3.2f, Z = 2.6f },
                Weapons = new List<Weapon> { new Weapon { Damage = 6, Name = "Hook shot" }, new Weapon { Name = "Bow and Arrow", Damage = 37 } },
                Path = new Vec3[]
                {
                    new Vec3 { X = 1f, Y = 2f, Z = 3f },
                    new Vec3 { X = 4f, Y = 5f, Z = 6f },
                    new Vec3 { X = 7f, Y = 8f, Z = 9f }
                }
            };

            // Ask the serializer what the max size necessary to write this particular monster. Different
            // monsters will need different amounts of space.
            // This first call will take a little while because of some dependencies that need to be loaded.
            // What's happening in the background is that FlatSharp is generating C# based on your reflected schema,
            // and then compiling a serializer for you using the C# compiler. Look at some of the other examples with FBS
            // files to avoid this first-run tax.
            int maxSize = FlatBufferSerializer.Default.GetMaxSize(monster);

            byte[] buffer = new byte[maxSize];
            int bytesWritten = FlatBufferSerializer.Default.Serialize(monster, buffer);

            // Looks the same to me!
            Monster parsedMonster = FlatBufferSerializer.Default.Parse<Monster>(buffer.AsSpan().Slice(0, bytesWritten).ToArray());
        }
    }

    /// <summary>
    /// The type of the enum attribute *must* match the underlying type. This is to prevent unintentional 
    /// changes to the underlying type of the enum, which is a binary break.
    /// </summary>
    [FlatBufferEnum(typeof(sbyte))]
    public enum Color : sbyte
    {
        Red = 0,
        Green,
        Blue = 2,
    }

    /// <summary>
    /// FlatBuffer structs are modeled as classes in FlatSharp, since we rely on inheritance. 
    /// Structs can contain only contain scalers and other structs. They are fixed-size objects whose schema 
    /// may not change. Structs are highly efficient to read and write, at the cost of 
    /// versioning the struct. Consider a table with a union of structs to get the best of both worlds.
    /// </summary>
    [FlatBufferStruct]
    public class Vec3 : object
    {
        [FlatBufferItem(0)]
        public virtual float X { get; set; }

        [FlatBufferItem(1)]
        public virtual float Y { get; set; }

        [FlatBufferItem(2)]
        public virtual float Z { get; set; }
    }

    /// <summary>
    /// Tables are general-purpose FlatBuffer objects. Tables can contain structs, vectors, strings, 
    /// unions, and other tables. Tables must have virtual properties, public constructors, 
    /// and inherit directly from system.object.
    /// </summary>
    [FlatBufferTable]
    public class Monster : object
    {
        [FlatBufferItem(0)]
        public virtual Vec3 Pos { get; set; }

        /// <summary>
        /// Be sure to make sure the default value has the right type as the property. Here we cast to short.
        /// </summary>
        [FlatBufferItem(1, DefaultValue = (short)150)]
        public virtual short Mana { get; set; } = 150;

        [FlatBufferItem(2, DefaultValue = (short)100)]
        public virtual short HP { get; set; } = 100;

        [FlatBufferItem(3)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Deprecated properties are not serialized and are skipped when deserializing.
        /// </summary>
        [FlatBufferItem(4, Deprecated = true, DefaultValue = false)]
        public virtual bool Friendly { get; set; } = false;

        /// <summary>
        /// You can create Memory and ReadOnlyMemory vectors of scalars. This provides direct access to the buffer.
        /// </summary>
        [FlatBufferItem(5)]
        public virtual Memory<byte> Inventory { get; set; }

        [FlatBufferItem(6, DefaultValue = Color.Blue)]
        public virtual Color Color { get; set; } = Color.Blue;

        /// <summary>
        /// For tables and struts, you can create vectors of IList, IReadOnlyList, and Array.
        /// </summary>
        [FlatBufferItem(7)]
        public virtual IList<Weapon> Weapons { get; set; }

        /// <summary>
        /// Unions (discriminated unions) are double-wide types that occupy 2 indexes. So this one occupies slots 8 and 9.
        /// A union of types A,B, and C will have exactly one of those set. FlatSharp exposes the FlatBufferUnion{T} classes to assist with this.
        /// Unions can contain other tables, but not scalars or vectors.
        /// </summary>
        [FlatBufferItem(8)]
        public virtual FlatBufferUnion<Weapon, string> Equipped { get; set; }

        /// <summary>
        /// Vectors can also be modeled as arrays.
        /// </summary>
        [FlatBufferItem(10)]
        public virtual Vec3[] Path { get; set; }

        /// <summary>
        /// Optional (nullable) scalars are supported, but may not have a default value besides null.
        /// </summary>
        [FlatBufferItem(11)]
        public virtual double? ManaRegenRate { get; set; } = null;

        /// <summary>
        /// Optional (nullable) enums are supported, but may not have a default value besides null.
        /// </summary>
        [FlatBufferItem(11)]
        public virtual Color? SecondaryColor { get; set; } = null;
    }

    [FlatBufferTable]
    public class Weapon : object
    {
        [FlatBufferItem(0)]
        public virtual string Name { get; set; }

        [FlatBufferItem(1)]
        public virtual short Damage { get; set; }
    }
}
