/*
 * Copyright 2021 James Courtney
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
    /// <summary>
    /// A context that FlatSharp-deserialized classes will pass to their parent 
    /// object on construction, if the parent object defines a constructor that accepts this object.
    /// </summary>
    public class FlatBufferDeserializationContext

    {
        /// <summary>
        /// Initializes a new FlatSharpConstructorContext with the given deserialization option.
        /// </summary>
        public FlatBufferDeserializationContext(
            FlatBufferDeserializationOption deserializationOption)
        {
            this.DeserializationOption = deserializationOption;
        }

        /// <summary>
        /// The deserialization options used to create the current subclass.
        /// </summary>
        public FlatBufferDeserializationOption DeserializationOption { get; }
    }
}
