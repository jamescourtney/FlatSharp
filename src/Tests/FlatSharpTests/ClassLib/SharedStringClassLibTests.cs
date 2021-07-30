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
    using FlatSharp;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SharedStringClassLibTests
    {
        [TestMethod]
        public void SharedString_Equality()
        {
            foreach (var value in new[] { "foo", null })
            {
                string a = value;
                string b = value;

                Assert.AreEqual(a, b);
                Assert.AreEqual((SharedString)a, b);
                Assert.IsTrue((SharedString)a == b);
                Assert.IsTrue(a == (SharedString)b);
                Assert.IsTrue((SharedString)a == (SharedString)b);
                Assert.AreEqual<SharedString>(a, b);

                if (b is not null)
                {
                    Assert.IsTrue(((SharedString)b).Equals(a));
                }
            }
        }

        [TestMethod]
        public void SharedString_Inequality()
        {
            string[] values = new[] { "foo", null };

            foreach (var a in values)
            {
                foreach (var b in values)
                {
                    if ((string)a == (string)b)
                    {
                        continue;
                    }

                    Assert.AreNotEqual(a, b);
                    Assert.AreNotEqual((SharedString)a, b);
                    Assert.IsTrue((SharedString)a != b);
                    Assert.IsTrue(a != (SharedString)b);
                    Assert.IsTrue((SharedString)a != (SharedString)b);
                    Assert.AreNotEqual<SharedString>(a, b);

                    if (b is not null)
                    {
                        Assert.IsFalse(((SharedString)b).Equals(a));
                        Assert.IsFalse(((SharedString)b).Equals(new List<int>()));
                    }
                }
            }
        }
    }
}
