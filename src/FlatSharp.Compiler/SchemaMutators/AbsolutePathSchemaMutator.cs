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
using FlatSharp.Compiler.SchemaModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// </summary>
internal class AbsolutePathSchemaMutator : ISchemaMutator
{
    private readonly string rootDirectory;

    public AbsolutePathSchemaMutator(string rootDirectory)
    {
        this.rootDirectory = rootDirectory;
    }

    public void Mutate(
        Schema schema,
        CompilerOptions options,
        List<Func<string, string>> postProcessors)
    {
        foreach (var fbsFile in schema.FbsFiles.Select(x => x.Value))
        {
            fbsFile.FileName = this.NormalizePath(fbsFile.FileName);
        }

        foreach (var @object in schema.Objects)
        {
            @object.DeclarationFile = this.NormalizePath(@object.DeclarationFile);
        }

        foreach (var @enum in schema.Enums)
        {
            @enum.DeclarationFile = this.NormalizePath(@enum.DeclarationFile);
        }

        if (schema.Services is not null)
        {
            foreach (var service in schema.Services)
            {
                service.DeclaringFile = this.NormalizePath(service.DeclaringFile);
            }
        }

        if (schema.RootTable is not null)
        {
            schema.RootTable.DeclarationFile = this.NormalizePath(schema.RootTable.DeclarationFile);
        }
    }

    private string NormalizePath(string fbsPath)
    {
        string[] parts = fbsPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        string path = this.rootDirectory;

        foreach (string part in parts)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && part == ":")
            {
                // flatc bug that seems to include the ':' as part of a windows path.
                continue;
            }

            path = Path.Combine(path, part);
        }

        return PathHelpers.NormalizePathName(path);
    }
}