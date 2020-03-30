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
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;

    internal class ErrorContext : IDisposable
    {
        private static readonly ThreadLocal<ErrorContext> ThreadLocalContext = new ThreadLocal<ErrorContext>(() => new ErrorContext());

        public static ErrorContext Current
        {
            get
            {
                var context = ThreadLocalContext.Value;
                if (context == null)
                {
                    context = new ErrorContext();
                    ThreadLocalContext.Value = context;
                }

                return context;
            }
            private set
            {
                if (ThreadLocalContext.Value != null && value != null && value != ThreadLocalContext.Value)
                {
                    throw new InvalidOperationException("Duplicate error contexts on same thread! Was another context not disposed?");
                }

                ThreadLocalContext.Value = value;
            }
        }

        private readonly LinkedList<(string scope, int? lineNumber, string context)> contextStack = new LinkedList<(string, int?, string)>();
        private readonly List<string> errors = new List<string>();

        private ErrorContext()
        {
        }

        public IEnumerable<string> Errors => this.errors;
                
        public void PushScope(string scope, int? lineNumber = null, string context = null)
        {
            this.contextStack.AddLast((scope, lineNumber, context));
        }

        public void PopScope()
        {
            this.contextStack.RemoveLast();
        }

        public void WithScope(string scope, Action callback)
        {
            this.WithScope(scope, () =>
            {
                callback();
                return true;
            });
        }
        
        public T WithScope<T>(string scope, Func<T> callback)
        {
            try
            {
                this.PushScope(scope);
                return callback();
            }
            catch (Exception ex)
            {
                this.RegisterError("Unexpected compiler exception: " + ex);
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

                string lineNumber = string.Empty;
                if (top.lineNumber != null)
                {
                    lineNumber = $", Line={lineNumber}";
                }

                string context = string.Empty;
                if (!string.IsNullOrEmpty(top.context))
                {
                    context = $", Context={top.context}";
                }

                string scope = string.Join(".", this.contextStack.Select(x => x.scope));

                this.errors.Add($"Message='{message}', Scope={scope}{lineNumber}{context}");
            }
            else
            {
                Debug.Assert(false);
                this.errors.Add(message);
            }
        }

        public void Dispose()
        {
            Debug.Assert(this.contextStack.Count == 0);
            Current = null;
        }
    }
}
