namespace FlatSharpTests
{
    public class EscapedCharInAttributeTests
    {
        [Fact]
        public void EscapedCharInAttribute_DoubleQuote()
        {
            string schema = "namespace ns;attribute custom;table Foo { Bar : int (custom: \"hello,\\\"world\\\"\"); }";

            var assembly = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());
            var foo = assembly.GetType("ns.Foo");
            var bar = foo.GetProperty("Bar");
            var metaAttr = bar.GetCustomAttribute<FlatBufferMetadataAttribute>();
            Assert.NotNull(metaAttr.Value);
            Assert.Equal("hello,\"world\"", metaAttr.Value.ToString());
        }

        [Fact]
        public void EscapedCharInAttribute_SingleQuote()
        {
            string schema = "namespace ns;attribute custom;table Foo { Bar : int (custom: \"hello,'world'\"); }";

            var assembly = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());
            var foo = assembly.GetType("ns.Foo");
            var bar = foo.GetProperty("Bar");
            var metaAttr = bar.GetCustomAttribute<FlatBufferMetadataAttribute>();
            Assert.NotNull(metaAttr.Value);
            Assert.Equal("hello,'world'", metaAttr.Value.ToString());
        }

        [Fact]
        public void EscapedCharInAttribute_BackSlash()
        {
            string schema = "namespace ns;attribute custom;table Foo { Bar : int (custom: \"hello,\\\\world\"); }";

            var assembly = FlatSharpCompiler.CompileAndLoadAssembly(schema, new());
            var foo = assembly.GetType("ns.Foo");
            var bar = foo.GetProperty("Bar");
            var metaAttr = bar.GetCustomAttribute<FlatBufferMetadataAttribute>();
            Assert.NotNull(metaAttr.Value);
            Assert.Equal("hello,\\world", metaAttr.Value.ToString());
        }
    }
}
