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

using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;

namespace Samples.GrpcExample;

/// <summary>
/// This sample shows the implementation and usage of a simple FlatBuffers gRPC service.
/// </summary>
public class GrpcExample : IFlatSharpSample
{
    public void Run()
    {
        {
            // The echo service often returns back the same item that it received. We should configure it to enable shared strings
            // and other optimizations.
            Action<SerializerSettings> settingsConfig = s =>
                s.UseMemoryCopySerialization()
                 .UseLazyDeserialization()
                 .UseDefaultSharedStringWriter();

            // Update the serializers used for the echo service based on what we configured.
            EchoService.Serializer<SingleMessage>.Value = EchoService.Serializer<SingleMessage>.Value.WithSettings(settingsConfig);
            EchoService.Serializer<MultiMessage>.Value = EchoService.Serializer<MultiMessage>.Value.WithSettings(settingsConfig);
        }

        Server server = new Server();
        server.Ports.Add("127.0.0.1", 50001, ServerCredentials.Insecure);
        server.Services.Add(EchoService.BindService(new ServerImpl()));
        server.Start();

        Thread.Sleep(1000);

        EchoService.EchoServiceClient client = new(new Channel("127.0.0.1", 50001, ChannelCredentials.Insecure));

        // We can use the client in two different ways:
        // 1) as a traditional gRPC client:
        GrpcOperations(client).GetAwaiter().GetResult();

        // 2) As an async interface with Channel<T>
        InterfaceOperations(client).GetAwaiter().GetResult();

        Thread.Sleep(1000);

        server.ShutdownAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// This example uses the Grpc operations on the stats service client. The Grpc methods allow specifying
    /// Grpc Headers and are tightly coupled with the Grpc interfaces.
    /// </summary>
    private static async Task GrpcOperations(EchoService.EchoServiceClient client)
    {
        // Unary operation
        {
            SingleMessage unaryResponse = await client.EchoUnary(new SingleMessage { Message = "hi there" }, default);
            Console.WriteLine($"EchoUnary: {unaryResponse.Message}");
        }

        // Client streaming
        {
            AsyncClientStreamingCall<SingleMessage, MultiMessage> call = client.EchoClientStreaming(default);

            await call.RequestStream.WriteAsync(new SingleMessage { Message = "foo" });
            await call.RequestStream.WriteAsync(new SingleMessage { Message = "bar" });
            await call.RequestStream.WriteAsync(new SingleMessage { Message = "baz" });
            await call.RequestStream.WriteAsync(new SingleMessage { Message = "bat" });

            await call.RequestStream.CompleteAsync();
            MultiMessage response = await call.ResponseAsync;

            Console.WriteLine("EchoClientStreaming: " + string.Join(", ", response.Message));
        }

        // Duplex Streaming
        {
            AsyncDuplexStreamingCall<SingleMessage, SingleMessage> call = client.EchoDuplex(default);

            for (int i = 1; i < 5; ++i)
            {
                await call.RequestStream.WriteAsync(new SingleMessage { Message = $"Item{i}" });

                if (await call.ResponseStream.MoveNext())
                {
                    SingleMessage response = call.ResponseStream.Current;
                    Console.WriteLine("[Duplex Streaming] Received: " + response.Message);
                }
                else
                {
                    // Error
                    return;
                }
            }

            await call.RequestStream.CompleteAsync();
        }
    }

    /// <summary>
    /// Instead of using the tightly-bound gRPC methods, we can instead use the service
    /// interface that FlatSharp generates. <see cref="EchoService.EchoServiceClient"/> implements <see cref="IEchoService"/>.
    /// </summary>
    private static async Task InterfaceOperations(IEchoService echoService)
    {
        // Unary operation
        {
            SingleMessage unaryResponse = await echoService.EchoUnary(new SingleMessage { Message = "hi there" }, default);
            Console.WriteLine($"EchoUnary: {unaryResponse.Message}");
        }

        // Client streaming
        {
            // to limit the rate of requests, you can instead create a bounded channel.
            var requestChannel = System.Threading.Channels.Channel.CreateUnbounded<SingleMessage>();

            Task<MultiMessage> responseTask = echoService.EchoClientStreaming(requestChannel.Reader, CancellationToken.None);

            await requestChannel.Writer.WriteAsync(new SingleMessage { Message = "foo" });
            await requestChannel.Writer.WriteAsync(new SingleMessage { Message = "bar" });
            await requestChannel.Writer.WriteAsync(new SingleMessage { Message = "baz" });
            await requestChannel.Writer.WriteAsync(new SingleMessage { Message = "bat" });

            // Indicate that the channel won't ever have anything else written to it. This will cause the async call to complete.
            // if the channel writer is not completed, the responseTask will never complete.
            requestChannel.Writer.Complete();

            MultiMessage response = await responseTask;
            Console.WriteLine("EchoClientStreaming: " + string.Join(", ", response.Message));
        }

        // Server Streaming
        {
            // This creates a bounded channel that can hold one item. To enable faster processing,
            // you can also create an unbounded channel.
            var responseChannel = System.Threading.Channels.Channel.CreateBounded<SingleMessage>(1);

            MultiMessage request = new()
            {
                Message = new List<string>
                {
                    "foo", "bar", "baz", "bat"
                }
            };

            Task responseTask = echoService.EchoServerStreaming(request, responseChannel.Writer, CancellationToken.None);

            while (await responseChannel.Reader.WaitToReadAsync())
            {
                while (responseChannel.Reader.TryRead(out SingleMessage? message))
                {
                    Console.WriteLine("EchoServerStreaming: " + message.Message);
                }
            }

            await responseTask;
        }

        // Duplex Streaming
        {
            var requestChannel = System.Threading.Channels.Channel.CreateBounded<SingleMessage>(1);
            var responseChannel = System.Threading.Channels.Channel.CreateBounded<SingleMessage>(1);

            Task call = echoService.EchoDuplex(requestChannel.Reader, responseChannel.Writer, CancellationToken.None);

            // To write to the remote server, we write to the request channel. To receive the remote server's 
            // responses, we read from the response channel.
            for (int i = 1; i < 5; ++i)
            {
                await requestChannel.Writer.WriteAsync(new SingleMessage { Message = $"Item{i}" });

                if (await responseChannel.Reader.WaitToReadAsync())
                {
                    SingleMessage response = await responseChannel.Reader.ReadAsync();
                    Console.WriteLine("[Duplex Streaming] Received: " + response.Message);
                }
            }

            // Complete the request channel when we are finished.
            requestChannel.Writer.Complete();
            await call;
        }
    }

    private class ServerImpl : EchoService.EchoServiceServerBase
    {
        public override Task<SingleMessage> EchoUnary(SingleMessage request, ServerCallContext callContext)
        {
            return Task.FromResult(request);
        }

        public override async Task<MultiMessage> EchoClientStreaming(IAsyncStreamReader<SingleMessage> requestStream, ServerCallContext callContext)
        {
            MultiMessage response = new()
            {
                Message = new List<string>()
            };

            while (await requestStream.MoveNext(callContext.CancellationToken))
            {
                response.Message.Add(requestStream.Current.Message);
            }

            return response;
        }

        public override async Task EchoServerStreaming(MultiMessage request, IServerStreamWriter<SingleMessage> responseStream, ServerCallContext callContext)
        {
            foreach (string message in request.Message)
            {
                await responseStream.WriteAsync(new SingleMessage { Message = message });
            }
        }

        public override async Task EchoDuplex(IAsyncStreamReader<SingleMessage> requestStream, IServerStreamWriter<SingleMessage> responseStream, ServerCallContext callContext)
        {
            while (await requestStream.MoveNext(callContext.CancellationToken))
            {
                await responseStream.WriteAsync(requestStream.Current);
            }
        }
    }
}
