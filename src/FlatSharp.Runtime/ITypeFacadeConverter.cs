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

namespace FlatSharp.Runtime
{
    /// <summary>
    /// Converts back and forth between a Facade Type (<typeparamref name="TFacadeType"/>) and an
    /// underlying type used for storage (<typeparamref name="TUnderlyingType"/>).
    /// </summary>
    /// <typeparam name="TUnderlyingType">The underlying type used for literal storage in the FlatBuffer.</typeparam>
    /// <typeparam name="TFacadeType">The facade type.</typeparam>
    public interface ITypeFacadeConverter<TUnderlyingType, TFacadeType>
    {
        /// <summary>
        /// Converts from the facade type to the underlying type. Note that the conversion must return non-null in all situations.
        /// </summary>
        TUnderlyingType ConvertToUnderlyingType(TFacadeType item);

        /// <summary>
        /// Converts from the underlying type into the facade type. This method will only be invoked when the underlying type
        /// has a non-default value.
        /// </summary>
        TFacadeType ConvertFromUnderlyingType(TUnderlyingType item);
    }
}
