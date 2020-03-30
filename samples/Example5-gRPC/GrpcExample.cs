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

            var client = new StatsService.StatsServiceClient(new Channel("127.0.0.1", 50001, ChannelCredentials.Insecure));

            // Three different ways of computing the average of [1,2,3,4]. We're pretty sure it's 2.5 at this point.

            // Unary operations take one request and send one response.
            // Our operation includes the whole array of data.
            UnaryOperation(client).GetAwaiter().GetResult();

            // Client streaming operations let the client send many requests and the server sends one response.
            // The client streams the individual numbers it wants averaged, and the server sends the aggregated response at the end.
            ClientStreamingOperation(client).GetAwaiter().GetResult();

            // Duplex streaming operations let the client send a stream of requests and the server send a stream of responses.
            // We send the cumulative average after each request in this example.
            DuplexStreamingOperation(client).GetAwaiter().GetResult();

            // Server streaming operations are the opposite of client streaming. One client request results in a stream of server responses.
            // However, this is left as an exercise to the reader.

            Thread.Sleep(1000);

            server.ShutdownAsync().GetAwaiter().GetResult();
        }

        private static async Task UnaryOperation(StatsService.StatsServiceClient client)
        {
            AverageResponse response = await client.SingleOperation(new BulkNumbers { Items = new[] { 1d, 2d, 3d, 4d } });
            Console.WriteLine("[Unary] Average is: " + response.Average);
        }

        private static async Task ClientStreamingOperation(StatsService.StatsServiceClient client)
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

        private static async Task DuplexStreamingOperation(StatsService.StatsServiceClient client)
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

        private class ServerImpl : StatsService.StatsServiceServerBase
        {
            public override Task<AverageResponse> SingleOperation(BulkNumbers request, ServerCallContext callContext)
            {
                return Task.FromResult(new AverageResponse
                {
                    Average = request.Items.Average()
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
