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

using FlatSharp.Compiler;
using FlatSharp.Compiler.Schema;
using System.Linq;

/// <summary>
/// </summary>
internal class PathNormalizerSchemaMutator : ISchemaMutator
{
    public void Mutate(
        Schema schema,
        CompilerOptions options,
        List<Func<string, string>> postProcessors)
    {
        foreach (var fbsFile in schema.FbsFiles.Select(x => x.Value))
        {
            fbsFile.FileName = PathHelpers.NormalizePathName(fbsFile.FileName);
        }

        foreach (var @object in schema.Objects)
        {
            @object.DeclarationFile = PathHelpers.NormalizePathName(@object.DeclarationFile);
        }

        foreach (var @enum in schema.Enums)
        {
            @enum.DeclarationFile = PathHelpers.NormalizePathName(@enum.DeclarationFile);
        }

        if (schema.Services is not null)
        {
            foreach (var service in schema.Services)
            {
                service.DeclaringFile = PathHelpers.NormalizePathName(service.DeclaringFile);
            }
        }

        if (schema.RootTable is not null)
        {
            schema.RootTable.DeclarationFile = PathHelpers.NormalizePathName(schema.RootTable.DeclarationFile);
        }
    }
}