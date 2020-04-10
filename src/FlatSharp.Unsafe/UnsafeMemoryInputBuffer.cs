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

namespace FlatSharp.Unsafe
{
    using System;
    using System.Buffers;
    using System.Runtime.CompilerServices;
    using System.Text;

    /// <summary>
    /// An unsafe memory input buffer. The underlying memory is pinned for the
    /// lifetime of the <see cref="UnsafeMemoryInputBuffer"/>, so it is not appropriate
    /// to keep these for long periods of time.
    /// </summary>
    public sealed unsafe class UnsafeMemoryInputBuffer : InputBuffer, IDisposable
    {
        private readonly Memory<byte> memory;
        private readonly MemoryHandle pinnedHandle;
        private readonly byte* pointer;
        private readonly int length;

        private bool disposed;

        /// <summary>
        /// Initializes a new <see cref="UnsafeMemoryInputBuffer"/> from the given <see cref="System.Memory{byte}"/> instance. The memory
        /// is pinned in place for the lifetime of this object.
        /// </summary>
        /// <param name="memory"></param>
        public UnsafeMemoryInputBuffer(Memory<byte> memory)
        {
            this.length = memory.Length;
            this.memory = memory;
            this.pinnedHandle = this.memory.Pin();
            this.pointer = (byte*)this.pinnedHandle.Pointer;

            if (!BitConverter.IsLittleEndian)
            {
                throw new InvalidOperationException("UnsafeMemoryInputBuffer only works on little-endian architectures presently. On big-endian systems, ArrayInputBuffer and MemoryInputBuffer will both work.");
            }

            GC.KeepAlive(this); // force GC to care about us until the end of the ctor (once everything has been assigned).
        }

        // don't let memory be pinned indefinitely.
        ~UnsafeMemoryInputBuffer()
        {
            this.Dispose();
        }

        /// <summary>
        /// Disposes the current object and unpins the underlying memory. Once this method is called,
        /// the <see cref="UnsafeMemoryInputBuffer"/> instance is no longer trustworthy. Future attempts
        /// to access this object will throw an <see cref="ObjectDisposedException"/>.
        /// </summary>
        public void Dispose()
        {
            this.disposed = true;
            this.pinnedHandle.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets the length of the buffer.
        /// </summary>
        public override int Length => this.length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override byte ReadByte(int offset)
        {
            checked
            {
                this.EnsureInBounds(offset, sizeof(byte));
                return *(this.pointer + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override double ReadDouble(int offset)
        {
            CheckAlignment(offset, sizeof(double));
            checked
            {
                this.EnsureInBounds(offset, sizeof(double));
                return *(double*)(this.pointer + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override float ReadFloat(int offset)
        {
            CheckAlignment(offset, sizeof(float));
            checked
            {
                this.EnsureInBounds(offset, sizeof(float));
                return *(float*)(this.pointer + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int ReadInt(int offset)
        {
            CheckAlignment(offset, sizeof(int));
            checked
            {
                this.EnsureInBounds(offset, sizeof(int));
                return *(int*)(this.pointer + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override long ReadLong(int offset)
        {
            CheckAlignment(offset, sizeof(long));
            checked
            {
                this.EnsureInBounds(offset, sizeof(long));
                return *(long*)(this.pointer + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sbyte ReadSByte(int offset)
        {
            checked
            {
                this.EnsureInBounds(offset, sizeof(sbyte));
                return *(sbyte*)(this.pointer + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override short ReadShort(int offset)
        {
            CheckAlignment(offset, sizeof(short));
            checked
            {
                this.EnsureInBounds(offset, sizeof(short));
                return *(short*)(this.pointer + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override uint ReadUInt(int offset)
        {
            CheckAlignment(offset, sizeof(uint));
            checked
            {
                this.EnsureInBounds(offset, sizeof(uint));
                return *(uint*)(this.pointer + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override ulong ReadULong(int offset)
        {
            CheckAlignment(offset, sizeof(ulong));
            checked
            {
                this.EnsureInBounds(offset, sizeof(ulong));
                return *(ulong*)(this.pointer + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override ushort ReadUShort(int offset)
        {
            CheckAlignment(offset, sizeof(ushort));
            checked
            {
                this.EnsureInBounds(offset, sizeof(ushort));
                return *(ushort*)(this.pointer + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override Memory<byte> GetByteMemory(int start, int length)
        {
            checked
            {
                this.EnsureInBounds(start, length);
                return this.memory.Slice(start, length);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override ReadOnlyMemory<byte> GetReadOnlyByteMemory(int start, int length)
        {
            return this.GetByteMemory(start, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override string ReadStringProtected(int offset, int byteLength, Encoding encoding)
        {
            checked
            {
                this.EnsureInBounds(offset, byteLength);
                return encoding.GetString(this.pointer + offset, byteLength);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureInBounds(int offset, int size)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(nameof(UnsafeMemoryInputBuffer));
            }

            checked
            {
                if (offset + size >= this.length || offset < 0)
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }
    }
}
