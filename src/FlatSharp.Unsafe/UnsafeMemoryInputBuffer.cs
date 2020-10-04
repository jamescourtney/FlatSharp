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
    public sealed unsafe class UnsafeMemoryInputBuffer : IInputBuffer, IDisposable
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
        public int Length => this.length;

        public ISharedStringReader SharedStringReader { get; set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ReadByte(int offset)
        {
            checked
            {
                this.EnsureInBounds(offset, sizeof(byte));
                return *(this.pointer + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double ReadDouble(int offset)
        {
            this.CheckAlignment(offset, sizeof(double));
            checked
            {
                this.EnsureInBounds(offset, sizeof(double));
                return *(double*)(this.pointer + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ReadFloat(int offset)
        {
            this.CheckAlignment(offset, sizeof(float));
            checked
            {
                this.EnsureInBounds(offset, sizeof(float));
                return *(float*)(this.pointer + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt(int offset)
        {
            this.CheckAlignment(offset, sizeof(int));
            checked
            {
                this.EnsureInBounds(offset, sizeof(int));
                return *(int*)(this.pointer + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ReadLong(int offset)
        {
            this.CheckAlignment(offset, sizeof(long));
            checked
            {
                this.EnsureInBounds(offset, sizeof(long));
                return *(long*)(this.pointer + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sbyte ReadSByte(int offset)
        {
            checked
            {
                this.EnsureInBounds(offset, sizeof(sbyte));
                return *(sbyte*)(this.pointer + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public short ReadShort(int offset)
        {
            this.CheckAlignment(offset, sizeof(short));
            checked
            {
                this.EnsureInBounds(offset, sizeof(short));
                return *(short*)(this.pointer + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ReadUInt(int offset)
        {
            this.CheckAlignment(offset, sizeof(uint));
            checked
            {
                this.EnsureInBounds(offset, sizeof(uint));
                return *(uint*)(this.pointer + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ReadULong(int offset)
        {
            this.CheckAlignment(offset, sizeof(ulong));
            checked
            {
                this.EnsureInBounds(offset, sizeof(ulong));
                return *(ulong*)(this.pointer + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ushort ReadUShort(int offset)
        {
            this.CheckAlignment(offset, sizeof(ushort));
            checked
            {
                this.EnsureInBounds(offset, sizeof(ushort));
                return *(ushort*)(this.pointer + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Memory<byte> GetByteMemory(int start, int length)
        {
            checked
            {
                EnsureInBounds(start, length);
                return this.memory.Slice(start, length);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyMemory<byte> GetReadOnlyByteMemory(int start, int length)
        {
            return this.GetByteMemory(start, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ReadString(int offset, int byteLength, Encoding encoding)
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
                if (offset + size > this.length || offset < 0 || size < 0)
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public TItem InvokeParse<TItem>(IGeneratedSerializer<TItem> serializer, int offset)
        {
            return serializer.Parse(new Wrapper(this), offset);
        }

        private readonly struct Wrapper : IInputBuffer
        {
            private readonly UnsafeMemoryInputBuffer buffer;

            public Wrapper(UnsafeMemoryInputBuffer buffer) => this.buffer = buffer;

            public ISharedStringReader SharedStringReader { get => this.buffer.SharedStringReader; set => this.buffer.SharedStringReader = value; }
            public int Length
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => this.buffer.Length;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Memory<byte> GetByteMemory(int start, int length) => this.buffer.GetByteMemory(start, length);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ReadOnlyMemory<byte> GetReadOnlyByteMemory(int start, int length) => this.buffer.GetReadOnlyByteMemory(start, length);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TItem InvokeParse<TItem>(IGeneratedSerializer<TItem> serializer, int offset) => throw new NotImplementedException();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public byte ReadByte(int offset) => this.buffer.ReadByte(offset);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public double ReadDouble(int offset) => this.buffer.ReadDouble(offset);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public float ReadFloat(int offset) => this.buffer.ReadFloat(offset);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int ReadInt(int offset) => this.buffer.ReadInt(offset);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public long ReadLong(int offset) => this.buffer.ReadLong(offset);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public sbyte ReadSByte(int offset) => this.buffer.ReadSByte(offset);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public short ReadShort(int offset) => this.buffer.ReadShort(offset);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public string ReadString(int offset, int byteLength, Encoding encoding) => this.buffer.ReadString(offset, byteLength, encoding);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public uint ReadUInt(int offset) => this.buffer.ReadUInt(offset);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ulong ReadULong(int offset) => this.buffer.ReadULong(offset);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ushort ReadUShort(int offset) => this.buffer.ReadUShort(offset);
        }
    }
}
