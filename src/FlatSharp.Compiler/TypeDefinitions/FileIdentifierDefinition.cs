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

namespace FlatSharp.Compiler
{
  using System.Collections.Generic;

  internal class FileIdentifierDefinition : BaseSchemaMember
  {
    public FileIdentifierDefinition(string name, BaseSchemaMember parent) : base(name, parent)
    {
    }

    protected override bool SupportsChildren => false;

    protected override void OnWriteCode(CodeWriter writer, CodeWritingPass pass, string forFile, IReadOnlyDictionary<string, string> precompiledSerailizers)
    {
      throw new System.NotImplementedException();
    }

    protected override string OnGetCopyExpression(string source)
    {
      throw new System.NotImplementedException();
    }
  }
}
