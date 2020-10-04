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
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents a shared string
    /// </summary>
    [DebuggerDisplay("{String}")]
    public sealed class SharedString : IEquatable<SharedString>
    {
        private int hashCode;
        private readonly string str;

        private SharedString(string str)
        {
            this.str = str;
        }

        public static SharedString Create(string str)
        {
            if (str == null)
            {
                return null;
            }

            return new SharedString(str);
        }

        public string String => this.str;

        public bool Equals(SharedString other)
        {
            if ((object)other == null)
            {
                return false;
            }

            return this.GetHashCode() == other.GetHashCode() && this.str == other.str;
        }

        public override string ToString()
        {
            return this.str;
        }

        public override int GetHashCode()
        {
            ref int hashCode = ref this.hashCode;
            if (hashCode == 0)
            {
                hashCode = this.str.GetHashCode();
            }

            return hashCode;
        }

        public override bool Equals(object obj)
        {
            return obj switch 
            { 
                SharedString sharedString => this.Equals(sharedString),
                string str => this.Equals(str),
                _ => false
            };
        }

        public bool Equals(string other)
        {
            return this.str == other;
        }

        public static bool operator ==(SharedString x, SharedString y) => StaticEquals(x, y);

        public static bool operator ==(SharedString x, string y) => x?.str == y;

        public static bool operator ==(string x, SharedString y) => y?.str == x;

        public static bool operator !=(SharedString x, SharedString y) => !StaticEquals(x, y);

        public static bool operator !=(SharedString x, string y) => !(x == y);

        public static bool operator !=(string x, SharedString y) => !(x == y);

        public static implicit operator string(SharedString sharedString) => sharedString?.str;

        public static implicit operator SharedString(string x) => SharedString.Create(x);

        internal static bool StaticEquals(SharedString x, SharedString y)
        {
            bool xNull = (object)x == null;
            bool yNull = (object)y == null;

            if (xNull || yNull)
            {
                return xNull && yNull;
            }

            return x.GetHashCode() == y.GetHashCode() &&
                   x.str == y.str;
        }
    }
}
