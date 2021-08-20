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
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Thrown when FlatSharp.Compiler encounters an error in an FBS file.
    /// </summary>
    public class InvalidFbsFileException : Exception
    {
        public InvalidFbsFileException(IEnumerable<string> errors) : base("Errors in FBS schema: \r\n" + string.Join("\r\n", errors))
        {
            this.Errors = errors.ToArray();
        }

        public InvalidFbsFileException(string error) : this(new[] { error })
        {
        }

        public string[] Errors { get; }
    }
}
