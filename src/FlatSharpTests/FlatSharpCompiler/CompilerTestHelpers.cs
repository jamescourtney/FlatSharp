namespace FlatSharpTests.Compiler
{
    using System;
    using FlatSharp;

    public static class CompilerTestHelpers
    {
        public static readonly FlatBufferSerializer CompilerTestSerializer = new FlatBufferSerializer(
            new FlatBufferSerializerOptions { EnableAppDomainInterceptOnAssemblyLoad = true });
    }
}
