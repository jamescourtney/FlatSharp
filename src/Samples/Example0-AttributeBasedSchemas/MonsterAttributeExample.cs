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
    /// </remarks>
    public class SchemaFilesExample
    {
        public static void Run()
        {
            // Make a monster!
            Monster monster = new Monster
            {
                Color = Color.Green,
                Equipped = new FlatBufferUnion<Weapon>(new Weapon { Damage = 100, Name = "Master sword" }),
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
    /// The type of the enum attribute *must* match the underlying type. This is to prevent unintentional changes to the underlying type
    /// of the enum, which is a binary break.
    /// </summary>
    [FlatBufferEnum(typeof(sbyte))]
    public enum Color : sbyte
    {
        Red = 0,
        Green,
        Blue = 2,
    }

    /// <summary>
    /// FlatBuffer structs are modeled as classes in FlatSharp. Structs can contain only contain scalers.
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
    /// Tables are general-purpose FlatBuffer objects. Tables can contain structs, vectors, strings, unions, and other tables.
    /// Tables must have virtual methods, public constructors, and inherit directly from system.object.
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
        /// Unions can contain other tables, but not scalars, vectors, or structs.
        /// </summary>
        [FlatBufferItem(8)]
        public virtual FlatBufferUnion<Weapon> Equipped { get; set; }

        /// <summary>
        /// Vectors can also be modeled as arrays.
        /// </summary>
        [FlatBufferItem(10)]
        public virtual Vec3[] Path { get; set; }
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
