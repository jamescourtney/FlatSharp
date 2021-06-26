/*
 * Copyright 2021 James Courtney
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

namespace FlatSharpTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using FlatSharpTests.EchoServiceTests;
    using Grpc.Core;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using SChannel = System.Threading.Channels.Channel;

    [TestClass]
    public class EchoServiceTestCases
    {
        [TestMethod]
        public Task EchoUnary()
        {
            return this.EchoTest(async client =>
            {
                var response = await client.EchoUnary(new StringMessage { Value = "hi" });
                Assert.AreEqual("hi", response.Value);
            });
        }

        [TestMethod]
        public Task EchoUnary_Interface()
        {
            return this.EchoTest_Interface(async client =>
            {
                var response = await client.EchoUnary(new StringMessage { Value = "hi" }, CancellationToken.None);
                Assert.AreEqual("hi", response.Value);
            });
        }

        [TestMethod]
        public Task EchoUnary_Interface_Cancelled()
        {
            return this.EchoTest_Interface(async client =>
            {
                var source = new CancellationTokenSource();
                source.Cancel();

                await Assert.ThrowsExceptionAsync<TaskCanceledException>(
                    () => client.EchoUnary(new StringMessage { Value = "hi" }, source.Token));
            });
        }

        [TestMethod]
        public Task EchoClientStreaming()
        {
            return this.EchoTest(async client =>
            {
                var streamingCall = client.EchoClientStreaming();

                List<string> messages = new();
                for (int i = 0; i < 100; ++i)
                {
                    string msg = Guid.NewGuid().ToString();
                    await streamingCall.RequestStream.WriteAsync(
                        new StringMessage { Value = msg });

                    messages.Add(msg);
                }

                await streamingCall.RequestStream.CompleteAsync();

                MultiStringMessage response = await streamingCall;

                Assert.AreEqual(100, response.Value.Count);
                for (int i = 0; i < 100; ++i)
                {
                    Assert.AreEqual(messages[i], response.Value[i]);
                }
            });
        }

        [TestMethod]
        public Task EchoClientStreaming_Interface()
        {
            return this.EchoTest_Interface(async client =>
            {
                var requestChannel = SChannel.CreateUnbounded<StringMessage>();

                List<string> messages = new();
                for (int i = 0; i < 100; ++i)
                {
                    string msg = Guid.NewGuid().ToString();
                    await requestChannel.Writer.WriteAsync(
                        new StringMessage { Value = msg });
                    messages.Add(msg);
                }

                requestChannel.Writer.Complete();

                MultiStringMessage response = await client.EchoClientStreaming(requestChannel, CancellationToken.None);

                Assert.AreEqual(100, response.Value.Count);
                for (int i = 0; i < 100; ++i)
                {
                    Assert.AreEqual(messages[i], response.Value[i]);
                }
            });
        }

        [TestMethod]
        public Task EchoClientStreaming_Interface_Cancelled()
        {
            return this.EchoTest_Interface(async client =>
            {
                CancellationTokenSource cts = new();
                var requestChannel = SChannel.CreateUnbounded<StringMessage>();
                await requestChannel.Writer.WriteAsync(new StringMessage { Value = "foo" });
                await requestChannel.Writer.WriteAsync(new StringMessage { Value = "bar" });

                var responseTask = client.EchoClientStreaming(requestChannel, cts.Token);
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                cts.Cancel();

                await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => responseTask);
            });
        }

        [TestMethod]
        public Task EchoServerStreaming()
        {
            return this.EchoTest(async client =>
            {
                MultiStringMessage message = new MultiStringMessage
                {
                    Value = Enumerable.Range(0, 100).Select(x => Guid.NewGuid().ToString()).ToList()
                };

                var streamingCall = client.EchoServerStreaming(message);
                int i = 0;
                while (await streamingCall.ResponseStream.MoveNext())
                {
                    Assert.AreEqual(message.Value[i++], streamingCall.ResponseStream.Current.Value);
                }

                Assert.AreEqual(100, i);
            });
        }

        [TestMethod]
        public Task EchoDuplexStreaming()
        {
            return this.EchoTest(async client =>
            {
                var duplexCall = client.EchoDuplexStreaming();
                for (int i = 0; i < 100; ++i)
                {
                    string str = Guid.NewGuid().ToString();
                    await duplexCall.RequestStream.WriteAsync(new StringMessage { Value = str });

                    await duplexCall.ResponseStream.MoveNext();
                    Assert.AreEqual(str, duplexCall.ResponseStream.Current.Value);
                }

                await duplexCall.RequestStream.CompleteAsync();
            });
        }

        private async Task EchoTest(Func<EchoService.EchoServiceClient, Task> callback)
        {
            Server server = new Server();
            try
            {
                server.Services.Add(EchoService.BindService(new EchoServer()));
                server.Ports.Add(new ServerPort("127.0.0.1", 0, ServerCredentials.Insecure));
                server.Start();

                int port = server.Ports.Single().BoundPort;

                EchoService.EchoServiceClient client = new EchoService.EchoServiceClient(new Channel("127.0.0.1", port, ChannelCredentials.Insecure));
                await callback(client);
            }
            finally
            {
                await server.KillAsync();
            }
        }

        private Task EchoTest_Interface(Func<EchoService.IEchoService, Task> callback)
        {
            return this.EchoTest(client => callback(client));
        }

        private class EchoServer : EchoService.EchoServiceServerBase
        {
            public override async Task EchoDuplexStreaming(IAsyncStreamReader<StringMessage> requestStream, IServerStreamWriter<StringMessage> responseStream, ServerCallContext callContext)
            {
                while (await requestStream.MoveNext(callContext.CancellationToken))
                {
                    await responseStream.WriteAsync(requestStream.Current);
                }
            }

            public override Task<StringMessage> EchoUnary(StringMessage request, ServerCallContext callContext)
            {
                return Task.FromResult(request);
            }

            public override async Task EchoServerStreaming(MultiStringMessage request, IServerStreamWriter<StringMessage> responseStream, ServerCallContext callContext)
            {
                foreach (var item in request.Value)
                {
                    await responseStream.WriteAsync(new StringMessage { Value = item });
                }
            }

            public override async Task<MultiStringMessage> EchoClientStreaming(IAsyncStreamReader<StringMessage> requestStream, ServerCallContext callContext)
            {
                List<string> messages = new List<string>();
                while (await requestStream.MoveNext(callContext.CancellationToken))
                {
                    messages.Add(requestStream.Current.Value);
                }

                return new MultiStringMessage { Value = messages };
            }
        }
    }
}