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

public interface IFlatBufferUnion
{
    byte Discriminator { get; }
}


public interface IFlatBufferUnionVisitor<TReturn, T1>
{
    
    TReturn Visit(T1 item);

}


public interface IFlatBufferUnion<T1> : IFlatBufferUnion
{
    
            T1 Item1  { get; }


}


public interface IFlatBufferUnionVisitor<TReturn, T1, T2>
{
    
    TReturn Visit(T1 item);

    
    TReturn Visit(T2 item);

}


public interface IFlatBufferUnion<T1, T2> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }


}


public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3>
{
    
    TReturn Visit(T1 item);

    
    TReturn Visit(T2 item);

    
    TReturn Visit(T3 item);

}


public interface IFlatBufferUnion<T1, T2, T3> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }


}


public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4>
{
    
    TReturn Visit(T1 item);

    
    TReturn Visit(T2 item);

    
    TReturn Visit(T3 item);

    
    TReturn Visit(T4 item);

}


public interface IFlatBufferUnion<T1, T2, T3, T4> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }


}


public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5>
{
    
    TReturn Visit(T1 item);

    
    TReturn Visit(T2 item);

    
    TReturn Visit(T3 item);

    
    TReturn Visit(T4 item);

    
    TReturn Visit(T5 item);

}


public interface IFlatBufferUnion<T1, T2, T3, T4, T5> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }


}


public interface IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6>
{
    
    TReturn Visit(T1 item);

    
    TReturn Visit(T2 item);

    
    TReturn Visit(T3 item);

    
    TReturn Visit(T4 item);

    
    TReturn Visit(T5 item);

    
    TReturn Visit(T6 item);

}


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }

    
            T26 Item26  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }

    
            T26 Item26  { get; }

    
            T27 Item27  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }

    
            T26 Item26  { get; }

    
            T27 Item27  { get; }

    
            T28 Item28  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }

    
            T26 Item26  { get; }

    
            T27 Item27  { get; }

    
            T28 Item28  { get; }

    
            T29 Item29  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }

    
            T26 Item26  { get; }

    
            T27 Item27  { get; }

    
            T28 Item28  { get; }

    
            T29 Item29  { get; }

    
            T30 Item30  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }

    
            T26 Item26  { get; }

    
            T27 Item27  { get; }

    
            T28 Item28  { get; }

    
            T29 Item29  { get; }

    
            T30 Item30  { get; }

    
            T31 Item31  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }

    
            T26 Item26  { get; }

    
            T27 Item27  { get; }

    
            T28 Item28  { get; }

    
            T29 Item29  { get; }

    
            T30 Item30  { get; }

    
            T31 Item31  { get; }

    
            T32 Item32  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }

    
            T26 Item26  { get; }

    
            T27 Item27  { get; }

    
            T28 Item28  { get; }

    
            T29 Item29  { get; }

    
            T30 Item30  { get; }

    
            T31 Item31  { get; }

    
            T32 Item32  { get; }

    
            T33 Item33  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }

    
            T26 Item26  { get; }

    
            T27 Item27  { get; }

    
            T28 Item28  { get; }

    
            T29 Item29  { get; }

    
            T30 Item30  { get; }

    
            T31 Item31  { get; }

    
            T32 Item32  { get; }

    
            T33 Item33  { get; }

    
            T34 Item34  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }

    
            T26 Item26  { get; }

    
            T27 Item27  { get; }

    
            T28 Item28  { get; }

    
            T29 Item29  { get; }

    
            T30 Item30  { get; }

    
            T31 Item31  { get; }

    
            T32 Item32  { get; }

    
            T33 Item33  { get; }

    
            T34 Item34  { get; }

    
            T35 Item35  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }

    
            T26 Item26  { get; }

    
            T27 Item27  { get; }

    
            T28 Item28  { get; }

    
            T29 Item29  { get; }

    
            T30 Item30  { get; }

    
            T31 Item31  { get; }

    
            T32 Item32  { get; }

    
            T33 Item33  { get; }

    
            T34 Item34  { get; }

    
            T35 Item35  { get; }

    
            T36 Item36  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }

    
            T26 Item26  { get; }

    
            T27 Item27  { get; }

    
            T28 Item28  { get; }

    
            T29 Item29  { get; }

    
            T30 Item30  { get; }

    
            T31 Item31  { get; }

    
            T32 Item32  { get; }

    
            T33 Item33  { get; }

    
            T34 Item34  { get; }

    
            T35 Item35  { get; }

    
            T36 Item36  { get; }

    
            T37 Item37  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }

    
            T26 Item26  { get; }

    
            T27 Item27  { get; }

    
            T28 Item28  { get; }

    
            T29 Item29  { get; }

    
            T30 Item30  { get; }

    
            T31 Item31  { get; }

    
            T32 Item32  { get; }

    
            T33 Item33  { get; }

    
            T34 Item34  { get; }

    
            T35 Item35  { get; }

    
            T36 Item36  { get; }

    
            T37 Item37  { get; }

    
            T38 Item38  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }

    
            T26 Item26  { get; }

    
            T27 Item27  { get; }

    
            T28 Item28  { get; }

    
            T29 Item29  { get; }

    
            T30 Item30  { get; }

    
            T31 Item31  { get; }

    
            T32 Item32  { get; }

    
            T33 Item33  { get; }

    
            T34 Item34  { get; }

    
            T35 Item35  { get; }

    
            T36 Item36  { get; }

    
            T37 Item37  { get; }

    
            T38 Item38  { get; }

    
            T39 Item39  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }

    
            T26 Item26  { get; }

    
            T27 Item27  { get; }

    
            T28 Item28  { get; }

    
            T29 Item29  { get; }

    
            T30 Item30  { get; }

    
            T31 Item31  { get; }

    
            T32 Item32  { get; }

    
            T33 Item33  { get; }

    
            T34 Item34  { get; }

    
            T35 Item35  { get; }

    
            T36 Item36  { get; }

    
            T37 Item37  { get; }

    
            T38 Item38  { get; }

    
            T39 Item39  { get; }

    
            T40 Item40  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }

    
            T26 Item26  { get; }

    
            T27 Item27  { get; }

    
            T28 Item28  { get; }

    
            T29 Item29  { get; }

    
            T30 Item30  { get; }

    
            T31 Item31  { get; }

    
            T32 Item32  { get; }

    
            T33 Item33  { get; }

    
            T34 Item34  { get; }

    
            T35 Item35  { get; }

    
            T36 Item36  { get; }

    
            T37 Item37  { get; }

    
            T38 Item38  { get; }

    
            T39 Item39  { get; }

    
            T40 Item40  { get; }

    
            T41 Item41  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }

    
            T26 Item26  { get; }

    
            T27 Item27  { get; }

    
            T28 Item28  { get; }

    
            T29 Item29  { get; }

    
            T30 Item30  { get; }

    
            T31 Item31  { get; }

    
            T32 Item32  { get; }

    
            T33 Item33  { get; }

    
            T34 Item34  { get; }

    
            T35 Item35  { get; }

    
            T36 Item36  { get; }

    
            T37 Item37  { get; }

    
            T38 Item38  { get; }

    
            T39 Item39  { get; }

    
            T40 Item40  { get; }

    
            T41 Item41  { get; }

    
            T42 Item42  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }

    
            T26 Item26  { get; }

    
            T27 Item27  { get; }

    
            T28 Item28  { get; }

    
            T29 Item29  { get; }

    
            T30 Item30  { get; }

    
            T31 Item31  { get; }

    
            T32 Item32  { get; }

    
            T33 Item33  { get; }

    
            T34 Item34  { get; }

    
            T35 Item35  { get; }

    
            T36 Item36  { get; }

    
            T37 Item37  { get; }

    
            T38 Item38  { get; }

    
            T39 Item39  { get; }

    
            T40 Item40  { get; }

    
            T41 Item41  { get; }

    
            T42 Item42  { get; }

    
            T43 Item43  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }

    
            T26 Item26  { get; }

    
            T27 Item27  { get; }

    
            T28 Item28  { get; }

    
            T29 Item29  { get; }

    
            T30 Item30  { get; }

    
            T31 Item31  { get; }

    
            T32 Item32  { get; }

    
            T33 Item33  { get; }

    
            T34 Item34  { get; }

    
            T35 Item35  { get; }

    
            T36 Item36  { get; }

    
            T37 Item37  { get; }

    
            T38 Item38  { get; }

    
            T39 Item39  { get; }

    
            T40 Item40  { get; }

    
            T41 Item41  { get; }

    
            T42 Item42  { get; }

    
            T43 Item43  { get; }

    
            T44 Item44  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }

    
            T26 Item26  { get; }

    
            T27 Item27  { get; }

    
            T28 Item28  { get; }

    
            T29 Item29  { get; }

    
            T30 Item30  { get; }

    
            T31 Item31  { get; }

    
            T32 Item32  { get; }

    
            T33 Item33  { get; }

    
            T34 Item34  { get; }

    
            T35 Item35  { get; }

    
            T36 Item36  { get; }

    
            T37 Item37  { get; }

    
            T38 Item38  { get; }

    
            T39 Item39  { get; }

    
            T40 Item40  { get; }

    
            T41 Item41  { get; }

    
            T42 Item42  { get; }

    
            T43 Item43  { get; }

    
            T44 Item44  { get; }

    
            T45 Item45  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }

    
            T26 Item26  { get; }

    
            T27 Item27  { get; }

    
            T28 Item28  { get; }

    
            T29 Item29  { get; }

    
            T30 Item30  { get; }

    
            T31 Item31  { get; }

    
            T32 Item32  { get; }

    
            T33 Item33  { get; }

    
            T34 Item34  { get; }

    
            T35 Item35  { get; }

    
            T36 Item36  { get; }

    
            T37 Item37  { get; }

    
            T38 Item38  { get; }

    
            T39 Item39  { get; }

    
            T40 Item40  { get; }

    
            T41 Item41  { get; }

    
            T42 Item42  { get; }

    
            T43 Item43  { get; }

    
            T44 Item44  { get; }

    
            T45 Item45  { get; }

    
            T46 Item46  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }

    
            T26 Item26  { get; }

    
            T27 Item27  { get; }

    
            T28 Item28  { get; }

    
            T29 Item29  { get; }

    
            T30 Item30  { get; }

    
            T31 Item31  { get; }

    
            T32 Item32  { get; }

    
            T33 Item33  { get; }

    
            T34 Item34  { get; }

    
            T35 Item35  { get; }

    
            T36 Item36  { get; }

    
            T37 Item37  { get; }

    
            T38 Item38  { get; }

    
            T39 Item39  { get; }

    
            T40 Item40  { get; }

    
            T41 Item41  { get; }

    
            T42 Item42  { get; }

    
            T43 Item43  { get; }

    
            T44 Item44  { get; }

    
            T45 Item45  { get; }

    
            T46 Item46  { get; }

    
            T47 Item47  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }

    
            T26 Item26  { get; }

    
            T27 Item27  { get; }

    
            T28 Item28  { get; }

    
            T29 Item29  { get; }

    
            T30 Item30  { get; }

    
            T31 Item31  { get; }

    
            T32 Item32  { get; }

    
            T33 Item33  { get; }

    
            T34 Item34  { get; }

    
            T35 Item35  { get; }

    
            T36 Item36  { get; }

    
            T37 Item37  { get; }

    
            T38 Item38  { get; }

    
            T39 Item39  { get; }

    
            T40 Item40  { get; }

    
            T41 Item41  { get; }

    
            T42 Item42  { get; }

    
            T43 Item43  { get; }

    
            T44 Item44  { get; }

    
            T45 Item45  { get; }

    
            T46 Item46  { get; }

    
            T47 Item47  { get; }

    
            T48 Item48  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }

    
            T26 Item26  { get; }

    
            T27 Item27  { get; }

    
            T28 Item28  { get; }

    
            T29 Item29  { get; }

    
            T30 Item30  { get; }

    
            T31 Item31  { get; }

    
            T32 Item32  { get; }

    
            T33 Item33  { get; }

    
            T34 Item34  { get; }

    
            T35 Item35  { get; }

    
            T36 Item36  { get; }

    
            T37 Item37  { get; }

    
            T38 Item38  { get; }

    
            T39 Item39  { get; }

    
            T40 Item40  { get; }

    
            T41 Item41  { get; }

    
            T42 Item42  { get; }

    
            T43 Item43  { get; }

    
            T44 Item44  { get; }

    
            T45 Item45  { get; }

    
            T46 Item46  { get; }

    
            T47 Item47  { get; }

    
            T48 Item48  { get; }

    
            T49 Item49  { get; }


}


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


public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50> : IFlatBufferUnion
{
    
            T1 Item1  { get; }

    
            T2 Item2  { get; }

    
            T3 Item3  { get; }

    
            T4 Item4  { get; }

    
            T5 Item5  { get; }

    
            T6 Item6  { get; }

    
            T7 Item7  { get; }

    
            T8 Item8  { get; }

    
            T9 Item9  { get; }

    
            T10 Item10  { get; }

    
            T11 Item11  { get; }

    
            T12 Item12  { get; }

    
            T13 Item13  { get; }

    
            T14 Item14  { get; }

    
            T15 Item15  { get; }

    
            T16 Item16  { get; }

    
            T17 Item17  { get; }

    
            T18 Item18  { get; }

    
            T19 Item19  { get; }

    
            T20 Item20  { get; }

    
            T21 Item21  { get; }

    
            T22 Item22  { get; }

    
            T23 Item23  { get; }

    
            T24 Item24  { get; }

    
            T25 Item25  { get; }

    
            T26 Item26  { get; }

    
            T27 Item27  { get; }

    
            T28 Item28  { get; }

    
            T29 Item29  { get; }

    
            T30 Item30  { get; }

    
            T31 Item31  { get; }

    
            T32 Item32  { get; }

    
            T33 Item33  { get; }

    
            T34 Item34  { get; }

    
            T35 Item35  { get; }

    
            T36 Item36  { get; }

    
            T37 Item37  { get; }

    
            T38 Item38  { get; }

    
            T39 Item39  { get; }

    
            T40 Item40  { get; }

    
            T41 Item41  { get; }

    
            T42 Item42  { get; }

    
            T43 Item43  { get; }

    
            T44 Item44  { get; }

    
            T45 Item45  { get; }

    
            T46 Item46  { get; }

    
            T47 Item47  { get; }

    
            T48 Item48  { get; }

    
            T49 Item49  { get; }

    
            T50 Item50  { get; }


}



        [ExcludeFromCodeCoverage]
        public struct FlatBufferUnion<T1> 
            : IFlatBufferUnion<T1>

                    where T1 : notnull
                {
            private readonly byte discriminator;
            private readonly object value;
                
            
            public FlatBufferUnion(T1 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 1;
                this.value = item;
            }
                
                        
            public byte Discriminator => this.discriminator;

            
            public T1 Item1
            {
                get 
                {
                    if (this.discriminator == 1)
                    {
                        return (T1)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T1? item)
            {
                if (this.discriminator == 1)
                {
                    item = (T1)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
                        

            public TReturn Accept<TVisitor, TReturn>(TVisitor visitor)
                where TVisitor : IFlatBufferUnionVisitor<TReturn, T1>
            {
                switch (this.discriminator)
                {
                                    case 1:
                        return visitor.Visit((T1)this.value);
                                    default:
                        throw new InvalidOperationException("Unexpected discriminator: " + this.discriminator);
                }
            }

            public override bool Equals(object? other)
            {
                if (other is FlatBufferUnion<T1> otherUnion)
                {
                    return this.discriminator == otherUnion.Discriminator &&
                            this.value.Equals(otherUnion.value);
                }
                else
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                return this.value.GetHashCode() ^ this.discriminator;
            }
        }

        [ExcludeFromCodeCoverage]
        public struct FlatBufferUnion<T1, T2> 
            : IFlatBufferUnion<T1, T2>

                    where T1 : notnull
                    where T2 : notnull
                {
            private readonly byte discriminator;
            private readonly object value;
                
            
            public FlatBufferUnion(T1 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 1;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T2 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 2;
                this.value = item;
            }
                
                        
            public byte Discriminator => this.discriminator;

            
            public T1 Item1
            {
                get 
                {
                    if (this.discriminator == 1)
                    {
                        return (T1)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T1? item)
            {
                if (this.discriminator == 1)
                {
                    item = (T1)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T2 Item2
            {
                get 
                {
                    if (this.discriminator == 2)
                    {
                        return (T2)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T2? item)
            {
                if (this.discriminator == 2)
                {
                    item = (T2)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
                        

            public TReturn Accept<TVisitor, TReturn>(TVisitor visitor)
                where TVisitor : IFlatBufferUnionVisitor<TReturn, T1, T2>
            {
                switch (this.discriminator)
                {
                                    case 1:
                        return visitor.Visit((T1)this.value);
                                    case 2:
                        return visitor.Visit((T2)this.value);
                                    default:
                        throw new InvalidOperationException("Unexpected discriminator: " + this.discriminator);
                }
            }

            public override bool Equals(object? other)
            {
                if (other is FlatBufferUnion<T1, T2> otherUnion)
                {
                    return this.discriminator == otherUnion.Discriminator &&
                            this.value.Equals(otherUnion.value);
                }
                else
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                return this.value.GetHashCode() ^ this.discriminator;
            }
        }

        [ExcludeFromCodeCoverage]
        public struct FlatBufferUnion<T1, T2, T3> 
            : IFlatBufferUnion<T1, T2, T3>

                    where T1 : notnull
                    where T2 : notnull
                    where T3 : notnull
                {
            private readonly byte discriminator;
            private readonly object value;
                
            
            public FlatBufferUnion(T1 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 1;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T2 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 2;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T3 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 3;
                this.value = item;
            }
                
                        
            public byte Discriminator => this.discriminator;

            
            public T1 Item1
            {
                get 
                {
                    if (this.discriminator == 1)
                    {
                        return (T1)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T1? item)
            {
                if (this.discriminator == 1)
                {
                    item = (T1)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T2 Item2
            {
                get 
                {
                    if (this.discriminator == 2)
                    {
                        return (T2)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T2? item)
            {
                if (this.discriminator == 2)
                {
                    item = (T2)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T3 Item3
            {
                get 
                {
                    if (this.discriminator == 3)
                    {
                        return (T3)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T3? item)
            {
                if (this.discriminator == 3)
                {
                    item = (T3)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
                        

            public TReturn Accept<TVisitor, TReturn>(TVisitor visitor)
                where TVisitor : IFlatBufferUnionVisitor<TReturn, T1, T2, T3>
            {
                switch (this.discriminator)
                {
                                    case 1:
                        return visitor.Visit((T1)this.value);
                                    case 2:
                        return visitor.Visit((T2)this.value);
                                    case 3:
                        return visitor.Visit((T3)this.value);
                                    default:
                        throw new InvalidOperationException("Unexpected discriminator: " + this.discriminator);
                }
            }

            public override bool Equals(object? other)
            {
                if (other is FlatBufferUnion<T1, T2, T3> otherUnion)
                {
                    return this.discriminator == otherUnion.Discriminator &&
                            this.value.Equals(otherUnion.value);
                }
                else
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                return this.value.GetHashCode() ^ this.discriminator;
            }
        }

        [ExcludeFromCodeCoverage]
        public struct FlatBufferUnion<T1, T2, T3, T4> 
            : IFlatBufferUnion<T1, T2, T3, T4>

                    where T1 : notnull
                    where T2 : notnull
                    where T3 : notnull
                    where T4 : notnull
                {
            private readonly byte discriminator;
            private readonly object value;
                
            
            public FlatBufferUnion(T1 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 1;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T2 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 2;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T3 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 3;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T4 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 4;
                this.value = item;
            }
                
                        
            public byte Discriminator => this.discriminator;

            
            public T1 Item1
            {
                get 
                {
                    if (this.discriminator == 1)
                    {
                        return (T1)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T1? item)
            {
                if (this.discriminator == 1)
                {
                    item = (T1)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T2 Item2
            {
                get 
                {
                    if (this.discriminator == 2)
                    {
                        return (T2)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T2? item)
            {
                if (this.discriminator == 2)
                {
                    item = (T2)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T3 Item3
            {
                get 
                {
                    if (this.discriminator == 3)
                    {
                        return (T3)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T3? item)
            {
                if (this.discriminator == 3)
                {
                    item = (T3)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T4 Item4
            {
                get 
                {
                    if (this.discriminator == 4)
                    {
                        return (T4)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T4? item)
            {
                if (this.discriminator == 4)
                {
                    item = (T4)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
                        

            public TReturn Accept<TVisitor, TReturn>(TVisitor visitor)
                where TVisitor : IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4>
            {
                switch (this.discriminator)
                {
                                    case 1:
                        return visitor.Visit((T1)this.value);
                                    case 2:
                        return visitor.Visit((T2)this.value);
                                    case 3:
                        return visitor.Visit((T3)this.value);
                                    case 4:
                        return visitor.Visit((T4)this.value);
                                    default:
                        throw new InvalidOperationException("Unexpected discriminator: " + this.discriminator);
                }
            }

            public override bool Equals(object? other)
            {
                if (other is FlatBufferUnion<T1, T2, T3, T4> otherUnion)
                {
                    return this.discriminator == otherUnion.Discriminator &&
                            this.value.Equals(otherUnion.value);
                }
                else
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                return this.value.GetHashCode() ^ this.discriminator;
            }
        }

        [ExcludeFromCodeCoverage]
        public struct FlatBufferUnion<T1, T2, T3, T4, T5> 
            : IFlatBufferUnion<T1, T2, T3, T4, T5>

                    where T1 : notnull
                    where T2 : notnull
                    where T3 : notnull
                    where T4 : notnull
                    where T5 : notnull
                {
            private readonly byte discriminator;
            private readonly object value;
                
            
            public FlatBufferUnion(T1 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 1;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T2 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 2;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T3 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 3;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T4 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 4;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T5 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 5;
                this.value = item;
            }
                
                        
            public byte Discriminator => this.discriminator;

            
            public T1 Item1
            {
                get 
                {
                    if (this.discriminator == 1)
                    {
                        return (T1)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T1? item)
            {
                if (this.discriminator == 1)
                {
                    item = (T1)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T2 Item2
            {
                get 
                {
                    if (this.discriminator == 2)
                    {
                        return (T2)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T2? item)
            {
                if (this.discriminator == 2)
                {
                    item = (T2)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T3 Item3
            {
                get 
                {
                    if (this.discriminator == 3)
                    {
                        return (T3)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T3? item)
            {
                if (this.discriminator == 3)
                {
                    item = (T3)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T4 Item4
            {
                get 
                {
                    if (this.discriminator == 4)
                    {
                        return (T4)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T4? item)
            {
                if (this.discriminator == 4)
                {
                    item = (T4)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T5 Item5
            {
                get 
                {
                    if (this.discriminator == 5)
                    {
                        return (T5)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T5? item)
            {
                if (this.discriminator == 5)
                {
                    item = (T5)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
                        

            public TReturn Accept<TVisitor, TReturn>(TVisitor visitor)
                where TVisitor : IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5>
            {
                switch (this.discriminator)
                {
                                    case 1:
                        return visitor.Visit((T1)this.value);
                                    case 2:
                        return visitor.Visit((T2)this.value);
                                    case 3:
                        return visitor.Visit((T3)this.value);
                                    case 4:
                        return visitor.Visit((T4)this.value);
                                    case 5:
                        return visitor.Visit((T5)this.value);
                                    default:
                        throw new InvalidOperationException("Unexpected discriminator: " + this.discriminator);
                }
            }

            public override bool Equals(object? other)
            {
                if (other is FlatBufferUnion<T1, T2, T3, T4, T5> otherUnion)
                {
                    return this.discriminator == otherUnion.Discriminator &&
                            this.value.Equals(otherUnion.value);
                }
                else
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                return this.value.GetHashCode() ^ this.discriminator;
            }
        }

        [ExcludeFromCodeCoverage]
        public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6> 
            : IFlatBufferUnion<T1, T2, T3, T4, T5, T6>

                    where T1 : notnull
                    where T2 : notnull
                    where T3 : notnull
                    where T4 : notnull
                    where T5 : notnull
                    where T6 : notnull
                {
            private readonly byte discriminator;
            private readonly object value;
                
            
            public FlatBufferUnion(T1 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 1;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T2 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 2;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T3 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 3;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T4 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 4;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T5 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 5;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T6 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 6;
                this.value = item;
            }
                
                        
            public byte Discriminator => this.discriminator;

            
            public T1 Item1
            {
                get 
                {
                    if (this.discriminator == 1)
                    {
                        return (T1)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T1? item)
            {
                if (this.discriminator == 1)
                {
                    item = (T1)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T2 Item2
            {
                get 
                {
                    if (this.discriminator == 2)
                    {
                        return (T2)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T2? item)
            {
                if (this.discriminator == 2)
                {
                    item = (T2)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T3 Item3
            {
                get 
                {
                    if (this.discriminator == 3)
                    {
                        return (T3)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T3? item)
            {
                if (this.discriminator == 3)
                {
                    item = (T3)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T4 Item4
            {
                get 
                {
                    if (this.discriminator == 4)
                    {
                        return (T4)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T4? item)
            {
                if (this.discriminator == 4)
                {
                    item = (T4)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T5 Item5
            {
                get 
                {
                    if (this.discriminator == 5)
                    {
                        return (T5)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T5? item)
            {
                if (this.discriminator == 5)
                {
                    item = (T5)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T6 Item6
            {
                get 
                {
                    if (this.discriminator == 6)
                    {
                        return (T6)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T6? item)
            {
                if (this.discriminator == 6)
                {
                    item = (T6)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
                        

            public TReturn Accept<TVisitor, TReturn>(TVisitor visitor)
                where TVisitor : IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6>
            {
                switch (this.discriminator)
                {
                                    case 1:
                        return visitor.Visit((T1)this.value);
                                    case 2:
                        return visitor.Visit((T2)this.value);
                                    case 3:
                        return visitor.Visit((T3)this.value);
                                    case 4:
                        return visitor.Visit((T4)this.value);
                                    case 5:
                        return visitor.Visit((T5)this.value);
                                    case 6:
                        return visitor.Visit((T6)this.value);
                                    default:
                        throw new InvalidOperationException("Unexpected discriminator: " + this.discriminator);
                }
            }

            public override bool Equals(object? other)
            {
                if (other is FlatBufferUnion<T1, T2, T3, T4, T5, T6> otherUnion)
                {
                    return this.discriminator == otherUnion.Discriminator &&
                            this.value.Equals(otherUnion.value);
                }
                else
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                return this.value.GetHashCode() ^ this.discriminator;
            }
        }

        [ExcludeFromCodeCoverage]
        public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7> 
            : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7>

                    where T1 : notnull
                    where T2 : notnull
                    where T3 : notnull
                    where T4 : notnull
                    where T5 : notnull
                    where T6 : notnull
                    where T7 : notnull
                {
            private readonly byte discriminator;
            private readonly object value;
                
            
            public FlatBufferUnion(T1 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 1;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T2 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 2;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T3 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 3;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T4 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 4;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T5 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 5;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T6 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 6;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T7 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 7;
                this.value = item;
            }
                
                        
            public byte Discriminator => this.discriminator;

            
            public T1 Item1
            {
                get 
                {
                    if (this.discriminator == 1)
                    {
                        return (T1)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T1? item)
            {
                if (this.discriminator == 1)
                {
                    item = (T1)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T2 Item2
            {
                get 
                {
                    if (this.discriminator == 2)
                    {
                        return (T2)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T2? item)
            {
                if (this.discriminator == 2)
                {
                    item = (T2)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T3 Item3
            {
                get 
                {
                    if (this.discriminator == 3)
                    {
                        return (T3)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T3? item)
            {
                if (this.discriminator == 3)
                {
                    item = (T3)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T4 Item4
            {
                get 
                {
                    if (this.discriminator == 4)
                    {
                        return (T4)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T4? item)
            {
                if (this.discriminator == 4)
                {
                    item = (T4)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T5 Item5
            {
                get 
                {
                    if (this.discriminator == 5)
                    {
                        return (T5)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T5? item)
            {
                if (this.discriminator == 5)
                {
                    item = (T5)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T6 Item6
            {
                get 
                {
                    if (this.discriminator == 6)
                    {
                        return (T6)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T6? item)
            {
                if (this.discriminator == 6)
                {
                    item = (T6)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T7 Item7
            {
                get 
                {
                    if (this.discriminator == 7)
                    {
                        return (T7)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T7? item)
            {
                if (this.discriminator == 7)
                {
                    item = (T7)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
                        

            public TReturn Accept<TVisitor, TReturn>(TVisitor visitor)
                where TVisitor : IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7>
            {
                switch (this.discriminator)
                {
                                    case 1:
                        return visitor.Visit((T1)this.value);
                                    case 2:
                        return visitor.Visit((T2)this.value);
                                    case 3:
                        return visitor.Visit((T3)this.value);
                                    case 4:
                        return visitor.Visit((T4)this.value);
                                    case 5:
                        return visitor.Visit((T5)this.value);
                                    case 6:
                        return visitor.Visit((T6)this.value);
                                    case 7:
                        return visitor.Visit((T7)this.value);
                                    default:
                        throw new InvalidOperationException("Unexpected discriminator: " + this.discriminator);
                }
            }

            public override bool Equals(object? other)
            {
                if (other is FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7> otherUnion)
                {
                    return this.discriminator == otherUnion.Discriminator &&
                            this.value.Equals(otherUnion.value);
                }
                else
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                return this.value.GetHashCode() ^ this.discriminator;
            }
        }

        [ExcludeFromCodeCoverage]
        public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8> 
            : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8>

                    where T1 : notnull
                    where T2 : notnull
                    where T3 : notnull
                    where T4 : notnull
                    where T5 : notnull
                    where T6 : notnull
                    where T7 : notnull
                    where T8 : notnull
                {
            private readonly byte discriminator;
            private readonly object value;
                
            
            public FlatBufferUnion(T1 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 1;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T2 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 2;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T3 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 3;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T4 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 4;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T5 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 5;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T6 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 6;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T7 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 7;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T8 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 8;
                this.value = item;
            }
                
                        
            public byte Discriminator => this.discriminator;

            
            public T1 Item1
            {
                get 
                {
                    if (this.discriminator == 1)
                    {
                        return (T1)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T1? item)
            {
                if (this.discriminator == 1)
                {
                    item = (T1)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T2 Item2
            {
                get 
                {
                    if (this.discriminator == 2)
                    {
                        return (T2)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T2? item)
            {
                if (this.discriminator == 2)
                {
                    item = (T2)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T3 Item3
            {
                get 
                {
                    if (this.discriminator == 3)
                    {
                        return (T3)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T3? item)
            {
                if (this.discriminator == 3)
                {
                    item = (T3)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T4 Item4
            {
                get 
                {
                    if (this.discriminator == 4)
                    {
                        return (T4)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T4? item)
            {
                if (this.discriminator == 4)
                {
                    item = (T4)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T5 Item5
            {
                get 
                {
                    if (this.discriminator == 5)
                    {
                        return (T5)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T5? item)
            {
                if (this.discriminator == 5)
                {
                    item = (T5)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T6 Item6
            {
                get 
                {
                    if (this.discriminator == 6)
                    {
                        return (T6)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T6? item)
            {
                if (this.discriminator == 6)
                {
                    item = (T6)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T7 Item7
            {
                get 
                {
                    if (this.discriminator == 7)
                    {
                        return (T7)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T7? item)
            {
                if (this.discriminator == 7)
                {
                    item = (T7)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T8 Item8
            {
                get 
                {
                    if (this.discriminator == 8)
                    {
                        return (T8)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T8? item)
            {
                if (this.discriminator == 8)
                {
                    item = (T8)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
                        

            public TReturn Accept<TVisitor, TReturn>(TVisitor visitor)
                where TVisitor : IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8>
            {
                switch (this.discriminator)
                {
                                    case 1:
                        return visitor.Visit((T1)this.value);
                                    case 2:
                        return visitor.Visit((T2)this.value);
                                    case 3:
                        return visitor.Visit((T3)this.value);
                                    case 4:
                        return visitor.Visit((T4)this.value);
                                    case 5:
                        return visitor.Visit((T5)this.value);
                                    case 6:
                        return visitor.Visit((T6)this.value);
                                    case 7:
                        return visitor.Visit((T7)this.value);
                                    case 8:
                        return visitor.Visit((T8)this.value);
                                    default:
                        throw new InvalidOperationException("Unexpected discriminator: " + this.discriminator);
                }
            }

            public override bool Equals(object? other)
            {
                if (other is FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8> otherUnion)
                {
                    return this.discriminator == otherUnion.Discriminator &&
                            this.value.Equals(otherUnion.value);
                }
                else
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                return this.value.GetHashCode() ^ this.discriminator;
            }
        }

        [ExcludeFromCodeCoverage]
        public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9> 
            : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>

                    where T1 : notnull
                    where T2 : notnull
                    where T3 : notnull
                    where T4 : notnull
                    where T5 : notnull
                    where T6 : notnull
                    where T7 : notnull
                    where T8 : notnull
                    where T9 : notnull
                {
            private readonly byte discriminator;
            private readonly object value;
                
            
            public FlatBufferUnion(T1 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 1;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T2 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 2;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T3 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 3;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T4 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 4;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T5 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 5;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T6 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 6;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T7 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 7;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T8 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 8;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T9 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 9;
                this.value = item;
            }
                
                        
            public byte Discriminator => this.discriminator;

            
            public T1 Item1
            {
                get 
                {
                    if (this.discriminator == 1)
                    {
                        return (T1)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T1? item)
            {
                if (this.discriminator == 1)
                {
                    item = (T1)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T2 Item2
            {
                get 
                {
                    if (this.discriminator == 2)
                    {
                        return (T2)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T2? item)
            {
                if (this.discriminator == 2)
                {
                    item = (T2)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T3 Item3
            {
                get 
                {
                    if (this.discriminator == 3)
                    {
                        return (T3)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T3? item)
            {
                if (this.discriminator == 3)
                {
                    item = (T3)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T4 Item4
            {
                get 
                {
                    if (this.discriminator == 4)
                    {
                        return (T4)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T4? item)
            {
                if (this.discriminator == 4)
                {
                    item = (T4)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T5 Item5
            {
                get 
                {
                    if (this.discriminator == 5)
                    {
                        return (T5)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T5? item)
            {
                if (this.discriminator == 5)
                {
                    item = (T5)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T6 Item6
            {
                get 
                {
                    if (this.discriminator == 6)
                    {
                        return (T6)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T6? item)
            {
                if (this.discriminator == 6)
                {
                    item = (T6)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T7 Item7
            {
                get 
                {
                    if (this.discriminator == 7)
                    {
                        return (T7)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T7? item)
            {
                if (this.discriminator == 7)
                {
                    item = (T7)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T8 Item8
            {
                get 
                {
                    if (this.discriminator == 8)
                    {
                        return (T8)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T8? item)
            {
                if (this.discriminator == 8)
                {
                    item = (T8)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T9 Item9
            {
                get 
                {
                    if (this.discriminator == 9)
                    {
                        return (T9)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T9? item)
            {
                if (this.discriminator == 9)
                {
                    item = (T9)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
                        

            public TReturn Accept<TVisitor, TReturn>(TVisitor visitor)
                where TVisitor : IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9>
            {
                switch (this.discriminator)
                {
                                    case 1:
                        return visitor.Visit((T1)this.value);
                                    case 2:
                        return visitor.Visit((T2)this.value);
                                    case 3:
                        return visitor.Visit((T3)this.value);
                                    case 4:
                        return visitor.Visit((T4)this.value);
                                    case 5:
                        return visitor.Visit((T5)this.value);
                                    case 6:
                        return visitor.Visit((T6)this.value);
                                    case 7:
                        return visitor.Visit((T7)this.value);
                                    case 8:
                        return visitor.Visit((T8)this.value);
                                    case 9:
                        return visitor.Visit((T9)this.value);
                                    default:
                        throw new InvalidOperationException("Unexpected discriminator: " + this.discriminator);
                }
            }

            public override bool Equals(object? other)
            {
                if (other is FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9> otherUnion)
                {
                    return this.discriminator == otherUnion.Discriminator &&
                            this.value.Equals(otherUnion.value);
                }
                else
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                return this.value.GetHashCode() ^ this.discriminator;
            }
        }

        [ExcludeFromCodeCoverage]
        public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> 
            : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>

                    where T1 : notnull
                    where T2 : notnull
                    where T3 : notnull
                    where T4 : notnull
                    where T5 : notnull
                    where T6 : notnull
                    where T7 : notnull
                    where T8 : notnull
                    where T9 : notnull
                    where T10 : notnull
                {
            private readonly byte discriminator;
            private readonly object value;
                
            
            public FlatBufferUnion(T1 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 1;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T2 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 2;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T3 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 3;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T4 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 4;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T5 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 5;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T6 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 6;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T7 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 7;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T8 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 8;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T9 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 9;
                this.value = item;
            }
                
            
            public FlatBufferUnion(T10 item)
            {
                if (item is null)
                {
                    throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
                }

                this.discriminator = 10;
                this.value = item;
            }
                
                        
            public byte Discriminator => this.discriminator;

            
            public T1 Item1
            {
                get 
                {
                    if (this.discriminator == 1)
                    {
                        return (T1)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T1? item)
            {
                if (this.discriminator == 1)
                {
                    item = (T1)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T2 Item2
            {
                get 
                {
                    if (this.discriminator == 2)
                    {
                        return (T2)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T2? item)
            {
                if (this.discriminator == 2)
                {
                    item = (T2)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T3 Item3
            {
                get 
                {
                    if (this.discriminator == 3)
                    {
                        return (T3)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T3? item)
            {
                if (this.discriminator == 3)
                {
                    item = (T3)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T4 Item4
            {
                get 
                {
                    if (this.discriminator == 4)
                    {
                        return (T4)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T4? item)
            {
                if (this.discriminator == 4)
                {
                    item = (T4)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T5 Item5
            {
                get 
                {
                    if (this.discriminator == 5)
                    {
                        return (T5)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T5? item)
            {
                if (this.discriminator == 5)
                {
                    item = (T5)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T6 Item6
            {
                get 
                {
                    if (this.discriminator == 6)
                    {
                        return (T6)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T6? item)
            {
                if (this.discriminator == 6)
                {
                    item = (T6)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T7 Item7
            {
                get 
                {
                    if (this.discriminator == 7)
                    {
                        return (T7)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T7? item)
            {
                if (this.discriminator == 7)
                {
                    item = (T7)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T8 Item8
            {
                get 
                {
                    if (this.discriminator == 8)
                    {
                        return (T8)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T8? item)
            {
                if (this.discriminator == 8)
                {
                    item = (T8)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T9 Item9
            {
                get 
                {
                    if (this.discriminator == 9)
                    {
                        return (T9)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T9? item)
            {
                if (this.discriminator == 9)
                {
                    item = (T9)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
            
            public T10 Item10
            {
                get 
                {
                    if (this.discriminator == 10)
                    {
                        return (T10)this.value;
                    }
                    else
                    {
                        throw new System.InvalidOperationException();
                    }
                }
            }

            public bool TryGet([NotNullWhen(true)] out T10? item)
            {
                if (this.discriminator == 10)
                {
                    item = (T10)this.value;
                    return true;
                }
                else
                {
                    item = default;
                    return false;
                }
            }
                
                        

            public TReturn Accept<TVisitor, TReturn>(TVisitor visitor)
                where TVisitor : IFlatBufferUnionVisitor<TReturn, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
            {
                switch (this.discriminator)
                {
                                    case 1:
                        return visitor.Visit((T1)this.value);
                                    case 2:
                        return visitor.Visit((T2)this.value);
                                    case 3:
                        return visitor.Visit((T3)this.value);
                                    case 4:
                        return visitor.Visit((T4)this.value);
                                    case 5:
                        return visitor.Visit((T5)this.value);
                                    case 6:
                        return visitor.Visit((T6)this.value);
                                    case 7:
                        return visitor.Visit((T7)this.value);
                                    case 8:
                        return visitor.Visit((T8)this.value);
                                    case 9:
                        return visitor.Visit((T9)this.value);
                                    case 10:
                        return visitor.Visit((T10)this.value);
                                    default:
                        throw new InvalidOperationException("Unexpected discriminator: " + this.discriminator);
                }
            }

            public override bool Equals(object? other)
            {
                if (other is FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> otherUnion)
                {
                    return this.discriminator == otherUnion.Discriminator &&
                            this.value.Equals(otherUnion.value);
                }
                else
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                return this.value.GetHashCode() ^ this.discriminator;
            }
        }
