/*
 * Copyright 2022 James Courtney
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
/// Describes a Flat Buffer union with a discriminator.
/// </summary>
public interface IFlatBufferUnion
{
    /// <summary>
    /// Gets the discriminator from the union.
    /// </summary>
    byte Discriminator { get; }
}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 1 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1>
{

    TReturn Visit(T1 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 1 items.
/// </summary>
public interface IFlatBufferUnion<T1> : IFlatBufferUnion
{

    T1 Item1 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 2 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 2 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 3 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 3 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 4 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 4 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 5 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 5 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 6 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 6 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 7 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 7 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 8 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 8 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 9 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 9 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 10 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 10 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 11 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 11 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 12 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 12 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 13 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 13 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 14 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 14 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 15 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 15 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 16 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 16 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 17 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 17 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 18 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 18 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 19 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 19 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 20 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 20 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 21 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 21 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 22 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 22 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 23 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 23 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 24 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 24 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 25 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 25 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 26 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 26 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 27 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 27 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 28 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 28 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 29 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 29 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 30 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 30 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 31 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 31 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 32 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 32 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 33 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 33 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 34 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 34 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 35 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 35 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 36 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 36 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 37 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 37 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 38 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 38 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 39 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 39 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 40 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 40 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 41 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 41 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 42 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 42 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 43 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 43 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 44 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 44 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 45 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 45 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 46 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 46 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 47 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 47 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 48 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 48 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 49 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 49 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 50 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 50 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 51 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 51 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 52 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 52 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 53 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 53 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 54 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 54 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 55 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 55 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 56 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 56 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 57 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 57 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 58 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 58 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 59 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 59 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 60 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 60 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 61 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 61 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 62 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 62 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 63 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 63 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 64 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 64 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 65 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 65 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 66 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 66 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 67 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 67 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 68 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 68 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 69 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 69 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 70 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 70 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 71 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 71 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 72 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 72 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 73 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 73 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 74 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 74 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 75 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 75 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 76 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);


    TReturn Visit(T76 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 76 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


    T76 Item76 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 77 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);


    TReturn Visit(T76 item);


    TReturn Visit(T77 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 77 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


    T76 Item76 { get; }


    T77 Item77 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 78 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);


    TReturn Visit(T76 item);


    TReturn Visit(T77 item);


    TReturn Visit(T78 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 78 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


    T76 Item76 { get; }


    T77 Item77 { get; }


    T78 Item78 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 79 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);


    TReturn Visit(T76 item);


    TReturn Visit(T77 item);


    TReturn Visit(T78 item);


    TReturn Visit(T79 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 79 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


    T76 Item76 { get; }


    T77 Item77 { get; }


    T78 Item78 { get; }


    T79 Item79 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 80 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);


    TReturn Visit(T76 item);


    TReturn Visit(T77 item);


    TReturn Visit(T78 item);


    TReturn Visit(T79 item);


    TReturn Visit(T80 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 80 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


    T76 Item76 { get; }


    T77 Item77 { get; }


    T78 Item78 { get; }


    T79 Item79 { get; }


    T80 Item80 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 81 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);


    TReturn Visit(T76 item);


    TReturn Visit(T77 item);


    TReturn Visit(T78 item);


    TReturn Visit(T79 item);


    TReturn Visit(T80 item);


    TReturn Visit(T81 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 81 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


    T76 Item76 { get; }


    T77 Item77 { get; }


    T78 Item78 { get; }


    T79 Item79 { get; }


    T80 Item80 { get; }


    T81 Item81 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 82 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);


    TReturn Visit(T76 item);


    TReturn Visit(T77 item);


    TReturn Visit(T78 item);


    TReturn Visit(T79 item);


    TReturn Visit(T80 item);


    TReturn Visit(T81 item);


    TReturn Visit(T82 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 82 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


    T76 Item76 { get; }


    T77 Item77 { get; }


    T78 Item78 { get; }


    T79 Item79 { get; }


    T80 Item80 { get; }


    T81 Item81 { get; }


    T82 Item82 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 83 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);


    TReturn Visit(T76 item);


    TReturn Visit(T77 item);


    TReturn Visit(T78 item);


    TReturn Visit(T79 item);


    TReturn Visit(T80 item);


    TReturn Visit(T81 item);


    TReturn Visit(T82 item);


    TReturn Visit(T83 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 83 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


    T76 Item76 { get; }


    T77 Item77 { get; }


    T78 Item78 { get; }


    T79 Item79 { get; }


    T80 Item80 { get; }


    T81 Item81 { get; }


    T82 Item82 { get; }


    T83 Item83 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 84 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);


    TReturn Visit(T76 item);


    TReturn Visit(T77 item);


    TReturn Visit(T78 item);


    TReturn Visit(T79 item);


    TReturn Visit(T80 item);


    TReturn Visit(T81 item);


    TReturn Visit(T82 item);


    TReturn Visit(T83 item);


    TReturn Visit(T84 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 84 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


    T76 Item76 { get; }


    T77 Item77 { get; }


    T78 Item78 { get; }


    T79 Item79 { get; }


    T80 Item80 { get; }


    T81 Item81 { get; }


    T82 Item82 { get; }


    T83 Item83 { get; }


    T84 Item84 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 85 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);


    TReturn Visit(T76 item);


    TReturn Visit(T77 item);


    TReturn Visit(T78 item);


    TReturn Visit(T79 item);


    TReturn Visit(T80 item);


    TReturn Visit(T81 item);


    TReturn Visit(T82 item);


    TReturn Visit(T83 item);


    TReturn Visit(T84 item);


    TReturn Visit(T85 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 85 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


    T76 Item76 { get; }


    T77 Item77 { get; }


    T78 Item78 { get; }


    T79 Item79 { get; }


    T80 Item80 { get; }


    T81 Item81 { get; }


    T82 Item82 { get; }


    T83 Item83 { get; }


    T84 Item84 { get; }


    T85 Item85 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 86 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);


    TReturn Visit(T76 item);


    TReturn Visit(T77 item);


    TReturn Visit(T78 item);


    TReturn Visit(T79 item);


    TReturn Visit(T80 item);


    TReturn Visit(T81 item);


    TReturn Visit(T82 item);


    TReturn Visit(T83 item);


    TReturn Visit(T84 item);


    TReturn Visit(T85 item);


    TReturn Visit(T86 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 86 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


    T76 Item76 { get; }


    T77 Item77 { get; }


    T78 Item78 { get; }


    T79 Item79 { get; }


    T80 Item80 { get; }


    T81 Item81 { get; }


    T82 Item82 { get; }


    T83 Item83 { get; }


    T84 Item84 { get; }


    T85 Item85 { get; }


    T86 Item86 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 87 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);


    TReturn Visit(T76 item);


    TReturn Visit(T77 item);


    TReturn Visit(T78 item);


    TReturn Visit(T79 item);


    TReturn Visit(T80 item);


    TReturn Visit(T81 item);


    TReturn Visit(T82 item);


    TReturn Visit(T83 item);


    TReturn Visit(T84 item);


    TReturn Visit(T85 item);


    TReturn Visit(T86 item);


    TReturn Visit(T87 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 87 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


    T76 Item76 { get; }


    T77 Item77 { get; }


    T78 Item78 { get; }


    T79 Item79 { get; }


    T80 Item80 { get; }


    T81 Item81 { get; }


    T82 Item82 { get; }


    T83 Item83 { get; }


    T84 Item84 { get; }


    T85 Item85 { get; }


    T86 Item86 { get; }


    T87 Item87 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 88 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);


    TReturn Visit(T76 item);


    TReturn Visit(T77 item);


    TReturn Visit(T78 item);


    TReturn Visit(T79 item);


    TReturn Visit(T80 item);


    TReturn Visit(T81 item);


    TReturn Visit(T82 item);


    TReturn Visit(T83 item);


    TReturn Visit(T84 item);


    TReturn Visit(T85 item);


    TReturn Visit(T86 item);


    TReturn Visit(T87 item);


    TReturn Visit(T88 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 88 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


    T76 Item76 { get; }


    T77 Item77 { get; }


    T78 Item78 { get; }


    T79 Item79 { get; }


    T80 Item80 { get; }


    T81 Item81 { get; }


    T82 Item82 { get; }


    T83 Item83 { get; }


    T84 Item84 { get; }


    T85 Item85 { get; }


    T86 Item86 { get; }


    T87 Item87 { get; }


    T88 Item88 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 89 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88, T89>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);


    TReturn Visit(T76 item);


    TReturn Visit(T77 item);


    TReturn Visit(T78 item);


    TReturn Visit(T79 item);


    TReturn Visit(T80 item);


    TReturn Visit(T81 item);


    TReturn Visit(T82 item);


    TReturn Visit(T83 item);


    TReturn Visit(T84 item);


    TReturn Visit(T85 item);


    TReturn Visit(T86 item);


    TReturn Visit(T87 item);


    TReturn Visit(T88 item);


    TReturn Visit(T89 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 89 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88, T89> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


    T76 Item76 { get; }


    T77 Item77 { get; }


    T78 Item78 { get; }


    T79 Item79 { get; }


    T80 Item80 { get; }


    T81 Item81 { get; }


    T82 Item82 { get; }


    T83 Item83 { get; }


    T84 Item84 { get; }


    T85 Item85 { get; }


    T86 Item86 { get; }


    T87 Item87 { get; }


    T88 Item88 { get; }


    T89 Item89 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 90 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88, T89, T90>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);


    TReturn Visit(T76 item);


    TReturn Visit(T77 item);


    TReturn Visit(T78 item);


    TReturn Visit(T79 item);


    TReturn Visit(T80 item);


    TReturn Visit(T81 item);


    TReturn Visit(T82 item);


    TReturn Visit(T83 item);


    TReturn Visit(T84 item);


    TReturn Visit(T85 item);


    TReturn Visit(T86 item);


    TReturn Visit(T87 item);


    TReturn Visit(T88 item);


    TReturn Visit(T89 item);


    TReturn Visit(T90 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 90 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88, T89, T90> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


    T76 Item76 { get; }


    T77 Item77 { get; }


    T78 Item78 { get; }


    T79 Item79 { get; }


    T80 Item80 { get; }


    T81 Item81 { get; }


    T82 Item82 { get; }


    T83 Item83 { get; }


    T84 Item84 { get; }


    T85 Item85 { get; }


    T86 Item86 { get; }


    T87 Item87 { get; }


    T88 Item88 { get; }


    T89 Item89 { get; }


    T90 Item90 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 91 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88, T89, T90, T91>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);


    TReturn Visit(T76 item);


    TReturn Visit(T77 item);


    TReturn Visit(T78 item);


    TReturn Visit(T79 item);


    TReturn Visit(T80 item);


    TReturn Visit(T81 item);


    TReturn Visit(T82 item);


    TReturn Visit(T83 item);


    TReturn Visit(T84 item);


    TReturn Visit(T85 item);


    TReturn Visit(T86 item);


    TReturn Visit(T87 item);


    TReturn Visit(T88 item);


    TReturn Visit(T89 item);


    TReturn Visit(T90 item);


    TReturn Visit(T91 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 91 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88, T89, T90, T91> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


    T76 Item76 { get; }


    T77 Item77 { get; }


    T78 Item78 { get; }


    T79 Item79 { get; }


    T80 Item80 { get; }


    T81 Item81 { get; }


    T82 Item82 { get; }


    T83 Item83 { get; }


    T84 Item84 { get; }


    T85 Item85 { get; }


    T86 Item86 { get; }


    T87 Item87 { get; }


    T88 Item88 { get; }


    T89 Item89 { get; }


    T90 Item90 { get; }


    T91 Item91 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 92 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88, T89, T90, T91, T92>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);


    TReturn Visit(T76 item);


    TReturn Visit(T77 item);


    TReturn Visit(T78 item);


    TReturn Visit(T79 item);


    TReturn Visit(T80 item);


    TReturn Visit(T81 item);


    TReturn Visit(T82 item);


    TReturn Visit(T83 item);


    TReturn Visit(T84 item);


    TReturn Visit(T85 item);


    TReturn Visit(T86 item);


    TReturn Visit(T87 item);


    TReturn Visit(T88 item);


    TReturn Visit(T89 item);


    TReturn Visit(T90 item);


    TReturn Visit(T91 item);


    TReturn Visit(T92 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 92 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88, T89, T90, T91, T92> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


    T76 Item76 { get; }


    T77 Item77 { get; }


    T78 Item78 { get; }


    T79 Item79 { get; }


    T80 Item80 { get; }


    T81 Item81 { get; }


    T82 Item82 { get; }


    T83 Item83 { get; }


    T84 Item84 { get; }


    T85 Item85 { get; }


    T86 Item86 { get; }


    T87 Item87 { get; }


    T88 Item88 { get; }


    T89 Item89 { get; }


    T90 Item90 { get; }


    T91 Item91 { get; }


    T92 Item92 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 93 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88, T89, T90, T91, T92, T93>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);


    TReturn Visit(T76 item);


    TReturn Visit(T77 item);


    TReturn Visit(T78 item);


    TReturn Visit(T79 item);


    TReturn Visit(T80 item);


    TReturn Visit(T81 item);


    TReturn Visit(T82 item);


    TReturn Visit(T83 item);


    TReturn Visit(T84 item);


    TReturn Visit(T85 item);


    TReturn Visit(T86 item);


    TReturn Visit(T87 item);


    TReturn Visit(T88 item);


    TReturn Visit(T89 item);


    TReturn Visit(T90 item);


    TReturn Visit(T91 item);


    TReturn Visit(T92 item);


    TReturn Visit(T93 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 93 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88, T89, T90, T91, T92, T93> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


    T76 Item76 { get; }


    T77 Item77 { get; }


    T78 Item78 { get; }


    T79 Item79 { get; }


    T80 Item80 { get; }


    T81 Item81 { get; }


    T82 Item82 { get; }


    T83 Item83 { get; }


    T84 Item84 { get; }


    T85 Item85 { get; }


    T86 Item86 { get; }


    T87 Item87 { get; }


    T88 Item88 { get; }


    T89 Item89 { get; }


    T90 Item90 { get; }


    T91 Item91 { get; }


    T92 Item92 { get; }


    T93 Item93 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 94 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88, T89, T90, T91, T92, T93, T94>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);


    TReturn Visit(T76 item);


    TReturn Visit(T77 item);


    TReturn Visit(T78 item);


    TReturn Visit(T79 item);


    TReturn Visit(T80 item);


    TReturn Visit(T81 item);


    TReturn Visit(T82 item);


    TReturn Visit(T83 item);


    TReturn Visit(T84 item);


    TReturn Visit(T85 item);


    TReturn Visit(T86 item);


    TReturn Visit(T87 item);


    TReturn Visit(T88 item);


    TReturn Visit(T89 item);


    TReturn Visit(T90 item);


    TReturn Visit(T91 item);


    TReturn Visit(T92 item);


    TReturn Visit(T93 item);


    TReturn Visit(T94 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 94 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88, T89, T90, T91, T92, T93, T94> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


    T76 Item76 { get; }


    T77 Item77 { get; }


    T78 Item78 { get; }


    T79 Item79 { get; }


    T80 Item80 { get; }


    T81 Item81 { get; }


    T82 Item82 { get; }


    T83 Item83 { get; }


    T84 Item84 { get; }


    T85 Item85 { get; }


    T86 Item86 { get; }


    T87 Item87 { get; }


    T88 Item88 { get; }


    T89 Item89 { get; }


    T90 Item90 { get; }


    T91 Item91 { get; }


    T92 Item92 { get; }


    T93 Item93 { get; }


    T94 Item94 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 95 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88, T89, T90, T91, T92, T93, T94, T95>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);


    TReturn Visit(T76 item);


    TReturn Visit(T77 item);


    TReturn Visit(T78 item);


    TReturn Visit(T79 item);


    TReturn Visit(T80 item);


    TReturn Visit(T81 item);


    TReturn Visit(T82 item);


    TReturn Visit(T83 item);


    TReturn Visit(T84 item);


    TReturn Visit(T85 item);


    TReturn Visit(T86 item);


    TReturn Visit(T87 item);


    TReturn Visit(T88 item);


    TReturn Visit(T89 item);


    TReturn Visit(T90 item);


    TReturn Visit(T91 item);


    TReturn Visit(T92 item);


    TReturn Visit(T93 item);


    TReturn Visit(T94 item);


    TReturn Visit(T95 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 95 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88, T89, T90, T91, T92, T93, T94, T95> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


    T76 Item76 { get; }


    T77 Item77 { get; }


    T78 Item78 { get; }


    T79 Item79 { get; }


    T80 Item80 { get; }


    T81 Item81 { get; }


    T82 Item82 { get; }


    T83 Item83 { get; }


    T84 Item84 { get; }


    T85 Item85 { get; }


    T86 Item86 { get; }


    T87 Item87 { get; }


    T88 Item88 { get; }


    T89 Item89 { get; }


    T90 Item90 { get; }


    T91 Item91 { get; }


    T92 Item92 { get; }


    T93 Item93 { get; }


    T94 Item94 { get; }


    T95 Item95 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 96 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88, T89, T90, T91, T92, T93, T94, T95, T96>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);


    TReturn Visit(T76 item);


    TReturn Visit(T77 item);


    TReturn Visit(T78 item);


    TReturn Visit(T79 item);


    TReturn Visit(T80 item);


    TReturn Visit(T81 item);


    TReturn Visit(T82 item);


    TReturn Visit(T83 item);


    TReturn Visit(T84 item);


    TReturn Visit(T85 item);


    TReturn Visit(T86 item);


    TReturn Visit(T87 item);


    TReturn Visit(T88 item);


    TReturn Visit(T89 item);


    TReturn Visit(T90 item);


    TReturn Visit(T91 item);


    TReturn Visit(T92 item);


    TReturn Visit(T93 item);


    TReturn Visit(T94 item);


    TReturn Visit(T95 item);


    TReturn Visit(T96 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 96 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88, T89, T90, T91, T92, T93, T94, T95, T96> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


    T76 Item76 { get; }


    T77 Item77 { get; }


    T78 Item78 { get; }


    T79 Item79 { get; }


    T80 Item80 { get; }


    T81 Item81 { get; }


    T82 Item82 { get; }


    T83 Item83 { get; }


    T84 Item84 { get; }


    T85 Item85 { get; }


    T86 Item86 { get; }


    T87 Item87 { get; }


    T88 Item88 { get; }


    T89 Item89 { get; }


    T90 Item90 { get; }


    T91 Item91 { get; }


    T92 Item92 { get; }


    T93 Item93 { get; }


    T94 Item94 { get; }


    T95 Item95 { get; }


    T96 Item96 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 97 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88, T89, T90, T91, T92, T93, T94, T95, T96, T97>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);


    TReturn Visit(T76 item);


    TReturn Visit(T77 item);


    TReturn Visit(T78 item);


    TReturn Visit(T79 item);


    TReturn Visit(T80 item);


    TReturn Visit(T81 item);


    TReturn Visit(T82 item);


    TReturn Visit(T83 item);


    TReturn Visit(T84 item);


    TReturn Visit(T85 item);


    TReturn Visit(T86 item);


    TReturn Visit(T87 item);


    TReturn Visit(T88 item);


    TReturn Visit(T89 item);


    TReturn Visit(T90 item);


    TReturn Visit(T91 item);


    TReturn Visit(T92 item);


    TReturn Visit(T93 item);


    TReturn Visit(T94 item);


    TReturn Visit(T95 item);


    TReturn Visit(T96 item);


    TReturn Visit(T97 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 97 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88, T89, T90, T91, T92, T93, T94, T95, T96, T97> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


    T76 Item76 { get; }


    T77 Item77 { get; }


    T78 Item78 { get; }


    T79 Item79 { get; }


    T80 Item80 { get; }


    T81 Item81 { get; }


    T82 Item82 { get; }


    T83 Item83 { get; }


    T84 Item84 { get; }


    T85 Item85 { get; }


    T86 Item86 { get; }


    T87 Item87 { get; }


    T88 Item88 { get; }


    T89 Item89 { get; }


    T90 Item90 { get; }


    T91 Item91 { get; }


    T92 Item92 { get; }


    T93 Item93 { get; }


    T94 Item94 { get; }


    T95 Item95 { get; }


    T96 Item96 { get; }


    T97 Item97 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 98 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88, T89, T90, T91, T92, T93, T94, T95, T96, T97, T98>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);


    TReturn Visit(T76 item);


    TReturn Visit(T77 item);


    TReturn Visit(T78 item);


    TReturn Visit(T79 item);


    TReturn Visit(T80 item);


    TReturn Visit(T81 item);


    TReturn Visit(T82 item);


    TReturn Visit(T83 item);


    TReturn Visit(T84 item);


    TReturn Visit(T85 item);


    TReturn Visit(T86 item);


    TReturn Visit(T87 item);


    TReturn Visit(T88 item);


    TReturn Visit(T89 item);


    TReturn Visit(T90 item);


    TReturn Visit(T91 item);


    TReturn Visit(T92 item);


    TReturn Visit(T93 item);


    TReturn Visit(T94 item);


    TReturn Visit(T95 item);


    TReturn Visit(T96 item);


    TReturn Visit(T97 item);


    TReturn Visit(T98 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 98 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88, T89, T90, T91, T92, T93, T94, T95, T96, T97, T98> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


    T76 Item76 { get; }


    T77 Item77 { get; }


    T78 Item78 { get; }


    T79 Item79 { get; }


    T80 Item80 { get; }


    T81 Item81 { get; }


    T82 Item82 { get; }


    T83 Item83 { get; }


    T84 Item84 { get; }


    T85 Item85 { get; }


    T86 Item86 { get; }


    T87 Item87 { get; }


    T88 Item88 { get; }


    T89 Item89 { get; }


    T90 Item90 { get; }


    T91 Item91 { get; }


    T92 Item92 { get; }


    T93 Item93 { get; }


    T94 Item94 { get; }


    T95 Item95 { get; }


    T96 Item96 { get; }


    T97 Item97 { get; }


    T98 Item98 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 99 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88, T89, T90, T91, T92, T93, T94, T95, T96, T97, T98, T99>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);


    TReturn Visit(T76 item);


    TReturn Visit(T77 item);


    TReturn Visit(T78 item);


    TReturn Visit(T79 item);


    TReturn Visit(T80 item);


    TReturn Visit(T81 item);


    TReturn Visit(T82 item);


    TReturn Visit(T83 item);


    TReturn Visit(T84 item);


    TReturn Visit(T85 item);


    TReturn Visit(T86 item);


    TReturn Visit(T87 item);


    TReturn Visit(T88 item);


    TReturn Visit(T89 item);


    TReturn Visit(T90 item);


    TReturn Visit(T91 item);


    TReturn Visit(T92 item);


    TReturn Visit(T93 item);


    TReturn Visit(T94 item);


    TReturn Visit(T95 item);


    TReturn Visit(T96 item);


    TReturn Visit(T97 item);


    TReturn Visit(T98 item);


    TReturn Visit(T99 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 99 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88, T89, T90, T91, T92, T93, T94, T95, T96, T97, T98, T99> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


    T76 Item76 { get; }


    T77 Item77 { get; }


    T78 Item78 { get; }


    T79 Item79 { get; }


    T80 Item80 { get; }


    T81 Item81 { get; }


    T82 Item82 { get; }


    T83 Item83 { get; }


    T84 Item84 { get; }


    T85 Item85 { get; }


    T86 Item86 { get; }


    T87 Item87 { get; }


    T88 Item88 { get; }


    T89 Item89 { get; }


    T90 Item90 { get; }


    T91 Item91 { get; }


    T92 Item92 { get; }


    T93 Item93 { get; }


    T94 Item94 { get; }


    T95 Item95 { get; }


    T96 Item96 { get; }


    T97 Item97 { get; }


    T98 Item98 { get; }


    T99 Item99 { get; }


}


/// <summary>
/// A Flat Buffer union visitor capable of visiting 100 items.
/// </summary>
public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88, T89, T90, T91, T92, T93, T94, T95, T96, T97, T98, T99, T100>
{

    TReturn Visit(T1 item);


    TReturn Visit(T2 item);


    TReturn Visit(T3 item);


    TReturn Visit(T4 item);


    TReturn Visit(T5 item);


    TReturn Visit(T6 item);


    TReturn Visit(T7 item);


    TReturn Visit(T8 item);


    TReturn Visit(T9 item);


    TReturn Visit(T10 item);


    TReturn Visit(T11 item);


    TReturn Visit(T12 item);


    TReturn Visit(T13 item);


    TReturn Visit(T14 item);


    TReturn Visit(T15 item);


    TReturn Visit(T16 item);


    TReturn Visit(T17 item);


    TReturn Visit(T18 item);


    TReturn Visit(T19 item);


    TReturn Visit(T20 item);


    TReturn Visit(T21 item);


    TReturn Visit(T22 item);


    TReturn Visit(T23 item);


    TReturn Visit(T24 item);


    TReturn Visit(T25 item);


    TReturn Visit(T26 item);


    TReturn Visit(T27 item);


    TReturn Visit(T28 item);


    TReturn Visit(T29 item);


    TReturn Visit(T30 item);


    TReturn Visit(T31 item);


    TReturn Visit(T32 item);


    TReturn Visit(T33 item);


    TReturn Visit(T34 item);


    TReturn Visit(T35 item);


    TReturn Visit(T36 item);


    TReturn Visit(T37 item);


    TReturn Visit(T38 item);


    TReturn Visit(T39 item);


    TReturn Visit(T40 item);


    TReturn Visit(T41 item);


    TReturn Visit(T42 item);


    TReturn Visit(T43 item);


    TReturn Visit(T44 item);


    TReturn Visit(T45 item);


    TReturn Visit(T46 item);


    TReturn Visit(T47 item);


    TReturn Visit(T48 item);


    TReturn Visit(T49 item);


    TReturn Visit(T50 item);


    TReturn Visit(T51 item);


    TReturn Visit(T52 item);


    TReturn Visit(T53 item);


    TReturn Visit(T54 item);


    TReturn Visit(T55 item);


    TReturn Visit(T56 item);


    TReturn Visit(T57 item);


    TReturn Visit(T58 item);


    TReturn Visit(T59 item);


    TReturn Visit(T60 item);


    TReturn Visit(T61 item);


    TReturn Visit(T62 item);


    TReturn Visit(T63 item);


    TReturn Visit(T64 item);


    TReturn Visit(T65 item);


    TReturn Visit(T66 item);


    TReturn Visit(T67 item);


    TReturn Visit(T68 item);


    TReturn Visit(T69 item);


    TReturn Visit(T70 item);


    TReturn Visit(T71 item);


    TReturn Visit(T72 item);


    TReturn Visit(T73 item);


    TReturn Visit(T74 item);


    TReturn Visit(T75 item);


    TReturn Visit(T76 item);


    TReturn Visit(T77 item);


    TReturn Visit(T78 item);


    TReturn Visit(T79 item);


    TReturn Visit(T80 item);


    TReturn Visit(T81 item);


    TReturn Visit(T82 item);


    TReturn Visit(T83 item);


    TReturn Visit(T84 item);


    TReturn Visit(T85 item);


    TReturn Visit(T86 item);


    TReturn Visit(T87 item);


    TReturn Visit(T88 item);


    TReturn Visit(T89 item);


    TReturn Visit(T90 item);


    TReturn Visit(T91 item);


    TReturn Visit(T92 item);


    TReturn Visit(T93 item);


    TReturn Visit(T94 item);


    TReturn Visit(T95 item);


    TReturn Visit(T96 item);


    TReturn Visit(T97 item);


    TReturn Visit(T98 item);


    TReturn Visit(T99 item);


    TReturn Visit(T100 item);

}


/// <summary>
/// A FlatBuffer union visitor describing a union of 100 items.
/// </summary>
public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53, T54, T55, T56, T57, T58, T59, T60, T61, T62, T63, T64, T65, T66, T67, T68, T69, T70, T71, T72, T73, T74, T75, T76, T77, T78, T79, T80, T81, T82, T83, T84, T85, T86, T87, T88, T89, T90, T91, T92, T93, T94, T95, T96, T97, T98, T99, T100> : IFlatBufferUnion
{

    T1 Item1 { get; }


    T2 Item2 { get; }


    T3 Item3 { get; }


    T4 Item4 { get; }


    T5 Item5 { get; }


    T6 Item6 { get; }


    T7 Item7 { get; }


    T8 Item8 { get; }


    T9 Item9 { get; }


    T10 Item10 { get; }


    T11 Item11 { get; }


    T12 Item12 { get; }


    T13 Item13 { get; }


    T14 Item14 { get; }


    T15 Item15 { get; }


    T16 Item16 { get; }


    T17 Item17 { get; }


    T18 Item18 { get; }


    T19 Item19 { get; }


    T20 Item20 { get; }


    T21 Item21 { get; }


    T22 Item22 { get; }


    T23 Item23 { get; }


    T24 Item24 { get; }


    T25 Item25 { get; }


    T26 Item26 { get; }


    T27 Item27 { get; }


    T28 Item28 { get; }


    T29 Item29 { get; }


    T30 Item30 { get; }


    T31 Item31 { get; }


    T32 Item32 { get; }


    T33 Item33 { get; }


    T34 Item34 { get; }


    T35 Item35 { get; }


    T36 Item36 { get; }


    T37 Item37 { get; }


    T38 Item38 { get; }


    T39 Item39 { get; }


    T40 Item40 { get; }


    T41 Item41 { get; }


    T42 Item42 { get; }


    T43 Item43 { get; }


    T44 Item44 { get; }


    T45 Item45 { get; }


    T46 Item46 { get; }


    T47 Item47 { get; }


    T48 Item48 { get; }


    T49 Item49 { get; }


    T50 Item50 { get; }


    T51 Item51 { get; }


    T52 Item52 { get; }


    T53 Item53 { get; }


    T54 Item54 { get; }


    T55 Item55 { get; }


    T56 Item56 { get; }


    T57 Item57 { get; }


    T58 Item58 { get; }


    T59 Item59 { get; }


    T60 Item60 { get; }


    T61 Item61 { get; }


    T62 Item62 { get; }


    T63 Item63 { get; }


    T64 Item64 { get; }


    T65 Item65 { get; }


    T66 Item66 { get; }


    T67 Item67 { get; }


    T68 Item68 { get; }


    T69 Item69 { get; }


    T70 Item70 { get; }


    T71 Item71 { get; }


    T72 Item72 { get; }


    T73 Item73 { get; }


    T74 Item74 { get; }


    T75 Item75 { get; }


    T76 Item76 { get; }


    T77 Item77 { get; }


    T78 Item78 { get; }


    T79 Item79 { get; }


    T80 Item80 { get; }


    T81 Item81 { get; }


    T82 Item82 { get; }


    T83 Item83 { get; }


    T84 Item84 { get; }


    T85 Item85 { get; }


    T86 Item86 { get; }


    T87 Item87 { get; }


    T88 Item88 { get; }


    T89 Item89 { get; }


    T90 Item90 { get; }


    T91 Item91 { get; }


    T92 Item92 { get; }


    T93 Item93 { get; }


    T94 Item94 { get; }


    T95 Item95 { get; }


    T96 Item96 { get; }


    T97 Item97 { get; }


    T98 Item98 { get; }


    T99 Item99 { get; }


    T100 Item100 { get; }


}

