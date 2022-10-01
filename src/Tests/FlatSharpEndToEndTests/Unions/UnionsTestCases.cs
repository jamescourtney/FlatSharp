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
    public void Union_Accept_Works()
    {
        var c = this.Setup();

        Assert.Equal(typeof(A), c.Value[0].Accept<UnionVisitor, Type>(new UnionVisitor()));
        Assert.Equal(typeof(B), c.Value[1].Accept<UnionVisitor, Type>(new UnionVisitor()));
        Assert.Equal(typeof(C), c.Value[2].Accept<UnionVisitor, Type>(new UnionVisitor()));
        Assert.Equal(typeof(D), c.Value[3].Accept<UnionVisitor, Type>(new UnionVisitor()));
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
                //new MyUnionExtended(new Container()),
            }
        };

        byte[] buffer = new byte[Container.Serializer.GetMaxSize(c)];
        Container.Serializer.Write(buffer, c);

        return Container.Serializer.Parse(buffer);
    }

    private struct UnionVisitor : IFlatBufferUnionVisitor<Type, A, B, C, D>
    {
        public Type Visit(A item)
        {
            return typeof(A);
        }

        public Type Visit(B item)
        {
            return typeof(B);
        }

        public Type Visit(C item)
        {
            return typeof(C);
        }

        public Type Visit(D item)
        {
            return typeof(D);
        }

        public Type VisitUnknown(byte discriminator)
        {
            return null;
        }
    }
}
