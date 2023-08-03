/*
 * Copyright 2023 James Courtney
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

using System.IO;
using System.Runtime.InteropServices;

namespace FlatSharp.Compiler;

public static class PathHelpers
{
    public static string NormalizePathName(string path)
    {
        FileInfo info = new FileInfo(path);
        FlatSharpInternal.Assert(info.Exists, "expect path to exist: " + path);

        path = info.FullName;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // case-insensitive!
            path = path.ToLowerInvariant();
        }

        return path;
    }
}