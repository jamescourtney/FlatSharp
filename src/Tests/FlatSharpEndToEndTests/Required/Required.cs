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
}

