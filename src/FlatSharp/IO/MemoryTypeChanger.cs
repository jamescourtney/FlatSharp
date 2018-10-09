/*
 * Copyright 2018 James Courtney
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
    using System.Buffers;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Provides a safe utility for changing the type of Memory{byte} to Memory{T}.
    /// </summary>
    internal class MemoryTypeChanger<T> : MemoryManager<T> where T : struct
    {
        private readonly Memory<byte> innerMemory;
        private MemoryHandle handle;

        public MemoryTypeChanger(Memory<byte> innerMemory)
        {
            this.innerMemory = innerMemory;
            this.handle = this.innerMemory.Pin();
        }

        ~MemoryTypeChanger()
        {
            this.Dispose(false);
        }

        public override Span<T> GetSpan()
        {
            return MemoryMarshal.Cast<byte, T>(this.innerMemory.Span);
        }

        public override MemoryHandle Pin(int elementIndex = 0)
        {
            // Don't track anything ourselves; only forward pins and unpins to the underlying memory.
            return this.innerMemory.Pin();
        }

        public override void Unpin()
        {
            // shouldn't ever happen, since all .Pin calls are forwarded to our inner implemention
            // which means that those unpins will be directed to the underlying item as well.
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            this.handle.Dispose();
            this.handle = default;
        }
    }
}
