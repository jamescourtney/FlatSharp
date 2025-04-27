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

using System.IO;
using System.Runtime.InteropServices.Marshalling;

namespace FlatSharpEndToEndTests.Required;

[TestClass]
public class RequiredTests
{
    [TestMethod]
    [DataRow("A")]
    [DataRow("B")]
    [DataRow("C")]
    [DataRow("E")]
    public void Serialize_ReferenceFieldNotPresent(string fieldName)
    {
        RequiredTable table = new RequiredTable
        {
            A = string.Empty,
            B = new string[0],
            C = new NonRequiredTable(),
            D = new byte[0],
            E = new RefStruct(),
            F = new ValueStruct(),
        };

        typeof(RequiredTable).GetProperty(fieldName).SetMethod.Invoke(table, new object[] { null });

        var ex = Assert.ThrowsException<InvalidOperationException>(() => table.AllocateAndSerialize());
        Assert.AreEqual(
            $"Table property 'FlatSharpEndToEndTests.Required.RequiredTable.{fieldName}' is marked as required, but was not set.",
            ex.Message);
    }

    [TestMethod]
    [DataRow("D")]
    [DataRow("F")]
    public void Serialize_ValueFields(string fieldName)
    {
        RequiredTable table = new RequiredTable
        {
            A = string.Empty,
            B = new string[0],
            C = new NonRequiredTable(),
            D = new byte[0],
            E = new RefStruct(),
            F = new ValueStruct(),
        };

        PropertyInfo info = typeof(RequiredTable).GetProperty(fieldName);
        Assert.IsTrue(info.PropertyType.IsValueType);
        Assert.IsNull(Nullable.GetUnderlyingType(info.PropertyType));
    }


    [TestMethod]
    [DynamicData(nameof(DynamicDataHelper.DeserializationModes), typeof(DynamicDataHelper))]
    public void Parse(FlatBufferDeserializationOption option)
    {
        void ParseAndUse(string fieldName)
        {
            NonRequiredTable original = new NonRequiredTable();

            if (fieldName != nameof(original.A))
            {
                original.A = string.Empty;
            }

            if (fieldName != nameof(original.B))
            {
                original.B = new string[0];
            }

            if (fieldName != nameof(original.C))
            {
                original.C = new NonRequiredTable();
            }

            if (fieldName != nameof(original.D))
            {
                original.D = new byte[0];
            }

            if (fieldName != nameof(original.E))
            {
                original.E = new RefStruct();
            }

            if (fieldName != nameof(original.F))
            {
                original.F = new ValueStruct();
            }

            byte[] buffer = original.AllocateAndSerialize();

            RequiredTable parsed = RequiredTable.Serializer.Parse(buffer, option);

            _ = parsed.A;
            _ = parsed.B;
            _ = parsed.C;
            _ = parsed.D;
            _ = parsed.E;
            _ = parsed.F;
        }

        for (char c = 'A'; c <= 'F'; ++c)
        {
            var ex = Assert.ThrowsException<InvalidDataException>(() => ParseAndUse(c.ToString()));
            Assert.AreEqual(
                $"Table property 'FlatSharpEndToEndTests.Required.RequiredTable.{c}' is marked as required, but was missing from the buffer.",
                ex.Message);
        }

        // Finally, make sure something doesn't throw:
        ParseAndUse("__");
    }

    [TestMethod]
    [DataRow(nameof(RequiredTable_Setters.Pub), true)]
    [DataRow(nameof(RequiredTable_Setters.PubInit), true)]
    [DataRow(nameof(RequiredTable_Setters.Prot), false)]
    [DataRow(nameof(RequiredTable_Setters.ProtectedInit), false)]
    [DataRow(nameof(RequiredTable_Setters.ProtectedInternal), false)]
    [DataRow(nameof(RequiredTable_Setters.ProtectedInternalInit), false)]
    [DataRow(nameof(RequiredTable_Setters.None), false)]
    [DataRow(nameof(RequiredTable_Setters.PubPartial), true)]
    [DataRow(nameof(RequiredTable_Setters.PubInitPartial), true)]
    [DataRow(nameof(RequiredTable_Setters.ProtPartial), false)]
    [DataRow(nameof(RequiredTable_Setters.ProtectedInitPartial), false)]
    [DataRow(nameof(RequiredTable_Setters.ProtectedInternalPartial), false)]
    [DataRow(nameof(RequiredTable_Setters.ProtectedInternalInitPartial), false)]
    [DataRow(nameof(RequiredTable_Setters.NonePartial), false)]
    public void Only_Public_Setters_Are_CSharp_Required(string propertyName, bool expectRequired)
    {
        PropertyInfo property = typeof(RequiredTable_Setters).GetProperty(propertyName);

#if NET7_0_OR_GREATER
        // Make sure the property is flagged as being required.
        Assert.AreEqual(
            expectRequired,
            typeof(RequiredTable_Setters).GetProperty(propertyName).GetCustomAttributes().Any(x => x.GetType().FullName == "System.Runtime.CompilerServices.RequiredMemberAttribute"));
#endif

        // Now make sure that FlatSharp still throws the error for required property missing even if the C# property is not required.
        RequiredTable_Setters table;
        if (propertyName == nameof(RequiredTable_Setters.None) || propertyName == nameof(RequiredTable_Setters.NonePartial))
        {
            table = new(false);
        }
        else
        {
            table = new(true);

            // Set to null.
            property.SetMethod.Invoke(table, new object[] { null });
        }

        // Serialize and expect error.
        Assert.ThrowsException<InvalidOperationException>(() => RequiredTable_Setters.Serializer.Write(new byte[1024], table));
    }
}

public partial class RequiredTable_Setters
{
#if NET7_0_OR_GREATER
    [SetsRequiredMembers]
#endif
    public RequiredTable_Setters(bool setNone)
    {
        this.Pub = "a";
        this.PubInit = "b";
        this.Prot = "c";
        this.ProtectedInit = "d";
        this.ProtectedInternal = "e";
        this.ProtectedInternalInit = "f";

        this.PubPartial = "a";
        this.PubInitPartial = "b";
        this.ProtPartial = "c";
        this.ProtectedInitPartial = "d";
        this.ProtectedInternalPartial = "e";
        this.ProtectedInternalInit = "f";

        if (setNone)
        {
            this.__None = "g";
            this.__NonePartial = "g";
        }
    }

    required public virtual partial string PubPartial { get; set; }

    required public virtual partial string PubInitPartial { get; init; }

    public virtual partial string ProtPartial { get; protected set; }

    public virtual partial string ProtectedInitPartial { get; protected init; }

    public virtual partial string ProtectedInternalPartial { get; protected internal set; }

    public virtual partial string ProtectedInternalInitPartial { get; protected internal init; }

    public virtual partial string NonePartial { get; }
}

