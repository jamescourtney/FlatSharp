namespace FlatSharp.TypeModel.Vectors;

/// <summary>
/// Defines a vector type model for a Unity NativeArray collection.
/// </summary>
public class UnityNativeArrayVectorTypeModel : BaseVectorTypeModel
{
    internal UnityNativeArrayVectorTypeModel(Type vectorType, TypeModelContainer provider) : base(vectorType, provider)
    {
    }

    public override string LengthPropertyName => nameof(Memory<byte>.Length);

    public override Type OnInitialize()
    {
        return this.ClrType.GetGenericArguments()[0];
    }
    
    public override void Validate()
    {
        FlatSharpInternal.Assert(
            this.ClrType.IsGenericType && this.ClrType.GetGenericTypeDefinition().FullName == "Unity.Collections.NativeArray`1",
            "Expecting Unity Native Array");

        var genericArgumentType = this.ClrType.GetGenericArguments()[0];

        if (this.ItemTypeModel.SchemaType != FlatBufferSchemaType.Scalar && this.ItemTypeModel.SchemaType != FlatBufferSchemaType.Struct)
        {
            throw new InvalidFlatBufferDefinitionException(
                $"UnityNativeArray vectors only support scalar or struct generic arguments. Type = {this.GetCompilableTypeName()}.");
        }

        FlatSharpInternal.Assert(this.ItemTypeModel.PhysicalLayout.Length == 1, "Expecting a simple layout");

        if (!genericArgumentType.IsValueType)
        {
            throw new InvalidFlatBufferDefinitionException(
                $"UnityNativeArray vectors only support value types. Type = {this.GetCompilableTypeName()}.");
        }

        base.Validate();
    }

    [ExcludeFromCodeCoverage]
    protected override string CreateLoop(FlatBufferSerializerOptions options, string vectorVariableName, string numberOfItemsVariableName, string expectedVariableName, string body)
    {
        FlatSharpInternal.Assert(false, "Not expecting to do loop get max size for memory vector");
        throw new Exception();
    }

    public override CodeGeneratedMethod CreateParseMethodBody(ParserCodeGenContext context)
    {
        ValidateWriteThrough(
            writeThroughSupported: false,
            this,
            this.typeModelContainer,
            context.AllFieldContexts);

        string alignmentCheck = $"FlatSharpInternal.AssertWellAligned<{this.ItemTypeModel.GetGlobalCompilableTypeName()}>({this.ItemTypeModel.PhysicalLayout[0].Alignment});";

        if (context.Options.GreedyDeserialize)
        {
            string body = $@"
                {alignmentCheck}
                var bufferSpan = {context.InputBufferVariableName}.UnsafeReadSpan<{context.InputBufferTypeName}, {this.ItemTypeModel.GetGlobalCompilableTypeName()}>({context.OffsetVariableName});
                var nativeArray = new NativeArray<{this.ItemTypeModel.GetGlobalCompilableTypeName()}>(bufferSpan.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
                bufferSpan.CopyTo(nativeArray.AsSpan());
                return nativeArray;
            ";

            return new CodeGeneratedMethod(body);
        }
        else
        {
            string body = $@"
                {alignmentCheck}
                if (!buffer.IsPinned)
                {{
                    {typeof(FSThrow).GGCTN()}.{nameof(FSThrow.NotSupported_NativeArray_NonPinned)}();
                }}

                var bufferSpan = {context.InputBufferVariableName}.UnsafeReadSpan<{context.InputBufferTypeName}, {this.ItemTypeModel.GetGlobalCompilableTypeName()}>({context.OffsetVariableName});
                var nativeArray = Unity.Collections.LowLevel.Unsafe.NativeArrayUnsafeUtilityEx.ConvertExistingDataToNativeArray<{this.ItemTypeModel.GetGlobalCompilableTypeName()}>(bufferSpan, Allocator.None);
                #if ENABLE_UNITY_COLLECTIONS_CHECKS
                Unity.Collections.LowLevel.Unsafe.NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref nativeArray, Unity.Collections.LowLevel.Unsafe.AtomicSafetyHandle.GetTempUnsafePtrSliceHandle());
                #endif
                return nativeArray;
            ";

            return new CodeGeneratedMethod(body);
        }
    }

    public override CodeGeneratedMethod CreateSerializeMethodBody(SerializationCodeGenContext context)
    {
        string writeNativeArray = $@"
            {context.SpanWriterVariableName}.UnsafeWriteSpan(
                {context.SpanVariableName},
                {context.ValueVariableName}.AsSpan(),
                {context.OffsetVariableName},
                {this.ItemTypeModel.PhysicalLayout[0].Alignment},
                {context.SerializationContextVariableName});
        ";

        return new CodeGeneratedMethod(writeNativeArray);
    }

    public override CodeGeneratedMethod CreateCloneMethodBody(CloneCodeGenContext context)
    {
        string parameters = $"{context.ItemVariableName}, Unity.Collections.Allocator.Persistent";
        string body = $"return new Unity.Collections.NativeArray<{this.ItemTypeModel.GetCompilableTypeName()}>({parameters});";
        return new CodeGeneratedMethod(body)
        {
            IsMethodInline = true,
        };
    }

    public override string GetDeserializedTypeName(FlatBufferDeserializationOption option, string inputBufferTypeName)
    {
        return this.GetGlobalCompilableTypeName();
    }
}
