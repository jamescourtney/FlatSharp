/*
 * Copyright 2018 James Courtney
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

namespace FlatSharpEndToEndTests.AccessModifiers;

[TestClass]
public class SetterTests
{
    [TestMethod]
    public void NoSetter()
    {
        var method = this.GetSetMethod(nameof(Table.None));
        Assert.IsNull(method);
    }

    [TestMethod]
    public void PublicImplicitSet()
    {
        var method = this.GetSetMethod(nameof(Table.PublicImplicit));
        Assert.IsTrue(method.IsPublic);
        Assert.IsFalse(IsInitMethod(method));
    }

    [TestMethod]
    public void PublicExplicitSet()
    {
        var method = this.GetSetMethod(nameof(Table.Public));
        Assert.IsTrue(method.IsPublic);
        Assert.IsFalse(IsInitMethod(method));
    }

    [TestMethod]
    public void ProtectedSet()
    {
        var method = this.GetSetMethod(nameof(Table.Protected));
        Assert.IsTrue(method.IsFamily);
        Assert.IsFalse(IsInitMethod(method));
    }

    [TestMethod]
    public void ProtectedInternalSet()
    {
        var method = this.GetSetMethod(nameof(Table.ProtectedInternal));
        Assert.IsTrue(method.IsFamilyOrAssembly);
        Assert.IsFalse(IsInitMethod(method));
    }

    [TestMethod]
    public void PublicExplicitInit()
    {
        var method = this.GetSetMethod(nameof(Table.PublicInit));
        Assert.IsTrue(method.IsPublic);
        Assert.IsTrue(IsInitMethod(method));
    }

    [TestMethod]
    public void ProtectedInit()
    {
        var method = this.GetSetMethod(nameof(Table.ProtectedInit));
        Assert.IsTrue(method.IsFamily);
        Assert.IsTrue(IsInitMethod(method));
    }

    [TestMethod]
    public void ProtectedInternalInit()
    {
        var method = this.GetSetMethod(nameof(Table.ProtectedInternalInit));
        Assert.IsTrue(method.IsFamilyOrAssembly);
        Assert.IsTrue(IsInitMethod(method));
    }

    private MethodInfo? GetSetMethod(string name)
    {
        return typeof(Table).GetProperty(name).SetMethod;
    }

    private static bool IsInitMethod(MethodInfo method)
    {
        return method.ReturnParameter.GetRequiredCustomModifiers().Any(x => x.FullName == "System.Runtime.CompilerServices.IsExternalInit");
    }
}

