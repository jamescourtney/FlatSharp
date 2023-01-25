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

namespace FlatSharp;

/// <summary>
/// A FlatBuffer vector that can be visited.
/// </summary>
public interface IVisitableReferenceVector<T> where T : class
{
    /// <summary>
    /// Provides access to a FlatBuffer vector.
    /// </summary>
    TReturn Accept<TVisitor, TReturn>(TVisitor visitor)
        where TVisitor : IReferenceVectorVisitor<T, TReturn>;
}

public interface IVisitableValueVector<T> where T : struct
{
    TReturn Accept<TVisitor, TReturn>(TVisitor visitor)
        where TVisitor : IValueVectorVisitor<T, TReturn>;
}

public interface IReferenceVectorVisitor<T, TReturn> where T : class
{
    TReturn Visit<TDerived, TVector>(TVector vector)
        where TVector : struct, ISimpleVector<TDerived>
        where TDerived : T;
}

public interface IValueVectorVisitor<T, TReturn> where T : struct
{
    TReturn Visit<TVector>(TVector vector)
        where TVector : struct, ISimpleVector<T>;
}

public interface ISimpleVector<T>
{
    int Count { get; }

    T this[int index] { get; set; }
}