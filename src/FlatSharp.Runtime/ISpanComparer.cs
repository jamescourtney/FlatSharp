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
 
namespace FlatSharp
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// An analog of IComparer for Spans. The implementation performs comparison consistent with the type the span represents.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ISpanComparer
    {
        /// <summary>
        /// Compares the two spans.
        /// </summary>
        int Compare(bool leftExists, ReadOnlySpan<byte> left, bool rightExists, ReadOnlySpan<byte> right);
    }
}
