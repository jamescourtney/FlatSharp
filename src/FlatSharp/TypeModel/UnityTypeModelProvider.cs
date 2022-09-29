/*
 * Copyright 2022 Unity Technologies
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

using FlatSharp.TypeModel.Vectors;

namespace FlatSharp.TypeModel;

public class UnityTypeModelProvider : ITypeModelProvider
{
    public bool TryCreateTypeModel(TypeModelContainer container, Type type,  [NotNullWhen(true)] out ITypeModel? typeModel)
    {
        if (type.IsGenericType)
        {
            var genericDef = type.GetGenericTypeDefinition();
            if (genericDef.Namespace == "Unity.Collections" &&  genericDef.Name == "NativeArray`1")
            {
                typeModel = new UnityNativeArrayVectorTypeModel(type, container);
                return true;
            }
        }

        typeModel = null;
        return false;
    }
}
