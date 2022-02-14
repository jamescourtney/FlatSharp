/*
 * Copyright 2022 James Courtney
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

namespace FlatSharpEndToEndTests.Documentation;

public class DocumentationTestCases
{
    private System.Xml.XmlDocument xmlDoc;

    public DocumentationTestCases()
    {
        this.xmlDoc = new System.Xml.XmlDocument();
        this.xmlDoc.Load("FlatSharpEndToEndTests.xml");
    }

    [Fact]
    public void EnumDocumentation()
    {
        Assert.Equal(
            "CommentTest:73c67946-5c52-4643-baa9-a1a0c0d758d7",
            this.GetSummaryText("T:FlatSharpEndToEndTests.Documentation.TestEnum"));

        Assert.Equal(
            "CommentTest:282048b4-2817-4312-93ec-8c21e8d42f8d",
            this.GetSummaryText("F:FlatSharpEndToEndTests.Documentation.TestEnum.A"));

        Assert.Null(this.GetSummaryText("F:FlatSharpEndToEndTests.Documentation.TestEnum.Uncommented"));
        Assert.Null(this.GetSummaryText("T:FlatSharpEndToEndTests.Documentation.UndocumentedEnum"));
    }

    [Fact]
    public void UnionDocumentation()
    {
        Assert.Equal(
            "CommentTest:d54eef2c-f1d0-497f-874f-eeb0e47588b6",
            this.GetSummaryText("T:FlatSharpEndToEndTests.Documentation.TestUnion"));

        Assert.Null(this.GetSummaryText("T:FlatSharpEndToEndTests.Documentation.UndocumentedUnion"));
    }

    [Fact]
    public void TableDocumentation()
    {
        Assert.Equal(
            "CommentTest:eb9a0f9a-6f4e-4839-be67-e719b411a274",
            this.GetSummaryText("T:FlatSharpEndToEndTests.Documentation.Table"));

        Assert.Equal(
            "CommentTest:41b60a53-10a9-4f4a-9d36-3bf73fb95392",
            this.GetSummaryText("P:FlatSharpEndToEndTests.Documentation.Table.Property"));

        Assert.Equal(
            "<><>",
            this.GetSummaryText("P:FlatSharpEndToEndTests.Documentation.Table.EscapeTest"));

        Assert.Null(this.GetSummaryText("F:FlatSharpEndToEndTests.Documentation.Table.Uncommented"));
        Assert.Null(this.GetSummaryText("T:FlatSharpEndToEndTests.Documentation.UndocumentedTable"));
    }

    [Fact]
    public void RefStructDocumentation()
    {
        Assert.Equal(
            "CommentTest:cb1b9369-c724-4106-b02b-2afe4984cf89",
            this.GetSummaryText("T:FlatSharpEndToEndTests.Documentation.RefStruct"));

        Assert.Equal(
            "CommentTest:95571f1b-c255-4191-a489-2b06ddcae495",
            this.GetSummaryText("P:FlatSharpEndToEndTests.Documentation.RefStruct.Property"));

        Assert.Equal(
            "CommentTest:93efd448-fa30-4ddb-9d3b-56ca3f10b403",
            this.GetSummaryText("P:FlatSharpEndToEndTests.Documentation.RefStruct.Vector"));

        Assert.Null(this.GetSummaryText("P:FlatSharpEndToEndTests.Documentation.RefStruct.UncommentedProperty"));
        Assert.Null(this.GetSummaryText("P:FlatSharpEndToEndTests.Documentation.RefStruct.UncommentedVector"));
        Assert.Null(this.GetSummaryText("T:FlatSharpEndToEndTests.Documentation.UncommentedRefStruct"));
    }

    [Fact]
    public void ValueStructDocumentation()
    {
        Assert.Equal(
            "CommentTest:7a379625-7511-48f9-8b57-34b41c073da8",
            this.GetSummaryText("T:FlatSharpEndToEndTests.Documentation.ValueStruct"));

        Assert.Equal(
            "CommentTest:62d5d69b-b3a0-4446-9abe-d0b314555a4b",
            this.GetSummaryText("F:FlatSharpEndToEndTests.Documentation.ValueStruct.Property"));

        Assert.Equal(
            "CommentTest:30add22e-345d-46e2-a1a2-346043cfd444",
            this.GetSummaryText("M:FlatSharpEndToEndTests.Documentation.ValueStruct.Vector_Item(FlatSharpEndToEndTests.Documentation.ValueStruct@,System.Int32)"));

        Assert.Equal(
            "CommentTest:30add22e-345d-46e2-a1a2-346043cfd444",
            this.GetSummaryText("M:FlatSharpEndToEndTests.Documentation.ValueStruct__FlatSharpExtensions.Vector(FlatSharpEndToEndTests.Documentation.ValueStruct@,System.Int32)"));

        Assert.Null(
            this.GetSummaryText("F:FlatSharpEndToEndTests.Documentation.ValueStruct.UncommentedProperty"));

        Assert.Null(
            this.GetSummaryText("M:FlatSharpEndToEndTests.Documentation.ValueStruct.UncommentedVector_Item(FlatSharpEndToEndTests.Documentation.ValueStruct@,System.Int32)"));

        Assert.Null(
            this.GetSummaryText("M:FlatSharpEndToEndTests.Documentation.ValueStruct__FlatSharpExtensions.UncommentedVector(FlatSharpEndToEndTests.Documentation.ValueStruct@,System.Int32)"));
    }

    [Fact]
    public void RpcServiceDocumentation()
    {
        Assert.Equal(
            "CommentTest:d9232e86-035e-4b86-bc77-a3cb04575ba1",
            this.GetSummaryText("T:FlatSharpEndToEndTests.Documentation.Service"));

        Assert.Equal(
            "CommentTest:d9232e86-035e-4b86-bc77-a3cb04575ba1",
            this.GetSummaryText("T:FlatSharpEndToEndTests.Documentation.Service.ServiceServerBase"));

        Assert.Equal(
            "CommentTest:d9232e86-035e-4b86-bc77-a3cb04575ba1",
            this.GetSummaryText("T:FlatSharpEndToEndTests.Documentation.Service.ServiceClient"));

        Assert.Equal(
            "CommentTest:d9232e86-035e-4b86-bc77-a3cb04575ba1",
            this.GetSummaryText("T:FlatSharpEndToEndTests.Documentation.IService"));

        Assert.Equal(
            "CommentTest:932b099d-1cff-4533-81f3-b0eef65e21f3",
            this.GetSummaryText("M:FlatSharpEndToEndTests.Documentation.IService.MethodCall(FlatSharpEndToEndTests.Documentation.Table,System.Threading.CancellationToken)"));

        Assert.Equal(
            "CommentTest:932b099d-1cff-4533-81f3-b0eef65e21f3",
            this.GetSummaryText("M:FlatSharpEndToEndTests.Documentation.Service.ServiceServerBase.MethodCall(FlatSharpEndToEndTests.Documentation.Table,Grpc.Core.ServerCallContext)"));

        Assert.Equal(
            "CommentTest:932b099d-1cff-4533-81f3-b0eef65e21f3",
            this.GetSummaryText("M:FlatSharpEndToEndTests.Documentation.Service.ServiceClient.MethodCall(FlatSharpEndToEndTests.Documentation.Table,Grpc.Core.Metadata,System.Nullable{System.DateTime},System.Threading.CancellationToken)"));

        Assert.Equal(
            "CommentTest:932b099d-1cff-4533-81f3-b0eef65e21f3",
            this.GetSummaryText("M:FlatSharpEndToEndTests.Documentation.Service.ServiceClient.MethodCall(FlatSharpEndToEndTests.Documentation.Table,Grpc.Core.CallOptions)"));
    }

    private string? GetSummaryText(string name)
    {
        return this.xmlDoc.SelectSingleNode($"//member[@name='{name}']/summary/text()")?.InnerText?.Trim();
    }
}
