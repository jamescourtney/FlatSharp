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

namespace Samples.GrpcExample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Grpc.Core;

    /// <summary>
    /// This sample shows the implementation and usage of a simple FlatBuffers GRPC service.
    /// </summary>
    public class GrpcExample
    {
        public static void Run()
        {
            Server server = new Server();
            server.Ports.Add("127.0.0.1", 50001, ServerCredentials.Insecure);
            server.Services.Add(StatsService.BindService(new ServerImpl()));
            server.Start();

            Thread.Sleep(1000);

            StatsService.StatsServiceClient client = new StatsService.StatsServiceClient(new Channel("127.0.0.1", 50001, ChannelCredentials.Insecure));

            // We can use the client in two different ways:
            // 1) as a traditional gRPC client:
            GrpcOperations(client).GetAwaiter().GetResult();

            // 2) As an async interface with Channel<T>
            InterfaceOperations((IStatsService)client).GetAwaiter().GetResult();

            Thread.Sleep(1000);

            server.ShutdownAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// This example uses the Grpc operations on the stats service client. The Grpc methods allow specifying
        /// Grpc Headers and are tightly coupled with the Grpc interfaces.
        /// </summary>
        private static async Task GrpcOperations(StatsService.StatsServiceClient client)
        {
            // Unary operation
            {
                AverageResponse response = await client.SingleOperation(new BulkNumbers { Items = new[] { 1d, 2d, 3d, 4d } });
                Console.WriteLine("[Unary] Average is: " + response.Average);
            }

            // Client streaming
            {
                AsyncClientStreamingCall<SingleNumber, AverageResponse> call = client.AverageStreaming(default);

                await call.RequestStream.WriteAsync(new SingleNumber { Item = 1d });
                await call.RequestStream.WriteAsync(new SingleNumber { Item = 2d });
                await call.RequestStream.WriteAsync(new SingleNumber { Item = 3d });
                await call.RequestStream.WriteAsync(new SingleNumber { Item = 4d });

                await call.RequestStream.CompleteAsync();
                AverageResponse response = await call.ResponseAsync;

                Console.WriteLine("[Client Streaming] Average is: " + response.Average);
            }

            // Duplex Streaming
            {
                AsyncDuplexStreamingCall<SingleNumber, AverageResponse> call = client.DuplexAverage(default);

                for (int i = 1; i < 5; ++i)
                {
                    await call.RequestStream.WriteAsync(new SingleNumber { Item = i });

                    if (await call.ResponseStream.MoveNext())
                    {
                        AverageResponse response = call.ResponseStream.Current;
                        Console.WriteLine("[Duplex Streaming] Current average is: " + response.Average);
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
        /// interface that flatsharp generates. <see cref="StatsService.StatsServiceClient"/> implements <see cref="IStatsService"/>.
        /// </summary>
        /// <param name="statsService"></param>
        /// <returns></returns>
        private static async Task InterfaceOperations(IStatsService statsService)
        {
            // Unary operation
            {
                AverageResponse response = await statsService.SingleOperation(new BulkNumbers { Items = new[] { 1d, 2d, 3d, 4d } }, CancellationToken.None);
                Console.WriteLine("[Unary] Average is: " + response.Average);
            }

            // Client streaming
            {
                // to limit the rate of requests, you can instead create a bounded channel.
                var requestChannel = System.Threading.Channels.Channel.CreateUnbounded<SingleNumber>();

                Task<AverageResponse> responseTask = statsService.AverageStreaming(requestChannel.Reader, CancellationToken.None);

                await requestChannel.Writer.WriteAsync(new SingleNumber { Item = 1d });
                await requestChannel.Writer.WriteAsync(new SingleNumber { Item = 2d });
                await requestChannel.Writer.WriteAsync(new SingleNumber { Item = 3d });
                await requestChannel.Writer.WriteAsync(new SingleNumber { Item = 4d });

                // Indicate that the channel won't ever have anything else written to it. This will cause the async call to complete.
                // if the channel writer is not completed, the responseTask will never complete.
                requestChannel.Writer.Complete();

                AverageResponse response = await responseTask;
                Console.WriteLine("[Client Streaming] Average is: " + response.Average);
            }

            // Duplex Streaming
            {
                var requestChannel = System.Threading.Channels.Channel.CreateBounded<SingleNumber>(1);
                var responseChannel = System.Threading.Channels.Channel.CreateBounded<AverageResponse>(1);

                Task call = statsService.DuplexAverage(requestChannel.Reader, responseChannel.Writer, CancellationToken.None);

                // To write to the remote server, we write to the request channel. To receive the remote server's 
                // responses, we read from the response channel.
                for (int i = 1; i < 5; ++i)
                {
                    await requestChannel.Writer.WriteAsync(new SingleNumber { Item = i });

                    if (await responseChannel.Reader.WaitToReadAsync())
                    {
                        AverageResponse response = await responseChannel.Reader.ReadAsync();
                        Console.WriteLine("[Duplex Streaming] Current average is: " + response.Average);
                    }
                }

                // Complete the request channel when we are finished.
                requestChannel.Writer.Complete();
                await call;
            }
        }

        private class ServerImpl : StatsService.StatsServiceServerBase
        {
            public override Task<AverageResponse> SingleOperation(BulkNumbers request, ServerCallContext callContext)
            {
                return Task.FromResult(new AverageResponse
                {
                    Average = request.Items is not null ? request.Items.Average() : 0,
                });
            }

            public override async Task<AverageResponse> AverageStreaming(IAsyncStreamReader<SingleNumber> requestStream, ServerCallContext callContext)
            {
                double runningSum = 0;
                int runningSamples = 0;
                
                while (await requestStream.MoveNext(default))
                {
                    SingleNumber item = requestStream.Current;

                    runningSum += item.Item;
                    runningSamples++;
                }

                return new AverageResponse
                {
                    Average = runningSum / runningSamples
                };
            }

            public override async Task DuplexAverage(IAsyncStreamReader<SingleNumber> requestStream, IServerStreamWriter<AverageResponse> responseStream, ServerCallContext callContext)
            {
                double runningSum = 0;
                int runningSamples = 0;

                while (await requestStream.MoveNext(default))
                {
                    SingleNumber item = requestStream.Current;

                    runningSum += item.Item;
                    runningSamples++;

                    await responseStream.WriteAsync(
                        new AverageResponse
                        {
                            Average = runningSum / runningSamples
                        });
                }
            }
        }
    }
}
