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
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents a shared string
    /// </summary>
    public class SharedString : IEquatable<SharedString>
    {
        private bool hasHashCode;
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

        public bool Equals(SharedString other) => StaticEquals(this, other);

        public override int GetHashCode()
        {
            ref bool hasHashCode = ref this.hasHashCode;
            if (hasHashCode)
            {
                return this.hashCode;
            }

            hasHashCode = true;
            var hashCode = this.str.GetHashCode();
            this.hashCode = hashCode;
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (obj is SharedString sharedStr)
            {
                return StaticEquals(this, sharedStr);
            }

            return false;
        }

        public static bool operator ==(SharedString x, SharedString y) => StaticEquals(x, y);

        public static bool operator !=(SharedString x, SharedString y) => !StaticEquals(x, y);

        public static implicit operator string(SharedString sharedString) => sharedString?.str;

        public static implicit operator SharedString(string x) => SharedString.Create(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool StaticEquals(SharedString x, SharedString y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }

            if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null))
            {
                return false;
            }

            return x.GetHashCode() == y.GetHashCode() &&
                   x.str == y.str;
        }
    }
}
