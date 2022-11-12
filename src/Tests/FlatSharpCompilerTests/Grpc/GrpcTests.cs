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

using System.Threading.Channels;

namespace FlatSharpTests.Compiler;

public class GrpcTests
{
    [Fact]
    public void NoPrecompiledSerializer()
    {
        string schema = $@"
                {MetadataHelpers.AllAttributes}
                namespace NoPrecompiledSerializer;

                rpc_service RouteGuide
                {{
                    GetFeature(Point):Point;
                }}

                table Point
                {{
                    latitude:int32;
                    longitude:int32;
                }}";

        var ex = Assert.Throws<InvalidFbsFileException>(() => FlatSharpCompiler.CompileAndLoadAssembly(
            schema,
            new(),
            additionalReferences: new[]
            {
                typeof(Grpc.Core.AsyncClientStreamingCall<,>).Assembly,
                typeof(ChannelReader<>).Assembly,
            }));

        Assert.Contains(
            "RPC call 'NoPrecompiledSerializer.RouteGuide.GetFeature' uses table 'NoPrecompiledSerializer.Point', which does not specify the 'fs_serializer' attribute.",
            ex.Message);
    }
}
