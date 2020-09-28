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

namespace FlatSharp.TypeModel
{
    /// <summary>
    /// An element of the physical layout of a Type Model. Types are serialized according to this layout, which represents the inline size
    /// when serializing the item. For tables, this is simply a tuple of (size: sizeof(uint), alignment: sizeof(uint)) since
    /// tables are written by reference instead of by value, but for structs 
    /// the layout depends on the inner members. This layout is widely used when serializing vectors, tables, structs and others.
    /// </summary>
    public class PhysicalLayoutElement
    {
        public PhysicalLayoutElement(int size, int alignment)
        {
            this.InlineSize = size;
            this.Alignment = alignment;
        }

        public int InlineSize { get; }

        public int Alignment { get; }
    }
}
