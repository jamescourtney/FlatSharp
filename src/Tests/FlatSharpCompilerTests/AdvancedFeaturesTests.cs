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

namespace FlatSharpTests.Compiler
{
    using System;
    using FlatSharp.Compiler;
    using FlatSharp.Compiler.SchemaModel;
    using Xunit;

    public class AdvancedFeaturesTests
    {
        [Fact]
        public void Unsupported_AdvancedFeature_Throws()
        {
            Assert.Throws<InvalidFbsFileException>(
                () => new RootModel(FlatSharp.Compiler.Schema.AdvancedFeatures.DefaultVectorsAndStrings));

            Assert.Throws<InvalidFbsFileException>(
                () => new RootModel(FlatSharp.Compiler.Schema.AdvancedFeatures.DefaultVectorsAndStrings | FlatSharp.Compiler.Schema.AdvancedFeatures.OptionalScalars));

            new RootModel(FlatSharp.Compiler.Schema.AdvancedFeatures.OptionalScalars);
        }
    }
}
