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
    using FlatSharp.TypeModel;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;

    internal class ErrorContext : IDisposable
    {
        private static readonly ThreadLocal<ErrorContext> ThreadLocalContext = new ThreadLocal<ErrorContext>(() => new ErrorContext());

        public static ErrorContext Current => ThreadLocalContext.Value;

        private readonly LinkedList<(string scope, int? lineNumber, string? context)> contextStack = new LinkedList<(string, int?, string?)>();
        private readonly List<string> errors = new List<string>();

        private ErrorContext()
        {
        }

        public IEnumerable<string> Errors => this.errors;
                
        public void PushScope(string scope, int? lineNumber = null, string? context = null)
        {
            this.contextStack.AddLast((scope, lineNumber, context));
        }

        public void PopScope()
        {
            this.contextStack.RemoveLast();
        }

        public void ThrowIfHasErrors()
        {
            if (this.Errors.Any())
            {
                throw new InvalidFbsFileException(this.Errors);
            }
        }

        public void WithScope(string scope, Action callback)
        {
            this.WithScope(scope, () =>
            {
                callback();
                return true;
            });
        }
        
        public T? WithScope<T>(string scope, Func<T> callback) where T : notnull
        {
            try
            {
                this.PushScope(scope);
                return callback();
            }
            catch (InvalidFlatBufferDefinitionException ex)
            {
                this.RegisterError(ex.Message);
                return default;
            }
            catch (InvalidFbsFileException)
            {
                throw;
            }
            catch (Exception ex)
            {
                this.RegisterError("Unexpected FlatSharp compiler error: " + ex);
                return default;
            }
            finally
            {
                this.PopScope();
            }
        }

        public void RegisterError(string message)
        {
            if (this.contextStack.Count > 0)
            {
                var top = this.contextStack.Last();
                string scope = string.Join(".", this.contextStack.Select(x => x.scope));
                this.errors.Add($"Message='{message}', Scope={scope}");
            }
            else
            {
                Debug.Assert(false);
                this.errors.Add(message);
            }
        }

        public void Dispose()
        {
            FlatSharpInternal.Assert(this.contextStack.Count == 0, "Context was not fully popped");
            ThreadLocalContext.Value = new ErrorContext();
        }
    }
}
