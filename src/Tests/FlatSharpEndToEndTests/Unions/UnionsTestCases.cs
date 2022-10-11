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

namespace FlatSharpEndToEndTests.Unions;

public class UnionsTestCases
{
    [Fact]
    public void Custom_Union_Accept_Works()
    {
        var c = this.Setup();

        UnionVisitor visitor = new();

        Assert.Equal(typeof(A), c.Value[0].Accept<UnionVisitor, Type>(visitor));
        Assert.Equal(typeof(B), c.Value[1].Accept<UnionVisitor, Type>(visitor));
        Assert.Equal(typeof(C), c.Value[2].Accept<UnionVisitor, Type>(visitor));
        Assert.Equal(typeof(D), c.Value[3].Accept<UnionVisitor, Type>(visitor));
    }


    [Fact]
    public void Builtin_Union_Accept_Works()
    {
        FlatBufferUnion<A, B, C, D>[] unions = new[]
        {
            new FlatBufferUnion<A, B, C, D>(new A()),
            new FlatBufferUnion<A, B, C, D>(new B()),
            new FlatBufferUnion<A, B, C, D>(new C()),
            new FlatBufferUnion<A, B, C, D>(new D()),
        };

        UnionVisitor visitor = new();

        Assert.Equal(typeof(A), unions[0].Accept<UnionVisitor, Type>(visitor));
        Assert.Equal(typeof(B), unions[1].Accept<UnionVisitor, Type>(visitor));
        Assert.Equal(typeof(C), unions[2].Accept<UnionVisitor, Type>(visitor));
        Assert.Equal(typeof(D), unions[3].Accept<UnionVisitor, Type>(visitor));
    }

    private Container Setup()
    {
        Container c = new Container
        {
            Value = new MyUnion[]
            {
                new MyUnion(new A()),
                new MyUnion(new B()),
                new MyUnion(new C()),
                new MyUnion(new D()),
            }
        };

        byte[] buffer = new byte[Container.Serializer.GetMaxSize(c)];
        Container.Serializer.Write(buffer, c);

        return Container.Serializer.Parse(buffer);
    }

    private struct UnionVisitor : MyUnion.Visitor<Type>
    {
        public Type Visit(A item) => typeof(A);

        public Type Visit(B item) => typeof(B);

        public Type Visit(C item) => typeof(C);

        public Type Visit(D item) => typeof(D);
    }
}