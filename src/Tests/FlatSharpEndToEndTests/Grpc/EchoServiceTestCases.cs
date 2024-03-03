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

using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Other.Namespace.Foobar;
using SChannel = System.Threading.Channels.Channel;

namespace FlatSharpEndToEndTests.GrpcTests;

#if NET7_0_OR_GREATER && !AOT
[TestClass]
public class EchoServiceTestCases
{
    [TestMethod]
    public async Task GrpcSerializersOverridable_EnableSharedStrings()
    {
        EchoService.Serializer<MultiStringMessage>.Value = MultiStringMessage.Serializer
            .WithSettings(s => s.UseDefaultSharedStringWriter());

        int length = await this.SharedStringsTest_Common();
        Assert.IsTrue(length <= 2048);
    }

    [TestMethod]
    public async Task GrpcSerializersOverridable_NoSharedStrings()
    {
        try
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => EchoService.Serializer<StringMessage>.Value = null);

            EchoService.Serializer<MultiStringMessage>.Value = MultiStringMessage.Serializer.WithSettings(
                s => s.DisableSharedStrings());

            int length = await this.SharedStringsTest_Common();
            Assert.IsTrue(length >= 100_000);
        }
        finally
        {
            EchoService.Serializer<MultiStringMessage>.Value = MultiStringMessage.Serializer;
        }
    }

    private async Task<int> SharedStringsTest_Common()
    {
        int length = 0;
        await this.EchoTest(async client =>
        {
            var requestStream = SChannel.CreateUnbounded<StringMessage>();

            byte[] random = new byte[1024];
            new Random().NextBytes(random);
            string randomB64 = Convert.ToBase64String(random);

            for (int i = 0; i < 100; ++i)
            {
                await requestStream.Writer.WriteAsync(new StringMessage { Value = randomB64 });
            }

            requestStream.Writer.Complete();

            MultiStringMessage response = await ((IEchoService)client).EchoClientStreaming(requestStream, default);

            // Test that the string is only written once. This confirms that our update to the serializer worked as intended.
            Assert.IsInstanceOfType<IFlatBufferDeserializedObject>(response);

            IFlatBufferDeserializedObject obj = (IFlatBufferDeserializedObject)response;
            length = obj.InputBuffer.Length;
        });

        return length;
    }

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
    public Task EchoUnary_Interface_Canceled()
    {
        return this.EchoTest_Interface(async client =>
        {
            var source = new CancellationTokenSource();
            source.Cancel();

            await this.AssertCanceled(
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
                await streamingCall.RequestStream.WriteAsync(new StringMessage { Value = msg });
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
                await requestChannel.Writer.WriteAsync(new StringMessage { Value = msg });
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
    public Task EchoClientStreaming_Interface_Canceled()
    {
        return this.EchoTest_Interface(async client =>
        {
            CancellationTokenSource cts = new();
            var channel = SChannel.CreateUnbounded<StringMessage>();
            await channel.Writer.WriteAsync(new StringMessage { Value = "foo" });
            await channel.Writer.WriteAsync(new StringMessage { Value = "bar" });

            var responseTask = client.EchoClientStreaming(channel, cts.Token);

            cts.Cancel();

            await this.AssertCanceled(() => responseTask);
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
    public Task EchoServerStreaming_Interface()
    {
        return this.EchoTest_Interface(async client =>
        {
            MultiStringMessage message = new MultiStringMessage
            {
                Value = Enumerable.Range(0, 100).Select(x => Guid.NewGuid().ToString()).ToList()
            };

            var channel = SChannel.CreateUnbounded<StringMessage>();

            var task = client.EchoServerStreaming(message, channel, CancellationToken.None);

            int i = 0;
            while (await channel.Reader.WaitToReadAsync())
            {
                Assert.IsTrue(channel.Reader.TryRead(out var item));
                Assert.AreEqual(message.Value[i++], item.Value);
            }

            Assert.AreEqual(100, i);

            await task;

            Assert.IsTrue(channel.Reader.Completion.IsCompleted);
            Assert.IsFalse(channel.Reader.Completion.IsFaulted);
        });
    }

    /*
    [TestMethod]
    public Task EchoServerStreaming_Interface_Canceled()
    {
        return this.EchoTest_Interface(async client =>
        {
            MultiStringMessage message = new MultiStringMessage
            {
                Value = Enumerable.Range(0, 100).Select(x => Guid.NewGuid().ToString()).ToList()
            };

            var channel = SChannel.CreateUnbounded<StringMessage>();
            CancellationTokenSource cts = new();

            var task = client.EchoServerStreaming(message, channel, cts.Token);

            int i = 0;
            while (await channel.Reader.WaitToReadAsync(cts.Token))
            {
                Assert.IsTrue(channel.Reader.TryRead(out var item));
                Assert.AreEqual(message.Value[i++], item.Value);

                if (i == 50)
                {
                    cts.Cancel();

                    await this.AssertCanceled(
                        async () => await channel.Reader.WaitToReadAsync(cts.Token));

                    break;
                }
            }

            await this.AssertCanceled(() => task);

            Assert.IsTrue(channel.Reader.Completion.IsCompleted);
            Assert.IsFalse(channel.Reader.Completion.IsFaulted);
        });
    }
    */

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

    [TestMethod]
    public Task EchoDuplexStreaming_Interface()
    {
        return this.EchoTest_Interface(async client =>
        {
            var sourceChannel = SChannel.CreateUnbounded<StringMessage>();
            var destChannel = SChannel.CreateUnbounded<StringMessage>();
            var duplexCall = client.EchoDuplexStreaming(sourceChannel.Reader, destChannel.Writer, CancellationToken.None);

            for (int i = 0; i < 100; ++i)
            {
                string str = Guid.NewGuid().ToString();
                await sourceChannel.Writer.WriteAsync(new StringMessage { Value = str });

                var item = await destChannel.Reader.ReadAsync();
                Assert.AreEqual(str, item.Value);
            }

            sourceChannel.Writer.Complete();
            await duplexCall;
            
            Assert.IsFalse(duplexCall.IsFaulted);
        });
    }

    [TestMethod]
    public Task EchoDuplexStreaming_Interface_Canceled_BeforeCompletion()
    {
        return this.EchoTest_Interface(async client =>
        {
            CancellationTokenSource cts = new();

            var sourceChannel = SChannel.CreateUnbounded<StringMessage>();
            var destChannel = SChannel.CreateUnbounded<StringMessage>();

            var duplexCall = client.EchoDuplexStreaming(sourceChannel.Reader, destChannel.Writer, cts.Token);

            for (int i = 0; i < 100; ++i)
            {
                string str = Guid.NewGuid().ToString();
                await sourceChannel.Writer.WriteAsync(new StringMessage { Value = str });

                var item = await destChannel.Reader.ReadAsync();
                Assert.AreEqual(str, item.Value);
            }

            cts.Cancel();
            await Task.Delay(50);
            sourceChannel.Writer.Complete();
            await this.AssertCanceled(() => duplexCall);

            Assert.IsTrue(destChannel.Reader.Completion.IsCompleted);
            Assert.IsTrue(destChannel.Reader.Completion.IsFaulted);
        });
    }

    [TestMethod]
    public Task EchoDuplexStreaming_Interface_Canceled_OnWrite()
    {
        return this.EchoTest_Interface(async client =>
        {
            CancellationTokenSource cts = new();

            var sourceChannel = SChannel.CreateUnbounded<StringMessage>();
            var destChannel = SChannel.CreateUnbounded<StringMessage>();

            var duplexCall = client.EchoDuplexStreaming(sourceChannel.Reader, destChannel.Writer, cts.Token);

            for (int i = 0; i < 100; ++i)
            {
                await sourceChannel.Writer.WriteAsync(new StringMessage { Value = i.ToString() });

                if (i == 50)
                {
                    cts.Cancel();
                    await Task.Delay(50);

                    try
                    {
                        await destChannel.Reader.ReadAsync();
                        await destChannel.Reader.ReadAsync();
                        await destChannel.Reader.ReadAsync();
                        Assert.Fail("Exception not thrown");
                    }
                    catch (System.Threading.Channels.ChannelClosedException)
                    {
                    }
                    catch (TaskCanceledException)
                    {
                    }

                    break;
                }
                else
                {
                    var item = await destChannel.Reader.ReadAsync();
                    Assert.AreEqual(i.ToString(), item.Value);
                }
            }

            await this.AssertCanceled(() => duplexCall);

            Assert.IsTrue(destChannel.Reader.Completion.IsCompleted);
            Assert.IsTrue(destChannel.Reader.Completion.IsCanceled || destChannel.Reader.Completion.IsFaulted);
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

    private Task EchoTest_Interface(Func<IEchoService, Task> callback)
    {
        return this.EchoTest(client => callback(client));
    }

    private async Task AssertCanceled(Func<Task> callback)
    {
        try
        {
            await callback();
            Assert.Fail("Exception was not thrown");
        }
        catch (OperationCanceledException)
        {
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
        {
        }
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

        public override Task<StringMessage> NsTest(Blah request, ServerCallContext callContext)
        {
            return Task.FromResult(new StringMessage { Value = "foo" });
        }
    }
}
#endif