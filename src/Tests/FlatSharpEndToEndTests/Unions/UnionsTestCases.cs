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

/*
public class UnionsTestCases
{
    [Fact]
    public void Union_Accept_Works()
    {
        var c = this.Setup();

        UnionVisitor visitor = new();

        Assert.Equal(typeof(A), c.Value[0].Accept<UnionVisitor, Type>(visitor));
        Assert.Equal(typeof(B), c.Value[1].Accept<UnionVisitor, Type>(visitor));
        Assert.Equal(typeof(C), c.Value[2].Accept<UnionVisitor, Type>(visitor));
        Assert.Equal(typeof(D), c.Value[3].Accept<UnionVisitor, Type>(visitor));
    }

    [Fact]
    public void Union_Switch_Func_Works()
    {
        static Type? CallSwitch(MyUnion union)
        {
            return union.Switch(
                caseDefault: () => null,
                caseA: a => typeof(A),
                caseB: b => typeof(B),
                caseC: c => typeof(C),
                caseD: d => typeof(D));
        }

        var c = this.Setup();

        Assert.Equal(typeof(A), CallSwitch(c.Value[0]));
        Assert.Equal(typeof(B), CallSwitch(c.Value[1]));
        Assert.Equal(typeof(C), CallSwitch(c.Value[2]));
        Assert.Equal(typeof(D), CallSwitch(c.Value[3]));
    }

    [Fact]
    public void Union_Switch_Func_WithState_Works()
    {
        static Type? CallSwitch(MyUnion union)
        {
            string? state = null;
            Type? type = union.Switch(
                "foobar",
                caseDefault: (s) => { state = s; return (Type)null; },
                caseA: (s, a) => { state = s; return typeof(A); },
                caseB: (s, b) => { state = s; return typeof(B); },
                caseC: (s, c) => { state = s; return typeof(C); },
                caseD: (s, d) => { state = s; return typeof(D); });

            Assert.Equal("foobar", state);
            return type;
        }

        var c = this.Setup();

        Assert.Equal(typeof(A), CallSwitch(c.Value[0]));
        Assert.Equal(typeof(B), CallSwitch(c.Value[1]));
        Assert.Equal(typeof(C), CallSwitch(c.Value[2]));
        Assert.Equal(typeof(D), CallSwitch(c.Value[3]));
    }

    [Fact]
    public void Union_Switch_Action_Works()
    {
        static Type? CallSwitch(MyUnion union)
        {
            Type? value = null;

            union.Switch(
                caseDefault: () => { },
                caseA: a => { value = typeof(A); },
                caseB: b => { value = typeof(B); },
                caseC: c => { value = typeof(C); },
                caseD: d => { value = typeof(D); });

            return value;
        }

        var c = this.Setup();

        Assert.Equal(typeof(A), CallSwitch(c.Value[0]));
        Assert.Equal(typeof(B), CallSwitch(c.Value[1]));
        Assert.Equal(typeof(C), CallSwitch(c.Value[2]));
        Assert.Equal(typeof(D), CallSwitch(c.Value[3]));
    }

    [Fact]
    public void Union_Switch_Action_WithState_Works()
    {
        static Type? CallSwitch(MyUnion union)
        {
            string? state = null;
            Type? type = null;

            union.Switch(
                "foobar",
                caseDefault: (s) => { state = s; },
                caseA: (s, a) => { state = s; type = typeof(A); },
                caseB: (s, b) => { state = s; type = typeof(B); },
                caseC: (s, c) => { state = s; type = typeof(C); },
                caseD: (s, d) => { state = s; type = typeof(D); });

            Assert.Equal("foobar", state);
            return type;
        }

        var c = this.Setup();

        Assert.Equal(typeof(A), CallSwitch(c.Value[0]));
        Assert.Equal(typeof(B), CallSwitch(c.Value[1]));
        Assert.Equal(typeof(C), CallSwitch(c.Value[2]));
        Assert.Equal(typeof(D), CallSwitch(c.Value[3]));
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
*/