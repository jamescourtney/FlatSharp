

namespace FlatSharp
{
    internal interface IUnion
    {
    }


    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1> Clone(
        System.Func<T1, T1> cloneT1
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1>(cloneT1(this.item1));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2>(cloneT2(this.item2));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3>(cloneT3(this.item3));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4>(cloneT4(this.item4));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5>(cloneT5(this.item5));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;


        private readonly T6 item6;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public FlatBufferUnion(T6 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 6;
            this.item6 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public T6 Item6
        {
            get
            {
                if (this.discriminator == 6)
                {
                    return this.item6;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T6 item)
        {
            item = default;
            if (this.discriminator == 6)
            {
                item = this.item6;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5, T6> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5,
System.Func<T6, T6> cloneT6
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6>(cloneT5(this.item5));
                case 6:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6>(cloneT6(this.item6));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5,
System.Action<T6> callback6)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                case 6:
                    callback6(this.item6);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5,
System.Action<TState, T6> callback6)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                case 6:
                    callback6(state, this.item6);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5,
System.Func<T6, TResult> callback6)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                case 6:
                    return callback6(this.item6);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5,
System.Func<TState, T6, TResult> callback6)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                case 6:
                    return callback6(state, this.item6);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;


        private readonly T6 item6;


        private readonly T7 item7;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public FlatBufferUnion(T6 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 6;
            this.item6 = item;
        }


        public FlatBufferUnion(T7 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 7;
            this.item7 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public T6 Item6
        {
            get
            {
                if (this.discriminator == 6)
                {
                    return this.item6;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T6 item)
        {
            item = default;
            if (this.discriminator == 6)
            {
                item = this.item6;
                return true;
            }

            return false;
        }


        public T7 Item7
        {
            get
            {
                if (this.discriminator == 7)
                {
                    return this.item7;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T7 item)
        {
            item = default;
            if (this.discriminator == 7)
            {
                item = this.item7;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5,
System.Func<T6, T6> cloneT6,
System.Func<T7, T7> cloneT7
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7>(cloneT5(this.item5));
                case 6:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7>(cloneT6(this.item6));
                case 7:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7>(cloneT7(this.item7));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5,
System.Action<T6> callback6,
System.Action<T7> callback7)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                case 6:
                    callback6(this.item6);
                    break;
                case 7:
                    callback7(this.item7);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5,
System.Action<TState, T6> callback6,
System.Action<TState, T7> callback7)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                case 6:
                    callback6(state, this.item6);
                    break;
                case 7:
                    callback7(state, this.item7);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5,
System.Func<T6, TResult> callback6,
System.Func<T7, TResult> callback7)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                case 6:
                    return callback6(this.item6);
                case 7:
                    return callback7(this.item7);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5,
System.Func<TState, T6, TResult> callback6,
System.Func<TState, T7, TResult> callback7)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                case 6:
                    return callback6(state, this.item6);
                case 7:
                    return callback7(state, this.item7);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;


        private readonly T6 item6;


        private readonly T7 item7;


        private readonly T8 item8;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public FlatBufferUnion(T6 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 6;
            this.item6 = item;
        }


        public FlatBufferUnion(T7 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 7;
            this.item7 = item;
        }


        public FlatBufferUnion(T8 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 8;
            this.item8 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public T6 Item6
        {
            get
            {
                if (this.discriminator == 6)
                {
                    return this.item6;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T6 item)
        {
            item = default;
            if (this.discriminator == 6)
            {
                item = this.item6;
                return true;
            }

            return false;
        }


        public T7 Item7
        {
            get
            {
                if (this.discriminator == 7)
                {
                    return this.item7;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T7 item)
        {
            item = default;
            if (this.discriminator == 7)
            {
                item = this.item7;
                return true;
            }

            return false;
        }


        public T8 Item8
        {
            get
            {
                if (this.discriminator == 8)
                {
                    return this.item8;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T8 item)
        {
            item = default;
            if (this.discriminator == 8)
            {
                item = this.item8;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5,
System.Func<T6, T6> cloneT6,
System.Func<T7, T7> cloneT7,
System.Func<T8, T8> cloneT8
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8>(cloneT5(this.item5));
                case 6:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8>(cloneT6(this.item6));
                case 7:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8>(cloneT7(this.item7));
                case 8:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8>(cloneT8(this.item8));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5,
System.Action<T6> callback6,
System.Action<T7> callback7,
System.Action<T8> callback8)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                case 6:
                    callback6(this.item6);
                    break;
                case 7:
                    callback7(this.item7);
                    break;
                case 8:
                    callback8(this.item8);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5,
System.Action<TState, T6> callback6,
System.Action<TState, T7> callback7,
System.Action<TState, T8> callback8)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                case 6:
                    callback6(state, this.item6);
                    break;
                case 7:
                    callback7(state, this.item7);
                    break;
                case 8:
                    callback8(state, this.item8);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5,
System.Func<T6, TResult> callback6,
System.Func<T7, TResult> callback7,
System.Func<T8, TResult> callback8)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                case 6:
                    return callback6(this.item6);
                case 7:
                    return callback7(this.item7);
                case 8:
                    return callback8(this.item8);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5,
System.Func<TState, T6, TResult> callback6,
System.Func<TState, T7, TResult> callback7,
System.Func<TState, T8, TResult> callback8)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                case 6:
                    return callback6(state, this.item6);
                case 7:
                    return callback7(state, this.item7);
                case 8:
                    return callback8(state, this.item8);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;


        private readonly T6 item6;


        private readonly T7 item7;


        private readonly T8 item8;


        private readonly T9 item9;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public FlatBufferUnion(T6 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 6;
            this.item6 = item;
        }


        public FlatBufferUnion(T7 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 7;
            this.item7 = item;
        }


        public FlatBufferUnion(T8 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 8;
            this.item8 = item;
        }


        public FlatBufferUnion(T9 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 9;
            this.item9 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public T6 Item6
        {
            get
            {
                if (this.discriminator == 6)
                {
                    return this.item6;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T6 item)
        {
            item = default;
            if (this.discriminator == 6)
            {
                item = this.item6;
                return true;
            }

            return false;
        }


        public T7 Item7
        {
            get
            {
                if (this.discriminator == 7)
                {
                    return this.item7;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T7 item)
        {
            item = default;
            if (this.discriminator == 7)
            {
                item = this.item7;
                return true;
            }

            return false;
        }


        public T8 Item8
        {
            get
            {
                if (this.discriminator == 8)
                {
                    return this.item8;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T8 item)
        {
            item = default;
            if (this.discriminator == 8)
            {
                item = this.item8;
                return true;
            }

            return false;
        }


        public T9 Item9
        {
            get
            {
                if (this.discriminator == 9)
                {
                    return this.item9;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T9 item)
        {
            item = default;
            if (this.discriminator == 9)
            {
                item = this.item9;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5,
System.Func<T6, T6> cloneT6,
System.Func<T7, T7> cloneT7,
System.Func<T8, T8> cloneT8,
System.Func<T9, T9> cloneT9
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(cloneT5(this.item5));
                case 6:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(cloneT6(this.item6));
                case 7:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(cloneT7(this.item7));
                case 8:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(cloneT8(this.item8));
                case 9:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(cloneT9(this.item9));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5,
System.Action<T6> callback6,
System.Action<T7> callback7,
System.Action<T8> callback8,
System.Action<T9> callback9)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                case 6:
                    callback6(this.item6);
                    break;
                case 7:
                    callback7(this.item7);
                    break;
                case 8:
                    callback8(this.item8);
                    break;
                case 9:
                    callback9(this.item9);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5,
System.Action<TState, T6> callback6,
System.Action<TState, T7> callback7,
System.Action<TState, T8> callback8,
System.Action<TState, T9> callback9)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                case 6:
                    callback6(state, this.item6);
                    break;
                case 7:
                    callback7(state, this.item7);
                    break;
                case 8:
                    callback8(state, this.item8);
                    break;
                case 9:
                    callback9(state, this.item9);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5,
System.Func<T6, TResult> callback6,
System.Func<T7, TResult> callback7,
System.Func<T8, TResult> callback8,
System.Func<T9, TResult> callback9)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                case 6:
                    return callback6(this.item6);
                case 7:
                    return callback7(this.item7);
                case 8:
                    return callback8(this.item8);
                case 9:
                    return callback9(this.item9);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5,
System.Func<TState, T6, TResult> callback6,
System.Func<TState, T7, TResult> callback7,
System.Func<TState, T8, TResult> callback8,
System.Func<TState, T9, TResult> callback9)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                case 6:
                    return callback6(state, this.item6);
                case 7:
                    return callback7(state, this.item7);
                case 8:
                    return callback8(state, this.item8);
                case 9:
                    return callback9(state, this.item9);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;


        private readonly T6 item6;


        private readonly T7 item7;


        private readonly T8 item8;


        private readonly T9 item9;


        private readonly T10 item10;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public FlatBufferUnion(T6 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 6;
            this.item6 = item;
        }


        public FlatBufferUnion(T7 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 7;
            this.item7 = item;
        }


        public FlatBufferUnion(T8 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 8;
            this.item8 = item;
        }


        public FlatBufferUnion(T9 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 9;
            this.item9 = item;
        }


        public FlatBufferUnion(T10 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 10;
            this.item10 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public T6 Item6
        {
            get
            {
                if (this.discriminator == 6)
                {
                    return this.item6;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T6 item)
        {
            item = default;
            if (this.discriminator == 6)
            {
                item = this.item6;
                return true;
            }

            return false;
        }


        public T7 Item7
        {
            get
            {
                if (this.discriminator == 7)
                {
                    return this.item7;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T7 item)
        {
            item = default;
            if (this.discriminator == 7)
            {
                item = this.item7;
                return true;
            }

            return false;
        }


        public T8 Item8
        {
            get
            {
                if (this.discriminator == 8)
                {
                    return this.item8;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T8 item)
        {
            item = default;
            if (this.discriminator == 8)
            {
                item = this.item8;
                return true;
            }

            return false;
        }


        public T9 Item9
        {
            get
            {
                if (this.discriminator == 9)
                {
                    return this.item9;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T9 item)
        {
            item = default;
            if (this.discriminator == 9)
            {
                item = this.item9;
                return true;
            }

            return false;
        }


        public T10 Item10
        {
            get
            {
                if (this.discriminator == 10)
                {
                    return this.item10;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T10 item)
        {
            item = default;
            if (this.discriminator == 10)
            {
                item = this.item10;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5,
System.Func<T6, T6> cloneT6,
System.Func<T7, T7> cloneT7,
System.Func<T8, T8> cloneT8,
System.Func<T9, T9> cloneT9,
System.Func<T10, T10> cloneT10
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(cloneT5(this.item5));
                case 6:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(cloneT6(this.item6));
                case 7:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(cloneT7(this.item7));
                case 8:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(cloneT8(this.item8));
                case 9:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(cloneT9(this.item9));
                case 10:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(cloneT10(this.item10));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5,
System.Action<T6> callback6,
System.Action<T7> callback7,
System.Action<T8> callback8,
System.Action<T9> callback9,
System.Action<T10> callback10)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                case 6:
                    callback6(this.item6);
                    break;
                case 7:
                    callback7(this.item7);
                    break;
                case 8:
                    callback8(this.item8);
                    break;
                case 9:
                    callback9(this.item9);
                    break;
                case 10:
                    callback10(this.item10);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5,
System.Action<TState, T6> callback6,
System.Action<TState, T7> callback7,
System.Action<TState, T8> callback8,
System.Action<TState, T9> callback9,
System.Action<TState, T10> callback10)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                case 6:
                    callback6(state, this.item6);
                    break;
                case 7:
                    callback7(state, this.item7);
                    break;
                case 8:
                    callback8(state, this.item8);
                    break;
                case 9:
                    callback9(state, this.item9);
                    break;
                case 10:
                    callback10(state, this.item10);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5,
System.Func<T6, TResult> callback6,
System.Func<T7, TResult> callback7,
System.Func<T8, TResult> callback8,
System.Func<T9, TResult> callback9,
System.Func<T10, TResult> callback10)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                case 6:
                    return callback6(this.item6);
                case 7:
                    return callback7(this.item7);
                case 8:
                    return callback8(this.item8);
                case 9:
                    return callback9(this.item9);
                case 10:
                    return callback10(this.item10);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5,
System.Func<TState, T6, TResult> callback6,
System.Func<TState, T7, TResult> callback7,
System.Func<TState, T8, TResult> callback8,
System.Func<TState, T9, TResult> callback9,
System.Func<TState, T10, TResult> callback10)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                case 6:
                    return callback6(state, this.item6);
                case 7:
                    return callback7(state, this.item7);
                case 8:
                    return callback8(state, this.item8);
                case 9:
                    return callback9(state, this.item9);
                case 10:
                    return callback10(state, this.item10);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;


        private readonly T6 item6;


        private readonly T7 item7;


        private readonly T8 item8;


        private readonly T9 item9;


        private readonly T10 item10;


        private readonly T11 item11;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public FlatBufferUnion(T6 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 6;
            this.item6 = item;
        }


        public FlatBufferUnion(T7 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 7;
            this.item7 = item;
        }


        public FlatBufferUnion(T8 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 8;
            this.item8 = item;
        }


        public FlatBufferUnion(T9 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 9;
            this.item9 = item;
        }


        public FlatBufferUnion(T10 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 10;
            this.item10 = item;
        }


        public FlatBufferUnion(T11 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 11;
            this.item11 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public T6 Item6
        {
            get
            {
                if (this.discriminator == 6)
                {
                    return this.item6;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T6 item)
        {
            item = default;
            if (this.discriminator == 6)
            {
                item = this.item6;
                return true;
            }

            return false;
        }


        public T7 Item7
        {
            get
            {
                if (this.discriminator == 7)
                {
                    return this.item7;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T7 item)
        {
            item = default;
            if (this.discriminator == 7)
            {
                item = this.item7;
                return true;
            }

            return false;
        }


        public T8 Item8
        {
            get
            {
                if (this.discriminator == 8)
                {
                    return this.item8;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T8 item)
        {
            item = default;
            if (this.discriminator == 8)
            {
                item = this.item8;
                return true;
            }

            return false;
        }


        public T9 Item9
        {
            get
            {
                if (this.discriminator == 9)
                {
                    return this.item9;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T9 item)
        {
            item = default;
            if (this.discriminator == 9)
            {
                item = this.item9;
                return true;
            }

            return false;
        }


        public T10 Item10
        {
            get
            {
                if (this.discriminator == 10)
                {
                    return this.item10;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T10 item)
        {
            item = default;
            if (this.discriminator == 10)
            {
                item = this.item10;
                return true;
            }

            return false;
        }


        public T11 Item11
        {
            get
            {
                if (this.discriminator == 11)
                {
                    return this.item11;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T11 item)
        {
            item = default;
            if (this.discriminator == 11)
            {
                item = this.item11;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5,
System.Func<T6, T6> cloneT6,
System.Func<T7, T7> cloneT7,
System.Func<T8, T8> cloneT8,
System.Func<T9, T9> cloneT9,
System.Func<T10, T10> cloneT10,
System.Func<T11, T11> cloneT11
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(cloneT5(this.item5));
                case 6:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(cloneT6(this.item6));
                case 7:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(cloneT7(this.item7));
                case 8:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(cloneT8(this.item8));
                case 9:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(cloneT9(this.item9));
                case 10:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(cloneT10(this.item10));
                case 11:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(cloneT11(this.item11));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5,
System.Action<T6> callback6,
System.Action<T7> callback7,
System.Action<T8> callback8,
System.Action<T9> callback9,
System.Action<T10> callback10,
System.Action<T11> callback11)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                case 6:
                    callback6(this.item6);
                    break;
                case 7:
                    callback7(this.item7);
                    break;
                case 8:
                    callback8(this.item8);
                    break;
                case 9:
                    callback9(this.item9);
                    break;
                case 10:
                    callback10(this.item10);
                    break;
                case 11:
                    callback11(this.item11);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5,
System.Action<TState, T6> callback6,
System.Action<TState, T7> callback7,
System.Action<TState, T8> callback8,
System.Action<TState, T9> callback9,
System.Action<TState, T10> callback10,
System.Action<TState, T11> callback11)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                case 6:
                    callback6(state, this.item6);
                    break;
                case 7:
                    callback7(state, this.item7);
                    break;
                case 8:
                    callback8(state, this.item8);
                    break;
                case 9:
                    callback9(state, this.item9);
                    break;
                case 10:
                    callback10(state, this.item10);
                    break;
                case 11:
                    callback11(state, this.item11);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5,
System.Func<T6, TResult> callback6,
System.Func<T7, TResult> callback7,
System.Func<T8, TResult> callback8,
System.Func<T9, TResult> callback9,
System.Func<T10, TResult> callback10,
System.Func<T11, TResult> callback11)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                case 6:
                    return callback6(this.item6);
                case 7:
                    return callback7(this.item7);
                case 8:
                    return callback8(this.item8);
                case 9:
                    return callback9(this.item9);
                case 10:
                    return callback10(this.item10);
                case 11:
                    return callback11(this.item11);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5,
System.Func<TState, T6, TResult> callback6,
System.Func<TState, T7, TResult> callback7,
System.Func<TState, T8, TResult> callback8,
System.Func<TState, T9, TResult> callback9,
System.Func<TState, T10, TResult> callback10,
System.Func<TState, T11, TResult> callback11)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                case 6:
                    return callback6(state, this.item6);
                case 7:
                    return callback7(state, this.item7);
                case 8:
                    return callback8(state, this.item8);
                case 9:
                    return callback9(state, this.item9);
                case 10:
                    return callback10(state, this.item10);
                case 11:
                    return callback11(state, this.item11);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;


        private readonly T6 item6;


        private readonly T7 item7;


        private readonly T8 item8;


        private readonly T9 item9;


        private readonly T10 item10;


        private readonly T11 item11;


        private readonly T12 item12;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public FlatBufferUnion(T6 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 6;
            this.item6 = item;
        }


        public FlatBufferUnion(T7 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 7;
            this.item7 = item;
        }


        public FlatBufferUnion(T8 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 8;
            this.item8 = item;
        }


        public FlatBufferUnion(T9 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 9;
            this.item9 = item;
        }


        public FlatBufferUnion(T10 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 10;
            this.item10 = item;
        }


        public FlatBufferUnion(T11 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 11;
            this.item11 = item;
        }


        public FlatBufferUnion(T12 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 12;
            this.item12 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public T6 Item6
        {
            get
            {
                if (this.discriminator == 6)
                {
                    return this.item6;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T6 item)
        {
            item = default;
            if (this.discriminator == 6)
            {
                item = this.item6;
                return true;
            }

            return false;
        }


        public T7 Item7
        {
            get
            {
                if (this.discriminator == 7)
                {
                    return this.item7;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T7 item)
        {
            item = default;
            if (this.discriminator == 7)
            {
                item = this.item7;
                return true;
            }

            return false;
        }


        public T8 Item8
        {
            get
            {
                if (this.discriminator == 8)
                {
                    return this.item8;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T8 item)
        {
            item = default;
            if (this.discriminator == 8)
            {
                item = this.item8;
                return true;
            }

            return false;
        }


        public T9 Item9
        {
            get
            {
                if (this.discriminator == 9)
                {
                    return this.item9;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T9 item)
        {
            item = default;
            if (this.discriminator == 9)
            {
                item = this.item9;
                return true;
            }

            return false;
        }


        public T10 Item10
        {
            get
            {
                if (this.discriminator == 10)
                {
                    return this.item10;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T10 item)
        {
            item = default;
            if (this.discriminator == 10)
            {
                item = this.item10;
                return true;
            }

            return false;
        }


        public T11 Item11
        {
            get
            {
                if (this.discriminator == 11)
                {
                    return this.item11;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T11 item)
        {
            item = default;
            if (this.discriminator == 11)
            {
                item = this.item11;
                return true;
            }

            return false;
        }


        public T12 Item12
        {
            get
            {
                if (this.discriminator == 12)
                {
                    return this.item12;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T12 item)
        {
            item = default;
            if (this.discriminator == 12)
            {
                item = this.item12;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5,
System.Func<T6, T6> cloneT6,
System.Func<T7, T7> cloneT7,
System.Func<T8, T8> cloneT8,
System.Func<T9, T9> cloneT9,
System.Func<T10, T10> cloneT10,
System.Func<T11, T11> cloneT11,
System.Func<T12, T12> cloneT12
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(cloneT5(this.item5));
                case 6:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(cloneT6(this.item6));
                case 7:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(cloneT7(this.item7));
                case 8:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(cloneT8(this.item8));
                case 9:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(cloneT9(this.item9));
                case 10:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(cloneT10(this.item10));
                case 11:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(cloneT11(this.item11));
                case 12:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(cloneT12(this.item12));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5,
System.Action<T6> callback6,
System.Action<T7> callback7,
System.Action<T8> callback8,
System.Action<T9> callback9,
System.Action<T10> callback10,
System.Action<T11> callback11,
System.Action<T12> callback12)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                case 6:
                    callback6(this.item6);
                    break;
                case 7:
                    callback7(this.item7);
                    break;
                case 8:
                    callback8(this.item8);
                    break;
                case 9:
                    callback9(this.item9);
                    break;
                case 10:
                    callback10(this.item10);
                    break;
                case 11:
                    callback11(this.item11);
                    break;
                case 12:
                    callback12(this.item12);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5,
System.Action<TState, T6> callback6,
System.Action<TState, T7> callback7,
System.Action<TState, T8> callback8,
System.Action<TState, T9> callback9,
System.Action<TState, T10> callback10,
System.Action<TState, T11> callback11,
System.Action<TState, T12> callback12)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                case 6:
                    callback6(state, this.item6);
                    break;
                case 7:
                    callback7(state, this.item7);
                    break;
                case 8:
                    callback8(state, this.item8);
                    break;
                case 9:
                    callback9(state, this.item9);
                    break;
                case 10:
                    callback10(state, this.item10);
                    break;
                case 11:
                    callback11(state, this.item11);
                    break;
                case 12:
                    callback12(state, this.item12);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5,
System.Func<T6, TResult> callback6,
System.Func<T7, TResult> callback7,
System.Func<T8, TResult> callback8,
System.Func<T9, TResult> callback9,
System.Func<T10, TResult> callback10,
System.Func<T11, TResult> callback11,
System.Func<T12, TResult> callback12)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                case 6:
                    return callback6(this.item6);
                case 7:
                    return callback7(this.item7);
                case 8:
                    return callback8(this.item8);
                case 9:
                    return callback9(this.item9);
                case 10:
                    return callback10(this.item10);
                case 11:
                    return callback11(this.item11);
                case 12:
                    return callback12(this.item12);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5,
System.Func<TState, T6, TResult> callback6,
System.Func<TState, T7, TResult> callback7,
System.Func<TState, T8, TResult> callback8,
System.Func<TState, T9, TResult> callback9,
System.Func<TState, T10, TResult> callback10,
System.Func<TState, T11, TResult> callback11,
System.Func<TState, T12, TResult> callback12)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                case 6:
                    return callback6(state, this.item6);
                case 7:
                    return callback7(state, this.item7);
                case 8:
                    return callback8(state, this.item8);
                case 9:
                    return callback9(state, this.item9);
                case 10:
                    return callback10(state, this.item10);
                case 11:
                    return callback11(state, this.item11);
                case 12:
                    return callback12(state, this.item12);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;


        private readonly T6 item6;


        private readonly T7 item7;


        private readonly T8 item8;


        private readonly T9 item9;


        private readonly T10 item10;


        private readonly T11 item11;


        private readonly T12 item12;


        private readonly T13 item13;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public FlatBufferUnion(T6 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 6;
            this.item6 = item;
        }


        public FlatBufferUnion(T7 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 7;
            this.item7 = item;
        }


        public FlatBufferUnion(T8 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 8;
            this.item8 = item;
        }


        public FlatBufferUnion(T9 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 9;
            this.item9 = item;
        }


        public FlatBufferUnion(T10 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 10;
            this.item10 = item;
        }


        public FlatBufferUnion(T11 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 11;
            this.item11 = item;
        }


        public FlatBufferUnion(T12 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 12;
            this.item12 = item;
        }


        public FlatBufferUnion(T13 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 13;
            this.item13 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public T6 Item6
        {
            get
            {
                if (this.discriminator == 6)
                {
                    return this.item6;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T6 item)
        {
            item = default;
            if (this.discriminator == 6)
            {
                item = this.item6;
                return true;
            }

            return false;
        }


        public T7 Item7
        {
            get
            {
                if (this.discriminator == 7)
                {
                    return this.item7;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T7 item)
        {
            item = default;
            if (this.discriminator == 7)
            {
                item = this.item7;
                return true;
            }

            return false;
        }


        public T8 Item8
        {
            get
            {
                if (this.discriminator == 8)
                {
                    return this.item8;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T8 item)
        {
            item = default;
            if (this.discriminator == 8)
            {
                item = this.item8;
                return true;
            }

            return false;
        }


        public T9 Item9
        {
            get
            {
                if (this.discriminator == 9)
                {
                    return this.item9;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T9 item)
        {
            item = default;
            if (this.discriminator == 9)
            {
                item = this.item9;
                return true;
            }

            return false;
        }


        public T10 Item10
        {
            get
            {
                if (this.discriminator == 10)
                {
                    return this.item10;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T10 item)
        {
            item = default;
            if (this.discriminator == 10)
            {
                item = this.item10;
                return true;
            }

            return false;
        }


        public T11 Item11
        {
            get
            {
                if (this.discriminator == 11)
                {
                    return this.item11;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T11 item)
        {
            item = default;
            if (this.discriminator == 11)
            {
                item = this.item11;
                return true;
            }

            return false;
        }


        public T12 Item12
        {
            get
            {
                if (this.discriminator == 12)
                {
                    return this.item12;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T12 item)
        {
            item = default;
            if (this.discriminator == 12)
            {
                item = this.item12;
                return true;
            }

            return false;
        }


        public T13 Item13
        {
            get
            {
                if (this.discriminator == 13)
                {
                    return this.item13;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T13 item)
        {
            item = default;
            if (this.discriminator == 13)
            {
                item = this.item13;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5,
System.Func<T6, T6> cloneT6,
System.Func<T7, T7> cloneT7,
System.Func<T8, T8> cloneT8,
System.Func<T9, T9> cloneT9,
System.Func<T10, T10> cloneT10,
System.Func<T11, T11> cloneT11,
System.Func<T12, T12> cloneT12,
System.Func<T13, T13> cloneT13
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(cloneT5(this.item5));
                case 6:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(cloneT6(this.item6));
                case 7:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(cloneT7(this.item7));
                case 8:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(cloneT8(this.item8));
                case 9:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(cloneT9(this.item9));
                case 10:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(cloneT10(this.item10));
                case 11:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(cloneT11(this.item11));
                case 12:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(cloneT12(this.item12));
                case 13:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(cloneT13(this.item13));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5,
System.Action<T6> callback6,
System.Action<T7> callback7,
System.Action<T8> callback8,
System.Action<T9> callback9,
System.Action<T10> callback10,
System.Action<T11> callback11,
System.Action<T12> callback12,
System.Action<T13> callback13)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                case 6:
                    callback6(this.item6);
                    break;
                case 7:
                    callback7(this.item7);
                    break;
                case 8:
                    callback8(this.item8);
                    break;
                case 9:
                    callback9(this.item9);
                    break;
                case 10:
                    callback10(this.item10);
                    break;
                case 11:
                    callback11(this.item11);
                    break;
                case 12:
                    callback12(this.item12);
                    break;
                case 13:
                    callback13(this.item13);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5,
System.Action<TState, T6> callback6,
System.Action<TState, T7> callback7,
System.Action<TState, T8> callback8,
System.Action<TState, T9> callback9,
System.Action<TState, T10> callback10,
System.Action<TState, T11> callback11,
System.Action<TState, T12> callback12,
System.Action<TState, T13> callback13)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                case 6:
                    callback6(state, this.item6);
                    break;
                case 7:
                    callback7(state, this.item7);
                    break;
                case 8:
                    callback8(state, this.item8);
                    break;
                case 9:
                    callback9(state, this.item9);
                    break;
                case 10:
                    callback10(state, this.item10);
                    break;
                case 11:
                    callback11(state, this.item11);
                    break;
                case 12:
                    callback12(state, this.item12);
                    break;
                case 13:
                    callback13(state, this.item13);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5,
System.Func<T6, TResult> callback6,
System.Func<T7, TResult> callback7,
System.Func<T8, TResult> callback8,
System.Func<T9, TResult> callback9,
System.Func<T10, TResult> callback10,
System.Func<T11, TResult> callback11,
System.Func<T12, TResult> callback12,
System.Func<T13, TResult> callback13)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                case 6:
                    return callback6(this.item6);
                case 7:
                    return callback7(this.item7);
                case 8:
                    return callback8(this.item8);
                case 9:
                    return callback9(this.item9);
                case 10:
                    return callback10(this.item10);
                case 11:
                    return callback11(this.item11);
                case 12:
                    return callback12(this.item12);
                case 13:
                    return callback13(this.item13);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5,
System.Func<TState, T6, TResult> callback6,
System.Func<TState, T7, TResult> callback7,
System.Func<TState, T8, TResult> callback8,
System.Func<TState, T9, TResult> callback9,
System.Func<TState, T10, TResult> callback10,
System.Func<TState, T11, TResult> callback11,
System.Func<TState, T12, TResult> callback12,
System.Func<TState, T13, TResult> callback13)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                case 6:
                    return callback6(state, this.item6);
                case 7:
                    return callback7(state, this.item7);
                case 8:
                    return callback8(state, this.item8);
                case 9:
                    return callback9(state, this.item9);
                case 10:
                    return callback10(state, this.item10);
                case 11:
                    return callback11(state, this.item11);
                case 12:
                    return callback12(state, this.item12);
                case 13:
                    return callback13(state, this.item13);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;


        private readonly T6 item6;


        private readonly T7 item7;


        private readonly T8 item8;


        private readonly T9 item9;


        private readonly T10 item10;


        private readonly T11 item11;


        private readonly T12 item12;


        private readonly T13 item13;


        private readonly T14 item14;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public FlatBufferUnion(T6 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 6;
            this.item6 = item;
        }


        public FlatBufferUnion(T7 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 7;
            this.item7 = item;
        }


        public FlatBufferUnion(T8 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 8;
            this.item8 = item;
        }


        public FlatBufferUnion(T9 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 9;
            this.item9 = item;
        }


        public FlatBufferUnion(T10 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 10;
            this.item10 = item;
        }


        public FlatBufferUnion(T11 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 11;
            this.item11 = item;
        }


        public FlatBufferUnion(T12 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 12;
            this.item12 = item;
        }


        public FlatBufferUnion(T13 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 13;
            this.item13 = item;
        }


        public FlatBufferUnion(T14 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 14;
            this.item14 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public T6 Item6
        {
            get
            {
                if (this.discriminator == 6)
                {
                    return this.item6;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T6 item)
        {
            item = default;
            if (this.discriminator == 6)
            {
                item = this.item6;
                return true;
            }

            return false;
        }


        public T7 Item7
        {
            get
            {
                if (this.discriminator == 7)
                {
                    return this.item7;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T7 item)
        {
            item = default;
            if (this.discriminator == 7)
            {
                item = this.item7;
                return true;
            }

            return false;
        }


        public T8 Item8
        {
            get
            {
                if (this.discriminator == 8)
                {
                    return this.item8;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T8 item)
        {
            item = default;
            if (this.discriminator == 8)
            {
                item = this.item8;
                return true;
            }

            return false;
        }


        public T9 Item9
        {
            get
            {
                if (this.discriminator == 9)
                {
                    return this.item9;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T9 item)
        {
            item = default;
            if (this.discriminator == 9)
            {
                item = this.item9;
                return true;
            }

            return false;
        }


        public T10 Item10
        {
            get
            {
                if (this.discriminator == 10)
                {
                    return this.item10;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T10 item)
        {
            item = default;
            if (this.discriminator == 10)
            {
                item = this.item10;
                return true;
            }

            return false;
        }


        public T11 Item11
        {
            get
            {
                if (this.discriminator == 11)
                {
                    return this.item11;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T11 item)
        {
            item = default;
            if (this.discriminator == 11)
            {
                item = this.item11;
                return true;
            }

            return false;
        }


        public T12 Item12
        {
            get
            {
                if (this.discriminator == 12)
                {
                    return this.item12;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T12 item)
        {
            item = default;
            if (this.discriminator == 12)
            {
                item = this.item12;
                return true;
            }

            return false;
        }


        public T13 Item13
        {
            get
            {
                if (this.discriminator == 13)
                {
                    return this.item13;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T13 item)
        {
            item = default;
            if (this.discriminator == 13)
            {
                item = this.item13;
                return true;
            }

            return false;
        }


        public T14 Item14
        {
            get
            {
                if (this.discriminator == 14)
                {
                    return this.item14;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T14 item)
        {
            item = default;
            if (this.discriminator == 14)
            {
                item = this.item14;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5,
System.Func<T6, T6> cloneT6,
System.Func<T7, T7> cloneT7,
System.Func<T8, T8> cloneT8,
System.Func<T9, T9> cloneT9,
System.Func<T10, T10> cloneT10,
System.Func<T11, T11> cloneT11,
System.Func<T12, T12> cloneT12,
System.Func<T13, T13> cloneT13,
System.Func<T14, T14> cloneT14
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(cloneT5(this.item5));
                case 6:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(cloneT6(this.item6));
                case 7:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(cloneT7(this.item7));
                case 8:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(cloneT8(this.item8));
                case 9:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(cloneT9(this.item9));
                case 10:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(cloneT10(this.item10));
                case 11:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(cloneT11(this.item11));
                case 12:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(cloneT12(this.item12));
                case 13:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(cloneT13(this.item13));
                case 14:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(cloneT14(this.item14));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5,
System.Action<T6> callback6,
System.Action<T7> callback7,
System.Action<T8> callback8,
System.Action<T9> callback9,
System.Action<T10> callback10,
System.Action<T11> callback11,
System.Action<T12> callback12,
System.Action<T13> callback13,
System.Action<T14> callback14)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                case 6:
                    callback6(this.item6);
                    break;
                case 7:
                    callback7(this.item7);
                    break;
                case 8:
                    callback8(this.item8);
                    break;
                case 9:
                    callback9(this.item9);
                    break;
                case 10:
                    callback10(this.item10);
                    break;
                case 11:
                    callback11(this.item11);
                    break;
                case 12:
                    callback12(this.item12);
                    break;
                case 13:
                    callback13(this.item13);
                    break;
                case 14:
                    callback14(this.item14);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5,
System.Action<TState, T6> callback6,
System.Action<TState, T7> callback7,
System.Action<TState, T8> callback8,
System.Action<TState, T9> callback9,
System.Action<TState, T10> callback10,
System.Action<TState, T11> callback11,
System.Action<TState, T12> callback12,
System.Action<TState, T13> callback13,
System.Action<TState, T14> callback14)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                case 6:
                    callback6(state, this.item6);
                    break;
                case 7:
                    callback7(state, this.item7);
                    break;
                case 8:
                    callback8(state, this.item8);
                    break;
                case 9:
                    callback9(state, this.item9);
                    break;
                case 10:
                    callback10(state, this.item10);
                    break;
                case 11:
                    callback11(state, this.item11);
                    break;
                case 12:
                    callback12(state, this.item12);
                    break;
                case 13:
                    callback13(state, this.item13);
                    break;
                case 14:
                    callback14(state, this.item14);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5,
System.Func<T6, TResult> callback6,
System.Func<T7, TResult> callback7,
System.Func<T8, TResult> callback8,
System.Func<T9, TResult> callback9,
System.Func<T10, TResult> callback10,
System.Func<T11, TResult> callback11,
System.Func<T12, TResult> callback12,
System.Func<T13, TResult> callback13,
System.Func<T14, TResult> callback14)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                case 6:
                    return callback6(this.item6);
                case 7:
                    return callback7(this.item7);
                case 8:
                    return callback8(this.item8);
                case 9:
                    return callback9(this.item9);
                case 10:
                    return callback10(this.item10);
                case 11:
                    return callback11(this.item11);
                case 12:
                    return callback12(this.item12);
                case 13:
                    return callback13(this.item13);
                case 14:
                    return callback14(this.item14);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5,
System.Func<TState, T6, TResult> callback6,
System.Func<TState, T7, TResult> callback7,
System.Func<TState, T8, TResult> callback8,
System.Func<TState, T9, TResult> callback9,
System.Func<TState, T10, TResult> callback10,
System.Func<TState, T11, TResult> callback11,
System.Func<TState, T12, TResult> callback12,
System.Func<TState, T13, TResult> callback13,
System.Func<TState, T14, TResult> callback14)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                case 6:
                    return callback6(state, this.item6);
                case 7:
                    return callback7(state, this.item7);
                case 8:
                    return callback8(state, this.item8);
                case 9:
                    return callback9(state, this.item9);
                case 10:
                    return callback10(state, this.item10);
                case 11:
                    return callback11(state, this.item11);
                case 12:
                    return callback12(state, this.item12);
                case 13:
                    return callback13(state, this.item13);
                case 14:
                    return callback14(state, this.item14);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;


        private readonly T6 item6;


        private readonly T7 item7;


        private readonly T8 item8;


        private readonly T9 item9;


        private readonly T10 item10;


        private readonly T11 item11;


        private readonly T12 item12;


        private readonly T13 item13;


        private readonly T14 item14;


        private readonly T15 item15;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public FlatBufferUnion(T6 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 6;
            this.item6 = item;
        }


        public FlatBufferUnion(T7 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 7;
            this.item7 = item;
        }


        public FlatBufferUnion(T8 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 8;
            this.item8 = item;
        }


        public FlatBufferUnion(T9 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 9;
            this.item9 = item;
        }


        public FlatBufferUnion(T10 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 10;
            this.item10 = item;
        }


        public FlatBufferUnion(T11 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 11;
            this.item11 = item;
        }


        public FlatBufferUnion(T12 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 12;
            this.item12 = item;
        }


        public FlatBufferUnion(T13 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 13;
            this.item13 = item;
        }


        public FlatBufferUnion(T14 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 14;
            this.item14 = item;
        }


        public FlatBufferUnion(T15 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 15;
            this.item15 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public T6 Item6
        {
            get
            {
                if (this.discriminator == 6)
                {
                    return this.item6;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T6 item)
        {
            item = default;
            if (this.discriminator == 6)
            {
                item = this.item6;
                return true;
            }

            return false;
        }


        public T7 Item7
        {
            get
            {
                if (this.discriminator == 7)
                {
                    return this.item7;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T7 item)
        {
            item = default;
            if (this.discriminator == 7)
            {
                item = this.item7;
                return true;
            }

            return false;
        }


        public T8 Item8
        {
            get
            {
                if (this.discriminator == 8)
                {
                    return this.item8;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T8 item)
        {
            item = default;
            if (this.discriminator == 8)
            {
                item = this.item8;
                return true;
            }

            return false;
        }


        public T9 Item9
        {
            get
            {
                if (this.discriminator == 9)
                {
                    return this.item9;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T9 item)
        {
            item = default;
            if (this.discriminator == 9)
            {
                item = this.item9;
                return true;
            }

            return false;
        }


        public T10 Item10
        {
            get
            {
                if (this.discriminator == 10)
                {
                    return this.item10;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T10 item)
        {
            item = default;
            if (this.discriminator == 10)
            {
                item = this.item10;
                return true;
            }

            return false;
        }


        public T11 Item11
        {
            get
            {
                if (this.discriminator == 11)
                {
                    return this.item11;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T11 item)
        {
            item = default;
            if (this.discriminator == 11)
            {
                item = this.item11;
                return true;
            }

            return false;
        }


        public T12 Item12
        {
            get
            {
                if (this.discriminator == 12)
                {
                    return this.item12;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T12 item)
        {
            item = default;
            if (this.discriminator == 12)
            {
                item = this.item12;
                return true;
            }

            return false;
        }


        public T13 Item13
        {
            get
            {
                if (this.discriminator == 13)
                {
                    return this.item13;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T13 item)
        {
            item = default;
            if (this.discriminator == 13)
            {
                item = this.item13;
                return true;
            }

            return false;
        }


        public T14 Item14
        {
            get
            {
                if (this.discriminator == 14)
                {
                    return this.item14;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T14 item)
        {
            item = default;
            if (this.discriminator == 14)
            {
                item = this.item14;
                return true;
            }

            return false;
        }


        public T15 Item15
        {
            get
            {
                if (this.discriminator == 15)
                {
                    return this.item15;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T15 item)
        {
            item = default;
            if (this.discriminator == 15)
            {
                item = this.item15;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5,
System.Func<T6, T6> cloneT6,
System.Func<T7, T7> cloneT7,
System.Func<T8, T8> cloneT8,
System.Func<T9, T9> cloneT9,
System.Func<T10, T10> cloneT10,
System.Func<T11, T11> cloneT11,
System.Func<T12, T12> cloneT12,
System.Func<T13, T13> cloneT13,
System.Func<T14, T14> cloneT14,
System.Func<T15, T15> cloneT15
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(cloneT5(this.item5));
                case 6:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(cloneT6(this.item6));
                case 7:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(cloneT7(this.item7));
                case 8:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(cloneT8(this.item8));
                case 9:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(cloneT9(this.item9));
                case 10:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(cloneT10(this.item10));
                case 11:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(cloneT11(this.item11));
                case 12:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(cloneT12(this.item12));
                case 13:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(cloneT13(this.item13));
                case 14:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(cloneT14(this.item14));
                case 15:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(cloneT15(this.item15));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5,
System.Action<T6> callback6,
System.Action<T7> callback7,
System.Action<T8> callback8,
System.Action<T9> callback9,
System.Action<T10> callback10,
System.Action<T11> callback11,
System.Action<T12> callback12,
System.Action<T13> callback13,
System.Action<T14> callback14,
System.Action<T15> callback15)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                case 6:
                    callback6(this.item6);
                    break;
                case 7:
                    callback7(this.item7);
                    break;
                case 8:
                    callback8(this.item8);
                    break;
                case 9:
                    callback9(this.item9);
                    break;
                case 10:
                    callback10(this.item10);
                    break;
                case 11:
                    callback11(this.item11);
                    break;
                case 12:
                    callback12(this.item12);
                    break;
                case 13:
                    callback13(this.item13);
                    break;
                case 14:
                    callback14(this.item14);
                    break;
                case 15:
                    callback15(this.item15);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5,
System.Action<TState, T6> callback6,
System.Action<TState, T7> callback7,
System.Action<TState, T8> callback8,
System.Action<TState, T9> callback9,
System.Action<TState, T10> callback10,
System.Action<TState, T11> callback11,
System.Action<TState, T12> callback12,
System.Action<TState, T13> callback13,
System.Action<TState, T14> callback14,
System.Action<TState, T15> callback15)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                case 6:
                    callback6(state, this.item6);
                    break;
                case 7:
                    callback7(state, this.item7);
                    break;
                case 8:
                    callback8(state, this.item8);
                    break;
                case 9:
                    callback9(state, this.item9);
                    break;
                case 10:
                    callback10(state, this.item10);
                    break;
                case 11:
                    callback11(state, this.item11);
                    break;
                case 12:
                    callback12(state, this.item12);
                    break;
                case 13:
                    callback13(state, this.item13);
                    break;
                case 14:
                    callback14(state, this.item14);
                    break;
                case 15:
                    callback15(state, this.item15);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5,
System.Func<T6, TResult> callback6,
System.Func<T7, TResult> callback7,
System.Func<T8, TResult> callback8,
System.Func<T9, TResult> callback9,
System.Func<T10, TResult> callback10,
System.Func<T11, TResult> callback11,
System.Func<T12, TResult> callback12,
System.Func<T13, TResult> callback13,
System.Func<T14, TResult> callback14,
System.Func<T15, TResult> callback15)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                case 6:
                    return callback6(this.item6);
                case 7:
                    return callback7(this.item7);
                case 8:
                    return callback8(this.item8);
                case 9:
                    return callback9(this.item9);
                case 10:
                    return callback10(this.item10);
                case 11:
                    return callback11(this.item11);
                case 12:
                    return callback12(this.item12);
                case 13:
                    return callback13(this.item13);
                case 14:
                    return callback14(this.item14);
                case 15:
                    return callback15(this.item15);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5,
System.Func<TState, T6, TResult> callback6,
System.Func<TState, T7, TResult> callback7,
System.Func<TState, T8, TResult> callback8,
System.Func<TState, T9, TResult> callback9,
System.Func<TState, T10, TResult> callback10,
System.Func<TState, T11, TResult> callback11,
System.Func<TState, T12, TResult> callback12,
System.Func<TState, T13, TResult> callback13,
System.Func<TState, T14, TResult> callback14,
System.Func<TState, T15, TResult> callback15)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                case 6:
                    return callback6(state, this.item6);
                case 7:
                    return callback7(state, this.item7);
                case 8:
                    return callback8(state, this.item8);
                case 9:
                    return callback9(state, this.item9);
                case 10:
                    return callback10(state, this.item10);
                case 11:
                    return callback11(state, this.item11);
                case 12:
                    return callback12(state, this.item12);
                case 13:
                    return callback13(state, this.item13);
                case 14:
                    return callback14(state, this.item14);
                case 15:
                    return callback15(state, this.item15);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;


        private readonly T6 item6;


        private readonly T7 item7;


        private readonly T8 item8;


        private readonly T9 item9;


        private readonly T10 item10;


        private readonly T11 item11;


        private readonly T12 item12;


        private readonly T13 item13;


        private readonly T14 item14;


        private readonly T15 item15;


        private readonly T16 item16;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public FlatBufferUnion(T6 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 6;
            this.item6 = item;
        }


        public FlatBufferUnion(T7 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 7;
            this.item7 = item;
        }


        public FlatBufferUnion(T8 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 8;
            this.item8 = item;
        }


        public FlatBufferUnion(T9 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 9;
            this.item9 = item;
        }


        public FlatBufferUnion(T10 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 10;
            this.item10 = item;
        }


        public FlatBufferUnion(T11 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 11;
            this.item11 = item;
        }


        public FlatBufferUnion(T12 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 12;
            this.item12 = item;
        }


        public FlatBufferUnion(T13 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 13;
            this.item13 = item;
        }


        public FlatBufferUnion(T14 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 14;
            this.item14 = item;
        }


        public FlatBufferUnion(T15 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 15;
            this.item15 = item;
        }


        public FlatBufferUnion(T16 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 16;
            this.item16 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public T6 Item6
        {
            get
            {
                if (this.discriminator == 6)
                {
                    return this.item6;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T6 item)
        {
            item = default;
            if (this.discriminator == 6)
            {
                item = this.item6;
                return true;
            }

            return false;
        }


        public T7 Item7
        {
            get
            {
                if (this.discriminator == 7)
                {
                    return this.item7;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T7 item)
        {
            item = default;
            if (this.discriminator == 7)
            {
                item = this.item7;
                return true;
            }

            return false;
        }


        public T8 Item8
        {
            get
            {
                if (this.discriminator == 8)
                {
                    return this.item8;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T8 item)
        {
            item = default;
            if (this.discriminator == 8)
            {
                item = this.item8;
                return true;
            }

            return false;
        }


        public T9 Item9
        {
            get
            {
                if (this.discriminator == 9)
                {
                    return this.item9;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T9 item)
        {
            item = default;
            if (this.discriminator == 9)
            {
                item = this.item9;
                return true;
            }

            return false;
        }


        public T10 Item10
        {
            get
            {
                if (this.discriminator == 10)
                {
                    return this.item10;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T10 item)
        {
            item = default;
            if (this.discriminator == 10)
            {
                item = this.item10;
                return true;
            }

            return false;
        }


        public T11 Item11
        {
            get
            {
                if (this.discriminator == 11)
                {
                    return this.item11;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T11 item)
        {
            item = default;
            if (this.discriminator == 11)
            {
                item = this.item11;
                return true;
            }

            return false;
        }


        public T12 Item12
        {
            get
            {
                if (this.discriminator == 12)
                {
                    return this.item12;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T12 item)
        {
            item = default;
            if (this.discriminator == 12)
            {
                item = this.item12;
                return true;
            }

            return false;
        }


        public T13 Item13
        {
            get
            {
                if (this.discriminator == 13)
                {
                    return this.item13;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T13 item)
        {
            item = default;
            if (this.discriminator == 13)
            {
                item = this.item13;
                return true;
            }

            return false;
        }


        public T14 Item14
        {
            get
            {
                if (this.discriminator == 14)
                {
                    return this.item14;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T14 item)
        {
            item = default;
            if (this.discriminator == 14)
            {
                item = this.item14;
                return true;
            }

            return false;
        }


        public T15 Item15
        {
            get
            {
                if (this.discriminator == 15)
                {
                    return this.item15;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T15 item)
        {
            item = default;
            if (this.discriminator == 15)
            {
                item = this.item15;
                return true;
            }

            return false;
        }


        public T16 Item16
        {
            get
            {
                if (this.discriminator == 16)
                {
                    return this.item16;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T16 item)
        {
            item = default;
            if (this.discriminator == 16)
            {
                item = this.item16;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5,
System.Func<T6, T6> cloneT6,
System.Func<T7, T7> cloneT7,
System.Func<T8, T8> cloneT8,
System.Func<T9, T9> cloneT9,
System.Func<T10, T10> cloneT10,
System.Func<T11, T11> cloneT11,
System.Func<T12, T12> cloneT12,
System.Func<T13, T13> cloneT13,
System.Func<T14, T14> cloneT14,
System.Func<T15, T15> cloneT15,
System.Func<T16, T16> cloneT16
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(cloneT5(this.item5));
                case 6:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(cloneT6(this.item6));
                case 7:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(cloneT7(this.item7));
                case 8:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(cloneT8(this.item8));
                case 9:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(cloneT9(this.item9));
                case 10:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(cloneT10(this.item10));
                case 11:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(cloneT11(this.item11));
                case 12:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(cloneT12(this.item12));
                case 13:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(cloneT13(this.item13));
                case 14:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(cloneT14(this.item14));
                case 15:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(cloneT15(this.item15));
                case 16:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(cloneT16(this.item16));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5,
System.Action<T6> callback6,
System.Action<T7> callback7,
System.Action<T8> callback8,
System.Action<T9> callback9,
System.Action<T10> callback10,
System.Action<T11> callback11,
System.Action<T12> callback12,
System.Action<T13> callback13,
System.Action<T14> callback14,
System.Action<T15> callback15,
System.Action<T16> callback16)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                case 6:
                    callback6(this.item6);
                    break;
                case 7:
                    callback7(this.item7);
                    break;
                case 8:
                    callback8(this.item8);
                    break;
                case 9:
                    callback9(this.item9);
                    break;
                case 10:
                    callback10(this.item10);
                    break;
                case 11:
                    callback11(this.item11);
                    break;
                case 12:
                    callback12(this.item12);
                    break;
                case 13:
                    callback13(this.item13);
                    break;
                case 14:
                    callback14(this.item14);
                    break;
                case 15:
                    callback15(this.item15);
                    break;
                case 16:
                    callback16(this.item16);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5,
System.Action<TState, T6> callback6,
System.Action<TState, T7> callback7,
System.Action<TState, T8> callback8,
System.Action<TState, T9> callback9,
System.Action<TState, T10> callback10,
System.Action<TState, T11> callback11,
System.Action<TState, T12> callback12,
System.Action<TState, T13> callback13,
System.Action<TState, T14> callback14,
System.Action<TState, T15> callback15,
System.Action<TState, T16> callback16)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                case 6:
                    callback6(state, this.item6);
                    break;
                case 7:
                    callback7(state, this.item7);
                    break;
                case 8:
                    callback8(state, this.item8);
                    break;
                case 9:
                    callback9(state, this.item9);
                    break;
                case 10:
                    callback10(state, this.item10);
                    break;
                case 11:
                    callback11(state, this.item11);
                    break;
                case 12:
                    callback12(state, this.item12);
                    break;
                case 13:
                    callback13(state, this.item13);
                    break;
                case 14:
                    callback14(state, this.item14);
                    break;
                case 15:
                    callback15(state, this.item15);
                    break;
                case 16:
                    callback16(state, this.item16);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5,
System.Func<T6, TResult> callback6,
System.Func<T7, TResult> callback7,
System.Func<T8, TResult> callback8,
System.Func<T9, TResult> callback9,
System.Func<T10, TResult> callback10,
System.Func<T11, TResult> callback11,
System.Func<T12, TResult> callback12,
System.Func<T13, TResult> callback13,
System.Func<T14, TResult> callback14,
System.Func<T15, TResult> callback15,
System.Func<T16, TResult> callback16)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                case 6:
                    return callback6(this.item6);
                case 7:
                    return callback7(this.item7);
                case 8:
                    return callback8(this.item8);
                case 9:
                    return callback9(this.item9);
                case 10:
                    return callback10(this.item10);
                case 11:
                    return callback11(this.item11);
                case 12:
                    return callback12(this.item12);
                case 13:
                    return callback13(this.item13);
                case 14:
                    return callback14(this.item14);
                case 15:
                    return callback15(this.item15);
                case 16:
                    return callback16(this.item16);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5,
System.Func<TState, T6, TResult> callback6,
System.Func<TState, T7, TResult> callback7,
System.Func<TState, T8, TResult> callback8,
System.Func<TState, T9, TResult> callback9,
System.Func<TState, T10, TResult> callback10,
System.Func<TState, T11, TResult> callback11,
System.Func<TState, T12, TResult> callback12,
System.Func<TState, T13, TResult> callback13,
System.Func<TState, T14, TResult> callback14,
System.Func<TState, T15, TResult> callback15,
System.Func<TState, T16, TResult> callback16)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                case 6:
                    return callback6(state, this.item6);
                case 7:
                    return callback7(state, this.item7);
                case 8:
                    return callback8(state, this.item8);
                case 9:
                    return callback9(state, this.item9);
                case 10:
                    return callback10(state, this.item10);
                case 11:
                    return callback11(state, this.item11);
                case 12:
                    return callback12(state, this.item12);
                case 13:
                    return callback13(state, this.item13);
                case 14:
                    return callback14(state, this.item14);
                case 15:
                    return callback15(state, this.item15);
                case 16:
                    return callback16(state, this.item16);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;


        private readonly T6 item6;


        private readonly T7 item7;


        private readonly T8 item8;


        private readonly T9 item9;


        private readonly T10 item10;


        private readonly T11 item11;


        private readonly T12 item12;


        private readonly T13 item13;


        private readonly T14 item14;


        private readonly T15 item15;


        private readonly T16 item16;


        private readonly T17 item17;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public FlatBufferUnion(T6 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 6;
            this.item6 = item;
        }


        public FlatBufferUnion(T7 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 7;
            this.item7 = item;
        }


        public FlatBufferUnion(T8 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 8;
            this.item8 = item;
        }


        public FlatBufferUnion(T9 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 9;
            this.item9 = item;
        }


        public FlatBufferUnion(T10 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 10;
            this.item10 = item;
        }


        public FlatBufferUnion(T11 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 11;
            this.item11 = item;
        }


        public FlatBufferUnion(T12 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 12;
            this.item12 = item;
        }


        public FlatBufferUnion(T13 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 13;
            this.item13 = item;
        }


        public FlatBufferUnion(T14 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 14;
            this.item14 = item;
        }


        public FlatBufferUnion(T15 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 15;
            this.item15 = item;
        }


        public FlatBufferUnion(T16 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 16;
            this.item16 = item;
        }


        public FlatBufferUnion(T17 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 17;
            this.item17 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public T6 Item6
        {
            get
            {
                if (this.discriminator == 6)
                {
                    return this.item6;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T6 item)
        {
            item = default;
            if (this.discriminator == 6)
            {
                item = this.item6;
                return true;
            }

            return false;
        }


        public T7 Item7
        {
            get
            {
                if (this.discriminator == 7)
                {
                    return this.item7;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T7 item)
        {
            item = default;
            if (this.discriminator == 7)
            {
                item = this.item7;
                return true;
            }

            return false;
        }


        public T8 Item8
        {
            get
            {
                if (this.discriminator == 8)
                {
                    return this.item8;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T8 item)
        {
            item = default;
            if (this.discriminator == 8)
            {
                item = this.item8;
                return true;
            }

            return false;
        }


        public T9 Item9
        {
            get
            {
                if (this.discriminator == 9)
                {
                    return this.item9;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T9 item)
        {
            item = default;
            if (this.discriminator == 9)
            {
                item = this.item9;
                return true;
            }

            return false;
        }


        public T10 Item10
        {
            get
            {
                if (this.discriminator == 10)
                {
                    return this.item10;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T10 item)
        {
            item = default;
            if (this.discriminator == 10)
            {
                item = this.item10;
                return true;
            }

            return false;
        }


        public T11 Item11
        {
            get
            {
                if (this.discriminator == 11)
                {
                    return this.item11;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T11 item)
        {
            item = default;
            if (this.discriminator == 11)
            {
                item = this.item11;
                return true;
            }

            return false;
        }


        public T12 Item12
        {
            get
            {
                if (this.discriminator == 12)
                {
                    return this.item12;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T12 item)
        {
            item = default;
            if (this.discriminator == 12)
            {
                item = this.item12;
                return true;
            }

            return false;
        }


        public T13 Item13
        {
            get
            {
                if (this.discriminator == 13)
                {
                    return this.item13;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T13 item)
        {
            item = default;
            if (this.discriminator == 13)
            {
                item = this.item13;
                return true;
            }

            return false;
        }


        public T14 Item14
        {
            get
            {
                if (this.discriminator == 14)
                {
                    return this.item14;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T14 item)
        {
            item = default;
            if (this.discriminator == 14)
            {
                item = this.item14;
                return true;
            }

            return false;
        }


        public T15 Item15
        {
            get
            {
                if (this.discriminator == 15)
                {
                    return this.item15;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T15 item)
        {
            item = default;
            if (this.discriminator == 15)
            {
                item = this.item15;
                return true;
            }

            return false;
        }


        public T16 Item16
        {
            get
            {
                if (this.discriminator == 16)
                {
                    return this.item16;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T16 item)
        {
            item = default;
            if (this.discriminator == 16)
            {
                item = this.item16;
                return true;
            }

            return false;
        }


        public T17 Item17
        {
            get
            {
                if (this.discriminator == 17)
                {
                    return this.item17;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T17 item)
        {
            item = default;
            if (this.discriminator == 17)
            {
                item = this.item17;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5,
System.Func<T6, T6> cloneT6,
System.Func<T7, T7> cloneT7,
System.Func<T8, T8> cloneT8,
System.Func<T9, T9> cloneT9,
System.Func<T10, T10> cloneT10,
System.Func<T11, T11> cloneT11,
System.Func<T12, T12> cloneT12,
System.Func<T13, T13> cloneT13,
System.Func<T14, T14> cloneT14,
System.Func<T15, T15> cloneT15,
System.Func<T16, T16> cloneT16,
System.Func<T17, T17> cloneT17
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(cloneT5(this.item5));
                case 6:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(cloneT6(this.item6));
                case 7:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(cloneT7(this.item7));
                case 8:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(cloneT8(this.item8));
                case 9:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(cloneT9(this.item9));
                case 10:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(cloneT10(this.item10));
                case 11:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(cloneT11(this.item11));
                case 12:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(cloneT12(this.item12));
                case 13:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(cloneT13(this.item13));
                case 14:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(cloneT14(this.item14));
                case 15:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(cloneT15(this.item15));
                case 16:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(cloneT16(this.item16));
                case 17:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(cloneT17(this.item17));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5,
System.Action<T6> callback6,
System.Action<T7> callback7,
System.Action<T8> callback8,
System.Action<T9> callback9,
System.Action<T10> callback10,
System.Action<T11> callback11,
System.Action<T12> callback12,
System.Action<T13> callback13,
System.Action<T14> callback14,
System.Action<T15> callback15,
System.Action<T16> callback16,
System.Action<T17> callback17)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                case 6:
                    callback6(this.item6);
                    break;
                case 7:
                    callback7(this.item7);
                    break;
                case 8:
                    callback8(this.item8);
                    break;
                case 9:
                    callback9(this.item9);
                    break;
                case 10:
                    callback10(this.item10);
                    break;
                case 11:
                    callback11(this.item11);
                    break;
                case 12:
                    callback12(this.item12);
                    break;
                case 13:
                    callback13(this.item13);
                    break;
                case 14:
                    callback14(this.item14);
                    break;
                case 15:
                    callback15(this.item15);
                    break;
                case 16:
                    callback16(this.item16);
                    break;
                case 17:
                    callback17(this.item17);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5,
System.Action<TState, T6> callback6,
System.Action<TState, T7> callback7,
System.Action<TState, T8> callback8,
System.Action<TState, T9> callback9,
System.Action<TState, T10> callback10,
System.Action<TState, T11> callback11,
System.Action<TState, T12> callback12,
System.Action<TState, T13> callback13,
System.Action<TState, T14> callback14,
System.Action<TState, T15> callback15,
System.Action<TState, T16> callback16,
System.Action<TState, T17> callback17)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                case 6:
                    callback6(state, this.item6);
                    break;
                case 7:
                    callback7(state, this.item7);
                    break;
                case 8:
                    callback8(state, this.item8);
                    break;
                case 9:
                    callback9(state, this.item9);
                    break;
                case 10:
                    callback10(state, this.item10);
                    break;
                case 11:
                    callback11(state, this.item11);
                    break;
                case 12:
                    callback12(state, this.item12);
                    break;
                case 13:
                    callback13(state, this.item13);
                    break;
                case 14:
                    callback14(state, this.item14);
                    break;
                case 15:
                    callback15(state, this.item15);
                    break;
                case 16:
                    callback16(state, this.item16);
                    break;
                case 17:
                    callback17(state, this.item17);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5,
System.Func<T6, TResult> callback6,
System.Func<T7, TResult> callback7,
System.Func<T8, TResult> callback8,
System.Func<T9, TResult> callback9,
System.Func<T10, TResult> callback10,
System.Func<T11, TResult> callback11,
System.Func<T12, TResult> callback12,
System.Func<T13, TResult> callback13,
System.Func<T14, TResult> callback14,
System.Func<T15, TResult> callback15,
System.Func<T16, TResult> callback16,
System.Func<T17, TResult> callback17)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                case 6:
                    return callback6(this.item6);
                case 7:
                    return callback7(this.item7);
                case 8:
                    return callback8(this.item8);
                case 9:
                    return callback9(this.item9);
                case 10:
                    return callback10(this.item10);
                case 11:
                    return callback11(this.item11);
                case 12:
                    return callback12(this.item12);
                case 13:
                    return callback13(this.item13);
                case 14:
                    return callback14(this.item14);
                case 15:
                    return callback15(this.item15);
                case 16:
                    return callback16(this.item16);
                case 17:
                    return callback17(this.item17);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5,
System.Func<TState, T6, TResult> callback6,
System.Func<TState, T7, TResult> callback7,
System.Func<TState, T8, TResult> callback8,
System.Func<TState, T9, TResult> callback9,
System.Func<TState, T10, TResult> callback10,
System.Func<TState, T11, TResult> callback11,
System.Func<TState, T12, TResult> callback12,
System.Func<TState, T13, TResult> callback13,
System.Func<TState, T14, TResult> callback14,
System.Func<TState, T15, TResult> callback15,
System.Func<TState, T16, TResult> callback16,
System.Func<TState, T17, TResult> callback17)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                case 6:
                    return callback6(state, this.item6);
                case 7:
                    return callback7(state, this.item7);
                case 8:
                    return callback8(state, this.item8);
                case 9:
                    return callback9(state, this.item9);
                case 10:
                    return callback10(state, this.item10);
                case 11:
                    return callback11(state, this.item11);
                case 12:
                    return callback12(state, this.item12);
                case 13:
                    return callback13(state, this.item13);
                case 14:
                    return callback14(state, this.item14);
                case 15:
                    return callback15(state, this.item15);
                case 16:
                    return callback16(state, this.item16);
                case 17:
                    return callback17(state, this.item17);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;


        private readonly T6 item6;


        private readonly T7 item7;


        private readonly T8 item8;


        private readonly T9 item9;


        private readonly T10 item10;


        private readonly T11 item11;


        private readonly T12 item12;


        private readonly T13 item13;


        private readonly T14 item14;


        private readonly T15 item15;


        private readonly T16 item16;


        private readonly T17 item17;


        private readonly T18 item18;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public FlatBufferUnion(T6 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 6;
            this.item6 = item;
        }


        public FlatBufferUnion(T7 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 7;
            this.item7 = item;
        }


        public FlatBufferUnion(T8 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 8;
            this.item8 = item;
        }


        public FlatBufferUnion(T9 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 9;
            this.item9 = item;
        }


        public FlatBufferUnion(T10 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 10;
            this.item10 = item;
        }


        public FlatBufferUnion(T11 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 11;
            this.item11 = item;
        }


        public FlatBufferUnion(T12 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 12;
            this.item12 = item;
        }


        public FlatBufferUnion(T13 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 13;
            this.item13 = item;
        }


        public FlatBufferUnion(T14 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 14;
            this.item14 = item;
        }


        public FlatBufferUnion(T15 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 15;
            this.item15 = item;
        }


        public FlatBufferUnion(T16 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 16;
            this.item16 = item;
        }


        public FlatBufferUnion(T17 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 17;
            this.item17 = item;
        }


        public FlatBufferUnion(T18 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 18;
            this.item18 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public T6 Item6
        {
            get
            {
                if (this.discriminator == 6)
                {
                    return this.item6;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T6 item)
        {
            item = default;
            if (this.discriminator == 6)
            {
                item = this.item6;
                return true;
            }

            return false;
        }


        public T7 Item7
        {
            get
            {
                if (this.discriminator == 7)
                {
                    return this.item7;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T7 item)
        {
            item = default;
            if (this.discriminator == 7)
            {
                item = this.item7;
                return true;
            }

            return false;
        }


        public T8 Item8
        {
            get
            {
                if (this.discriminator == 8)
                {
                    return this.item8;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T8 item)
        {
            item = default;
            if (this.discriminator == 8)
            {
                item = this.item8;
                return true;
            }

            return false;
        }


        public T9 Item9
        {
            get
            {
                if (this.discriminator == 9)
                {
                    return this.item9;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T9 item)
        {
            item = default;
            if (this.discriminator == 9)
            {
                item = this.item9;
                return true;
            }

            return false;
        }


        public T10 Item10
        {
            get
            {
                if (this.discriminator == 10)
                {
                    return this.item10;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T10 item)
        {
            item = default;
            if (this.discriminator == 10)
            {
                item = this.item10;
                return true;
            }

            return false;
        }


        public T11 Item11
        {
            get
            {
                if (this.discriminator == 11)
                {
                    return this.item11;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T11 item)
        {
            item = default;
            if (this.discriminator == 11)
            {
                item = this.item11;
                return true;
            }

            return false;
        }


        public T12 Item12
        {
            get
            {
                if (this.discriminator == 12)
                {
                    return this.item12;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T12 item)
        {
            item = default;
            if (this.discriminator == 12)
            {
                item = this.item12;
                return true;
            }

            return false;
        }


        public T13 Item13
        {
            get
            {
                if (this.discriminator == 13)
                {
                    return this.item13;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T13 item)
        {
            item = default;
            if (this.discriminator == 13)
            {
                item = this.item13;
                return true;
            }

            return false;
        }


        public T14 Item14
        {
            get
            {
                if (this.discriminator == 14)
                {
                    return this.item14;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T14 item)
        {
            item = default;
            if (this.discriminator == 14)
            {
                item = this.item14;
                return true;
            }

            return false;
        }


        public T15 Item15
        {
            get
            {
                if (this.discriminator == 15)
                {
                    return this.item15;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T15 item)
        {
            item = default;
            if (this.discriminator == 15)
            {
                item = this.item15;
                return true;
            }

            return false;
        }


        public T16 Item16
        {
            get
            {
                if (this.discriminator == 16)
                {
                    return this.item16;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T16 item)
        {
            item = default;
            if (this.discriminator == 16)
            {
                item = this.item16;
                return true;
            }

            return false;
        }


        public T17 Item17
        {
            get
            {
                if (this.discriminator == 17)
                {
                    return this.item17;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T17 item)
        {
            item = default;
            if (this.discriminator == 17)
            {
                item = this.item17;
                return true;
            }

            return false;
        }


        public T18 Item18
        {
            get
            {
                if (this.discriminator == 18)
                {
                    return this.item18;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T18 item)
        {
            item = default;
            if (this.discriminator == 18)
            {
                item = this.item18;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5,
System.Func<T6, T6> cloneT6,
System.Func<T7, T7> cloneT7,
System.Func<T8, T8> cloneT8,
System.Func<T9, T9> cloneT9,
System.Func<T10, T10> cloneT10,
System.Func<T11, T11> cloneT11,
System.Func<T12, T12> cloneT12,
System.Func<T13, T13> cloneT13,
System.Func<T14, T14> cloneT14,
System.Func<T15, T15> cloneT15,
System.Func<T16, T16> cloneT16,
System.Func<T17, T17> cloneT17,
System.Func<T18, T18> cloneT18
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(cloneT5(this.item5));
                case 6:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(cloneT6(this.item6));
                case 7:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(cloneT7(this.item7));
                case 8:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(cloneT8(this.item8));
                case 9:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(cloneT9(this.item9));
                case 10:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(cloneT10(this.item10));
                case 11:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(cloneT11(this.item11));
                case 12:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(cloneT12(this.item12));
                case 13:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(cloneT13(this.item13));
                case 14:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(cloneT14(this.item14));
                case 15:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(cloneT15(this.item15));
                case 16:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(cloneT16(this.item16));
                case 17:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(cloneT17(this.item17));
                case 18:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(cloneT18(this.item18));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5,
System.Action<T6> callback6,
System.Action<T7> callback7,
System.Action<T8> callback8,
System.Action<T9> callback9,
System.Action<T10> callback10,
System.Action<T11> callback11,
System.Action<T12> callback12,
System.Action<T13> callback13,
System.Action<T14> callback14,
System.Action<T15> callback15,
System.Action<T16> callback16,
System.Action<T17> callback17,
System.Action<T18> callback18)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                case 6:
                    callback6(this.item6);
                    break;
                case 7:
                    callback7(this.item7);
                    break;
                case 8:
                    callback8(this.item8);
                    break;
                case 9:
                    callback9(this.item9);
                    break;
                case 10:
                    callback10(this.item10);
                    break;
                case 11:
                    callback11(this.item11);
                    break;
                case 12:
                    callback12(this.item12);
                    break;
                case 13:
                    callback13(this.item13);
                    break;
                case 14:
                    callback14(this.item14);
                    break;
                case 15:
                    callback15(this.item15);
                    break;
                case 16:
                    callback16(this.item16);
                    break;
                case 17:
                    callback17(this.item17);
                    break;
                case 18:
                    callback18(this.item18);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5,
System.Action<TState, T6> callback6,
System.Action<TState, T7> callback7,
System.Action<TState, T8> callback8,
System.Action<TState, T9> callback9,
System.Action<TState, T10> callback10,
System.Action<TState, T11> callback11,
System.Action<TState, T12> callback12,
System.Action<TState, T13> callback13,
System.Action<TState, T14> callback14,
System.Action<TState, T15> callback15,
System.Action<TState, T16> callback16,
System.Action<TState, T17> callback17,
System.Action<TState, T18> callback18)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                case 6:
                    callback6(state, this.item6);
                    break;
                case 7:
                    callback7(state, this.item7);
                    break;
                case 8:
                    callback8(state, this.item8);
                    break;
                case 9:
                    callback9(state, this.item9);
                    break;
                case 10:
                    callback10(state, this.item10);
                    break;
                case 11:
                    callback11(state, this.item11);
                    break;
                case 12:
                    callback12(state, this.item12);
                    break;
                case 13:
                    callback13(state, this.item13);
                    break;
                case 14:
                    callback14(state, this.item14);
                    break;
                case 15:
                    callback15(state, this.item15);
                    break;
                case 16:
                    callback16(state, this.item16);
                    break;
                case 17:
                    callback17(state, this.item17);
                    break;
                case 18:
                    callback18(state, this.item18);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5,
System.Func<T6, TResult> callback6,
System.Func<T7, TResult> callback7,
System.Func<T8, TResult> callback8,
System.Func<T9, TResult> callback9,
System.Func<T10, TResult> callback10,
System.Func<T11, TResult> callback11,
System.Func<T12, TResult> callback12,
System.Func<T13, TResult> callback13,
System.Func<T14, TResult> callback14,
System.Func<T15, TResult> callback15,
System.Func<T16, TResult> callback16,
System.Func<T17, TResult> callback17,
System.Func<T18, TResult> callback18)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                case 6:
                    return callback6(this.item6);
                case 7:
                    return callback7(this.item7);
                case 8:
                    return callback8(this.item8);
                case 9:
                    return callback9(this.item9);
                case 10:
                    return callback10(this.item10);
                case 11:
                    return callback11(this.item11);
                case 12:
                    return callback12(this.item12);
                case 13:
                    return callback13(this.item13);
                case 14:
                    return callback14(this.item14);
                case 15:
                    return callback15(this.item15);
                case 16:
                    return callback16(this.item16);
                case 17:
                    return callback17(this.item17);
                case 18:
                    return callback18(this.item18);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5,
System.Func<TState, T6, TResult> callback6,
System.Func<TState, T7, TResult> callback7,
System.Func<TState, T8, TResult> callback8,
System.Func<TState, T9, TResult> callback9,
System.Func<TState, T10, TResult> callback10,
System.Func<TState, T11, TResult> callback11,
System.Func<TState, T12, TResult> callback12,
System.Func<TState, T13, TResult> callback13,
System.Func<TState, T14, TResult> callback14,
System.Func<TState, T15, TResult> callback15,
System.Func<TState, T16, TResult> callback16,
System.Func<TState, T17, TResult> callback17,
System.Func<TState, T18, TResult> callback18)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                case 6:
                    return callback6(state, this.item6);
                case 7:
                    return callback7(state, this.item7);
                case 8:
                    return callback8(state, this.item8);
                case 9:
                    return callback9(state, this.item9);
                case 10:
                    return callback10(state, this.item10);
                case 11:
                    return callback11(state, this.item11);
                case 12:
                    return callback12(state, this.item12);
                case 13:
                    return callback13(state, this.item13);
                case 14:
                    return callback14(state, this.item14);
                case 15:
                    return callback15(state, this.item15);
                case 16:
                    return callback16(state, this.item16);
                case 17:
                    return callback17(state, this.item17);
                case 18:
                    return callback18(state, this.item18);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;


        private readonly T6 item6;


        private readonly T7 item7;


        private readonly T8 item8;


        private readonly T9 item9;


        private readonly T10 item10;


        private readonly T11 item11;


        private readonly T12 item12;


        private readonly T13 item13;


        private readonly T14 item14;


        private readonly T15 item15;


        private readonly T16 item16;


        private readonly T17 item17;


        private readonly T18 item18;


        private readonly T19 item19;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public FlatBufferUnion(T6 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 6;
            this.item6 = item;
        }


        public FlatBufferUnion(T7 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 7;
            this.item7 = item;
        }


        public FlatBufferUnion(T8 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 8;
            this.item8 = item;
        }


        public FlatBufferUnion(T9 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 9;
            this.item9 = item;
        }


        public FlatBufferUnion(T10 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 10;
            this.item10 = item;
        }


        public FlatBufferUnion(T11 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 11;
            this.item11 = item;
        }


        public FlatBufferUnion(T12 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 12;
            this.item12 = item;
        }


        public FlatBufferUnion(T13 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 13;
            this.item13 = item;
        }


        public FlatBufferUnion(T14 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 14;
            this.item14 = item;
        }


        public FlatBufferUnion(T15 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 15;
            this.item15 = item;
        }


        public FlatBufferUnion(T16 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 16;
            this.item16 = item;
        }


        public FlatBufferUnion(T17 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 17;
            this.item17 = item;
        }


        public FlatBufferUnion(T18 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 18;
            this.item18 = item;
        }


        public FlatBufferUnion(T19 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 19;
            this.item19 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public T6 Item6
        {
            get
            {
                if (this.discriminator == 6)
                {
                    return this.item6;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T6 item)
        {
            item = default;
            if (this.discriminator == 6)
            {
                item = this.item6;
                return true;
            }

            return false;
        }


        public T7 Item7
        {
            get
            {
                if (this.discriminator == 7)
                {
                    return this.item7;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T7 item)
        {
            item = default;
            if (this.discriminator == 7)
            {
                item = this.item7;
                return true;
            }

            return false;
        }


        public T8 Item8
        {
            get
            {
                if (this.discriminator == 8)
                {
                    return this.item8;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T8 item)
        {
            item = default;
            if (this.discriminator == 8)
            {
                item = this.item8;
                return true;
            }

            return false;
        }


        public T9 Item9
        {
            get
            {
                if (this.discriminator == 9)
                {
                    return this.item9;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T9 item)
        {
            item = default;
            if (this.discriminator == 9)
            {
                item = this.item9;
                return true;
            }

            return false;
        }


        public T10 Item10
        {
            get
            {
                if (this.discriminator == 10)
                {
                    return this.item10;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T10 item)
        {
            item = default;
            if (this.discriminator == 10)
            {
                item = this.item10;
                return true;
            }

            return false;
        }


        public T11 Item11
        {
            get
            {
                if (this.discriminator == 11)
                {
                    return this.item11;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T11 item)
        {
            item = default;
            if (this.discriminator == 11)
            {
                item = this.item11;
                return true;
            }

            return false;
        }


        public T12 Item12
        {
            get
            {
                if (this.discriminator == 12)
                {
                    return this.item12;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T12 item)
        {
            item = default;
            if (this.discriminator == 12)
            {
                item = this.item12;
                return true;
            }

            return false;
        }


        public T13 Item13
        {
            get
            {
                if (this.discriminator == 13)
                {
                    return this.item13;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T13 item)
        {
            item = default;
            if (this.discriminator == 13)
            {
                item = this.item13;
                return true;
            }

            return false;
        }


        public T14 Item14
        {
            get
            {
                if (this.discriminator == 14)
                {
                    return this.item14;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T14 item)
        {
            item = default;
            if (this.discriminator == 14)
            {
                item = this.item14;
                return true;
            }

            return false;
        }


        public T15 Item15
        {
            get
            {
                if (this.discriminator == 15)
                {
                    return this.item15;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T15 item)
        {
            item = default;
            if (this.discriminator == 15)
            {
                item = this.item15;
                return true;
            }

            return false;
        }


        public T16 Item16
        {
            get
            {
                if (this.discriminator == 16)
                {
                    return this.item16;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T16 item)
        {
            item = default;
            if (this.discriminator == 16)
            {
                item = this.item16;
                return true;
            }

            return false;
        }


        public T17 Item17
        {
            get
            {
                if (this.discriminator == 17)
                {
                    return this.item17;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T17 item)
        {
            item = default;
            if (this.discriminator == 17)
            {
                item = this.item17;
                return true;
            }

            return false;
        }


        public T18 Item18
        {
            get
            {
                if (this.discriminator == 18)
                {
                    return this.item18;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T18 item)
        {
            item = default;
            if (this.discriminator == 18)
            {
                item = this.item18;
                return true;
            }

            return false;
        }


        public T19 Item19
        {
            get
            {
                if (this.discriminator == 19)
                {
                    return this.item19;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T19 item)
        {
            item = default;
            if (this.discriminator == 19)
            {
                item = this.item19;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5,
System.Func<T6, T6> cloneT6,
System.Func<T7, T7> cloneT7,
System.Func<T8, T8> cloneT8,
System.Func<T9, T9> cloneT9,
System.Func<T10, T10> cloneT10,
System.Func<T11, T11> cloneT11,
System.Func<T12, T12> cloneT12,
System.Func<T13, T13> cloneT13,
System.Func<T14, T14> cloneT14,
System.Func<T15, T15> cloneT15,
System.Func<T16, T16> cloneT16,
System.Func<T17, T17> cloneT17,
System.Func<T18, T18> cloneT18,
System.Func<T19, T19> cloneT19
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(cloneT5(this.item5));
                case 6:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(cloneT6(this.item6));
                case 7:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(cloneT7(this.item7));
                case 8:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(cloneT8(this.item8));
                case 9:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(cloneT9(this.item9));
                case 10:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(cloneT10(this.item10));
                case 11:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(cloneT11(this.item11));
                case 12:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(cloneT12(this.item12));
                case 13:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(cloneT13(this.item13));
                case 14:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(cloneT14(this.item14));
                case 15:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(cloneT15(this.item15));
                case 16:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(cloneT16(this.item16));
                case 17:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(cloneT17(this.item17));
                case 18:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(cloneT18(this.item18));
                case 19:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(cloneT19(this.item19));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5,
System.Action<T6> callback6,
System.Action<T7> callback7,
System.Action<T8> callback8,
System.Action<T9> callback9,
System.Action<T10> callback10,
System.Action<T11> callback11,
System.Action<T12> callback12,
System.Action<T13> callback13,
System.Action<T14> callback14,
System.Action<T15> callback15,
System.Action<T16> callback16,
System.Action<T17> callback17,
System.Action<T18> callback18,
System.Action<T19> callback19)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                case 6:
                    callback6(this.item6);
                    break;
                case 7:
                    callback7(this.item7);
                    break;
                case 8:
                    callback8(this.item8);
                    break;
                case 9:
                    callback9(this.item9);
                    break;
                case 10:
                    callback10(this.item10);
                    break;
                case 11:
                    callback11(this.item11);
                    break;
                case 12:
                    callback12(this.item12);
                    break;
                case 13:
                    callback13(this.item13);
                    break;
                case 14:
                    callback14(this.item14);
                    break;
                case 15:
                    callback15(this.item15);
                    break;
                case 16:
                    callback16(this.item16);
                    break;
                case 17:
                    callback17(this.item17);
                    break;
                case 18:
                    callback18(this.item18);
                    break;
                case 19:
                    callback19(this.item19);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5,
System.Action<TState, T6> callback6,
System.Action<TState, T7> callback7,
System.Action<TState, T8> callback8,
System.Action<TState, T9> callback9,
System.Action<TState, T10> callback10,
System.Action<TState, T11> callback11,
System.Action<TState, T12> callback12,
System.Action<TState, T13> callback13,
System.Action<TState, T14> callback14,
System.Action<TState, T15> callback15,
System.Action<TState, T16> callback16,
System.Action<TState, T17> callback17,
System.Action<TState, T18> callback18,
System.Action<TState, T19> callback19)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                case 6:
                    callback6(state, this.item6);
                    break;
                case 7:
                    callback7(state, this.item7);
                    break;
                case 8:
                    callback8(state, this.item8);
                    break;
                case 9:
                    callback9(state, this.item9);
                    break;
                case 10:
                    callback10(state, this.item10);
                    break;
                case 11:
                    callback11(state, this.item11);
                    break;
                case 12:
                    callback12(state, this.item12);
                    break;
                case 13:
                    callback13(state, this.item13);
                    break;
                case 14:
                    callback14(state, this.item14);
                    break;
                case 15:
                    callback15(state, this.item15);
                    break;
                case 16:
                    callback16(state, this.item16);
                    break;
                case 17:
                    callback17(state, this.item17);
                    break;
                case 18:
                    callback18(state, this.item18);
                    break;
                case 19:
                    callback19(state, this.item19);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5,
System.Func<T6, TResult> callback6,
System.Func<T7, TResult> callback7,
System.Func<T8, TResult> callback8,
System.Func<T9, TResult> callback9,
System.Func<T10, TResult> callback10,
System.Func<T11, TResult> callback11,
System.Func<T12, TResult> callback12,
System.Func<T13, TResult> callback13,
System.Func<T14, TResult> callback14,
System.Func<T15, TResult> callback15,
System.Func<T16, TResult> callback16,
System.Func<T17, TResult> callback17,
System.Func<T18, TResult> callback18,
System.Func<T19, TResult> callback19)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                case 6:
                    return callback6(this.item6);
                case 7:
                    return callback7(this.item7);
                case 8:
                    return callback8(this.item8);
                case 9:
                    return callback9(this.item9);
                case 10:
                    return callback10(this.item10);
                case 11:
                    return callback11(this.item11);
                case 12:
                    return callback12(this.item12);
                case 13:
                    return callback13(this.item13);
                case 14:
                    return callback14(this.item14);
                case 15:
                    return callback15(this.item15);
                case 16:
                    return callback16(this.item16);
                case 17:
                    return callback17(this.item17);
                case 18:
                    return callback18(this.item18);
                case 19:
                    return callback19(this.item19);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5,
System.Func<TState, T6, TResult> callback6,
System.Func<TState, T7, TResult> callback7,
System.Func<TState, T8, TResult> callback8,
System.Func<TState, T9, TResult> callback9,
System.Func<TState, T10, TResult> callback10,
System.Func<TState, T11, TResult> callback11,
System.Func<TState, T12, TResult> callback12,
System.Func<TState, T13, TResult> callback13,
System.Func<TState, T14, TResult> callback14,
System.Func<TState, T15, TResult> callback15,
System.Func<TState, T16, TResult> callback16,
System.Func<TState, T17, TResult> callback17,
System.Func<TState, T18, TResult> callback18,
System.Func<TState, T19, TResult> callback19)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                case 6:
                    return callback6(state, this.item6);
                case 7:
                    return callback7(state, this.item7);
                case 8:
                    return callback8(state, this.item8);
                case 9:
                    return callback9(state, this.item9);
                case 10:
                    return callback10(state, this.item10);
                case 11:
                    return callback11(state, this.item11);
                case 12:
                    return callback12(state, this.item12);
                case 13:
                    return callback13(state, this.item13);
                case 14:
                    return callback14(state, this.item14);
                case 15:
                    return callback15(state, this.item15);
                case 16:
                    return callback16(state, this.item16);
                case 17:
                    return callback17(state, this.item17);
                case 18:
                    return callback18(state, this.item18);
                case 19:
                    return callback19(state, this.item19);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;


        private readonly T6 item6;


        private readonly T7 item7;


        private readonly T8 item8;


        private readonly T9 item9;


        private readonly T10 item10;


        private readonly T11 item11;


        private readonly T12 item12;


        private readonly T13 item13;


        private readonly T14 item14;


        private readonly T15 item15;


        private readonly T16 item16;


        private readonly T17 item17;


        private readonly T18 item18;


        private readonly T19 item19;


        private readonly T20 item20;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public FlatBufferUnion(T6 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 6;
            this.item6 = item;
        }


        public FlatBufferUnion(T7 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 7;
            this.item7 = item;
        }


        public FlatBufferUnion(T8 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 8;
            this.item8 = item;
        }


        public FlatBufferUnion(T9 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 9;
            this.item9 = item;
        }


        public FlatBufferUnion(T10 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 10;
            this.item10 = item;
        }


        public FlatBufferUnion(T11 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 11;
            this.item11 = item;
        }


        public FlatBufferUnion(T12 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 12;
            this.item12 = item;
        }


        public FlatBufferUnion(T13 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 13;
            this.item13 = item;
        }


        public FlatBufferUnion(T14 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 14;
            this.item14 = item;
        }


        public FlatBufferUnion(T15 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 15;
            this.item15 = item;
        }


        public FlatBufferUnion(T16 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 16;
            this.item16 = item;
        }


        public FlatBufferUnion(T17 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 17;
            this.item17 = item;
        }


        public FlatBufferUnion(T18 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 18;
            this.item18 = item;
        }


        public FlatBufferUnion(T19 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 19;
            this.item19 = item;
        }


        public FlatBufferUnion(T20 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 20;
            this.item20 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public T6 Item6
        {
            get
            {
                if (this.discriminator == 6)
                {
                    return this.item6;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T6 item)
        {
            item = default;
            if (this.discriminator == 6)
            {
                item = this.item6;
                return true;
            }

            return false;
        }


        public T7 Item7
        {
            get
            {
                if (this.discriminator == 7)
                {
                    return this.item7;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T7 item)
        {
            item = default;
            if (this.discriminator == 7)
            {
                item = this.item7;
                return true;
            }

            return false;
        }


        public T8 Item8
        {
            get
            {
                if (this.discriminator == 8)
                {
                    return this.item8;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T8 item)
        {
            item = default;
            if (this.discriminator == 8)
            {
                item = this.item8;
                return true;
            }

            return false;
        }


        public T9 Item9
        {
            get
            {
                if (this.discriminator == 9)
                {
                    return this.item9;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T9 item)
        {
            item = default;
            if (this.discriminator == 9)
            {
                item = this.item9;
                return true;
            }

            return false;
        }


        public T10 Item10
        {
            get
            {
                if (this.discriminator == 10)
                {
                    return this.item10;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T10 item)
        {
            item = default;
            if (this.discriminator == 10)
            {
                item = this.item10;
                return true;
            }

            return false;
        }


        public T11 Item11
        {
            get
            {
                if (this.discriminator == 11)
                {
                    return this.item11;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T11 item)
        {
            item = default;
            if (this.discriminator == 11)
            {
                item = this.item11;
                return true;
            }

            return false;
        }


        public T12 Item12
        {
            get
            {
                if (this.discriminator == 12)
                {
                    return this.item12;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T12 item)
        {
            item = default;
            if (this.discriminator == 12)
            {
                item = this.item12;
                return true;
            }

            return false;
        }


        public T13 Item13
        {
            get
            {
                if (this.discriminator == 13)
                {
                    return this.item13;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T13 item)
        {
            item = default;
            if (this.discriminator == 13)
            {
                item = this.item13;
                return true;
            }

            return false;
        }


        public T14 Item14
        {
            get
            {
                if (this.discriminator == 14)
                {
                    return this.item14;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T14 item)
        {
            item = default;
            if (this.discriminator == 14)
            {
                item = this.item14;
                return true;
            }

            return false;
        }


        public T15 Item15
        {
            get
            {
                if (this.discriminator == 15)
                {
                    return this.item15;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T15 item)
        {
            item = default;
            if (this.discriminator == 15)
            {
                item = this.item15;
                return true;
            }

            return false;
        }


        public T16 Item16
        {
            get
            {
                if (this.discriminator == 16)
                {
                    return this.item16;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T16 item)
        {
            item = default;
            if (this.discriminator == 16)
            {
                item = this.item16;
                return true;
            }

            return false;
        }


        public T17 Item17
        {
            get
            {
                if (this.discriminator == 17)
                {
                    return this.item17;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T17 item)
        {
            item = default;
            if (this.discriminator == 17)
            {
                item = this.item17;
                return true;
            }

            return false;
        }


        public T18 Item18
        {
            get
            {
                if (this.discriminator == 18)
                {
                    return this.item18;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T18 item)
        {
            item = default;
            if (this.discriminator == 18)
            {
                item = this.item18;
                return true;
            }

            return false;
        }


        public T19 Item19
        {
            get
            {
                if (this.discriminator == 19)
                {
                    return this.item19;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T19 item)
        {
            item = default;
            if (this.discriminator == 19)
            {
                item = this.item19;
                return true;
            }

            return false;
        }


        public T20 Item20
        {
            get
            {
                if (this.discriminator == 20)
                {
                    return this.item20;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T20 item)
        {
            item = default;
            if (this.discriminator == 20)
            {
                item = this.item20;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5,
System.Func<T6, T6> cloneT6,
System.Func<T7, T7> cloneT7,
System.Func<T8, T8> cloneT8,
System.Func<T9, T9> cloneT9,
System.Func<T10, T10> cloneT10,
System.Func<T11, T11> cloneT11,
System.Func<T12, T12> cloneT12,
System.Func<T13, T13> cloneT13,
System.Func<T14, T14> cloneT14,
System.Func<T15, T15> cloneT15,
System.Func<T16, T16> cloneT16,
System.Func<T17, T17> cloneT17,
System.Func<T18, T18> cloneT18,
System.Func<T19, T19> cloneT19,
System.Func<T20, T20> cloneT20
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(cloneT5(this.item5));
                case 6:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(cloneT6(this.item6));
                case 7:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(cloneT7(this.item7));
                case 8:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(cloneT8(this.item8));
                case 9:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(cloneT9(this.item9));
                case 10:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(cloneT10(this.item10));
                case 11:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(cloneT11(this.item11));
                case 12:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(cloneT12(this.item12));
                case 13:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(cloneT13(this.item13));
                case 14:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(cloneT14(this.item14));
                case 15:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(cloneT15(this.item15));
                case 16:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(cloneT16(this.item16));
                case 17:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(cloneT17(this.item17));
                case 18:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(cloneT18(this.item18));
                case 19:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(cloneT19(this.item19));
                case 20:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(cloneT20(this.item20));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5,
System.Action<T6> callback6,
System.Action<T7> callback7,
System.Action<T8> callback8,
System.Action<T9> callback9,
System.Action<T10> callback10,
System.Action<T11> callback11,
System.Action<T12> callback12,
System.Action<T13> callback13,
System.Action<T14> callback14,
System.Action<T15> callback15,
System.Action<T16> callback16,
System.Action<T17> callback17,
System.Action<T18> callback18,
System.Action<T19> callback19,
System.Action<T20> callback20)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                case 6:
                    callback6(this.item6);
                    break;
                case 7:
                    callback7(this.item7);
                    break;
                case 8:
                    callback8(this.item8);
                    break;
                case 9:
                    callback9(this.item9);
                    break;
                case 10:
                    callback10(this.item10);
                    break;
                case 11:
                    callback11(this.item11);
                    break;
                case 12:
                    callback12(this.item12);
                    break;
                case 13:
                    callback13(this.item13);
                    break;
                case 14:
                    callback14(this.item14);
                    break;
                case 15:
                    callback15(this.item15);
                    break;
                case 16:
                    callback16(this.item16);
                    break;
                case 17:
                    callback17(this.item17);
                    break;
                case 18:
                    callback18(this.item18);
                    break;
                case 19:
                    callback19(this.item19);
                    break;
                case 20:
                    callback20(this.item20);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5,
System.Action<TState, T6> callback6,
System.Action<TState, T7> callback7,
System.Action<TState, T8> callback8,
System.Action<TState, T9> callback9,
System.Action<TState, T10> callback10,
System.Action<TState, T11> callback11,
System.Action<TState, T12> callback12,
System.Action<TState, T13> callback13,
System.Action<TState, T14> callback14,
System.Action<TState, T15> callback15,
System.Action<TState, T16> callback16,
System.Action<TState, T17> callback17,
System.Action<TState, T18> callback18,
System.Action<TState, T19> callback19,
System.Action<TState, T20> callback20)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                case 6:
                    callback6(state, this.item6);
                    break;
                case 7:
                    callback7(state, this.item7);
                    break;
                case 8:
                    callback8(state, this.item8);
                    break;
                case 9:
                    callback9(state, this.item9);
                    break;
                case 10:
                    callback10(state, this.item10);
                    break;
                case 11:
                    callback11(state, this.item11);
                    break;
                case 12:
                    callback12(state, this.item12);
                    break;
                case 13:
                    callback13(state, this.item13);
                    break;
                case 14:
                    callback14(state, this.item14);
                    break;
                case 15:
                    callback15(state, this.item15);
                    break;
                case 16:
                    callback16(state, this.item16);
                    break;
                case 17:
                    callback17(state, this.item17);
                    break;
                case 18:
                    callback18(state, this.item18);
                    break;
                case 19:
                    callback19(state, this.item19);
                    break;
                case 20:
                    callback20(state, this.item20);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5,
System.Func<T6, TResult> callback6,
System.Func<T7, TResult> callback7,
System.Func<T8, TResult> callback8,
System.Func<T9, TResult> callback9,
System.Func<T10, TResult> callback10,
System.Func<T11, TResult> callback11,
System.Func<T12, TResult> callback12,
System.Func<T13, TResult> callback13,
System.Func<T14, TResult> callback14,
System.Func<T15, TResult> callback15,
System.Func<T16, TResult> callback16,
System.Func<T17, TResult> callback17,
System.Func<T18, TResult> callback18,
System.Func<T19, TResult> callback19,
System.Func<T20, TResult> callback20)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                case 6:
                    return callback6(this.item6);
                case 7:
                    return callback7(this.item7);
                case 8:
                    return callback8(this.item8);
                case 9:
                    return callback9(this.item9);
                case 10:
                    return callback10(this.item10);
                case 11:
                    return callback11(this.item11);
                case 12:
                    return callback12(this.item12);
                case 13:
                    return callback13(this.item13);
                case 14:
                    return callback14(this.item14);
                case 15:
                    return callback15(this.item15);
                case 16:
                    return callback16(this.item16);
                case 17:
                    return callback17(this.item17);
                case 18:
                    return callback18(this.item18);
                case 19:
                    return callback19(this.item19);
                case 20:
                    return callback20(this.item20);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5,
System.Func<TState, T6, TResult> callback6,
System.Func<TState, T7, TResult> callback7,
System.Func<TState, T8, TResult> callback8,
System.Func<TState, T9, TResult> callback9,
System.Func<TState, T10, TResult> callback10,
System.Func<TState, T11, TResult> callback11,
System.Func<TState, T12, TResult> callback12,
System.Func<TState, T13, TResult> callback13,
System.Func<TState, T14, TResult> callback14,
System.Func<TState, T15, TResult> callback15,
System.Func<TState, T16, TResult> callback16,
System.Func<TState, T17, TResult> callback17,
System.Func<TState, T18, TResult> callback18,
System.Func<TState, T19, TResult> callback19,
System.Func<TState, T20, TResult> callback20)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                case 6:
                    return callback6(state, this.item6);
                case 7:
                    return callback7(state, this.item7);
                case 8:
                    return callback8(state, this.item8);
                case 9:
                    return callback9(state, this.item9);
                case 10:
                    return callback10(state, this.item10);
                case 11:
                    return callback11(state, this.item11);
                case 12:
                    return callback12(state, this.item12);
                case 13:
                    return callback13(state, this.item13);
                case 14:
                    return callback14(state, this.item14);
                case 15:
                    return callback15(state, this.item15);
                case 16:
                    return callback16(state, this.item16);
                case 17:
                    return callback17(state, this.item17);
                case 18:
                    return callback18(state, this.item18);
                case 19:
                    return callback19(state, this.item19);
                case 20:
                    return callback20(state, this.item20);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;


        private readonly T6 item6;


        private readonly T7 item7;


        private readonly T8 item8;


        private readonly T9 item9;


        private readonly T10 item10;


        private readonly T11 item11;


        private readonly T12 item12;


        private readonly T13 item13;


        private readonly T14 item14;


        private readonly T15 item15;


        private readonly T16 item16;


        private readonly T17 item17;


        private readonly T18 item18;


        private readonly T19 item19;


        private readonly T20 item20;


        private readonly T21 item21;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public FlatBufferUnion(T6 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 6;
            this.item6 = item;
        }


        public FlatBufferUnion(T7 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 7;
            this.item7 = item;
        }


        public FlatBufferUnion(T8 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 8;
            this.item8 = item;
        }


        public FlatBufferUnion(T9 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 9;
            this.item9 = item;
        }


        public FlatBufferUnion(T10 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 10;
            this.item10 = item;
        }


        public FlatBufferUnion(T11 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 11;
            this.item11 = item;
        }


        public FlatBufferUnion(T12 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 12;
            this.item12 = item;
        }


        public FlatBufferUnion(T13 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 13;
            this.item13 = item;
        }


        public FlatBufferUnion(T14 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 14;
            this.item14 = item;
        }


        public FlatBufferUnion(T15 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 15;
            this.item15 = item;
        }


        public FlatBufferUnion(T16 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 16;
            this.item16 = item;
        }


        public FlatBufferUnion(T17 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 17;
            this.item17 = item;
        }


        public FlatBufferUnion(T18 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 18;
            this.item18 = item;
        }


        public FlatBufferUnion(T19 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 19;
            this.item19 = item;
        }


        public FlatBufferUnion(T20 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 20;
            this.item20 = item;
        }


        public FlatBufferUnion(T21 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 21;
            this.item21 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public T6 Item6
        {
            get
            {
                if (this.discriminator == 6)
                {
                    return this.item6;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T6 item)
        {
            item = default;
            if (this.discriminator == 6)
            {
                item = this.item6;
                return true;
            }

            return false;
        }


        public T7 Item7
        {
            get
            {
                if (this.discriminator == 7)
                {
                    return this.item7;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T7 item)
        {
            item = default;
            if (this.discriminator == 7)
            {
                item = this.item7;
                return true;
            }

            return false;
        }


        public T8 Item8
        {
            get
            {
                if (this.discriminator == 8)
                {
                    return this.item8;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T8 item)
        {
            item = default;
            if (this.discriminator == 8)
            {
                item = this.item8;
                return true;
            }

            return false;
        }


        public T9 Item9
        {
            get
            {
                if (this.discriminator == 9)
                {
                    return this.item9;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T9 item)
        {
            item = default;
            if (this.discriminator == 9)
            {
                item = this.item9;
                return true;
            }

            return false;
        }


        public T10 Item10
        {
            get
            {
                if (this.discriminator == 10)
                {
                    return this.item10;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T10 item)
        {
            item = default;
            if (this.discriminator == 10)
            {
                item = this.item10;
                return true;
            }

            return false;
        }


        public T11 Item11
        {
            get
            {
                if (this.discriminator == 11)
                {
                    return this.item11;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T11 item)
        {
            item = default;
            if (this.discriminator == 11)
            {
                item = this.item11;
                return true;
            }

            return false;
        }


        public T12 Item12
        {
            get
            {
                if (this.discriminator == 12)
                {
                    return this.item12;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T12 item)
        {
            item = default;
            if (this.discriminator == 12)
            {
                item = this.item12;
                return true;
            }

            return false;
        }


        public T13 Item13
        {
            get
            {
                if (this.discriminator == 13)
                {
                    return this.item13;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T13 item)
        {
            item = default;
            if (this.discriminator == 13)
            {
                item = this.item13;
                return true;
            }

            return false;
        }


        public T14 Item14
        {
            get
            {
                if (this.discriminator == 14)
                {
                    return this.item14;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T14 item)
        {
            item = default;
            if (this.discriminator == 14)
            {
                item = this.item14;
                return true;
            }

            return false;
        }


        public T15 Item15
        {
            get
            {
                if (this.discriminator == 15)
                {
                    return this.item15;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T15 item)
        {
            item = default;
            if (this.discriminator == 15)
            {
                item = this.item15;
                return true;
            }

            return false;
        }


        public T16 Item16
        {
            get
            {
                if (this.discriminator == 16)
                {
                    return this.item16;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T16 item)
        {
            item = default;
            if (this.discriminator == 16)
            {
                item = this.item16;
                return true;
            }

            return false;
        }


        public T17 Item17
        {
            get
            {
                if (this.discriminator == 17)
                {
                    return this.item17;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T17 item)
        {
            item = default;
            if (this.discriminator == 17)
            {
                item = this.item17;
                return true;
            }

            return false;
        }


        public T18 Item18
        {
            get
            {
                if (this.discriminator == 18)
                {
                    return this.item18;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T18 item)
        {
            item = default;
            if (this.discriminator == 18)
            {
                item = this.item18;
                return true;
            }

            return false;
        }


        public T19 Item19
        {
            get
            {
                if (this.discriminator == 19)
                {
                    return this.item19;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T19 item)
        {
            item = default;
            if (this.discriminator == 19)
            {
                item = this.item19;
                return true;
            }

            return false;
        }


        public T20 Item20
        {
            get
            {
                if (this.discriminator == 20)
                {
                    return this.item20;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T20 item)
        {
            item = default;
            if (this.discriminator == 20)
            {
                item = this.item20;
                return true;
            }

            return false;
        }


        public T21 Item21
        {
            get
            {
                if (this.discriminator == 21)
                {
                    return this.item21;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T21 item)
        {
            item = default;
            if (this.discriminator == 21)
            {
                item = this.item21;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5,
System.Func<T6, T6> cloneT6,
System.Func<T7, T7> cloneT7,
System.Func<T8, T8> cloneT8,
System.Func<T9, T9> cloneT9,
System.Func<T10, T10> cloneT10,
System.Func<T11, T11> cloneT11,
System.Func<T12, T12> cloneT12,
System.Func<T13, T13> cloneT13,
System.Func<T14, T14> cloneT14,
System.Func<T15, T15> cloneT15,
System.Func<T16, T16> cloneT16,
System.Func<T17, T17> cloneT17,
System.Func<T18, T18> cloneT18,
System.Func<T19, T19> cloneT19,
System.Func<T20, T20> cloneT20,
System.Func<T21, T21> cloneT21
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(cloneT5(this.item5));
                case 6:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(cloneT6(this.item6));
                case 7:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(cloneT7(this.item7));
                case 8:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(cloneT8(this.item8));
                case 9:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(cloneT9(this.item9));
                case 10:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(cloneT10(this.item10));
                case 11:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(cloneT11(this.item11));
                case 12:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(cloneT12(this.item12));
                case 13:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(cloneT13(this.item13));
                case 14:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(cloneT14(this.item14));
                case 15:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(cloneT15(this.item15));
                case 16:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(cloneT16(this.item16));
                case 17:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(cloneT17(this.item17));
                case 18:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(cloneT18(this.item18));
                case 19:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(cloneT19(this.item19));
                case 20:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(cloneT20(this.item20));
                case 21:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(cloneT21(this.item21));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5,
System.Action<T6> callback6,
System.Action<T7> callback7,
System.Action<T8> callback8,
System.Action<T9> callback9,
System.Action<T10> callback10,
System.Action<T11> callback11,
System.Action<T12> callback12,
System.Action<T13> callback13,
System.Action<T14> callback14,
System.Action<T15> callback15,
System.Action<T16> callback16,
System.Action<T17> callback17,
System.Action<T18> callback18,
System.Action<T19> callback19,
System.Action<T20> callback20,
System.Action<T21> callback21)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                case 6:
                    callback6(this.item6);
                    break;
                case 7:
                    callback7(this.item7);
                    break;
                case 8:
                    callback8(this.item8);
                    break;
                case 9:
                    callback9(this.item9);
                    break;
                case 10:
                    callback10(this.item10);
                    break;
                case 11:
                    callback11(this.item11);
                    break;
                case 12:
                    callback12(this.item12);
                    break;
                case 13:
                    callback13(this.item13);
                    break;
                case 14:
                    callback14(this.item14);
                    break;
                case 15:
                    callback15(this.item15);
                    break;
                case 16:
                    callback16(this.item16);
                    break;
                case 17:
                    callback17(this.item17);
                    break;
                case 18:
                    callback18(this.item18);
                    break;
                case 19:
                    callback19(this.item19);
                    break;
                case 20:
                    callback20(this.item20);
                    break;
                case 21:
                    callback21(this.item21);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5,
System.Action<TState, T6> callback6,
System.Action<TState, T7> callback7,
System.Action<TState, T8> callback8,
System.Action<TState, T9> callback9,
System.Action<TState, T10> callback10,
System.Action<TState, T11> callback11,
System.Action<TState, T12> callback12,
System.Action<TState, T13> callback13,
System.Action<TState, T14> callback14,
System.Action<TState, T15> callback15,
System.Action<TState, T16> callback16,
System.Action<TState, T17> callback17,
System.Action<TState, T18> callback18,
System.Action<TState, T19> callback19,
System.Action<TState, T20> callback20,
System.Action<TState, T21> callback21)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                case 6:
                    callback6(state, this.item6);
                    break;
                case 7:
                    callback7(state, this.item7);
                    break;
                case 8:
                    callback8(state, this.item8);
                    break;
                case 9:
                    callback9(state, this.item9);
                    break;
                case 10:
                    callback10(state, this.item10);
                    break;
                case 11:
                    callback11(state, this.item11);
                    break;
                case 12:
                    callback12(state, this.item12);
                    break;
                case 13:
                    callback13(state, this.item13);
                    break;
                case 14:
                    callback14(state, this.item14);
                    break;
                case 15:
                    callback15(state, this.item15);
                    break;
                case 16:
                    callback16(state, this.item16);
                    break;
                case 17:
                    callback17(state, this.item17);
                    break;
                case 18:
                    callback18(state, this.item18);
                    break;
                case 19:
                    callback19(state, this.item19);
                    break;
                case 20:
                    callback20(state, this.item20);
                    break;
                case 21:
                    callback21(state, this.item21);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5,
System.Func<T6, TResult> callback6,
System.Func<T7, TResult> callback7,
System.Func<T8, TResult> callback8,
System.Func<T9, TResult> callback9,
System.Func<T10, TResult> callback10,
System.Func<T11, TResult> callback11,
System.Func<T12, TResult> callback12,
System.Func<T13, TResult> callback13,
System.Func<T14, TResult> callback14,
System.Func<T15, TResult> callback15,
System.Func<T16, TResult> callback16,
System.Func<T17, TResult> callback17,
System.Func<T18, TResult> callback18,
System.Func<T19, TResult> callback19,
System.Func<T20, TResult> callback20,
System.Func<T21, TResult> callback21)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                case 6:
                    return callback6(this.item6);
                case 7:
                    return callback7(this.item7);
                case 8:
                    return callback8(this.item8);
                case 9:
                    return callback9(this.item9);
                case 10:
                    return callback10(this.item10);
                case 11:
                    return callback11(this.item11);
                case 12:
                    return callback12(this.item12);
                case 13:
                    return callback13(this.item13);
                case 14:
                    return callback14(this.item14);
                case 15:
                    return callback15(this.item15);
                case 16:
                    return callback16(this.item16);
                case 17:
                    return callback17(this.item17);
                case 18:
                    return callback18(this.item18);
                case 19:
                    return callback19(this.item19);
                case 20:
                    return callback20(this.item20);
                case 21:
                    return callback21(this.item21);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5,
System.Func<TState, T6, TResult> callback6,
System.Func<TState, T7, TResult> callback7,
System.Func<TState, T8, TResult> callback8,
System.Func<TState, T9, TResult> callback9,
System.Func<TState, T10, TResult> callback10,
System.Func<TState, T11, TResult> callback11,
System.Func<TState, T12, TResult> callback12,
System.Func<TState, T13, TResult> callback13,
System.Func<TState, T14, TResult> callback14,
System.Func<TState, T15, TResult> callback15,
System.Func<TState, T16, TResult> callback16,
System.Func<TState, T17, TResult> callback17,
System.Func<TState, T18, TResult> callback18,
System.Func<TState, T19, TResult> callback19,
System.Func<TState, T20, TResult> callback20,
System.Func<TState, T21, TResult> callback21)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                case 6:
                    return callback6(state, this.item6);
                case 7:
                    return callback7(state, this.item7);
                case 8:
                    return callback8(state, this.item8);
                case 9:
                    return callback9(state, this.item9);
                case 10:
                    return callback10(state, this.item10);
                case 11:
                    return callback11(state, this.item11);
                case 12:
                    return callback12(state, this.item12);
                case 13:
                    return callback13(state, this.item13);
                case 14:
                    return callback14(state, this.item14);
                case 15:
                    return callback15(state, this.item15);
                case 16:
                    return callback16(state, this.item16);
                case 17:
                    return callback17(state, this.item17);
                case 18:
                    return callback18(state, this.item18);
                case 19:
                    return callback19(state, this.item19);
                case 20:
                    return callback20(state, this.item20);
                case 21:
                    return callback21(state, this.item21);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;


        private readonly T6 item6;


        private readonly T7 item7;


        private readonly T8 item8;


        private readonly T9 item9;


        private readonly T10 item10;


        private readonly T11 item11;


        private readonly T12 item12;


        private readonly T13 item13;


        private readonly T14 item14;


        private readonly T15 item15;


        private readonly T16 item16;


        private readonly T17 item17;


        private readonly T18 item18;


        private readonly T19 item19;


        private readonly T20 item20;


        private readonly T21 item21;


        private readonly T22 item22;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public FlatBufferUnion(T6 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 6;
            this.item6 = item;
        }


        public FlatBufferUnion(T7 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 7;
            this.item7 = item;
        }


        public FlatBufferUnion(T8 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 8;
            this.item8 = item;
        }


        public FlatBufferUnion(T9 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 9;
            this.item9 = item;
        }


        public FlatBufferUnion(T10 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 10;
            this.item10 = item;
        }


        public FlatBufferUnion(T11 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 11;
            this.item11 = item;
        }


        public FlatBufferUnion(T12 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 12;
            this.item12 = item;
        }


        public FlatBufferUnion(T13 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 13;
            this.item13 = item;
        }


        public FlatBufferUnion(T14 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 14;
            this.item14 = item;
        }


        public FlatBufferUnion(T15 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 15;
            this.item15 = item;
        }


        public FlatBufferUnion(T16 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 16;
            this.item16 = item;
        }


        public FlatBufferUnion(T17 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 17;
            this.item17 = item;
        }


        public FlatBufferUnion(T18 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 18;
            this.item18 = item;
        }


        public FlatBufferUnion(T19 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 19;
            this.item19 = item;
        }


        public FlatBufferUnion(T20 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 20;
            this.item20 = item;
        }


        public FlatBufferUnion(T21 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 21;
            this.item21 = item;
        }


        public FlatBufferUnion(T22 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 22;
            this.item22 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public T6 Item6
        {
            get
            {
                if (this.discriminator == 6)
                {
                    return this.item6;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T6 item)
        {
            item = default;
            if (this.discriminator == 6)
            {
                item = this.item6;
                return true;
            }

            return false;
        }


        public T7 Item7
        {
            get
            {
                if (this.discriminator == 7)
                {
                    return this.item7;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T7 item)
        {
            item = default;
            if (this.discriminator == 7)
            {
                item = this.item7;
                return true;
            }

            return false;
        }


        public T8 Item8
        {
            get
            {
                if (this.discriminator == 8)
                {
                    return this.item8;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T8 item)
        {
            item = default;
            if (this.discriminator == 8)
            {
                item = this.item8;
                return true;
            }

            return false;
        }


        public T9 Item9
        {
            get
            {
                if (this.discriminator == 9)
                {
                    return this.item9;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T9 item)
        {
            item = default;
            if (this.discriminator == 9)
            {
                item = this.item9;
                return true;
            }

            return false;
        }


        public T10 Item10
        {
            get
            {
                if (this.discriminator == 10)
                {
                    return this.item10;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T10 item)
        {
            item = default;
            if (this.discriminator == 10)
            {
                item = this.item10;
                return true;
            }

            return false;
        }


        public T11 Item11
        {
            get
            {
                if (this.discriminator == 11)
                {
                    return this.item11;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T11 item)
        {
            item = default;
            if (this.discriminator == 11)
            {
                item = this.item11;
                return true;
            }

            return false;
        }


        public T12 Item12
        {
            get
            {
                if (this.discriminator == 12)
                {
                    return this.item12;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T12 item)
        {
            item = default;
            if (this.discriminator == 12)
            {
                item = this.item12;
                return true;
            }

            return false;
        }


        public T13 Item13
        {
            get
            {
                if (this.discriminator == 13)
                {
                    return this.item13;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T13 item)
        {
            item = default;
            if (this.discriminator == 13)
            {
                item = this.item13;
                return true;
            }

            return false;
        }


        public T14 Item14
        {
            get
            {
                if (this.discriminator == 14)
                {
                    return this.item14;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T14 item)
        {
            item = default;
            if (this.discriminator == 14)
            {
                item = this.item14;
                return true;
            }

            return false;
        }


        public T15 Item15
        {
            get
            {
                if (this.discriminator == 15)
                {
                    return this.item15;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T15 item)
        {
            item = default;
            if (this.discriminator == 15)
            {
                item = this.item15;
                return true;
            }

            return false;
        }


        public T16 Item16
        {
            get
            {
                if (this.discriminator == 16)
                {
                    return this.item16;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T16 item)
        {
            item = default;
            if (this.discriminator == 16)
            {
                item = this.item16;
                return true;
            }

            return false;
        }


        public T17 Item17
        {
            get
            {
                if (this.discriminator == 17)
                {
                    return this.item17;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T17 item)
        {
            item = default;
            if (this.discriminator == 17)
            {
                item = this.item17;
                return true;
            }

            return false;
        }


        public T18 Item18
        {
            get
            {
                if (this.discriminator == 18)
                {
                    return this.item18;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T18 item)
        {
            item = default;
            if (this.discriminator == 18)
            {
                item = this.item18;
                return true;
            }

            return false;
        }


        public T19 Item19
        {
            get
            {
                if (this.discriminator == 19)
                {
                    return this.item19;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T19 item)
        {
            item = default;
            if (this.discriminator == 19)
            {
                item = this.item19;
                return true;
            }

            return false;
        }


        public T20 Item20
        {
            get
            {
                if (this.discriminator == 20)
                {
                    return this.item20;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T20 item)
        {
            item = default;
            if (this.discriminator == 20)
            {
                item = this.item20;
                return true;
            }

            return false;
        }


        public T21 Item21
        {
            get
            {
                if (this.discriminator == 21)
                {
                    return this.item21;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T21 item)
        {
            item = default;
            if (this.discriminator == 21)
            {
                item = this.item21;
                return true;
            }

            return false;
        }


        public T22 Item22
        {
            get
            {
                if (this.discriminator == 22)
                {
                    return this.item22;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T22 item)
        {
            item = default;
            if (this.discriminator == 22)
            {
                item = this.item22;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5,
System.Func<T6, T6> cloneT6,
System.Func<T7, T7> cloneT7,
System.Func<T8, T8> cloneT8,
System.Func<T9, T9> cloneT9,
System.Func<T10, T10> cloneT10,
System.Func<T11, T11> cloneT11,
System.Func<T12, T12> cloneT12,
System.Func<T13, T13> cloneT13,
System.Func<T14, T14> cloneT14,
System.Func<T15, T15> cloneT15,
System.Func<T16, T16> cloneT16,
System.Func<T17, T17> cloneT17,
System.Func<T18, T18> cloneT18,
System.Func<T19, T19> cloneT19,
System.Func<T20, T20> cloneT20,
System.Func<T21, T21> cloneT21,
System.Func<T22, T22> cloneT22
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(cloneT5(this.item5));
                case 6:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(cloneT6(this.item6));
                case 7:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(cloneT7(this.item7));
                case 8:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(cloneT8(this.item8));
                case 9:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(cloneT9(this.item9));
                case 10:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(cloneT10(this.item10));
                case 11:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(cloneT11(this.item11));
                case 12:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(cloneT12(this.item12));
                case 13:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(cloneT13(this.item13));
                case 14:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(cloneT14(this.item14));
                case 15:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(cloneT15(this.item15));
                case 16:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(cloneT16(this.item16));
                case 17:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(cloneT17(this.item17));
                case 18:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(cloneT18(this.item18));
                case 19:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(cloneT19(this.item19));
                case 20:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(cloneT20(this.item20));
                case 21:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(cloneT21(this.item21));
                case 22:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(cloneT22(this.item22));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5,
System.Action<T6> callback6,
System.Action<T7> callback7,
System.Action<T8> callback8,
System.Action<T9> callback9,
System.Action<T10> callback10,
System.Action<T11> callback11,
System.Action<T12> callback12,
System.Action<T13> callback13,
System.Action<T14> callback14,
System.Action<T15> callback15,
System.Action<T16> callback16,
System.Action<T17> callback17,
System.Action<T18> callback18,
System.Action<T19> callback19,
System.Action<T20> callback20,
System.Action<T21> callback21,
System.Action<T22> callback22)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                case 6:
                    callback6(this.item6);
                    break;
                case 7:
                    callback7(this.item7);
                    break;
                case 8:
                    callback8(this.item8);
                    break;
                case 9:
                    callback9(this.item9);
                    break;
                case 10:
                    callback10(this.item10);
                    break;
                case 11:
                    callback11(this.item11);
                    break;
                case 12:
                    callback12(this.item12);
                    break;
                case 13:
                    callback13(this.item13);
                    break;
                case 14:
                    callback14(this.item14);
                    break;
                case 15:
                    callback15(this.item15);
                    break;
                case 16:
                    callback16(this.item16);
                    break;
                case 17:
                    callback17(this.item17);
                    break;
                case 18:
                    callback18(this.item18);
                    break;
                case 19:
                    callback19(this.item19);
                    break;
                case 20:
                    callback20(this.item20);
                    break;
                case 21:
                    callback21(this.item21);
                    break;
                case 22:
                    callback22(this.item22);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5,
System.Action<TState, T6> callback6,
System.Action<TState, T7> callback7,
System.Action<TState, T8> callback8,
System.Action<TState, T9> callback9,
System.Action<TState, T10> callback10,
System.Action<TState, T11> callback11,
System.Action<TState, T12> callback12,
System.Action<TState, T13> callback13,
System.Action<TState, T14> callback14,
System.Action<TState, T15> callback15,
System.Action<TState, T16> callback16,
System.Action<TState, T17> callback17,
System.Action<TState, T18> callback18,
System.Action<TState, T19> callback19,
System.Action<TState, T20> callback20,
System.Action<TState, T21> callback21,
System.Action<TState, T22> callback22)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                case 6:
                    callback6(state, this.item6);
                    break;
                case 7:
                    callback7(state, this.item7);
                    break;
                case 8:
                    callback8(state, this.item8);
                    break;
                case 9:
                    callback9(state, this.item9);
                    break;
                case 10:
                    callback10(state, this.item10);
                    break;
                case 11:
                    callback11(state, this.item11);
                    break;
                case 12:
                    callback12(state, this.item12);
                    break;
                case 13:
                    callback13(state, this.item13);
                    break;
                case 14:
                    callback14(state, this.item14);
                    break;
                case 15:
                    callback15(state, this.item15);
                    break;
                case 16:
                    callback16(state, this.item16);
                    break;
                case 17:
                    callback17(state, this.item17);
                    break;
                case 18:
                    callback18(state, this.item18);
                    break;
                case 19:
                    callback19(state, this.item19);
                    break;
                case 20:
                    callback20(state, this.item20);
                    break;
                case 21:
                    callback21(state, this.item21);
                    break;
                case 22:
                    callback22(state, this.item22);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5,
System.Func<T6, TResult> callback6,
System.Func<T7, TResult> callback7,
System.Func<T8, TResult> callback8,
System.Func<T9, TResult> callback9,
System.Func<T10, TResult> callback10,
System.Func<T11, TResult> callback11,
System.Func<T12, TResult> callback12,
System.Func<T13, TResult> callback13,
System.Func<T14, TResult> callback14,
System.Func<T15, TResult> callback15,
System.Func<T16, TResult> callback16,
System.Func<T17, TResult> callback17,
System.Func<T18, TResult> callback18,
System.Func<T19, TResult> callback19,
System.Func<T20, TResult> callback20,
System.Func<T21, TResult> callback21,
System.Func<T22, TResult> callback22)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                case 6:
                    return callback6(this.item6);
                case 7:
                    return callback7(this.item7);
                case 8:
                    return callback8(this.item8);
                case 9:
                    return callback9(this.item9);
                case 10:
                    return callback10(this.item10);
                case 11:
                    return callback11(this.item11);
                case 12:
                    return callback12(this.item12);
                case 13:
                    return callback13(this.item13);
                case 14:
                    return callback14(this.item14);
                case 15:
                    return callback15(this.item15);
                case 16:
                    return callback16(this.item16);
                case 17:
                    return callback17(this.item17);
                case 18:
                    return callback18(this.item18);
                case 19:
                    return callback19(this.item19);
                case 20:
                    return callback20(this.item20);
                case 21:
                    return callback21(this.item21);
                case 22:
                    return callback22(this.item22);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5,
System.Func<TState, T6, TResult> callback6,
System.Func<TState, T7, TResult> callback7,
System.Func<TState, T8, TResult> callback8,
System.Func<TState, T9, TResult> callback9,
System.Func<TState, T10, TResult> callback10,
System.Func<TState, T11, TResult> callback11,
System.Func<TState, T12, TResult> callback12,
System.Func<TState, T13, TResult> callback13,
System.Func<TState, T14, TResult> callback14,
System.Func<TState, T15, TResult> callback15,
System.Func<TState, T16, TResult> callback16,
System.Func<TState, T17, TResult> callback17,
System.Func<TState, T18, TResult> callback18,
System.Func<TState, T19, TResult> callback19,
System.Func<TState, T20, TResult> callback20,
System.Func<TState, T21, TResult> callback21,
System.Func<TState, T22, TResult> callback22)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                case 6:
                    return callback6(state, this.item6);
                case 7:
                    return callback7(state, this.item7);
                case 8:
                    return callback8(state, this.item8);
                case 9:
                    return callback9(state, this.item9);
                case 10:
                    return callback10(state, this.item10);
                case 11:
                    return callback11(state, this.item11);
                case 12:
                    return callback12(state, this.item12);
                case 13:
                    return callback13(state, this.item13);
                case 14:
                    return callback14(state, this.item14);
                case 15:
                    return callback15(state, this.item15);
                case 16:
                    return callback16(state, this.item16);
                case 17:
                    return callback17(state, this.item17);
                case 18:
                    return callback18(state, this.item18);
                case 19:
                    return callback19(state, this.item19);
                case 20:
                    return callback20(state, this.item20);
                case 21:
                    return callback21(state, this.item21);
                case 22:
                    return callback22(state, this.item22);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;


        private readonly T6 item6;


        private readonly T7 item7;


        private readonly T8 item8;


        private readonly T9 item9;


        private readonly T10 item10;


        private readonly T11 item11;


        private readonly T12 item12;


        private readonly T13 item13;


        private readonly T14 item14;


        private readonly T15 item15;


        private readonly T16 item16;


        private readonly T17 item17;


        private readonly T18 item18;


        private readonly T19 item19;


        private readonly T20 item20;


        private readonly T21 item21;


        private readonly T22 item22;


        private readonly T23 item23;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public FlatBufferUnion(T6 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 6;
            this.item6 = item;
        }


        public FlatBufferUnion(T7 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 7;
            this.item7 = item;
        }


        public FlatBufferUnion(T8 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 8;
            this.item8 = item;
        }


        public FlatBufferUnion(T9 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 9;
            this.item9 = item;
        }


        public FlatBufferUnion(T10 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 10;
            this.item10 = item;
        }


        public FlatBufferUnion(T11 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 11;
            this.item11 = item;
        }


        public FlatBufferUnion(T12 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 12;
            this.item12 = item;
        }


        public FlatBufferUnion(T13 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 13;
            this.item13 = item;
        }


        public FlatBufferUnion(T14 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 14;
            this.item14 = item;
        }


        public FlatBufferUnion(T15 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 15;
            this.item15 = item;
        }


        public FlatBufferUnion(T16 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 16;
            this.item16 = item;
        }


        public FlatBufferUnion(T17 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 17;
            this.item17 = item;
        }


        public FlatBufferUnion(T18 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 18;
            this.item18 = item;
        }


        public FlatBufferUnion(T19 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 19;
            this.item19 = item;
        }


        public FlatBufferUnion(T20 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 20;
            this.item20 = item;
        }


        public FlatBufferUnion(T21 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 21;
            this.item21 = item;
        }


        public FlatBufferUnion(T22 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 22;
            this.item22 = item;
        }


        public FlatBufferUnion(T23 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 23;
            this.item23 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public T6 Item6
        {
            get
            {
                if (this.discriminator == 6)
                {
                    return this.item6;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T6 item)
        {
            item = default;
            if (this.discriminator == 6)
            {
                item = this.item6;
                return true;
            }

            return false;
        }


        public T7 Item7
        {
            get
            {
                if (this.discriminator == 7)
                {
                    return this.item7;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T7 item)
        {
            item = default;
            if (this.discriminator == 7)
            {
                item = this.item7;
                return true;
            }

            return false;
        }


        public T8 Item8
        {
            get
            {
                if (this.discriminator == 8)
                {
                    return this.item8;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T8 item)
        {
            item = default;
            if (this.discriminator == 8)
            {
                item = this.item8;
                return true;
            }

            return false;
        }


        public T9 Item9
        {
            get
            {
                if (this.discriminator == 9)
                {
                    return this.item9;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T9 item)
        {
            item = default;
            if (this.discriminator == 9)
            {
                item = this.item9;
                return true;
            }

            return false;
        }


        public T10 Item10
        {
            get
            {
                if (this.discriminator == 10)
                {
                    return this.item10;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T10 item)
        {
            item = default;
            if (this.discriminator == 10)
            {
                item = this.item10;
                return true;
            }

            return false;
        }


        public T11 Item11
        {
            get
            {
                if (this.discriminator == 11)
                {
                    return this.item11;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T11 item)
        {
            item = default;
            if (this.discriminator == 11)
            {
                item = this.item11;
                return true;
            }

            return false;
        }


        public T12 Item12
        {
            get
            {
                if (this.discriminator == 12)
                {
                    return this.item12;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T12 item)
        {
            item = default;
            if (this.discriminator == 12)
            {
                item = this.item12;
                return true;
            }

            return false;
        }


        public T13 Item13
        {
            get
            {
                if (this.discriminator == 13)
                {
                    return this.item13;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T13 item)
        {
            item = default;
            if (this.discriminator == 13)
            {
                item = this.item13;
                return true;
            }

            return false;
        }


        public T14 Item14
        {
            get
            {
                if (this.discriminator == 14)
                {
                    return this.item14;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T14 item)
        {
            item = default;
            if (this.discriminator == 14)
            {
                item = this.item14;
                return true;
            }

            return false;
        }


        public T15 Item15
        {
            get
            {
                if (this.discriminator == 15)
                {
                    return this.item15;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T15 item)
        {
            item = default;
            if (this.discriminator == 15)
            {
                item = this.item15;
                return true;
            }

            return false;
        }


        public T16 Item16
        {
            get
            {
                if (this.discriminator == 16)
                {
                    return this.item16;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T16 item)
        {
            item = default;
            if (this.discriminator == 16)
            {
                item = this.item16;
                return true;
            }

            return false;
        }


        public T17 Item17
        {
            get
            {
                if (this.discriminator == 17)
                {
                    return this.item17;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T17 item)
        {
            item = default;
            if (this.discriminator == 17)
            {
                item = this.item17;
                return true;
            }

            return false;
        }


        public T18 Item18
        {
            get
            {
                if (this.discriminator == 18)
                {
                    return this.item18;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T18 item)
        {
            item = default;
            if (this.discriminator == 18)
            {
                item = this.item18;
                return true;
            }

            return false;
        }


        public T19 Item19
        {
            get
            {
                if (this.discriminator == 19)
                {
                    return this.item19;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T19 item)
        {
            item = default;
            if (this.discriminator == 19)
            {
                item = this.item19;
                return true;
            }

            return false;
        }


        public T20 Item20
        {
            get
            {
                if (this.discriminator == 20)
                {
                    return this.item20;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T20 item)
        {
            item = default;
            if (this.discriminator == 20)
            {
                item = this.item20;
                return true;
            }

            return false;
        }


        public T21 Item21
        {
            get
            {
                if (this.discriminator == 21)
                {
                    return this.item21;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T21 item)
        {
            item = default;
            if (this.discriminator == 21)
            {
                item = this.item21;
                return true;
            }

            return false;
        }


        public T22 Item22
        {
            get
            {
                if (this.discriminator == 22)
                {
                    return this.item22;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T22 item)
        {
            item = default;
            if (this.discriminator == 22)
            {
                item = this.item22;
                return true;
            }

            return false;
        }


        public T23 Item23
        {
            get
            {
                if (this.discriminator == 23)
                {
                    return this.item23;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T23 item)
        {
            item = default;
            if (this.discriminator == 23)
            {
                item = this.item23;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5,
System.Func<T6, T6> cloneT6,
System.Func<T7, T7> cloneT7,
System.Func<T8, T8> cloneT8,
System.Func<T9, T9> cloneT9,
System.Func<T10, T10> cloneT10,
System.Func<T11, T11> cloneT11,
System.Func<T12, T12> cloneT12,
System.Func<T13, T13> cloneT13,
System.Func<T14, T14> cloneT14,
System.Func<T15, T15> cloneT15,
System.Func<T16, T16> cloneT16,
System.Func<T17, T17> cloneT17,
System.Func<T18, T18> cloneT18,
System.Func<T19, T19> cloneT19,
System.Func<T20, T20> cloneT20,
System.Func<T21, T21> cloneT21,
System.Func<T22, T22> cloneT22,
System.Func<T23, T23> cloneT23
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(cloneT5(this.item5));
                case 6:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(cloneT6(this.item6));
                case 7:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(cloneT7(this.item7));
                case 8:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(cloneT8(this.item8));
                case 9:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(cloneT9(this.item9));
                case 10:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(cloneT10(this.item10));
                case 11:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(cloneT11(this.item11));
                case 12:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(cloneT12(this.item12));
                case 13:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(cloneT13(this.item13));
                case 14:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(cloneT14(this.item14));
                case 15:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(cloneT15(this.item15));
                case 16:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(cloneT16(this.item16));
                case 17:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(cloneT17(this.item17));
                case 18:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(cloneT18(this.item18));
                case 19:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(cloneT19(this.item19));
                case 20:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(cloneT20(this.item20));
                case 21:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(cloneT21(this.item21));
                case 22:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(cloneT22(this.item22));
                case 23:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(cloneT23(this.item23));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5,
System.Action<T6> callback6,
System.Action<T7> callback7,
System.Action<T8> callback8,
System.Action<T9> callback9,
System.Action<T10> callback10,
System.Action<T11> callback11,
System.Action<T12> callback12,
System.Action<T13> callback13,
System.Action<T14> callback14,
System.Action<T15> callback15,
System.Action<T16> callback16,
System.Action<T17> callback17,
System.Action<T18> callback18,
System.Action<T19> callback19,
System.Action<T20> callback20,
System.Action<T21> callback21,
System.Action<T22> callback22,
System.Action<T23> callback23)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                case 6:
                    callback6(this.item6);
                    break;
                case 7:
                    callback7(this.item7);
                    break;
                case 8:
                    callback8(this.item8);
                    break;
                case 9:
                    callback9(this.item9);
                    break;
                case 10:
                    callback10(this.item10);
                    break;
                case 11:
                    callback11(this.item11);
                    break;
                case 12:
                    callback12(this.item12);
                    break;
                case 13:
                    callback13(this.item13);
                    break;
                case 14:
                    callback14(this.item14);
                    break;
                case 15:
                    callback15(this.item15);
                    break;
                case 16:
                    callback16(this.item16);
                    break;
                case 17:
                    callback17(this.item17);
                    break;
                case 18:
                    callback18(this.item18);
                    break;
                case 19:
                    callback19(this.item19);
                    break;
                case 20:
                    callback20(this.item20);
                    break;
                case 21:
                    callback21(this.item21);
                    break;
                case 22:
                    callback22(this.item22);
                    break;
                case 23:
                    callback23(this.item23);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5,
System.Action<TState, T6> callback6,
System.Action<TState, T7> callback7,
System.Action<TState, T8> callback8,
System.Action<TState, T9> callback9,
System.Action<TState, T10> callback10,
System.Action<TState, T11> callback11,
System.Action<TState, T12> callback12,
System.Action<TState, T13> callback13,
System.Action<TState, T14> callback14,
System.Action<TState, T15> callback15,
System.Action<TState, T16> callback16,
System.Action<TState, T17> callback17,
System.Action<TState, T18> callback18,
System.Action<TState, T19> callback19,
System.Action<TState, T20> callback20,
System.Action<TState, T21> callback21,
System.Action<TState, T22> callback22,
System.Action<TState, T23> callback23)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                case 6:
                    callback6(state, this.item6);
                    break;
                case 7:
                    callback7(state, this.item7);
                    break;
                case 8:
                    callback8(state, this.item8);
                    break;
                case 9:
                    callback9(state, this.item9);
                    break;
                case 10:
                    callback10(state, this.item10);
                    break;
                case 11:
                    callback11(state, this.item11);
                    break;
                case 12:
                    callback12(state, this.item12);
                    break;
                case 13:
                    callback13(state, this.item13);
                    break;
                case 14:
                    callback14(state, this.item14);
                    break;
                case 15:
                    callback15(state, this.item15);
                    break;
                case 16:
                    callback16(state, this.item16);
                    break;
                case 17:
                    callback17(state, this.item17);
                    break;
                case 18:
                    callback18(state, this.item18);
                    break;
                case 19:
                    callback19(state, this.item19);
                    break;
                case 20:
                    callback20(state, this.item20);
                    break;
                case 21:
                    callback21(state, this.item21);
                    break;
                case 22:
                    callback22(state, this.item22);
                    break;
                case 23:
                    callback23(state, this.item23);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5,
System.Func<T6, TResult> callback6,
System.Func<T7, TResult> callback7,
System.Func<T8, TResult> callback8,
System.Func<T9, TResult> callback9,
System.Func<T10, TResult> callback10,
System.Func<T11, TResult> callback11,
System.Func<T12, TResult> callback12,
System.Func<T13, TResult> callback13,
System.Func<T14, TResult> callback14,
System.Func<T15, TResult> callback15,
System.Func<T16, TResult> callback16,
System.Func<T17, TResult> callback17,
System.Func<T18, TResult> callback18,
System.Func<T19, TResult> callback19,
System.Func<T20, TResult> callback20,
System.Func<T21, TResult> callback21,
System.Func<T22, TResult> callback22,
System.Func<T23, TResult> callback23)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                case 6:
                    return callback6(this.item6);
                case 7:
                    return callback7(this.item7);
                case 8:
                    return callback8(this.item8);
                case 9:
                    return callback9(this.item9);
                case 10:
                    return callback10(this.item10);
                case 11:
                    return callback11(this.item11);
                case 12:
                    return callback12(this.item12);
                case 13:
                    return callback13(this.item13);
                case 14:
                    return callback14(this.item14);
                case 15:
                    return callback15(this.item15);
                case 16:
                    return callback16(this.item16);
                case 17:
                    return callback17(this.item17);
                case 18:
                    return callback18(this.item18);
                case 19:
                    return callback19(this.item19);
                case 20:
                    return callback20(this.item20);
                case 21:
                    return callback21(this.item21);
                case 22:
                    return callback22(this.item22);
                case 23:
                    return callback23(this.item23);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5,
System.Func<TState, T6, TResult> callback6,
System.Func<TState, T7, TResult> callback7,
System.Func<TState, T8, TResult> callback8,
System.Func<TState, T9, TResult> callback9,
System.Func<TState, T10, TResult> callback10,
System.Func<TState, T11, TResult> callback11,
System.Func<TState, T12, TResult> callback12,
System.Func<TState, T13, TResult> callback13,
System.Func<TState, T14, TResult> callback14,
System.Func<TState, T15, TResult> callback15,
System.Func<TState, T16, TResult> callback16,
System.Func<TState, T17, TResult> callback17,
System.Func<TState, T18, TResult> callback18,
System.Func<TState, T19, TResult> callback19,
System.Func<TState, T20, TResult> callback20,
System.Func<TState, T21, TResult> callback21,
System.Func<TState, T22, TResult> callback22,
System.Func<TState, T23, TResult> callback23)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                case 6:
                    return callback6(state, this.item6);
                case 7:
                    return callback7(state, this.item7);
                case 8:
                    return callback8(state, this.item8);
                case 9:
                    return callback9(state, this.item9);
                case 10:
                    return callback10(state, this.item10);
                case 11:
                    return callback11(state, this.item11);
                case 12:
                    return callback12(state, this.item12);
                case 13:
                    return callback13(state, this.item13);
                case 14:
                    return callback14(state, this.item14);
                case 15:
                    return callback15(state, this.item15);
                case 16:
                    return callback16(state, this.item16);
                case 17:
                    return callback17(state, this.item17);
                case 18:
                    return callback18(state, this.item18);
                case 19:
                    return callback19(state, this.item19);
                case 20:
                    return callback20(state, this.item20);
                case 21:
                    return callback21(state, this.item21);
                case 22:
                    return callback22(state, this.item22);
                case 23:
                    return callback23(state, this.item23);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;


        private readonly T6 item6;


        private readonly T7 item7;


        private readonly T8 item8;


        private readonly T9 item9;


        private readonly T10 item10;


        private readonly T11 item11;


        private readonly T12 item12;


        private readonly T13 item13;


        private readonly T14 item14;


        private readonly T15 item15;


        private readonly T16 item16;


        private readonly T17 item17;


        private readonly T18 item18;


        private readonly T19 item19;


        private readonly T20 item20;


        private readonly T21 item21;


        private readonly T22 item22;


        private readonly T23 item23;


        private readonly T24 item24;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public FlatBufferUnion(T6 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 6;
            this.item6 = item;
        }


        public FlatBufferUnion(T7 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 7;
            this.item7 = item;
        }


        public FlatBufferUnion(T8 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 8;
            this.item8 = item;
        }


        public FlatBufferUnion(T9 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 9;
            this.item9 = item;
        }


        public FlatBufferUnion(T10 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 10;
            this.item10 = item;
        }


        public FlatBufferUnion(T11 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 11;
            this.item11 = item;
        }


        public FlatBufferUnion(T12 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 12;
            this.item12 = item;
        }


        public FlatBufferUnion(T13 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 13;
            this.item13 = item;
        }


        public FlatBufferUnion(T14 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 14;
            this.item14 = item;
        }


        public FlatBufferUnion(T15 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 15;
            this.item15 = item;
        }


        public FlatBufferUnion(T16 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 16;
            this.item16 = item;
        }


        public FlatBufferUnion(T17 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 17;
            this.item17 = item;
        }


        public FlatBufferUnion(T18 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 18;
            this.item18 = item;
        }


        public FlatBufferUnion(T19 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 19;
            this.item19 = item;
        }


        public FlatBufferUnion(T20 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 20;
            this.item20 = item;
        }


        public FlatBufferUnion(T21 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 21;
            this.item21 = item;
        }


        public FlatBufferUnion(T22 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 22;
            this.item22 = item;
        }


        public FlatBufferUnion(T23 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 23;
            this.item23 = item;
        }


        public FlatBufferUnion(T24 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 24;
            this.item24 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public T6 Item6
        {
            get
            {
                if (this.discriminator == 6)
                {
                    return this.item6;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T6 item)
        {
            item = default;
            if (this.discriminator == 6)
            {
                item = this.item6;
                return true;
            }

            return false;
        }


        public T7 Item7
        {
            get
            {
                if (this.discriminator == 7)
                {
                    return this.item7;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T7 item)
        {
            item = default;
            if (this.discriminator == 7)
            {
                item = this.item7;
                return true;
            }

            return false;
        }


        public T8 Item8
        {
            get
            {
                if (this.discriminator == 8)
                {
                    return this.item8;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T8 item)
        {
            item = default;
            if (this.discriminator == 8)
            {
                item = this.item8;
                return true;
            }

            return false;
        }


        public T9 Item9
        {
            get
            {
                if (this.discriminator == 9)
                {
                    return this.item9;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T9 item)
        {
            item = default;
            if (this.discriminator == 9)
            {
                item = this.item9;
                return true;
            }

            return false;
        }


        public T10 Item10
        {
            get
            {
                if (this.discriminator == 10)
                {
                    return this.item10;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T10 item)
        {
            item = default;
            if (this.discriminator == 10)
            {
                item = this.item10;
                return true;
            }

            return false;
        }


        public T11 Item11
        {
            get
            {
                if (this.discriminator == 11)
                {
                    return this.item11;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T11 item)
        {
            item = default;
            if (this.discriminator == 11)
            {
                item = this.item11;
                return true;
            }

            return false;
        }


        public T12 Item12
        {
            get
            {
                if (this.discriminator == 12)
                {
                    return this.item12;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T12 item)
        {
            item = default;
            if (this.discriminator == 12)
            {
                item = this.item12;
                return true;
            }

            return false;
        }


        public T13 Item13
        {
            get
            {
                if (this.discriminator == 13)
                {
                    return this.item13;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T13 item)
        {
            item = default;
            if (this.discriminator == 13)
            {
                item = this.item13;
                return true;
            }

            return false;
        }


        public T14 Item14
        {
            get
            {
                if (this.discriminator == 14)
                {
                    return this.item14;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T14 item)
        {
            item = default;
            if (this.discriminator == 14)
            {
                item = this.item14;
                return true;
            }

            return false;
        }


        public T15 Item15
        {
            get
            {
                if (this.discriminator == 15)
                {
                    return this.item15;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T15 item)
        {
            item = default;
            if (this.discriminator == 15)
            {
                item = this.item15;
                return true;
            }

            return false;
        }


        public T16 Item16
        {
            get
            {
                if (this.discriminator == 16)
                {
                    return this.item16;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T16 item)
        {
            item = default;
            if (this.discriminator == 16)
            {
                item = this.item16;
                return true;
            }

            return false;
        }


        public T17 Item17
        {
            get
            {
                if (this.discriminator == 17)
                {
                    return this.item17;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T17 item)
        {
            item = default;
            if (this.discriminator == 17)
            {
                item = this.item17;
                return true;
            }

            return false;
        }


        public T18 Item18
        {
            get
            {
                if (this.discriminator == 18)
                {
                    return this.item18;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T18 item)
        {
            item = default;
            if (this.discriminator == 18)
            {
                item = this.item18;
                return true;
            }

            return false;
        }


        public T19 Item19
        {
            get
            {
                if (this.discriminator == 19)
                {
                    return this.item19;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T19 item)
        {
            item = default;
            if (this.discriminator == 19)
            {
                item = this.item19;
                return true;
            }

            return false;
        }


        public T20 Item20
        {
            get
            {
                if (this.discriminator == 20)
                {
                    return this.item20;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T20 item)
        {
            item = default;
            if (this.discriminator == 20)
            {
                item = this.item20;
                return true;
            }

            return false;
        }


        public T21 Item21
        {
            get
            {
                if (this.discriminator == 21)
                {
                    return this.item21;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T21 item)
        {
            item = default;
            if (this.discriminator == 21)
            {
                item = this.item21;
                return true;
            }

            return false;
        }


        public T22 Item22
        {
            get
            {
                if (this.discriminator == 22)
                {
                    return this.item22;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T22 item)
        {
            item = default;
            if (this.discriminator == 22)
            {
                item = this.item22;
                return true;
            }

            return false;
        }


        public T23 Item23
        {
            get
            {
                if (this.discriminator == 23)
                {
                    return this.item23;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T23 item)
        {
            item = default;
            if (this.discriminator == 23)
            {
                item = this.item23;
                return true;
            }

            return false;
        }


        public T24 Item24
        {
            get
            {
                if (this.discriminator == 24)
                {
                    return this.item24;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T24 item)
        {
            item = default;
            if (this.discriminator == 24)
            {
                item = this.item24;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5,
System.Func<T6, T6> cloneT6,
System.Func<T7, T7> cloneT7,
System.Func<T8, T8> cloneT8,
System.Func<T9, T9> cloneT9,
System.Func<T10, T10> cloneT10,
System.Func<T11, T11> cloneT11,
System.Func<T12, T12> cloneT12,
System.Func<T13, T13> cloneT13,
System.Func<T14, T14> cloneT14,
System.Func<T15, T15> cloneT15,
System.Func<T16, T16> cloneT16,
System.Func<T17, T17> cloneT17,
System.Func<T18, T18> cloneT18,
System.Func<T19, T19> cloneT19,
System.Func<T20, T20> cloneT20,
System.Func<T21, T21> cloneT21,
System.Func<T22, T22> cloneT22,
System.Func<T23, T23> cloneT23,
System.Func<T24, T24> cloneT24
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(cloneT5(this.item5));
                case 6:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(cloneT6(this.item6));
                case 7:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(cloneT7(this.item7));
                case 8:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(cloneT8(this.item8));
                case 9:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(cloneT9(this.item9));
                case 10:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(cloneT10(this.item10));
                case 11:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(cloneT11(this.item11));
                case 12:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(cloneT12(this.item12));
                case 13:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(cloneT13(this.item13));
                case 14:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(cloneT14(this.item14));
                case 15:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(cloneT15(this.item15));
                case 16:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(cloneT16(this.item16));
                case 17:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(cloneT17(this.item17));
                case 18:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(cloneT18(this.item18));
                case 19:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(cloneT19(this.item19));
                case 20:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(cloneT20(this.item20));
                case 21:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(cloneT21(this.item21));
                case 22:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(cloneT22(this.item22));
                case 23:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(cloneT23(this.item23));
                case 24:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>(cloneT24(this.item24));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5,
System.Action<T6> callback6,
System.Action<T7> callback7,
System.Action<T8> callback8,
System.Action<T9> callback9,
System.Action<T10> callback10,
System.Action<T11> callback11,
System.Action<T12> callback12,
System.Action<T13> callback13,
System.Action<T14> callback14,
System.Action<T15> callback15,
System.Action<T16> callback16,
System.Action<T17> callback17,
System.Action<T18> callback18,
System.Action<T19> callback19,
System.Action<T20> callback20,
System.Action<T21> callback21,
System.Action<T22> callback22,
System.Action<T23> callback23,
System.Action<T24> callback24)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                case 6:
                    callback6(this.item6);
                    break;
                case 7:
                    callback7(this.item7);
                    break;
                case 8:
                    callback8(this.item8);
                    break;
                case 9:
                    callback9(this.item9);
                    break;
                case 10:
                    callback10(this.item10);
                    break;
                case 11:
                    callback11(this.item11);
                    break;
                case 12:
                    callback12(this.item12);
                    break;
                case 13:
                    callback13(this.item13);
                    break;
                case 14:
                    callback14(this.item14);
                    break;
                case 15:
                    callback15(this.item15);
                    break;
                case 16:
                    callback16(this.item16);
                    break;
                case 17:
                    callback17(this.item17);
                    break;
                case 18:
                    callback18(this.item18);
                    break;
                case 19:
                    callback19(this.item19);
                    break;
                case 20:
                    callback20(this.item20);
                    break;
                case 21:
                    callback21(this.item21);
                    break;
                case 22:
                    callback22(this.item22);
                    break;
                case 23:
                    callback23(this.item23);
                    break;
                case 24:
                    callback24(this.item24);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5,
System.Action<TState, T6> callback6,
System.Action<TState, T7> callback7,
System.Action<TState, T8> callback8,
System.Action<TState, T9> callback9,
System.Action<TState, T10> callback10,
System.Action<TState, T11> callback11,
System.Action<TState, T12> callback12,
System.Action<TState, T13> callback13,
System.Action<TState, T14> callback14,
System.Action<TState, T15> callback15,
System.Action<TState, T16> callback16,
System.Action<TState, T17> callback17,
System.Action<TState, T18> callback18,
System.Action<TState, T19> callback19,
System.Action<TState, T20> callback20,
System.Action<TState, T21> callback21,
System.Action<TState, T22> callback22,
System.Action<TState, T23> callback23,
System.Action<TState, T24> callback24)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                case 6:
                    callback6(state, this.item6);
                    break;
                case 7:
                    callback7(state, this.item7);
                    break;
                case 8:
                    callback8(state, this.item8);
                    break;
                case 9:
                    callback9(state, this.item9);
                    break;
                case 10:
                    callback10(state, this.item10);
                    break;
                case 11:
                    callback11(state, this.item11);
                    break;
                case 12:
                    callback12(state, this.item12);
                    break;
                case 13:
                    callback13(state, this.item13);
                    break;
                case 14:
                    callback14(state, this.item14);
                    break;
                case 15:
                    callback15(state, this.item15);
                    break;
                case 16:
                    callback16(state, this.item16);
                    break;
                case 17:
                    callback17(state, this.item17);
                    break;
                case 18:
                    callback18(state, this.item18);
                    break;
                case 19:
                    callback19(state, this.item19);
                    break;
                case 20:
                    callback20(state, this.item20);
                    break;
                case 21:
                    callback21(state, this.item21);
                    break;
                case 22:
                    callback22(state, this.item22);
                    break;
                case 23:
                    callback23(state, this.item23);
                    break;
                case 24:
                    callback24(state, this.item24);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5,
System.Func<T6, TResult> callback6,
System.Func<T7, TResult> callback7,
System.Func<T8, TResult> callback8,
System.Func<T9, TResult> callback9,
System.Func<T10, TResult> callback10,
System.Func<T11, TResult> callback11,
System.Func<T12, TResult> callback12,
System.Func<T13, TResult> callback13,
System.Func<T14, TResult> callback14,
System.Func<T15, TResult> callback15,
System.Func<T16, TResult> callback16,
System.Func<T17, TResult> callback17,
System.Func<T18, TResult> callback18,
System.Func<T19, TResult> callback19,
System.Func<T20, TResult> callback20,
System.Func<T21, TResult> callback21,
System.Func<T22, TResult> callback22,
System.Func<T23, TResult> callback23,
System.Func<T24, TResult> callback24)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                case 6:
                    return callback6(this.item6);
                case 7:
                    return callback7(this.item7);
                case 8:
                    return callback8(this.item8);
                case 9:
                    return callback9(this.item9);
                case 10:
                    return callback10(this.item10);
                case 11:
                    return callback11(this.item11);
                case 12:
                    return callback12(this.item12);
                case 13:
                    return callback13(this.item13);
                case 14:
                    return callback14(this.item14);
                case 15:
                    return callback15(this.item15);
                case 16:
                    return callback16(this.item16);
                case 17:
                    return callback17(this.item17);
                case 18:
                    return callback18(this.item18);
                case 19:
                    return callback19(this.item19);
                case 20:
                    return callback20(this.item20);
                case 21:
                    return callback21(this.item21);
                case 22:
                    return callback22(this.item22);
                case 23:
                    return callback23(this.item23);
                case 24:
                    return callback24(this.item24);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5,
System.Func<TState, T6, TResult> callback6,
System.Func<TState, T7, TResult> callback7,
System.Func<TState, T8, TResult> callback8,
System.Func<TState, T9, TResult> callback9,
System.Func<TState, T10, TResult> callback10,
System.Func<TState, T11, TResult> callback11,
System.Func<TState, T12, TResult> callback12,
System.Func<TState, T13, TResult> callback13,
System.Func<TState, T14, TResult> callback14,
System.Func<TState, T15, TResult> callback15,
System.Func<TState, T16, TResult> callback16,
System.Func<TState, T17, TResult> callback17,
System.Func<TState, T18, TResult> callback18,
System.Func<TState, T19, TResult> callback19,
System.Func<TState, T20, TResult> callback20,
System.Func<TState, T21, TResult> callback21,
System.Func<TState, T22, TResult> callback22,
System.Func<TState, T23, TResult> callback23,
System.Func<TState, T24, TResult> callback24)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                case 6:
                    return callback6(state, this.item6);
                case 7:
                    return callback7(state, this.item7);
                case 8:
                    return callback8(state, this.item8);
                case 9:
                    return callback9(state, this.item9);
                case 10:
                    return callback10(state, this.item10);
                case 11:
                    return callback11(state, this.item11);
                case 12:
                    return callback12(state, this.item12);
                case 13:
                    return callback13(state, this.item13);
                case 14:
                    return callback14(state, this.item14);
                case 15:
                    return callback15(state, this.item15);
                case 16:
                    return callback16(state, this.item16);
                case 17:
                    return callback17(state, this.item17);
                case 18:
                    return callback18(state, this.item18);
                case 19:
                    return callback19(state, this.item19);
                case 20:
                    return callback20(state, this.item20);
                case 21:
                    return callback21(state, this.item21);
                case 22:
                    return callback22(state, this.item22);
                case 23:
                    return callback23(state, this.item23);
                case 24:
                    return callback24(state, this.item24);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;


        private readonly T6 item6;


        private readonly T7 item7;


        private readonly T8 item8;


        private readonly T9 item9;


        private readonly T10 item10;


        private readonly T11 item11;


        private readonly T12 item12;


        private readonly T13 item13;


        private readonly T14 item14;


        private readonly T15 item15;


        private readonly T16 item16;


        private readonly T17 item17;


        private readonly T18 item18;


        private readonly T19 item19;


        private readonly T20 item20;


        private readonly T21 item21;


        private readonly T22 item22;


        private readonly T23 item23;


        private readonly T24 item24;


        private readonly T25 item25;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public FlatBufferUnion(T6 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 6;
            this.item6 = item;
        }


        public FlatBufferUnion(T7 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 7;
            this.item7 = item;
        }


        public FlatBufferUnion(T8 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 8;
            this.item8 = item;
        }


        public FlatBufferUnion(T9 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 9;
            this.item9 = item;
        }


        public FlatBufferUnion(T10 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 10;
            this.item10 = item;
        }


        public FlatBufferUnion(T11 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 11;
            this.item11 = item;
        }


        public FlatBufferUnion(T12 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 12;
            this.item12 = item;
        }


        public FlatBufferUnion(T13 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 13;
            this.item13 = item;
        }


        public FlatBufferUnion(T14 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 14;
            this.item14 = item;
        }


        public FlatBufferUnion(T15 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 15;
            this.item15 = item;
        }


        public FlatBufferUnion(T16 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 16;
            this.item16 = item;
        }


        public FlatBufferUnion(T17 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 17;
            this.item17 = item;
        }


        public FlatBufferUnion(T18 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 18;
            this.item18 = item;
        }


        public FlatBufferUnion(T19 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 19;
            this.item19 = item;
        }


        public FlatBufferUnion(T20 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 20;
            this.item20 = item;
        }


        public FlatBufferUnion(T21 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 21;
            this.item21 = item;
        }


        public FlatBufferUnion(T22 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 22;
            this.item22 = item;
        }


        public FlatBufferUnion(T23 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 23;
            this.item23 = item;
        }


        public FlatBufferUnion(T24 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 24;
            this.item24 = item;
        }


        public FlatBufferUnion(T25 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 25;
            this.item25 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public T6 Item6
        {
            get
            {
                if (this.discriminator == 6)
                {
                    return this.item6;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T6 item)
        {
            item = default;
            if (this.discriminator == 6)
            {
                item = this.item6;
                return true;
            }

            return false;
        }


        public T7 Item7
        {
            get
            {
                if (this.discriminator == 7)
                {
                    return this.item7;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T7 item)
        {
            item = default;
            if (this.discriminator == 7)
            {
                item = this.item7;
                return true;
            }

            return false;
        }


        public T8 Item8
        {
            get
            {
                if (this.discriminator == 8)
                {
                    return this.item8;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T8 item)
        {
            item = default;
            if (this.discriminator == 8)
            {
                item = this.item8;
                return true;
            }

            return false;
        }


        public T9 Item9
        {
            get
            {
                if (this.discriminator == 9)
                {
                    return this.item9;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T9 item)
        {
            item = default;
            if (this.discriminator == 9)
            {
                item = this.item9;
                return true;
            }

            return false;
        }


        public T10 Item10
        {
            get
            {
                if (this.discriminator == 10)
                {
                    return this.item10;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T10 item)
        {
            item = default;
            if (this.discriminator == 10)
            {
                item = this.item10;
                return true;
            }

            return false;
        }


        public T11 Item11
        {
            get
            {
                if (this.discriminator == 11)
                {
                    return this.item11;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T11 item)
        {
            item = default;
            if (this.discriminator == 11)
            {
                item = this.item11;
                return true;
            }

            return false;
        }


        public T12 Item12
        {
            get
            {
                if (this.discriminator == 12)
                {
                    return this.item12;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T12 item)
        {
            item = default;
            if (this.discriminator == 12)
            {
                item = this.item12;
                return true;
            }

            return false;
        }


        public T13 Item13
        {
            get
            {
                if (this.discriminator == 13)
                {
                    return this.item13;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T13 item)
        {
            item = default;
            if (this.discriminator == 13)
            {
                item = this.item13;
                return true;
            }

            return false;
        }


        public T14 Item14
        {
            get
            {
                if (this.discriminator == 14)
                {
                    return this.item14;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T14 item)
        {
            item = default;
            if (this.discriminator == 14)
            {
                item = this.item14;
                return true;
            }

            return false;
        }


        public T15 Item15
        {
            get
            {
                if (this.discriminator == 15)
                {
                    return this.item15;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T15 item)
        {
            item = default;
            if (this.discriminator == 15)
            {
                item = this.item15;
                return true;
            }

            return false;
        }


        public T16 Item16
        {
            get
            {
                if (this.discriminator == 16)
                {
                    return this.item16;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T16 item)
        {
            item = default;
            if (this.discriminator == 16)
            {
                item = this.item16;
                return true;
            }

            return false;
        }


        public T17 Item17
        {
            get
            {
                if (this.discriminator == 17)
                {
                    return this.item17;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T17 item)
        {
            item = default;
            if (this.discriminator == 17)
            {
                item = this.item17;
                return true;
            }

            return false;
        }


        public T18 Item18
        {
            get
            {
                if (this.discriminator == 18)
                {
                    return this.item18;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T18 item)
        {
            item = default;
            if (this.discriminator == 18)
            {
                item = this.item18;
                return true;
            }

            return false;
        }


        public T19 Item19
        {
            get
            {
                if (this.discriminator == 19)
                {
                    return this.item19;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T19 item)
        {
            item = default;
            if (this.discriminator == 19)
            {
                item = this.item19;
                return true;
            }

            return false;
        }


        public T20 Item20
        {
            get
            {
                if (this.discriminator == 20)
                {
                    return this.item20;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T20 item)
        {
            item = default;
            if (this.discriminator == 20)
            {
                item = this.item20;
                return true;
            }

            return false;
        }


        public T21 Item21
        {
            get
            {
                if (this.discriminator == 21)
                {
                    return this.item21;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T21 item)
        {
            item = default;
            if (this.discriminator == 21)
            {
                item = this.item21;
                return true;
            }

            return false;
        }


        public T22 Item22
        {
            get
            {
                if (this.discriminator == 22)
                {
                    return this.item22;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T22 item)
        {
            item = default;
            if (this.discriminator == 22)
            {
                item = this.item22;
                return true;
            }

            return false;
        }


        public T23 Item23
        {
            get
            {
                if (this.discriminator == 23)
                {
                    return this.item23;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T23 item)
        {
            item = default;
            if (this.discriminator == 23)
            {
                item = this.item23;
                return true;
            }

            return false;
        }


        public T24 Item24
        {
            get
            {
                if (this.discriminator == 24)
                {
                    return this.item24;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T24 item)
        {
            item = default;
            if (this.discriminator == 24)
            {
                item = this.item24;
                return true;
            }

            return false;
        }


        public T25 Item25
        {
            get
            {
                if (this.discriminator == 25)
                {
                    return this.item25;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T25 item)
        {
            item = default;
            if (this.discriminator == 25)
            {
                item = this.item25;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5,
System.Func<T6, T6> cloneT6,
System.Func<T7, T7> cloneT7,
System.Func<T8, T8> cloneT8,
System.Func<T9, T9> cloneT9,
System.Func<T10, T10> cloneT10,
System.Func<T11, T11> cloneT11,
System.Func<T12, T12> cloneT12,
System.Func<T13, T13> cloneT13,
System.Func<T14, T14> cloneT14,
System.Func<T15, T15> cloneT15,
System.Func<T16, T16> cloneT16,
System.Func<T17, T17> cloneT17,
System.Func<T18, T18> cloneT18,
System.Func<T19, T19> cloneT19,
System.Func<T20, T20> cloneT20,
System.Func<T21, T21> cloneT21,
System.Func<T22, T22> cloneT22,
System.Func<T23, T23> cloneT23,
System.Func<T24, T24> cloneT24,
System.Func<T25, T25> cloneT25
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(cloneT5(this.item5));
                case 6:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(cloneT6(this.item6));
                case 7:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(cloneT7(this.item7));
                case 8:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(cloneT8(this.item8));
                case 9:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(cloneT9(this.item9));
                case 10:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(cloneT10(this.item10));
                case 11:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(cloneT11(this.item11));
                case 12:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(cloneT12(this.item12));
                case 13:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(cloneT13(this.item13));
                case 14:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(cloneT14(this.item14));
                case 15:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(cloneT15(this.item15));
                case 16:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(cloneT16(this.item16));
                case 17:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(cloneT17(this.item17));
                case 18:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(cloneT18(this.item18));
                case 19:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(cloneT19(this.item19));
                case 20:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(cloneT20(this.item20));
                case 21:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(cloneT21(this.item21));
                case 22:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(cloneT22(this.item22));
                case 23:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(cloneT23(this.item23));
                case 24:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(cloneT24(this.item24));
                case 25:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>(cloneT25(this.item25));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5,
System.Action<T6> callback6,
System.Action<T7> callback7,
System.Action<T8> callback8,
System.Action<T9> callback9,
System.Action<T10> callback10,
System.Action<T11> callback11,
System.Action<T12> callback12,
System.Action<T13> callback13,
System.Action<T14> callback14,
System.Action<T15> callback15,
System.Action<T16> callback16,
System.Action<T17> callback17,
System.Action<T18> callback18,
System.Action<T19> callback19,
System.Action<T20> callback20,
System.Action<T21> callback21,
System.Action<T22> callback22,
System.Action<T23> callback23,
System.Action<T24> callback24,
System.Action<T25> callback25)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                case 6:
                    callback6(this.item6);
                    break;
                case 7:
                    callback7(this.item7);
                    break;
                case 8:
                    callback8(this.item8);
                    break;
                case 9:
                    callback9(this.item9);
                    break;
                case 10:
                    callback10(this.item10);
                    break;
                case 11:
                    callback11(this.item11);
                    break;
                case 12:
                    callback12(this.item12);
                    break;
                case 13:
                    callback13(this.item13);
                    break;
                case 14:
                    callback14(this.item14);
                    break;
                case 15:
                    callback15(this.item15);
                    break;
                case 16:
                    callback16(this.item16);
                    break;
                case 17:
                    callback17(this.item17);
                    break;
                case 18:
                    callback18(this.item18);
                    break;
                case 19:
                    callback19(this.item19);
                    break;
                case 20:
                    callback20(this.item20);
                    break;
                case 21:
                    callback21(this.item21);
                    break;
                case 22:
                    callback22(this.item22);
                    break;
                case 23:
                    callback23(this.item23);
                    break;
                case 24:
                    callback24(this.item24);
                    break;
                case 25:
                    callback25(this.item25);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5,
System.Action<TState, T6> callback6,
System.Action<TState, T7> callback7,
System.Action<TState, T8> callback8,
System.Action<TState, T9> callback9,
System.Action<TState, T10> callback10,
System.Action<TState, T11> callback11,
System.Action<TState, T12> callback12,
System.Action<TState, T13> callback13,
System.Action<TState, T14> callback14,
System.Action<TState, T15> callback15,
System.Action<TState, T16> callback16,
System.Action<TState, T17> callback17,
System.Action<TState, T18> callback18,
System.Action<TState, T19> callback19,
System.Action<TState, T20> callback20,
System.Action<TState, T21> callback21,
System.Action<TState, T22> callback22,
System.Action<TState, T23> callback23,
System.Action<TState, T24> callback24,
System.Action<TState, T25> callback25)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                case 6:
                    callback6(state, this.item6);
                    break;
                case 7:
                    callback7(state, this.item7);
                    break;
                case 8:
                    callback8(state, this.item8);
                    break;
                case 9:
                    callback9(state, this.item9);
                    break;
                case 10:
                    callback10(state, this.item10);
                    break;
                case 11:
                    callback11(state, this.item11);
                    break;
                case 12:
                    callback12(state, this.item12);
                    break;
                case 13:
                    callback13(state, this.item13);
                    break;
                case 14:
                    callback14(state, this.item14);
                    break;
                case 15:
                    callback15(state, this.item15);
                    break;
                case 16:
                    callback16(state, this.item16);
                    break;
                case 17:
                    callback17(state, this.item17);
                    break;
                case 18:
                    callback18(state, this.item18);
                    break;
                case 19:
                    callback19(state, this.item19);
                    break;
                case 20:
                    callback20(state, this.item20);
                    break;
                case 21:
                    callback21(state, this.item21);
                    break;
                case 22:
                    callback22(state, this.item22);
                    break;
                case 23:
                    callback23(state, this.item23);
                    break;
                case 24:
                    callback24(state, this.item24);
                    break;
                case 25:
                    callback25(state, this.item25);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5,
System.Func<T6, TResult> callback6,
System.Func<T7, TResult> callback7,
System.Func<T8, TResult> callback8,
System.Func<T9, TResult> callback9,
System.Func<T10, TResult> callback10,
System.Func<T11, TResult> callback11,
System.Func<T12, TResult> callback12,
System.Func<T13, TResult> callback13,
System.Func<T14, TResult> callback14,
System.Func<T15, TResult> callback15,
System.Func<T16, TResult> callback16,
System.Func<T17, TResult> callback17,
System.Func<T18, TResult> callback18,
System.Func<T19, TResult> callback19,
System.Func<T20, TResult> callback20,
System.Func<T21, TResult> callback21,
System.Func<T22, TResult> callback22,
System.Func<T23, TResult> callback23,
System.Func<T24, TResult> callback24,
System.Func<T25, TResult> callback25)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                case 6:
                    return callback6(this.item6);
                case 7:
                    return callback7(this.item7);
                case 8:
                    return callback8(this.item8);
                case 9:
                    return callback9(this.item9);
                case 10:
                    return callback10(this.item10);
                case 11:
                    return callback11(this.item11);
                case 12:
                    return callback12(this.item12);
                case 13:
                    return callback13(this.item13);
                case 14:
                    return callback14(this.item14);
                case 15:
                    return callback15(this.item15);
                case 16:
                    return callback16(this.item16);
                case 17:
                    return callback17(this.item17);
                case 18:
                    return callback18(this.item18);
                case 19:
                    return callback19(this.item19);
                case 20:
                    return callback20(this.item20);
                case 21:
                    return callback21(this.item21);
                case 22:
                    return callback22(this.item22);
                case 23:
                    return callback23(this.item23);
                case 24:
                    return callback24(this.item24);
                case 25:
                    return callback25(this.item25);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5,
System.Func<TState, T6, TResult> callback6,
System.Func<TState, T7, TResult> callback7,
System.Func<TState, T8, TResult> callback8,
System.Func<TState, T9, TResult> callback9,
System.Func<TState, T10, TResult> callback10,
System.Func<TState, T11, TResult> callback11,
System.Func<TState, T12, TResult> callback12,
System.Func<TState, T13, TResult> callback13,
System.Func<TState, T14, TResult> callback14,
System.Func<TState, T15, TResult> callback15,
System.Func<TState, T16, TResult> callback16,
System.Func<TState, T17, TResult> callback17,
System.Func<TState, T18, TResult> callback18,
System.Func<TState, T19, TResult> callback19,
System.Func<TState, T20, TResult> callback20,
System.Func<TState, T21, TResult> callback21,
System.Func<TState, T22, TResult> callback22,
System.Func<TState, T23, TResult> callback23,
System.Func<TState, T24, TResult> callback24,
System.Func<TState, T25, TResult> callback25)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                case 6:
                    return callback6(state, this.item6);
                case 7:
                    return callback7(state, this.item7);
                case 8:
                    return callback8(state, this.item8);
                case 9:
                    return callback9(state, this.item9);
                case 10:
                    return callback10(state, this.item10);
                case 11:
                    return callback11(state, this.item11);
                case 12:
                    return callback12(state, this.item12);
                case 13:
                    return callback13(state, this.item13);
                case 14:
                    return callback14(state, this.item14);
                case 15:
                    return callback15(state, this.item15);
                case 16:
                    return callback16(state, this.item16);
                case 17:
                    return callback17(state, this.item17);
                case 18:
                    return callback18(state, this.item18);
                case 19:
                    return callback19(state, this.item19);
                case 20:
                    return callback20(state, this.item20);
                case 21:
                    return callback21(state, this.item21);
                case 22:
                    return callback22(state, this.item22);
                case 23:
                    return callback23(state, this.item23);
                case 24:
                    return callback24(state, this.item24);
                case 25:
                    return callback25(state, this.item25);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;


        private readonly T6 item6;


        private readonly T7 item7;


        private readonly T8 item8;


        private readonly T9 item9;


        private readonly T10 item10;


        private readonly T11 item11;


        private readonly T12 item12;


        private readonly T13 item13;


        private readonly T14 item14;


        private readonly T15 item15;


        private readonly T16 item16;


        private readonly T17 item17;


        private readonly T18 item18;


        private readonly T19 item19;


        private readonly T20 item20;


        private readonly T21 item21;


        private readonly T22 item22;


        private readonly T23 item23;


        private readonly T24 item24;


        private readonly T25 item25;


        private readonly T26 item26;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public FlatBufferUnion(T6 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 6;
            this.item6 = item;
        }


        public FlatBufferUnion(T7 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 7;
            this.item7 = item;
        }


        public FlatBufferUnion(T8 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 8;
            this.item8 = item;
        }


        public FlatBufferUnion(T9 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 9;
            this.item9 = item;
        }


        public FlatBufferUnion(T10 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 10;
            this.item10 = item;
        }


        public FlatBufferUnion(T11 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 11;
            this.item11 = item;
        }


        public FlatBufferUnion(T12 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 12;
            this.item12 = item;
        }


        public FlatBufferUnion(T13 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 13;
            this.item13 = item;
        }


        public FlatBufferUnion(T14 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 14;
            this.item14 = item;
        }


        public FlatBufferUnion(T15 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 15;
            this.item15 = item;
        }


        public FlatBufferUnion(T16 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 16;
            this.item16 = item;
        }


        public FlatBufferUnion(T17 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 17;
            this.item17 = item;
        }


        public FlatBufferUnion(T18 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 18;
            this.item18 = item;
        }


        public FlatBufferUnion(T19 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 19;
            this.item19 = item;
        }


        public FlatBufferUnion(T20 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 20;
            this.item20 = item;
        }


        public FlatBufferUnion(T21 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 21;
            this.item21 = item;
        }


        public FlatBufferUnion(T22 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 22;
            this.item22 = item;
        }


        public FlatBufferUnion(T23 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 23;
            this.item23 = item;
        }


        public FlatBufferUnion(T24 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 24;
            this.item24 = item;
        }


        public FlatBufferUnion(T25 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 25;
            this.item25 = item;
        }


        public FlatBufferUnion(T26 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 26;
            this.item26 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public T6 Item6
        {
            get
            {
                if (this.discriminator == 6)
                {
                    return this.item6;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T6 item)
        {
            item = default;
            if (this.discriminator == 6)
            {
                item = this.item6;
                return true;
            }

            return false;
        }


        public T7 Item7
        {
            get
            {
                if (this.discriminator == 7)
                {
                    return this.item7;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T7 item)
        {
            item = default;
            if (this.discriminator == 7)
            {
                item = this.item7;
                return true;
            }

            return false;
        }


        public T8 Item8
        {
            get
            {
                if (this.discriminator == 8)
                {
                    return this.item8;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T8 item)
        {
            item = default;
            if (this.discriminator == 8)
            {
                item = this.item8;
                return true;
            }

            return false;
        }


        public T9 Item9
        {
            get
            {
                if (this.discriminator == 9)
                {
                    return this.item9;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T9 item)
        {
            item = default;
            if (this.discriminator == 9)
            {
                item = this.item9;
                return true;
            }

            return false;
        }


        public T10 Item10
        {
            get
            {
                if (this.discriminator == 10)
                {
                    return this.item10;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T10 item)
        {
            item = default;
            if (this.discriminator == 10)
            {
                item = this.item10;
                return true;
            }

            return false;
        }


        public T11 Item11
        {
            get
            {
                if (this.discriminator == 11)
                {
                    return this.item11;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T11 item)
        {
            item = default;
            if (this.discriminator == 11)
            {
                item = this.item11;
                return true;
            }

            return false;
        }


        public T12 Item12
        {
            get
            {
                if (this.discriminator == 12)
                {
                    return this.item12;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T12 item)
        {
            item = default;
            if (this.discriminator == 12)
            {
                item = this.item12;
                return true;
            }

            return false;
        }


        public T13 Item13
        {
            get
            {
                if (this.discriminator == 13)
                {
                    return this.item13;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T13 item)
        {
            item = default;
            if (this.discriminator == 13)
            {
                item = this.item13;
                return true;
            }

            return false;
        }


        public T14 Item14
        {
            get
            {
                if (this.discriminator == 14)
                {
                    return this.item14;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T14 item)
        {
            item = default;
            if (this.discriminator == 14)
            {
                item = this.item14;
                return true;
            }

            return false;
        }


        public T15 Item15
        {
            get
            {
                if (this.discriminator == 15)
                {
                    return this.item15;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T15 item)
        {
            item = default;
            if (this.discriminator == 15)
            {
                item = this.item15;
                return true;
            }

            return false;
        }


        public T16 Item16
        {
            get
            {
                if (this.discriminator == 16)
                {
                    return this.item16;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T16 item)
        {
            item = default;
            if (this.discriminator == 16)
            {
                item = this.item16;
                return true;
            }

            return false;
        }


        public T17 Item17
        {
            get
            {
                if (this.discriminator == 17)
                {
                    return this.item17;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T17 item)
        {
            item = default;
            if (this.discriminator == 17)
            {
                item = this.item17;
                return true;
            }

            return false;
        }


        public T18 Item18
        {
            get
            {
                if (this.discriminator == 18)
                {
                    return this.item18;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T18 item)
        {
            item = default;
            if (this.discriminator == 18)
            {
                item = this.item18;
                return true;
            }

            return false;
        }


        public T19 Item19
        {
            get
            {
                if (this.discriminator == 19)
                {
                    return this.item19;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T19 item)
        {
            item = default;
            if (this.discriminator == 19)
            {
                item = this.item19;
                return true;
            }

            return false;
        }


        public T20 Item20
        {
            get
            {
                if (this.discriminator == 20)
                {
                    return this.item20;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T20 item)
        {
            item = default;
            if (this.discriminator == 20)
            {
                item = this.item20;
                return true;
            }

            return false;
        }


        public T21 Item21
        {
            get
            {
                if (this.discriminator == 21)
                {
                    return this.item21;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T21 item)
        {
            item = default;
            if (this.discriminator == 21)
            {
                item = this.item21;
                return true;
            }

            return false;
        }


        public T22 Item22
        {
            get
            {
                if (this.discriminator == 22)
                {
                    return this.item22;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T22 item)
        {
            item = default;
            if (this.discriminator == 22)
            {
                item = this.item22;
                return true;
            }

            return false;
        }


        public T23 Item23
        {
            get
            {
                if (this.discriminator == 23)
                {
                    return this.item23;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T23 item)
        {
            item = default;
            if (this.discriminator == 23)
            {
                item = this.item23;
                return true;
            }

            return false;
        }


        public T24 Item24
        {
            get
            {
                if (this.discriminator == 24)
                {
                    return this.item24;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T24 item)
        {
            item = default;
            if (this.discriminator == 24)
            {
                item = this.item24;
                return true;
            }

            return false;
        }


        public T25 Item25
        {
            get
            {
                if (this.discriminator == 25)
                {
                    return this.item25;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T25 item)
        {
            item = default;
            if (this.discriminator == 25)
            {
                item = this.item25;
                return true;
            }

            return false;
        }


        public T26 Item26
        {
            get
            {
                if (this.discriminator == 26)
                {
                    return this.item26;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T26 item)
        {
            item = default;
            if (this.discriminator == 26)
            {
                item = this.item26;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5,
System.Func<T6, T6> cloneT6,
System.Func<T7, T7> cloneT7,
System.Func<T8, T8> cloneT8,
System.Func<T9, T9> cloneT9,
System.Func<T10, T10> cloneT10,
System.Func<T11, T11> cloneT11,
System.Func<T12, T12> cloneT12,
System.Func<T13, T13> cloneT13,
System.Func<T14, T14> cloneT14,
System.Func<T15, T15> cloneT15,
System.Func<T16, T16> cloneT16,
System.Func<T17, T17> cloneT17,
System.Func<T18, T18> cloneT18,
System.Func<T19, T19> cloneT19,
System.Func<T20, T20> cloneT20,
System.Func<T21, T21> cloneT21,
System.Func<T22, T22> cloneT22,
System.Func<T23, T23> cloneT23,
System.Func<T24, T24> cloneT24,
System.Func<T25, T25> cloneT25,
System.Func<T26, T26> cloneT26
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT5(this.item5));
                case 6:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT6(this.item6));
                case 7:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT7(this.item7));
                case 8:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT8(this.item8));
                case 9:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT9(this.item9));
                case 10:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT10(this.item10));
                case 11:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT11(this.item11));
                case 12:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT12(this.item12));
                case 13:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT13(this.item13));
                case 14:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT14(this.item14));
                case 15:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT15(this.item15));
                case 16:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT16(this.item16));
                case 17:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT17(this.item17));
                case 18:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT18(this.item18));
                case 19:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT19(this.item19));
                case 20:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT20(this.item20));
                case 21:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT21(this.item21));
                case 22:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT22(this.item22));
                case 23:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT23(this.item23));
                case 24:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT24(this.item24));
                case 25:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT25(this.item25));
                case 26:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>(cloneT26(this.item26));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5,
System.Action<T6> callback6,
System.Action<T7> callback7,
System.Action<T8> callback8,
System.Action<T9> callback9,
System.Action<T10> callback10,
System.Action<T11> callback11,
System.Action<T12> callback12,
System.Action<T13> callback13,
System.Action<T14> callback14,
System.Action<T15> callback15,
System.Action<T16> callback16,
System.Action<T17> callback17,
System.Action<T18> callback18,
System.Action<T19> callback19,
System.Action<T20> callback20,
System.Action<T21> callback21,
System.Action<T22> callback22,
System.Action<T23> callback23,
System.Action<T24> callback24,
System.Action<T25> callback25,
System.Action<T26> callback26)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                case 6:
                    callback6(this.item6);
                    break;
                case 7:
                    callback7(this.item7);
                    break;
                case 8:
                    callback8(this.item8);
                    break;
                case 9:
                    callback9(this.item9);
                    break;
                case 10:
                    callback10(this.item10);
                    break;
                case 11:
                    callback11(this.item11);
                    break;
                case 12:
                    callback12(this.item12);
                    break;
                case 13:
                    callback13(this.item13);
                    break;
                case 14:
                    callback14(this.item14);
                    break;
                case 15:
                    callback15(this.item15);
                    break;
                case 16:
                    callback16(this.item16);
                    break;
                case 17:
                    callback17(this.item17);
                    break;
                case 18:
                    callback18(this.item18);
                    break;
                case 19:
                    callback19(this.item19);
                    break;
                case 20:
                    callback20(this.item20);
                    break;
                case 21:
                    callback21(this.item21);
                    break;
                case 22:
                    callback22(this.item22);
                    break;
                case 23:
                    callback23(this.item23);
                    break;
                case 24:
                    callback24(this.item24);
                    break;
                case 25:
                    callback25(this.item25);
                    break;
                case 26:
                    callback26(this.item26);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5,
System.Action<TState, T6> callback6,
System.Action<TState, T7> callback7,
System.Action<TState, T8> callback8,
System.Action<TState, T9> callback9,
System.Action<TState, T10> callback10,
System.Action<TState, T11> callback11,
System.Action<TState, T12> callback12,
System.Action<TState, T13> callback13,
System.Action<TState, T14> callback14,
System.Action<TState, T15> callback15,
System.Action<TState, T16> callback16,
System.Action<TState, T17> callback17,
System.Action<TState, T18> callback18,
System.Action<TState, T19> callback19,
System.Action<TState, T20> callback20,
System.Action<TState, T21> callback21,
System.Action<TState, T22> callback22,
System.Action<TState, T23> callback23,
System.Action<TState, T24> callback24,
System.Action<TState, T25> callback25,
System.Action<TState, T26> callback26)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                case 6:
                    callback6(state, this.item6);
                    break;
                case 7:
                    callback7(state, this.item7);
                    break;
                case 8:
                    callback8(state, this.item8);
                    break;
                case 9:
                    callback9(state, this.item9);
                    break;
                case 10:
                    callback10(state, this.item10);
                    break;
                case 11:
                    callback11(state, this.item11);
                    break;
                case 12:
                    callback12(state, this.item12);
                    break;
                case 13:
                    callback13(state, this.item13);
                    break;
                case 14:
                    callback14(state, this.item14);
                    break;
                case 15:
                    callback15(state, this.item15);
                    break;
                case 16:
                    callback16(state, this.item16);
                    break;
                case 17:
                    callback17(state, this.item17);
                    break;
                case 18:
                    callback18(state, this.item18);
                    break;
                case 19:
                    callback19(state, this.item19);
                    break;
                case 20:
                    callback20(state, this.item20);
                    break;
                case 21:
                    callback21(state, this.item21);
                    break;
                case 22:
                    callback22(state, this.item22);
                    break;
                case 23:
                    callback23(state, this.item23);
                    break;
                case 24:
                    callback24(state, this.item24);
                    break;
                case 25:
                    callback25(state, this.item25);
                    break;
                case 26:
                    callback26(state, this.item26);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5,
System.Func<T6, TResult> callback6,
System.Func<T7, TResult> callback7,
System.Func<T8, TResult> callback8,
System.Func<T9, TResult> callback9,
System.Func<T10, TResult> callback10,
System.Func<T11, TResult> callback11,
System.Func<T12, TResult> callback12,
System.Func<T13, TResult> callback13,
System.Func<T14, TResult> callback14,
System.Func<T15, TResult> callback15,
System.Func<T16, TResult> callback16,
System.Func<T17, TResult> callback17,
System.Func<T18, TResult> callback18,
System.Func<T19, TResult> callback19,
System.Func<T20, TResult> callback20,
System.Func<T21, TResult> callback21,
System.Func<T22, TResult> callback22,
System.Func<T23, TResult> callback23,
System.Func<T24, TResult> callback24,
System.Func<T25, TResult> callback25,
System.Func<T26, TResult> callback26)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                case 6:
                    return callback6(this.item6);
                case 7:
                    return callback7(this.item7);
                case 8:
                    return callback8(this.item8);
                case 9:
                    return callback9(this.item9);
                case 10:
                    return callback10(this.item10);
                case 11:
                    return callback11(this.item11);
                case 12:
                    return callback12(this.item12);
                case 13:
                    return callback13(this.item13);
                case 14:
                    return callback14(this.item14);
                case 15:
                    return callback15(this.item15);
                case 16:
                    return callback16(this.item16);
                case 17:
                    return callback17(this.item17);
                case 18:
                    return callback18(this.item18);
                case 19:
                    return callback19(this.item19);
                case 20:
                    return callback20(this.item20);
                case 21:
                    return callback21(this.item21);
                case 22:
                    return callback22(this.item22);
                case 23:
                    return callback23(this.item23);
                case 24:
                    return callback24(this.item24);
                case 25:
                    return callback25(this.item25);
                case 26:
                    return callback26(this.item26);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5,
System.Func<TState, T6, TResult> callback6,
System.Func<TState, T7, TResult> callback7,
System.Func<TState, T8, TResult> callback8,
System.Func<TState, T9, TResult> callback9,
System.Func<TState, T10, TResult> callback10,
System.Func<TState, T11, TResult> callback11,
System.Func<TState, T12, TResult> callback12,
System.Func<TState, T13, TResult> callback13,
System.Func<TState, T14, TResult> callback14,
System.Func<TState, T15, TResult> callback15,
System.Func<TState, T16, TResult> callback16,
System.Func<TState, T17, TResult> callback17,
System.Func<TState, T18, TResult> callback18,
System.Func<TState, T19, TResult> callback19,
System.Func<TState, T20, TResult> callback20,
System.Func<TState, T21, TResult> callback21,
System.Func<TState, T22, TResult> callback22,
System.Func<TState, T23, TResult> callback23,
System.Func<TState, T24, TResult> callback24,
System.Func<TState, T25, TResult> callback25,
System.Func<TState, T26, TResult> callback26)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                case 6:
                    return callback6(state, this.item6);
                case 7:
                    return callback7(state, this.item7);
                case 8:
                    return callback8(state, this.item8);
                case 9:
                    return callback9(state, this.item9);
                case 10:
                    return callback10(state, this.item10);
                case 11:
                    return callback11(state, this.item11);
                case 12:
                    return callback12(state, this.item12);
                case 13:
                    return callback13(state, this.item13);
                case 14:
                    return callback14(state, this.item14);
                case 15:
                    return callback15(state, this.item15);
                case 16:
                    return callback16(state, this.item16);
                case 17:
                    return callback17(state, this.item17);
                case 18:
                    return callback18(state, this.item18);
                case 19:
                    return callback19(state, this.item19);
                case 20:
                    return callback20(state, this.item20);
                case 21:
                    return callback21(state, this.item21);
                case 22:
                    return callback22(state, this.item22);
                case 23:
                    return callback23(state, this.item23);
                case 24:
                    return callback24(state, this.item24);
                case 25:
                    return callback25(state, this.item25);
                case 26:
                    return callback26(state, this.item26);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;


        private readonly T6 item6;


        private readonly T7 item7;


        private readonly T8 item8;


        private readonly T9 item9;


        private readonly T10 item10;


        private readonly T11 item11;


        private readonly T12 item12;


        private readonly T13 item13;


        private readonly T14 item14;


        private readonly T15 item15;


        private readonly T16 item16;


        private readonly T17 item17;


        private readonly T18 item18;


        private readonly T19 item19;


        private readonly T20 item20;


        private readonly T21 item21;


        private readonly T22 item22;


        private readonly T23 item23;


        private readonly T24 item24;


        private readonly T25 item25;


        private readonly T26 item26;


        private readonly T27 item27;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public FlatBufferUnion(T6 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 6;
            this.item6 = item;
        }


        public FlatBufferUnion(T7 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 7;
            this.item7 = item;
        }


        public FlatBufferUnion(T8 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 8;
            this.item8 = item;
        }


        public FlatBufferUnion(T9 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 9;
            this.item9 = item;
        }


        public FlatBufferUnion(T10 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 10;
            this.item10 = item;
        }


        public FlatBufferUnion(T11 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 11;
            this.item11 = item;
        }


        public FlatBufferUnion(T12 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 12;
            this.item12 = item;
        }


        public FlatBufferUnion(T13 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 13;
            this.item13 = item;
        }


        public FlatBufferUnion(T14 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 14;
            this.item14 = item;
        }


        public FlatBufferUnion(T15 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 15;
            this.item15 = item;
        }


        public FlatBufferUnion(T16 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 16;
            this.item16 = item;
        }


        public FlatBufferUnion(T17 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 17;
            this.item17 = item;
        }


        public FlatBufferUnion(T18 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 18;
            this.item18 = item;
        }


        public FlatBufferUnion(T19 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 19;
            this.item19 = item;
        }


        public FlatBufferUnion(T20 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 20;
            this.item20 = item;
        }


        public FlatBufferUnion(T21 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 21;
            this.item21 = item;
        }


        public FlatBufferUnion(T22 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 22;
            this.item22 = item;
        }


        public FlatBufferUnion(T23 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 23;
            this.item23 = item;
        }


        public FlatBufferUnion(T24 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 24;
            this.item24 = item;
        }


        public FlatBufferUnion(T25 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 25;
            this.item25 = item;
        }


        public FlatBufferUnion(T26 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 26;
            this.item26 = item;
        }


        public FlatBufferUnion(T27 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 27;
            this.item27 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public T6 Item6
        {
            get
            {
                if (this.discriminator == 6)
                {
                    return this.item6;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T6 item)
        {
            item = default;
            if (this.discriminator == 6)
            {
                item = this.item6;
                return true;
            }

            return false;
        }


        public T7 Item7
        {
            get
            {
                if (this.discriminator == 7)
                {
                    return this.item7;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T7 item)
        {
            item = default;
            if (this.discriminator == 7)
            {
                item = this.item7;
                return true;
            }

            return false;
        }


        public T8 Item8
        {
            get
            {
                if (this.discriminator == 8)
                {
                    return this.item8;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T8 item)
        {
            item = default;
            if (this.discriminator == 8)
            {
                item = this.item8;
                return true;
            }

            return false;
        }


        public T9 Item9
        {
            get
            {
                if (this.discriminator == 9)
                {
                    return this.item9;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T9 item)
        {
            item = default;
            if (this.discriminator == 9)
            {
                item = this.item9;
                return true;
            }

            return false;
        }


        public T10 Item10
        {
            get
            {
                if (this.discriminator == 10)
                {
                    return this.item10;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T10 item)
        {
            item = default;
            if (this.discriminator == 10)
            {
                item = this.item10;
                return true;
            }

            return false;
        }


        public T11 Item11
        {
            get
            {
                if (this.discriminator == 11)
                {
                    return this.item11;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T11 item)
        {
            item = default;
            if (this.discriminator == 11)
            {
                item = this.item11;
                return true;
            }

            return false;
        }


        public T12 Item12
        {
            get
            {
                if (this.discriminator == 12)
                {
                    return this.item12;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T12 item)
        {
            item = default;
            if (this.discriminator == 12)
            {
                item = this.item12;
                return true;
            }

            return false;
        }


        public T13 Item13
        {
            get
            {
                if (this.discriminator == 13)
                {
                    return this.item13;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T13 item)
        {
            item = default;
            if (this.discriminator == 13)
            {
                item = this.item13;
                return true;
            }

            return false;
        }


        public T14 Item14
        {
            get
            {
                if (this.discriminator == 14)
                {
                    return this.item14;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T14 item)
        {
            item = default;
            if (this.discriminator == 14)
            {
                item = this.item14;
                return true;
            }

            return false;
        }


        public T15 Item15
        {
            get
            {
                if (this.discriminator == 15)
                {
                    return this.item15;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T15 item)
        {
            item = default;
            if (this.discriminator == 15)
            {
                item = this.item15;
                return true;
            }

            return false;
        }


        public T16 Item16
        {
            get
            {
                if (this.discriminator == 16)
                {
                    return this.item16;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T16 item)
        {
            item = default;
            if (this.discriminator == 16)
            {
                item = this.item16;
                return true;
            }

            return false;
        }


        public T17 Item17
        {
            get
            {
                if (this.discriminator == 17)
                {
                    return this.item17;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T17 item)
        {
            item = default;
            if (this.discriminator == 17)
            {
                item = this.item17;
                return true;
            }

            return false;
        }


        public T18 Item18
        {
            get
            {
                if (this.discriminator == 18)
                {
                    return this.item18;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T18 item)
        {
            item = default;
            if (this.discriminator == 18)
            {
                item = this.item18;
                return true;
            }

            return false;
        }


        public T19 Item19
        {
            get
            {
                if (this.discriminator == 19)
                {
                    return this.item19;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T19 item)
        {
            item = default;
            if (this.discriminator == 19)
            {
                item = this.item19;
                return true;
            }

            return false;
        }


        public T20 Item20
        {
            get
            {
                if (this.discriminator == 20)
                {
                    return this.item20;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T20 item)
        {
            item = default;
            if (this.discriminator == 20)
            {
                item = this.item20;
                return true;
            }

            return false;
        }


        public T21 Item21
        {
            get
            {
                if (this.discriminator == 21)
                {
                    return this.item21;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T21 item)
        {
            item = default;
            if (this.discriminator == 21)
            {
                item = this.item21;
                return true;
            }

            return false;
        }


        public T22 Item22
        {
            get
            {
                if (this.discriminator == 22)
                {
                    return this.item22;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T22 item)
        {
            item = default;
            if (this.discriminator == 22)
            {
                item = this.item22;
                return true;
            }

            return false;
        }


        public T23 Item23
        {
            get
            {
                if (this.discriminator == 23)
                {
                    return this.item23;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T23 item)
        {
            item = default;
            if (this.discriminator == 23)
            {
                item = this.item23;
                return true;
            }

            return false;
        }


        public T24 Item24
        {
            get
            {
                if (this.discriminator == 24)
                {
                    return this.item24;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T24 item)
        {
            item = default;
            if (this.discriminator == 24)
            {
                item = this.item24;
                return true;
            }

            return false;
        }


        public T25 Item25
        {
            get
            {
                if (this.discriminator == 25)
                {
                    return this.item25;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T25 item)
        {
            item = default;
            if (this.discriminator == 25)
            {
                item = this.item25;
                return true;
            }

            return false;
        }


        public T26 Item26
        {
            get
            {
                if (this.discriminator == 26)
                {
                    return this.item26;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T26 item)
        {
            item = default;
            if (this.discriminator == 26)
            {
                item = this.item26;
                return true;
            }

            return false;
        }


        public T27 Item27
        {
            get
            {
                if (this.discriminator == 27)
                {
                    return this.item27;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T27 item)
        {
            item = default;
            if (this.discriminator == 27)
            {
                item = this.item27;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5,
System.Func<T6, T6> cloneT6,
System.Func<T7, T7> cloneT7,
System.Func<T8, T8> cloneT8,
System.Func<T9, T9> cloneT9,
System.Func<T10, T10> cloneT10,
System.Func<T11, T11> cloneT11,
System.Func<T12, T12> cloneT12,
System.Func<T13, T13> cloneT13,
System.Func<T14, T14> cloneT14,
System.Func<T15, T15> cloneT15,
System.Func<T16, T16> cloneT16,
System.Func<T17, T17> cloneT17,
System.Func<T18, T18> cloneT18,
System.Func<T19, T19> cloneT19,
System.Func<T20, T20> cloneT20,
System.Func<T21, T21> cloneT21,
System.Func<T22, T22> cloneT22,
System.Func<T23, T23> cloneT23,
System.Func<T24, T24> cloneT24,
System.Func<T25, T25> cloneT25,
System.Func<T26, T26> cloneT26,
System.Func<T27, T27> cloneT27
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT5(this.item5));
                case 6:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT6(this.item6));
                case 7:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT7(this.item7));
                case 8:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT8(this.item8));
                case 9:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT9(this.item9));
                case 10:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT10(this.item10));
                case 11:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT11(this.item11));
                case 12:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT12(this.item12));
                case 13:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT13(this.item13));
                case 14:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT14(this.item14));
                case 15:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT15(this.item15));
                case 16:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT16(this.item16));
                case 17:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT17(this.item17));
                case 18:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT18(this.item18));
                case 19:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT19(this.item19));
                case 20:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT20(this.item20));
                case 21:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT21(this.item21));
                case 22:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT22(this.item22));
                case 23:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT23(this.item23));
                case 24:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT24(this.item24));
                case 25:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT25(this.item25));
                case 26:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT26(this.item26));
                case 27:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>(cloneT27(this.item27));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5,
System.Action<T6> callback6,
System.Action<T7> callback7,
System.Action<T8> callback8,
System.Action<T9> callback9,
System.Action<T10> callback10,
System.Action<T11> callback11,
System.Action<T12> callback12,
System.Action<T13> callback13,
System.Action<T14> callback14,
System.Action<T15> callback15,
System.Action<T16> callback16,
System.Action<T17> callback17,
System.Action<T18> callback18,
System.Action<T19> callback19,
System.Action<T20> callback20,
System.Action<T21> callback21,
System.Action<T22> callback22,
System.Action<T23> callback23,
System.Action<T24> callback24,
System.Action<T25> callback25,
System.Action<T26> callback26,
System.Action<T27> callback27)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                case 6:
                    callback6(this.item6);
                    break;
                case 7:
                    callback7(this.item7);
                    break;
                case 8:
                    callback8(this.item8);
                    break;
                case 9:
                    callback9(this.item9);
                    break;
                case 10:
                    callback10(this.item10);
                    break;
                case 11:
                    callback11(this.item11);
                    break;
                case 12:
                    callback12(this.item12);
                    break;
                case 13:
                    callback13(this.item13);
                    break;
                case 14:
                    callback14(this.item14);
                    break;
                case 15:
                    callback15(this.item15);
                    break;
                case 16:
                    callback16(this.item16);
                    break;
                case 17:
                    callback17(this.item17);
                    break;
                case 18:
                    callback18(this.item18);
                    break;
                case 19:
                    callback19(this.item19);
                    break;
                case 20:
                    callback20(this.item20);
                    break;
                case 21:
                    callback21(this.item21);
                    break;
                case 22:
                    callback22(this.item22);
                    break;
                case 23:
                    callback23(this.item23);
                    break;
                case 24:
                    callback24(this.item24);
                    break;
                case 25:
                    callback25(this.item25);
                    break;
                case 26:
                    callback26(this.item26);
                    break;
                case 27:
                    callback27(this.item27);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5,
System.Action<TState, T6> callback6,
System.Action<TState, T7> callback7,
System.Action<TState, T8> callback8,
System.Action<TState, T9> callback9,
System.Action<TState, T10> callback10,
System.Action<TState, T11> callback11,
System.Action<TState, T12> callback12,
System.Action<TState, T13> callback13,
System.Action<TState, T14> callback14,
System.Action<TState, T15> callback15,
System.Action<TState, T16> callback16,
System.Action<TState, T17> callback17,
System.Action<TState, T18> callback18,
System.Action<TState, T19> callback19,
System.Action<TState, T20> callback20,
System.Action<TState, T21> callback21,
System.Action<TState, T22> callback22,
System.Action<TState, T23> callback23,
System.Action<TState, T24> callback24,
System.Action<TState, T25> callback25,
System.Action<TState, T26> callback26,
System.Action<TState, T27> callback27)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                case 6:
                    callback6(state, this.item6);
                    break;
                case 7:
                    callback7(state, this.item7);
                    break;
                case 8:
                    callback8(state, this.item8);
                    break;
                case 9:
                    callback9(state, this.item9);
                    break;
                case 10:
                    callback10(state, this.item10);
                    break;
                case 11:
                    callback11(state, this.item11);
                    break;
                case 12:
                    callback12(state, this.item12);
                    break;
                case 13:
                    callback13(state, this.item13);
                    break;
                case 14:
                    callback14(state, this.item14);
                    break;
                case 15:
                    callback15(state, this.item15);
                    break;
                case 16:
                    callback16(state, this.item16);
                    break;
                case 17:
                    callback17(state, this.item17);
                    break;
                case 18:
                    callback18(state, this.item18);
                    break;
                case 19:
                    callback19(state, this.item19);
                    break;
                case 20:
                    callback20(state, this.item20);
                    break;
                case 21:
                    callback21(state, this.item21);
                    break;
                case 22:
                    callback22(state, this.item22);
                    break;
                case 23:
                    callback23(state, this.item23);
                    break;
                case 24:
                    callback24(state, this.item24);
                    break;
                case 25:
                    callback25(state, this.item25);
                    break;
                case 26:
                    callback26(state, this.item26);
                    break;
                case 27:
                    callback27(state, this.item27);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5,
System.Func<T6, TResult> callback6,
System.Func<T7, TResult> callback7,
System.Func<T8, TResult> callback8,
System.Func<T9, TResult> callback9,
System.Func<T10, TResult> callback10,
System.Func<T11, TResult> callback11,
System.Func<T12, TResult> callback12,
System.Func<T13, TResult> callback13,
System.Func<T14, TResult> callback14,
System.Func<T15, TResult> callback15,
System.Func<T16, TResult> callback16,
System.Func<T17, TResult> callback17,
System.Func<T18, TResult> callback18,
System.Func<T19, TResult> callback19,
System.Func<T20, TResult> callback20,
System.Func<T21, TResult> callback21,
System.Func<T22, TResult> callback22,
System.Func<T23, TResult> callback23,
System.Func<T24, TResult> callback24,
System.Func<T25, TResult> callback25,
System.Func<T26, TResult> callback26,
System.Func<T27, TResult> callback27)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                case 6:
                    return callback6(this.item6);
                case 7:
                    return callback7(this.item7);
                case 8:
                    return callback8(this.item8);
                case 9:
                    return callback9(this.item9);
                case 10:
                    return callback10(this.item10);
                case 11:
                    return callback11(this.item11);
                case 12:
                    return callback12(this.item12);
                case 13:
                    return callback13(this.item13);
                case 14:
                    return callback14(this.item14);
                case 15:
                    return callback15(this.item15);
                case 16:
                    return callback16(this.item16);
                case 17:
                    return callback17(this.item17);
                case 18:
                    return callback18(this.item18);
                case 19:
                    return callback19(this.item19);
                case 20:
                    return callback20(this.item20);
                case 21:
                    return callback21(this.item21);
                case 22:
                    return callback22(this.item22);
                case 23:
                    return callback23(this.item23);
                case 24:
                    return callback24(this.item24);
                case 25:
                    return callback25(this.item25);
                case 26:
                    return callback26(this.item26);
                case 27:
                    return callback27(this.item27);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5,
System.Func<TState, T6, TResult> callback6,
System.Func<TState, T7, TResult> callback7,
System.Func<TState, T8, TResult> callback8,
System.Func<TState, T9, TResult> callback9,
System.Func<TState, T10, TResult> callback10,
System.Func<TState, T11, TResult> callback11,
System.Func<TState, T12, TResult> callback12,
System.Func<TState, T13, TResult> callback13,
System.Func<TState, T14, TResult> callback14,
System.Func<TState, T15, TResult> callback15,
System.Func<TState, T16, TResult> callback16,
System.Func<TState, T17, TResult> callback17,
System.Func<TState, T18, TResult> callback18,
System.Func<TState, T19, TResult> callback19,
System.Func<TState, T20, TResult> callback20,
System.Func<TState, T21, TResult> callback21,
System.Func<TState, T22, TResult> callback22,
System.Func<TState, T23, TResult> callback23,
System.Func<TState, T24, TResult> callback24,
System.Func<TState, T25, TResult> callback25,
System.Func<TState, T26, TResult> callback26,
System.Func<TState, T27, TResult> callback27)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                case 6:
                    return callback6(state, this.item6);
                case 7:
                    return callback7(state, this.item7);
                case 8:
                    return callback8(state, this.item8);
                case 9:
                    return callback9(state, this.item9);
                case 10:
                    return callback10(state, this.item10);
                case 11:
                    return callback11(state, this.item11);
                case 12:
                    return callback12(state, this.item12);
                case 13:
                    return callback13(state, this.item13);
                case 14:
                    return callback14(state, this.item14);
                case 15:
                    return callback15(state, this.item15);
                case 16:
                    return callback16(state, this.item16);
                case 17:
                    return callback17(state, this.item17);
                case 18:
                    return callback18(state, this.item18);
                case 19:
                    return callback19(state, this.item19);
                case 20:
                    return callback20(state, this.item20);
                case 21:
                    return callback21(state, this.item21);
                case 22:
                    return callback22(state, this.item22);
                case 23:
                    return callback23(state, this.item23);
                case 24:
                    return callback24(state, this.item24);
                case 25:
                    return callback25(state, this.item25);
                case 26:
                    return callback26(state, this.item26);
                case 27:
                    return callback27(state, this.item27);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;


        private readonly T6 item6;


        private readonly T7 item7;


        private readonly T8 item8;


        private readonly T9 item9;


        private readonly T10 item10;


        private readonly T11 item11;


        private readonly T12 item12;


        private readonly T13 item13;


        private readonly T14 item14;


        private readonly T15 item15;


        private readonly T16 item16;


        private readonly T17 item17;


        private readonly T18 item18;


        private readonly T19 item19;


        private readonly T20 item20;


        private readonly T21 item21;


        private readonly T22 item22;


        private readonly T23 item23;


        private readonly T24 item24;


        private readonly T25 item25;


        private readonly T26 item26;


        private readonly T27 item27;


        private readonly T28 item28;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public FlatBufferUnion(T6 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 6;
            this.item6 = item;
        }


        public FlatBufferUnion(T7 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 7;
            this.item7 = item;
        }


        public FlatBufferUnion(T8 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 8;
            this.item8 = item;
        }


        public FlatBufferUnion(T9 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 9;
            this.item9 = item;
        }


        public FlatBufferUnion(T10 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 10;
            this.item10 = item;
        }


        public FlatBufferUnion(T11 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 11;
            this.item11 = item;
        }


        public FlatBufferUnion(T12 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 12;
            this.item12 = item;
        }


        public FlatBufferUnion(T13 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 13;
            this.item13 = item;
        }


        public FlatBufferUnion(T14 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 14;
            this.item14 = item;
        }


        public FlatBufferUnion(T15 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 15;
            this.item15 = item;
        }


        public FlatBufferUnion(T16 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 16;
            this.item16 = item;
        }


        public FlatBufferUnion(T17 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 17;
            this.item17 = item;
        }


        public FlatBufferUnion(T18 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 18;
            this.item18 = item;
        }


        public FlatBufferUnion(T19 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 19;
            this.item19 = item;
        }


        public FlatBufferUnion(T20 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 20;
            this.item20 = item;
        }


        public FlatBufferUnion(T21 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 21;
            this.item21 = item;
        }


        public FlatBufferUnion(T22 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 22;
            this.item22 = item;
        }


        public FlatBufferUnion(T23 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 23;
            this.item23 = item;
        }


        public FlatBufferUnion(T24 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 24;
            this.item24 = item;
        }


        public FlatBufferUnion(T25 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 25;
            this.item25 = item;
        }


        public FlatBufferUnion(T26 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 26;
            this.item26 = item;
        }


        public FlatBufferUnion(T27 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 27;
            this.item27 = item;
        }


        public FlatBufferUnion(T28 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 28;
            this.item28 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public T6 Item6
        {
            get
            {
                if (this.discriminator == 6)
                {
                    return this.item6;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T6 item)
        {
            item = default;
            if (this.discriminator == 6)
            {
                item = this.item6;
                return true;
            }

            return false;
        }


        public T7 Item7
        {
            get
            {
                if (this.discriminator == 7)
                {
                    return this.item7;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T7 item)
        {
            item = default;
            if (this.discriminator == 7)
            {
                item = this.item7;
                return true;
            }

            return false;
        }


        public T8 Item8
        {
            get
            {
                if (this.discriminator == 8)
                {
                    return this.item8;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T8 item)
        {
            item = default;
            if (this.discriminator == 8)
            {
                item = this.item8;
                return true;
            }

            return false;
        }


        public T9 Item9
        {
            get
            {
                if (this.discriminator == 9)
                {
                    return this.item9;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T9 item)
        {
            item = default;
            if (this.discriminator == 9)
            {
                item = this.item9;
                return true;
            }

            return false;
        }


        public T10 Item10
        {
            get
            {
                if (this.discriminator == 10)
                {
                    return this.item10;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T10 item)
        {
            item = default;
            if (this.discriminator == 10)
            {
                item = this.item10;
                return true;
            }

            return false;
        }


        public T11 Item11
        {
            get
            {
                if (this.discriminator == 11)
                {
                    return this.item11;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T11 item)
        {
            item = default;
            if (this.discriminator == 11)
            {
                item = this.item11;
                return true;
            }

            return false;
        }


        public T12 Item12
        {
            get
            {
                if (this.discriminator == 12)
                {
                    return this.item12;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T12 item)
        {
            item = default;
            if (this.discriminator == 12)
            {
                item = this.item12;
                return true;
            }

            return false;
        }


        public T13 Item13
        {
            get
            {
                if (this.discriminator == 13)
                {
                    return this.item13;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T13 item)
        {
            item = default;
            if (this.discriminator == 13)
            {
                item = this.item13;
                return true;
            }

            return false;
        }


        public T14 Item14
        {
            get
            {
                if (this.discriminator == 14)
                {
                    return this.item14;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T14 item)
        {
            item = default;
            if (this.discriminator == 14)
            {
                item = this.item14;
                return true;
            }

            return false;
        }


        public T15 Item15
        {
            get
            {
                if (this.discriminator == 15)
                {
                    return this.item15;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T15 item)
        {
            item = default;
            if (this.discriminator == 15)
            {
                item = this.item15;
                return true;
            }

            return false;
        }


        public T16 Item16
        {
            get
            {
                if (this.discriminator == 16)
                {
                    return this.item16;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T16 item)
        {
            item = default;
            if (this.discriminator == 16)
            {
                item = this.item16;
                return true;
            }

            return false;
        }


        public T17 Item17
        {
            get
            {
                if (this.discriminator == 17)
                {
                    return this.item17;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T17 item)
        {
            item = default;
            if (this.discriminator == 17)
            {
                item = this.item17;
                return true;
            }

            return false;
        }


        public T18 Item18
        {
            get
            {
                if (this.discriminator == 18)
                {
                    return this.item18;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T18 item)
        {
            item = default;
            if (this.discriminator == 18)
            {
                item = this.item18;
                return true;
            }

            return false;
        }


        public T19 Item19
        {
            get
            {
                if (this.discriminator == 19)
                {
                    return this.item19;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T19 item)
        {
            item = default;
            if (this.discriminator == 19)
            {
                item = this.item19;
                return true;
            }

            return false;
        }


        public T20 Item20
        {
            get
            {
                if (this.discriminator == 20)
                {
                    return this.item20;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T20 item)
        {
            item = default;
            if (this.discriminator == 20)
            {
                item = this.item20;
                return true;
            }

            return false;
        }


        public T21 Item21
        {
            get
            {
                if (this.discriminator == 21)
                {
                    return this.item21;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T21 item)
        {
            item = default;
            if (this.discriminator == 21)
            {
                item = this.item21;
                return true;
            }

            return false;
        }


        public T22 Item22
        {
            get
            {
                if (this.discriminator == 22)
                {
                    return this.item22;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T22 item)
        {
            item = default;
            if (this.discriminator == 22)
            {
                item = this.item22;
                return true;
            }

            return false;
        }


        public T23 Item23
        {
            get
            {
                if (this.discriminator == 23)
                {
                    return this.item23;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T23 item)
        {
            item = default;
            if (this.discriminator == 23)
            {
                item = this.item23;
                return true;
            }

            return false;
        }


        public T24 Item24
        {
            get
            {
                if (this.discriminator == 24)
                {
                    return this.item24;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T24 item)
        {
            item = default;
            if (this.discriminator == 24)
            {
                item = this.item24;
                return true;
            }

            return false;
        }


        public T25 Item25
        {
            get
            {
                if (this.discriminator == 25)
                {
                    return this.item25;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T25 item)
        {
            item = default;
            if (this.discriminator == 25)
            {
                item = this.item25;
                return true;
            }

            return false;
        }


        public T26 Item26
        {
            get
            {
                if (this.discriminator == 26)
                {
                    return this.item26;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T26 item)
        {
            item = default;
            if (this.discriminator == 26)
            {
                item = this.item26;
                return true;
            }

            return false;
        }


        public T27 Item27
        {
            get
            {
                if (this.discriminator == 27)
                {
                    return this.item27;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T27 item)
        {
            item = default;
            if (this.discriminator == 27)
            {
                item = this.item27;
                return true;
            }

            return false;
        }


        public T28 Item28
        {
            get
            {
                if (this.discriminator == 28)
                {
                    return this.item28;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T28 item)
        {
            item = default;
            if (this.discriminator == 28)
            {
                item = this.item28;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5,
System.Func<T6, T6> cloneT6,
System.Func<T7, T7> cloneT7,
System.Func<T8, T8> cloneT8,
System.Func<T9, T9> cloneT9,
System.Func<T10, T10> cloneT10,
System.Func<T11, T11> cloneT11,
System.Func<T12, T12> cloneT12,
System.Func<T13, T13> cloneT13,
System.Func<T14, T14> cloneT14,
System.Func<T15, T15> cloneT15,
System.Func<T16, T16> cloneT16,
System.Func<T17, T17> cloneT17,
System.Func<T18, T18> cloneT18,
System.Func<T19, T19> cloneT19,
System.Func<T20, T20> cloneT20,
System.Func<T21, T21> cloneT21,
System.Func<T22, T22> cloneT22,
System.Func<T23, T23> cloneT23,
System.Func<T24, T24> cloneT24,
System.Func<T25, T25> cloneT25,
System.Func<T26, T26> cloneT26,
System.Func<T27, T27> cloneT27,
System.Func<T28, T28> cloneT28
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT5(this.item5));
                case 6:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT6(this.item6));
                case 7:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT7(this.item7));
                case 8:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT8(this.item8));
                case 9:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT9(this.item9));
                case 10:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT10(this.item10));
                case 11:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT11(this.item11));
                case 12:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT12(this.item12));
                case 13:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT13(this.item13));
                case 14:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT14(this.item14));
                case 15:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT15(this.item15));
                case 16:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT16(this.item16));
                case 17:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT17(this.item17));
                case 18:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT18(this.item18));
                case 19:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT19(this.item19));
                case 20:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT20(this.item20));
                case 21:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT21(this.item21));
                case 22:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT22(this.item22));
                case 23:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT23(this.item23));
                case 24:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT24(this.item24));
                case 25:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT25(this.item25));
                case 26:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT26(this.item26));
                case 27:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT27(this.item27));
                case 28:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>(cloneT28(this.item28));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5,
System.Action<T6> callback6,
System.Action<T7> callback7,
System.Action<T8> callback8,
System.Action<T9> callback9,
System.Action<T10> callback10,
System.Action<T11> callback11,
System.Action<T12> callback12,
System.Action<T13> callback13,
System.Action<T14> callback14,
System.Action<T15> callback15,
System.Action<T16> callback16,
System.Action<T17> callback17,
System.Action<T18> callback18,
System.Action<T19> callback19,
System.Action<T20> callback20,
System.Action<T21> callback21,
System.Action<T22> callback22,
System.Action<T23> callback23,
System.Action<T24> callback24,
System.Action<T25> callback25,
System.Action<T26> callback26,
System.Action<T27> callback27,
System.Action<T28> callback28)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                case 6:
                    callback6(this.item6);
                    break;
                case 7:
                    callback7(this.item7);
                    break;
                case 8:
                    callback8(this.item8);
                    break;
                case 9:
                    callback9(this.item9);
                    break;
                case 10:
                    callback10(this.item10);
                    break;
                case 11:
                    callback11(this.item11);
                    break;
                case 12:
                    callback12(this.item12);
                    break;
                case 13:
                    callback13(this.item13);
                    break;
                case 14:
                    callback14(this.item14);
                    break;
                case 15:
                    callback15(this.item15);
                    break;
                case 16:
                    callback16(this.item16);
                    break;
                case 17:
                    callback17(this.item17);
                    break;
                case 18:
                    callback18(this.item18);
                    break;
                case 19:
                    callback19(this.item19);
                    break;
                case 20:
                    callback20(this.item20);
                    break;
                case 21:
                    callback21(this.item21);
                    break;
                case 22:
                    callback22(this.item22);
                    break;
                case 23:
                    callback23(this.item23);
                    break;
                case 24:
                    callback24(this.item24);
                    break;
                case 25:
                    callback25(this.item25);
                    break;
                case 26:
                    callback26(this.item26);
                    break;
                case 27:
                    callback27(this.item27);
                    break;
                case 28:
                    callback28(this.item28);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5,
System.Action<TState, T6> callback6,
System.Action<TState, T7> callback7,
System.Action<TState, T8> callback8,
System.Action<TState, T9> callback9,
System.Action<TState, T10> callback10,
System.Action<TState, T11> callback11,
System.Action<TState, T12> callback12,
System.Action<TState, T13> callback13,
System.Action<TState, T14> callback14,
System.Action<TState, T15> callback15,
System.Action<TState, T16> callback16,
System.Action<TState, T17> callback17,
System.Action<TState, T18> callback18,
System.Action<TState, T19> callback19,
System.Action<TState, T20> callback20,
System.Action<TState, T21> callback21,
System.Action<TState, T22> callback22,
System.Action<TState, T23> callback23,
System.Action<TState, T24> callback24,
System.Action<TState, T25> callback25,
System.Action<TState, T26> callback26,
System.Action<TState, T27> callback27,
System.Action<TState, T28> callback28)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                case 6:
                    callback6(state, this.item6);
                    break;
                case 7:
                    callback7(state, this.item7);
                    break;
                case 8:
                    callback8(state, this.item8);
                    break;
                case 9:
                    callback9(state, this.item9);
                    break;
                case 10:
                    callback10(state, this.item10);
                    break;
                case 11:
                    callback11(state, this.item11);
                    break;
                case 12:
                    callback12(state, this.item12);
                    break;
                case 13:
                    callback13(state, this.item13);
                    break;
                case 14:
                    callback14(state, this.item14);
                    break;
                case 15:
                    callback15(state, this.item15);
                    break;
                case 16:
                    callback16(state, this.item16);
                    break;
                case 17:
                    callback17(state, this.item17);
                    break;
                case 18:
                    callback18(state, this.item18);
                    break;
                case 19:
                    callback19(state, this.item19);
                    break;
                case 20:
                    callback20(state, this.item20);
                    break;
                case 21:
                    callback21(state, this.item21);
                    break;
                case 22:
                    callback22(state, this.item22);
                    break;
                case 23:
                    callback23(state, this.item23);
                    break;
                case 24:
                    callback24(state, this.item24);
                    break;
                case 25:
                    callback25(state, this.item25);
                    break;
                case 26:
                    callback26(state, this.item26);
                    break;
                case 27:
                    callback27(state, this.item27);
                    break;
                case 28:
                    callback28(state, this.item28);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5,
System.Func<T6, TResult> callback6,
System.Func<T7, TResult> callback7,
System.Func<T8, TResult> callback8,
System.Func<T9, TResult> callback9,
System.Func<T10, TResult> callback10,
System.Func<T11, TResult> callback11,
System.Func<T12, TResult> callback12,
System.Func<T13, TResult> callback13,
System.Func<T14, TResult> callback14,
System.Func<T15, TResult> callback15,
System.Func<T16, TResult> callback16,
System.Func<T17, TResult> callback17,
System.Func<T18, TResult> callback18,
System.Func<T19, TResult> callback19,
System.Func<T20, TResult> callback20,
System.Func<T21, TResult> callback21,
System.Func<T22, TResult> callback22,
System.Func<T23, TResult> callback23,
System.Func<T24, TResult> callback24,
System.Func<T25, TResult> callback25,
System.Func<T26, TResult> callback26,
System.Func<T27, TResult> callback27,
System.Func<T28, TResult> callback28)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                case 6:
                    return callback6(this.item6);
                case 7:
                    return callback7(this.item7);
                case 8:
                    return callback8(this.item8);
                case 9:
                    return callback9(this.item9);
                case 10:
                    return callback10(this.item10);
                case 11:
                    return callback11(this.item11);
                case 12:
                    return callback12(this.item12);
                case 13:
                    return callback13(this.item13);
                case 14:
                    return callback14(this.item14);
                case 15:
                    return callback15(this.item15);
                case 16:
                    return callback16(this.item16);
                case 17:
                    return callback17(this.item17);
                case 18:
                    return callback18(this.item18);
                case 19:
                    return callback19(this.item19);
                case 20:
                    return callback20(this.item20);
                case 21:
                    return callback21(this.item21);
                case 22:
                    return callback22(this.item22);
                case 23:
                    return callback23(this.item23);
                case 24:
                    return callback24(this.item24);
                case 25:
                    return callback25(this.item25);
                case 26:
                    return callback26(this.item26);
                case 27:
                    return callback27(this.item27);
                case 28:
                    return callback28(this.item28);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5,
System.Func<TState, T6, TResult> callback6,
System.Func<TState, T7, TResult> callback7,
System.Func<TState, T8, TResult> callback8,
System.Func<TState, T9, TResult> callback9,
System.Func<TState, T10, TResult> callback10,
System.Func<TState, T11, TResult> callback11,
System.Func<TState, T12, TResult> callback12,
System.Func<TState, T13, TResult> callback13,
System.Func<TState, T14, TResult> callback14,
System.Func<TState, T15, TResult> callback15,
System.Func<TState, T16, TResult> callback16,
System.Func<TState, T17, TResult> callback17,
System.Func<TState, T18, TResult> callback18,
System.Func<TState, T19, TResult> callback19,
System.Func<TState, T20, TResult> callback20,
System.Func<TState, T21, TResult> callback21,
System.Func<TState, T22, TResult> callback22,
System.Func<TState, T23, TResult> callback23,
System.Func<TState, T24, TResult> callback24,
System.Func<TState, T25, TResult> callback25,
System.Func<TState, T26, TResult> callback26,
System.Func<TState, T27, TResult> callback27,
System.Func<TState, T28, TResult> callback28)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                case 6:
                    return callback6(state, this.item6);
                case 7:
                    return callback7(state, this.item7);
                case 8:
                    return callback8(state, this.item8);
                case 9:
                    return callback9(state, this.item9);
                case 10:
                    return callback10(state, this.item10);
                case 11:
                    return callback11(state, this.item11);
                case 12:
                    return callback12(state, this.item12);
                case 13:
                    return callback13(state, this.item13);
                case 14:
                    return callback14(state, this.item14);
                case 15:
                    return callback15(state, this.item15);
                case 16:
                    return callback16(state, this.item16);
                case 17:
                    return callback17(state, this.item17);
                case 18:
                    return callback18(state, this.item18);
                case 19:
                    return callback19(state, this.item19);
                case 20:
                    return callback20(state, this.item20);
                case 21:
                    return callback21(state, this.item21);
                case 22:
                    return callback22(state, this.item22);
                case 23:
                    return callback23(state, this.item23);
                case 24:
                    return callback24(state, this.item24);
                case 25:
                    return callback25(state, this.item25);
                case 26:
                    return callback26(state, this.item26);
                case 27:
                    return callback27(state, this.item27);
                case 28:
                    return callback28(state, this.item28);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;


        private readonly T6 item6;


        private readonly T7 item7;


        private readonly T8 item8;


        private readonly T9 item9;


        private readonly T10 item10;


        private readonly T11 item11;


        private readonly T12 item12;


        private readonly T13 item13;


        private readonly T14 item14;


        private readonly T15 item15;


        private readonly T16 item16;


        private readonly T17 item17;


        private readonly T18 item18;


        private readonly T19 item19;


        private readonly T20 item20;


        private readonly T21 item21;


        private readonly T22 item22;


        private readonly T23 item23;


        private readonly T24 item24;


        private readonly T25 item25;


        private readonly T26 item26;


        private readonly T27 item27;


        private readonly T28 item28;


        private readonly T29 item29;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public FlatBufferUnion(T6 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 6;
            this.item6 = item;
        }


        public FlatBufferUnion(T7 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 7;
            this.item7 = item;
        }


        public FlatBufferUnion(T8 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 8;
            this.item8 = item;
        }


        public FlatBufferUnion(T9 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 9;
            this.item9 = item;
        }


        public FlatBufferUnion(T10 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 10;
            this.item10 = item;
        }


        public FlatBufferUnion(T11 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 11;
            this.item11 = item;
        }


        public FlatBufferUnion(T12 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 12;
            this.item12 = item;
        }


        public FlatBufferUnion(T13 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 13;
            this.item13 = item;
        }


        public FlatBufferUnion(T14 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 14;
            this.item14 = item;
        }


        public FlatBufferUnion(T15 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 15;
            this.item15 = item;
        }


        public FlatBufferUnion(T16 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 16;
            this.item16 = item;
        }


        public FlatBufferUnion(T17 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 17;
            this.item17 = item;
        }


        public FlatBufferUnion(T18 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 18;
            this.item18 = item;
        }


        public FlatBufferUnion(T19 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 19;
            this.item19 = item;
        }


        public FlatBufferUnion(T20 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 20;
            this.item20 = item;
        }


        public FlatBufferUnion(T21 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 21;
            this.item21 = item;
        }


        public FlatBufferUnion(T22 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 22;
            this.item22 = item;
        }


        public FlatBufferUnion(T23 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 23;
            this.item23 = item;
        }


        public FlatBufferUnion(T24 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 24;
            this.item24 = item;
        }


        public FlatBufferUnion(T25 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 25;
            this.item25 = item;
        }


        public FlatBufferUnion(T26 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 26;
            this.item26 = item;
        }


        public FlatBufferUnion(T27 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 27;
            this.item27 = item;
        }


        public FlatBufferUnion(T28 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 28;
            this.item28 = item;
        }


        public FlatBufferUnion(T29 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 29;
            this.item29 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public T6 Item6
        {
            get
            {
                if (this.discriminator == 6)
                {
                    return this.item6;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T6 item)
        {
            item = default;
            if (this.discriminator == 6)
            {
                item = this.item6;
                return true;
            }

            return false;
        }


        public T7 Item7
        {
            get
            {
                if (this.discriminator == 7)
                {
                    return this.item7;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T7 item)
        {
            item = default;
            if (this.discriminator == 7)
            {
                item = this.item7;
                return true;
            }

            return false;
        }


        public T8 Item8
        {
            get
            {
                if (this.discriminator == 8)
                {
                    return this.item8;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T8 item)
        {
            item = default;
            if (this.discriminator == 8)
            {
                item = this.item8;
                return true;
            }

            return false;
        }


        public T9 Item9
        {
            get
            {
                if (this.discriminator == 9)
                {
                    return this.item9;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T9 item)
        {
            item = default;
            if (this.discriminator == 9)
            {
                item = this.item9;
                return true;
            }

            return false;
        }


        public T10 Item10
        {
            get
            {
                if (this.discriminator == 10)
                {
                    return this.item10;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T10 item)
        {
            item = default;
            if (this.discriminator == 10)
            {
                item = this.item10;
                return true;
            }

            return false;
        }


        public T11 Item11
        {
            get
            {
                if (this.discriminator == 11)
                {
                    return this.item11;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T11 item)
        {
            item = default;
            if (this.discriminator == 11)
            {
                item = this.item11;
                return true;
            }

            return false;
        }


        public T12 Item12
        {
            get
            {
                if (this.discriminator == 12)
                {
                    return this.item12;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T12 item)
        {
            item = default;
            if (this.discriminator == 12)
            {
                item = this.item12;
                return true;
            }

            return false;
        }


        public T13 Item13
        {
            get
            {
                if (this.discriminator == 13)
                {
                    return this.item13;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T13 item)
        {
            item = default;
            if (this.discriminator == 13)
            {
                item = this.item13;
                return true;
            }

            return false;
        }


        public T14 Item14
        {
            get
            {
                if (this.discriminator == 14)
                {
                    return this.item14;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T14 item)
        {
            item = default;
            if (this.discriminator == 14)
            {
                item = this.item14;
                return true;
            }

            return false;
        }


        public T15 Item15
        {
            get
            {
                if (this.discriminator == 15)
                {
                    return this.item15;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T15 item)
        {
            item = default;
            if (this.discriminator == 15)
            {
                item = this.item15;
                return true;
            }

            return false;
        }


        public T16 Item16
        {
            get
            {
                if (this.discriminator == 16)
                {
                    return this.item16;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T16 item)
        {
            item = default;
            if (this.discriminator == 16)
            {
                item = this.item16;
                return true;
            }

            return false;
        }


        public T17 Item17
        {
            get
            {
                if (this.discriminator == 17)
                {
                    return this.item17;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T17 item)
        {
            item = default;
            if (this.discriminator == 17)
            {
                item = this.item17;
                return true;
            }

            return false;
        }


        public T18 Item18
        {
            get
            {
                if (this.discriminator == 18)
                {
                    return this.item18;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T18 item)
        {
            item = default;
            if (this.discriminator == 18)
            {
                item = this.item18;
                return true;
            }

            return false;
        }


        public T19 Item19
        {
            get
            {
                if (this.discriminator == 19)
                {
                    return this.item19;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T19 item)
        {
            item = default;
            if (this.discriminator == 19)
            {
                item = this.item19;
                return true;
            }

            return false;
        }


        public T20 Item20
        {
            get
            {
                if (this.discriminator == 20)
                {
                    return this.item20;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T20 item)
        {
            item = default;
            if (this.discriminator == 20)
            {
                item = this.item20;
                return true;
            }

            return false;
        }


        public T21 Item21
        {
            get
            {
                if (this.discriminator == 21)
                {
                    return this.item21;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T21 item)
        {
            item = default;
            if (this.discriminator == 21)
            {
                item = this.item21;
                return true;
            }

            return false;
        }


        public T22 Item22
        {
            get
            {
                if (this.discriminator == 22)
                {
                    return this.item22;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T22 item)
        {
            item = default;
            if (this.discriminator == 22)
            {
                item = this.item22;
                return true;
            }

            return false;
        }


        public T23 Item23
        {
            get
            {
                if (this.discriminator == 23)
                {
                    return this.item23;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T23 item)
        {
            item = default;
            if (this.discriminator == 23)
            {
                item = this.item23;
                return true;
            }

            return false;
        }


        public T24 Item24
        {
            get
            {
                if (this.discriminator == 24)
                {
                    return this.item24;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T24 item)
        {
            item = default;
            if (this.discriminator == 24)
            {
                item = this.item24;
                return true;
            }

            return false;
        }


        public T25 Item25
        {
            get
            {
                if (this.discriminator == 25)
                {
                    return this.item25;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T25 item)
        {
            item = default;
            if (this.discriminator == 25)
            {
                item = this.item25;
                return true;
            }

            return false;
        }


        public T26 Item26
        {
            get
            {
                if (this.discriminator == 26)
                {
                    return this.item26;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T26 item)
        {
            item = default;
            if (this.discriminator == 26)
            {
                item = this.item26;
                return true;
            }

            return false;
        }


        public T27 Item27
        {
            get
            {
                if (this.discriminator == 27)
                {
                    return this.item27;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T27 item)
        {
            item = default;
            if (this.discriminator == 27)
            {
                item = this.item27;
                return true;
            }

            return false;
        }


        public T28 Item28
        {
            get
            {
                if (this.discriminator == 28)
                {
                    return this.item28;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T28 item)
        {
            item = default;
            if (this.discriminator == 28)
            {
                item = this.item28;
                return true;
            }

            return false;
        }


        public T29 Item29
        {
            get
            {
                if (this.discriminator == 29)
                {
                    return this.item29;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T29 item)
        {
            item = default;
            if (this.discriminator == 29)
            {
                item = this.item29;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5,
System.Func<T6, T6> cloneT6,
System.Func<T7, T7> cloneT7,
System.Func<T8, T8> cloneT8,
System.Func<T9, T9> cloneT9,
System.Func<T10, T10> cloneT10,
System.Func<T11, T11> cloneT11,
System.Func<T12, T12> cloneT12,
System.Func<T13, T13> cloneT13,
System.Func<T14, T14> cloneT14,
System.Func<T15, T15> cloneT15,
System.Func<T16, T16> cloneT16,
System.Func<T17, T17> cloneT17,
System.Func<T18, T18> cloneT18,
System.Func<T19, T19> cloneT19,
System.Func<T20, T20> cloneT20,
System.Func<T21, T21> cloneT21,
System.Func<T22, T22> cloneT22,
System.Func<T23, T23> cloneT23,
System.Func<T24, T24> cloneT24,
System.Func<T25, T25> cloneT25,
System.Func<T26, T26> cloneT26,
System.Func<T27, T27> cloneT27,
System.Func<T28, T28> cloneT28,
System.Func<T29, T29> cloneT29
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT5(this.item5));
                case 6:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT6(this.item6));
                case 7:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT7(this.item7));
                case 8:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT8(this.item8));
                case 9:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT9(this.item9));
                case 10:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT10(this.item10));
                case 11:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT11(this.item11));
                case 12:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT12(this.item12));
                case 13:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT13(this.item13));
                case 14:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT14(this.item14));
                case 15:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT15(this.item15));
                case 16:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT16(this.item16));
                case 17:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT17(this.item17));
                case 18:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT18(this.item18));
                case 19:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT19(this.item19));
                case 20:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT20(this.item20));
                case 21:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT21(this.item21));
                case 22:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT22(this.item22));
                case 23:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT23(this.item23));
                case 24:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT24(this.item24));
                case 25:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT25(this.item25));
                case 26:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT26(this.item26));
                case 27:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT27(this.item27));
                case 28:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT28(this.item28));
                case 29:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>(cloneT29(this.item29));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5,
System.Action<T6> callback6,
System.Action<T7> callback7,
System.Action<T8> callback8,
System.Action<T9> callback9,
System.Action<T10> callback10,
System.Action<T11> callback11,
System.Action<T12> callback12,
System.Action<T13> callback13,
System.Action<T14> callback14,
System.Action<T15> callback15,
System.Action<T16> callback16,
System.Action<T17> callback17,
System.Action<T18> callback18,
System.Action<T19> callback19,
System.Action<T20> callback20,
System.Action<T21> callback21,
System.Action<T22> callback22,
System.Action<T23> callback23,
System.Action<T24> callback24,
System.Action<T25> callback25,
System.Action<T26> callback26,
System.Action<T27> callback27,
System.Action<T28> callback28,
System.Action<T29> callback29)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                case 6:
                    callback6(this.item6);
                    break;
                case 7:
                    callback7(this.item7);
                    break;
                case 8:
                    callback8(this.item8);
                    break;
                case 9:
                    callback9(this.item9);
                    break;
                case 10:
                    callback10(this.item10);
                    break;
                case 11:
                    callback11(this.item11);
                    break;
                case 12:
                    callback12(this.item12);
                    break;
                case 13:
                    callback13(this.item13);
                    break;
                case 14:
                    callback14(this.item14);
                    break;
                case 15:
                    callback15(this.item15);
                    break;
                case 16:
                    callback16(this.item16);
                    break;
                case 17:
                    callback17(this.item17);
                    break;
                case 18:
                    callback18(this.item18);
                    break;
                case 19:
                    callback19(this.item19);
                    break;
                case 20:
                    callback20(this.item20);
                    break;
                case 21:
                    callback21(this.item21);
                    break;
                case 22:
                    callback22(this.item22);
                    break;
                case 23:
                    callback23(this.item23);
                    break;
                case 24:
                    callback24(this.item24);
                    break;
                case 25:
                    callback25(this.item25);
                    break;
                case 26:
                    callback26(this.item26);
                    break;
                case 27:
                    callback27(this.item27);
                    break;
                case 28:
                    callback28(this.item28);
                    break;
                case 29:
                    callback29(this.item29);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5,
System.Action<TState, T6> callback6,
System.Action<TState, T7> callback7,
System.Action<TState, T8> callback8,
System.Action<TState, T9> callback9,
System.Action<TState, T10> callback10,
System.Action<TState, T11> callback11,
System.Action<TState, T12> callback12,
System.Action<TState, T13> callback13,
System.Action<TState, T14> callback14,
System.Action<TState, T15> callback15,
System.Action<TState, T16> callback16,
System.Action<TState, T17> callback17,
System.Action<TState, T18> callback18,
System.Action<TState, T19> callback19,
System.Action<TState, T20> callback20,
System.Action<TState, T21> callback21,
System.Action<TState, T22> callback22,
System.Action<TState, T23> callback23,
System.Action<TState, T24> callback24,
System.Action<TState, T25> callback25,
System.Action<TState, T26> callback26,
System.Action<TState, T27> callback27,
System.Action<TState, T28> callback28,
System.Action<TState, T29> callback29)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                case 6:
                    callback6(state, this.item6);
                    break;
                case 7:
                    callback7(state, this.item7);
                    break;
                case 8:
                    callback8(state, this.item8);
                    break;
                case 9:
                    callback9(state, this.item9);
                    break;
                case 10:
                    callback10(state, this.item10);
                    break;
                case 11:
                    callback11(state, this.item11);
                    break;
                case 12:
                    callback12(state, this.item12);
                    break;
                case 13:
                    callback13(state, this.item13);
                    break;
                case 14:
                    callback14(state, this.item14);
                    break;
                case 15:
                    callback15(state, this.item15);
                    break;
                case 16:
                    callback16(state, this.item16);
                    break;
                case 17:
                    callback17(state, this.item17);
                    break;
                case 18:
                    callback18(state, this.item18);
                    break;
                case 19:
                    callback19(state, this.item19);
                    break;
                case 20:
                    callback20(state, this.item20);
                    break;
                case 21:
                    callback21(state, this.item21);
                    break;
                case 22:
                    callback22(state, this.item22);
                    break;
                case 23:
                    callback23(state, this.item23);
                    break;
                case 24:
                    callback24(state, this.item24);
                    break;
                case 25:
                    callback25(state, this.item25);
                    break;
                case 26:
                    callback26(state, this.item26);
                    break;
                case 27:
                    callback27(state, this.item27);
                    break;
                case 28:
                    callback28(state, this.item28);
                    break;
                case 29:
                    callback29(state, this.item29);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5,
System.Func<T6, TResult> callback6,
System.Func<T7, TResult> callback7,
System.Func<T8, TResult> callback8,
System.Func<T9, TResult> callback9,
System.Func<T10, TResult> callback10,
System.Func<T11, TResult> callback11,
System.Func<T12, TResult> callback12,
System.Func<T13, TResult> callback13,
System.Func<T14, TResult> callback14,
System.Func<T15, TResult> callback15,
System.Func<T16, TResult> callback16,
System.Func<T17, TResult> callback17,
System.Func<T18, TResult> callback18,
System.Func<T19, TResult> callback19,
System.Func<T20, TResult> callback20,
System.Func<T21, TResult> callback21,
System.Func<T22, TResult> callback22,
System.Func<T23, TResult> callback23,
System.Func<T24, TResult> callback24,
System.Func<T25, TResult> callback25,
System.Func<T26, TResult> callback26,
System.Func<T27, TResult> callback27,
System.Func<T28, TResult> callback28,
System.Func<T29, TResult> callback29)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                case 6:
                    return callback6(this.item6);
                case 7:
                    return callback7(this.item7);
                case 8:
                    return callback8(this.item8);
                case 9:
                    return callback9(this.item9);
                case 10:
                    return callback10(this.item10);
                case 11:
                    return callback11(this.item11);
                case 12:
                    return callback12(this.item12);
                case 13:
                    return callback13(this.item13);
                case 14:
                    return callback14(this.item14);
                case 15:
                    return callback15(this.item15);
                case 16:
                    return callback16(this.item16);
                case 17:
                    return callback17(this.item17);
                case 18:
                    return callback18(this.item18);
                case 19:
                    return callback19(this.item19);
                case 20:
                    return callback20(this.item20);
                case 21:
                    return callback21(this.item21);
                case 22:
                    return callback22(this.item22);
                case 23:
                    return callback23(this.item23);
                case 24:
                    return callback24(this.item24);
                case 25:
                    return callback25(this.item25);
                case 26:
                    return callback26(this.item26);
                case 27:
                    return callback27(this.item27);
                case 28:
                    return callback28(this.item28);
                case 29:
                    return callback29(this.item29);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5,
System.Func<TState, T6, TResult> callback6,
System.Func<TState, T7, TResult> callback7,
System.Func<TState, T8, TResult> callback8,
System.Func<TState, T9, TResult> callback9,
System.Func<TState, T10, TResult> callback10,
System.Func<TState, T11, TResult> callback11,
System.Func<TState, T12, TResult> callback12,
System.Func<TState, T13, TResult> callback13,
System.Func<TState, T14, TResult> callback14,
System.Func<TState, T15, TResult> callback15,
System.Func<TState, T16, TResult> callback16,
System.Func<TState, T17, TResult> callback17,
System.Func<TState, T18, TResult> callback18,
System.Func<TState, T19, TResult> callback19,
System.Func<TState, T20, TResult> callback20,
System.Func<TState, T21, TResult> callback21,
System.Func<TState, T22, TResult> callback22,
System.Func<TState, T23, TResult> callback23,
System.Func<TState, T24, TResult> callback24,
System.Func<TState, T25, TResult> callback25,
System.Func<TState, T26, TResult> callback26,
System.Func<TState, T27, TResult> callback27,
System.Func<TState, T28, TResult> callback28,
System.Func<TState, T29, TResult> callback29)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                case 6:
                    return callback6(state, this.item6);
                case 7:
                    return callback7(state, this.item7);
                case 8:
                    return callback8(state, this.item8);
                case 9:
                    return callback9(state, this.item9);
                case 10:
                    return callback10(state, this.item10);
                case 11:
                    return callback11(state, this.item11);
                case 12:
                    return callback12(state, this.item12);
                case 13:
                    return callback13(state, this.item13);
                case 14:
                    return callback14(state, this.item14);
                case 15:
                    return callback15(state, this.item15);
                case 16:
                    return callback16(state, this.item16);
                case 17:
                    return callback17(state, this.item17);
                case 18:
                    return callback18(state, this.item18);
                case 19:
                    return callback19(state, this.item19);
                case 20:
                    return callback20(state, this.item20);
                case 21:
                    return callback21(state, this.item21);
                case 22:
                    return callback22(state, this.item22);
                case 23:
                    return callback23(state, this.item23);
                case 24:
                    return callback24(state, this.item24);
                case 25:
                    return callback25(state, this.item25);
                case 26:
                    return callback26(state, this.item26);
                case 27:
                    return callback27(state, this.item27);
                case 28:
                    return callback28(state, this.item28);
                case 29:
                    return callback29(state, this.item29);
                default:
                    return unknownType(state);
            }
        }
    }
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30> : IUnion
    {
        private readonly byte discriminator;


        private readonly T1 item1;


        private readonly T2 item2;


        private readonly T3 item3;


        private readonly T4 item4;


        private readonly T5 item5;


        private readonly T6 item6;


        private readonly T7 item7;


        private readonly T8 item8;


        private readonly T9 item9;


        private readonly T10 item10;


        private readonly T11 item11;


        private readonly T12 item12;


        private readonly T13 item13;


        private readonly T14 item14;


        private readonly T15 item15;


        private readonly T16 item16;


        private readonly T17 item17;


        private readonly T18 item18;


        private readonly T19 item19;


        private readonly T20 item20;


        private readonly T21 item21;


        private readonly T22 item22;


        private readonly T23 item23;


        private readonly T24 item24;


        private readonly T25 item25;


        private readonly T26 item26;


        private readonly T27 item27;


        private readonly T28 item28;


        private readonly T29 item29;


        private readonly T30 item30;



        public FlatBufferUnion(T1 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 1;
            this.item1 = item;
        }


        public FlatBufferUnion(T2 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 2;
            this.item2 = item;
        }


        public FlatBufferUnion(T3 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 3;
            this.item3 = item;
        }


        public FlatBufferUnion(T4 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 4;
            this.item4 = item;
        }


        public FlatBufferUnion(T5 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 5;
            this.item5 = item;
        }


        public FlatBufferUnion(T6 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 6;
            this.item6 = item;
        }


        public FlatBufferUnion(T7 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 7;
            this.item7 = item;
        }


        public FlatBufferUnion(T8 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 8;
            this.item8 = item;
        }


        public FlatBufferUnion(T9 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 9;
            this.item9 = item;
        }


        public FlatBufferUnion(T10 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 10;
            this.item10 = item;
        }


        public FlatBufferUnion(T11 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 11;
            this.item11 = item;
        }


        public FlatBufferUnion(T12 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 12;
            this.item12 = item;
        }


        public FlatBufferUnion(T13 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 13;
            this.item13 = item;
        }


        public FlatBufferUnion(T14 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 14;
            this.item14 = item;
        }


        public FlatBufferUnion(T15 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 15;
            this.item15 = item;
        }


        public FlatBufferUnion(T16 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 16;
            this.item16 = item;
        }


        public FlatBufferUnion(T17 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 17;
            this.item17 = item;
        }


        public FlatBufferUnion(T18 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 18;
            this.item18 = item;
        }


        public FlatBufferUnion(T19 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 19;
            this.item19 = item;
        }


        public FlatBufferUnion(T20 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 20;
            this.item20 = item;
        }


        public FlatBufferUnion(T21 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 21;
            this.item21 = item;
        }


        public FlatBufferUnion(T22 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 22;
            this.item22 = item;
        }


        public FlatBufferUnion(T23 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 23;
            this.item23 = item;
        }


        public FlatBufferUnion(T24 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 24;
            this.item24 = item;
        }


        public FlatBufferUnion(T25 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 25;
            this.item25 = item;
        }


        public FlatBufferUnion(T26 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 26;
            this.item26 = item;
        }


        public FlatBufferUnion(T27 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 27;
            this.item27 = item;
        }


        public FlatBufferUnion(T28 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 28;
            this.item28 = item;
        }


        public FlatBufferUnion(T29 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 29;
            this.item29 = item;
        }


        public FlatBufferUnion(T30 item)
        {
            if (object.ReferenceEquals(item, null))
            {
                throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
            }

            this.discriminator = 30;
            this.item30 = item;
        }


        public byte Discriminator => this.discriminator;


        public T1 Item1
        {
            get
            {
                if (this.discriminator == 1)
                {
                    return this.item1;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T1 item)
        {
            item = default;
            if (this.discriminator == 1)
            {
                item = this.item1;
                return true;
            }

            return false;
        }


        public T2 Item2
        {
            get
            {
                if (this.discriminator == 2)
                {
                    return this.item2;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T2 item)
        {
            item = default;
            if (this.discriminator == 2)
            {
                item = this.item2;
                return true;
            }

            return false;
        }


        public T3 Item3
        {
            get
            {
                if (this.discriminator == 3)
                {
                    return this.item3;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T3 item)
        {
            item = default;
            if (this.discriminator == 3)
            {
                item = this.item3;
                return true;
            }

            return false;
        }


        public T4 Item4
        {
            get
            {
                if (this.discriminator == 4)
                {
                    return this.item4;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T4 item)
        {
            item = default;
            if (this.discriminator == 4)
            {
                item = this.item4;
                return true;
            }

            return false;
        }


        public T5 Item5
        {
            get
            {
                if (this.discriminator == 5)
                {
                    return this.item5;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T5 item)
        {
            item = default;
            if (this.discriminator == 5)
            {
                item = this.item5;
                return true;
            }

            return false;
        }


        public T6 Item6
        {
            get
            {
                if (this.discriminator == 6)
                {
                    return this.item6;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T6 item)
        {
            item = default;
            if (this.discriminator == 6)
            {
                item = this.item6;
                return true;
            }

            return false;
        }


        public T7 Item7
        {
            get
            {
                if (this.discriminator == 7)
                {
                    return this.item7;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T7 item)
        {
            item = default;
            if (this.discriminator == 7)
            {
                item = this.item7;
                return true;
            }

            return false;
        }


        public T8 Item8
        {
            get
            {
                if (this.discriminator == 8)
                {
                    return this.item8;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T8 item)
        {
            item = default;
            if (this.discriminator == 8)
            {
                item = this.item8;
                return true;
            }

            return false;
        }


        public T9 Item9
        {
            get
            {
                if (this.discriminator == 9)
                {
                    return this.item9;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T9 item)
        {
            item = default;
            if (this.discriminator == 9)
            {
                item = this.item9;
                return true;
            }

            return false;
        }


        public T10 Item10
        {
            get
            {
                if (this.discriminator == 10)
                {
                    return this.item10;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T10 item)
        {
            item = default;
            if (this.discriminator == 10)
            {
                item = this.item10;
                return true;
            }

            return false;
        }


        public T11 Item11
        {
            get
            {
                if (this.discriminator == 11)
                {
                    return this.item11;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T11 item)
        {
            item = default;
            if (this.discriminator == 11)
            {
                item = this.item11;
                return true;
            }

            return false;
        }


        public T12 Item12
        {
            get
            {
                if (this.discriminator == 12)
                {
                    return this.item12;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T12 item)
        {
            item = default;
            if (this.discriminator == 12)
            {
                item = this.item12;
                return true;
            }

            return false;
        }


        public T13 Item13
        {
            get
            {
                if (this.discriminator == 13)
                {
                    return this.item13;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T13 item)
        {
            item = default;
            if (this.discriminator == 13)
            {
                item = this.item13;
                return true;
            }

            return false;
        }


        public T14 Item14
        {
            get
            {
                if (this.discriminator == 14)
                {
                    return this.item14;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T14 item)
        {
            item = default;
            if (this.discriminator == 14)
            {
                item = this.item14;
                return true;
            }

            return false;
        }


        public T15 Item15
        {
            get
            {
                if (this.discriminator == 15)
                {
                    return this.item15;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T15 item)
        {
            item = default;
            if (this.discriminator == 15)
            {
                item = this.item15;
                return true;
            }

            return false;
        }


        public T16 Item16
        {
            get
            {
                if (this.discriminator == 16)
                {
                    return this.item16;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T16 item)
        {
            item = default;
            if (this.discriminator == 16)
            {
                item = this.item16;
                return true;
            }

            return false;
        }


        public T17 Item17
        {
            get
            {
                if (this.discriminator == 17)
                {
                    return this.item17;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T17 item)
        {
            item = default;
            if (this.discriminator == 17)
            {
                item = this.item17;
                return true;
            }

            return false;
        }


        public T18 Item18
        {
            get
            {
                if (this.discriminator == 18)
                {
                    return this.item18;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T18 item)
        {
            item = default;
            if (this.discriminator == 18)
            {
                item = this.item18;
                return true;
            }

            return false;
        }


        public T19 Item19
        {
            get
            {
                if (this.discriminator == 19)
                {
                    return this.item19;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T19 item)
        {
            item = default;
            if (this.discriminator == 19)
            {
                item = this.item19;
                return true;
            }

            return false;
        }


        public T20 Item20
        {
            get
            {
                if (this.discriminator == 20)
                {
                    return this.item20;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T20 item)
        {
            item = default;
            if (this.discriminator == 20)
            {
                item = this.item20;
                return true;
            }

            return false;
        }


        public T21 Item21
        {
            get
            {
                if (this.discriminator == 21)
                {
                    return this.item21;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T21 item)
        {
            item = default;
            if (this.discriminator == 21)
            {
                item = this.item21;
                return true;
            }

            return false;
        }


        public T22 Item22
        {
            get
            {
                if (this.discriminator == 22)
                {
                    return this.item22;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T22 item)
        {
            item = default;
            if (this.discriminator == 22)
            {
                item = this.item22;
                return true;
            }

            return false;
        }


        public T23 Item23
        {
            get
            {
                if (this.discriminator == 23)
                {
                    return this.item23;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T23 item)
        {
            item = default;
            if (this.discriminator == 23)
            {
                item = this.item23;
                return true;
            }

            return false;
        }


        public T24 Item24
        {
            get
            {
                if (this.discriminator == 24)
                {
                    return this.item24;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T24 item)
        {
            item = default;
            if (this.discriminator == 24)
            {
                item = this.item24;
                return true;
            }

            return false;
        }


        public T25 Item25
        {
            get
            {
                if (this.discriminator == 25)
                {
                    return this.item25;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T25 item)
        {
            item = default;
            if (this.discriminator == 25)
            {
                item = this.item25;
                return true;
            }

            return false;
        }


        public T26 Item26
        {
            get
            {
                if (this.discriminator == 26)
                {
                    return this.item26;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T26 item)
        {
            item = default;
            if (this.discriminator == 26)
            {
                item = this.item26;
                return true;
            }

            return false;
        }


        public T27 Item27
        {
            get
            {
                if (this.discriminator == 27)
                {
                    return this.item27;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T27 item)
        {
            item = default;
            if (this.discriminator == 27)
            {
                item = this.item27;
                return true;
            }

            return false;
        }


        public T28 Item28
        {
            get
            {
                if (this.discriminator == 28)
                {
                    return this.item28;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T28 item)
        {
            item = default;
            if (this.discriminator == 28)
            {
                item = this.item28;
                return true;
            }

            return false;
        }


        public T29 Item29
        {
            get
            {
                if (this.discriminator == 29)
                {
                    return this.item29;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T29 item)
        {
            item = default;
            if (this.discriminator == 29)
            {
                item = this.item29;
                return true;
            }

            return false;
        }


        public T30 Item30
        {
            get
            {
                if (this.discriminator == 30)
                {
                    return this.item30;
                }
                else
                {
                    throw new System.InvalidOperationException();
                }
            }
        }

        public bool TryGet(out T30 item)
        {
            item = default;
            if (this.discriminator == 30)
            {
                item = this.item30;
                return true;
            }

            return false;
        }


        public FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30> Clone(
        System.Func<T1, T1> cloneT1,
System.Func<T2, T2> cloneT2,
System.Func<T3, T3> cloneT3,
System.Func<T4, T4> cloneT4,
System.Func<T5, T5> cloneT5,
System.Func<T6, T6> cloneT6,
System.Func<T7, T7> cloneT7,
System.Func<T8, T8> cloneT8,
System.Func<T9, T9> cloneT9,
System.Func<T10, T10> cloneT10,
System.Func<T11, T11> cloneT11,
System.Func<T12, T12> cloneT12,
System.Func<T13, T13> cloneT13,
System.Func<T14, T14> cloneT14,
System.Func<T15, T15> cloneT15,
System.Func<T16, T16> cloneT16,
System.Func<T17, T17> cloneT17,
System.Func<T18, T18> cloneT18,
System.Func<T19, T19> cloneT19,
System.Func<T20, T20> cloneT20,
System.Func<T21, T21> cloneT21,
System.Func<T22, T22> cloneT22,
System.Func<T23, T23> cloneT23,
System.Func<T24, T24> cloneT24,
System.Func<T25, T25> cloneT25,
System.Func<T26, T26> cloneT26,
System.Func<T27, T27> cloneT27,
System.Func<T28, T28> cloneT28,
System.Func<T29, T29> cloneT29,
System.Func<T30, T30> cloneT30
        )
        {
            switch (this.discriminator)
            {
                case 1:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT1(this.item1));
                case 2:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT2(this.item2));
                case 3:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT3(this.item3));
                case 4:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT4(this.item4));
                case 5:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT5(this.item5));
                case 6:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT6(this.item6));
                case 7:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT7(this.item7));
                case 8:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT8(this.item8));
                case 9:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT9(this.item9));
                case 10:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT10(this.item10));
                case 11:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT11(this.item11));
                case 12:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT12(this.item12));
                case 13:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT13(this.item13));
                case 14:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT14(this.item14));
                case 15:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT15(this.item15));
                case 16:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT16(this.item16));
                case 17:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT17(this.item17));
                case 18:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT18(this.item18));
                case 19:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT19(this.item19));
                case 20:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT20(this.item20));
                case 21:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT21(this.item21));
                case 22:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT22(this.item22));
                case 23:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT23(this.item23));
                case 24:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT24(this.item24));
                case 25:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT25(this.item25));
                case 26:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT26(this.item26));
                case 27:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT27(this.item27));
                case 28:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT28(this.item28));
                case 29:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT29(this.item29));
                case 30:
                    return new FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>(cloneT30(this.item30));
            }

            throw new System.InvalidOperationException();
        }

        public void Switch(
            System.Action unknownType,
            System.Action<T1> callback1,
System.Action<T2> callback2,
System.Action<T3> callback3,
System.Action<T4> callback4,
System.Action<T5> callback5,
System.Action<T6> callback6,
System.Action<T7> callback7,
System.Action<T8> callback8,
System.Action<T9> callback9,
System.Action<T10> callback10,
System.Action<T11> callback11,
System.Action<T12> callback12,
System.Action<T13> callback13,
System.Action<T14> callback14,
System.Action<T15> callback15,
System.Action<T16> callback16,
System.Action<T17> callback17,
System.Action<T18> callback18,
System.Action<T19> callback19,
System.Action<T20> callback20,
System.Action<T21> callback21,
System.Action<T22> callback22,
System.Action<T23> callback23,
System.Action<T24> callback24,
System.Action<T25> callback25,
System.Action<T26> callback26,
System.Action<T27> callback27,
System.Action<T28> callback28,
System.Action<T29> callback29,
System.Action<T30> callback30)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(this.item1);
                    break;
                case 2:
                    callback2(this.item2);
                    break;
                case 3:
                    callback3(this.item3);
                    break;
                case 4:
                    callback4(this.item4);
                    break;
                case 5:
                    callback5(this.item5);
                    break;
                case 6:
                    callback6(this.item6);
                    break;
                case 7:
                    callback7(this.item7);
                    break;
                case 8:
                    callback8(this.item8);
                    break;
                case 9:
                    callback9(this.item9);
                    break;
                case 10:
                    callback10(this.item10);
                    break;
                case 11:
                    callback11(this.item11);
                    break;
                case 12:
                    callback12(this.item12);
                    break;
                case 13:
                    callback13(this.item13);
                    break;
                case 14:
                    callback14(this.item14);
                    break;
                case 15:
                    callback15(this.item15);
                    break;
                case 16:
                    callback16(this.item16);
                    break;
                case 17:
                    callback17(this.item17);
                    break;
                case 18:
                    callback18(this.item18);
                    break;
                case 19:
                    callback19(this.item19);
                    break;
                case 20:
                    callback20(this.item20);
                    break;
                case 21:
                    callback21(this.item21);
                    break;
                case 22:
                    callback22(this.item22);
                    break;
                case 23:
                    callback23(this.item23);
                    break;
                case 24:
                    callback24(this.item24);
                    break;
                case 25:
                    callback25(this.item25);
                    break;
                case 26:
                    callback26(this.item26);
                    break;
                case 27:
                    callback27(this.item27);
                    break;
                case 28:
                    callback28(this.item28);
                    break;
                case 29:
                    callback29(this.item29);
                    break;
                case 30:
                    callback30(this.item30);
                    break;
                default:
                    unknownType();
                    break;
            }
        }

        public void Switch<TState>(
            TState state,
            System.Action<TState> unknownType,
            System.Action<TState, T1> callback1,
System.Action<TState, T2> callback2,
System.Action<TState, T3> callback3,
System.Action<TState, T4> callback4,
System.Action<TState, T5> callback5,
System.Action<TState, T6> callback6,
System.Action<TState, T7> callback7,
System.Action<TState, T8> callback8,
System.Action<TState, T9> callback9,
System.Action<TState, T10> callback10,
System.Action<TState, T11> callback11,
System.Action<TState, T12> callback12,
System.Action<TState, T13> callback13,
System.Action<TState, T14> callback14,
System.Action<TState, T15> callback15,
System.Action<TState, T16> callback16,
System.Action<TState, T17> callback17,
System.Action<TState, T18> callback18,
System.Action<TState, T19> callback19,
System.Action<TState, T20> callback20,
System.Action<TState, T21> callback21,
System.Action<TState, T22> callback22,
System.Action<TState, T23> callback23,
System.Action<TState, T24> callback24,
System.Action<TState, T25> callback25,
System.Action<TState, T26> callback26,
System.Action<TState, T27> callback27,
System.Action<TState, T28> callback28,
System.Action<TState, T29> callback29,
System.Action<TState, T30> callback30)
        {
            switch (this.discriminator)
            {
                case 1:
                    callback1(state, this.item1);
                    break;
                case 2:
                    callback2(state, this.item2);
                    break;
                case 3:
                    callback3(state, this.item3);
                    break;
                case 4:
                    callback4(state, this.item4);
                    break;
                case 5:
                    callback5(state, this.item5);
                    break;
                case 6:
                    callback6(state, this.item6);
                    break;
                case 7:
                    callback7(state, this.item7);
                    break;
                case 8:
                    callback8(state, this.item8);
                    break;
                case 9:
                    callback9(state, this.item9);
                    break;
                case 10:
                    callback10(state, this.item10);
                    break;
                case 11:
                    callback11(state, this.item11);
                    break;
                case 12:
                    callback12(state, this.item12);
                    break;
                case 13:
                    callback13(state, this.item13);
                    break;
                case 14:
                    callback14(state, this.item14);
                    break;
                case 15:
                    callback15(state, this.item15);
                    break;
                case 16:
                    callback16(state, this.item16);
                    break;
                case 17:
                    callback17(state, this.item17);
                    break;
                case 18:
                    callback18(state, this.item18);
                    break;
                case 19:
                    callback19(state, this.item19);
                    break;
                case 20:
                    callback20(state, this.item20);
                    break;
                case 21:
                    callback21(state, this.item21);
                    break;
                case 22:
                    callback22(state, this.item22);
                    break;
                case 23:
                    callback23(state, this.item23);
                    break;
                case 24:
                    callback24(state, this.item24);
                    break;
                case 25:
                    callback25(state, this.item25);
                    break;
                case 26:
                    callback26(state, this.item26);
                    break;
                case 27:
                    callback27(state, this.item27);
                    break;
                case 28:
                    callback28(state, this.item28);
                    break;
                case 29:
                    callback29(state, this.item29);
                    break;
                case 30:
                    callback30(state, this.item30);
                    break;
                default:
                    unknownType(state);
                    break;
            }
        }


        public TResult Switch<TResult>(
            System.Func<TResult> unknownType,
            System.Func<T1, TResult> callback1,
System.Func<T2, TResult> callback2,
System.Func<T3, TResult> callback3,
System.Func<T4, TResult> callback4,
System.Func<T5, TResult> callback5,
System.Func<T6, TResult> callback6,
System.Func<T7, TResult> callback7,
System.Func<T8, TResult> callback8,
System.Func<T9, TResult> callback9,
System.Func<T10, TResult> callback10,
System.Func<T11, TResult> callback11,
System.Func<T12, TResult> callback12,
System.Func<T13, TResult> callback13,
System.Func<T14, TResult> callback14,
System.Func<T15, TResult> callback15,
System.Func<T16, TResult> callback16,
System.Func<T17, TResult> callback17,
System.Func<T18, TResult> callback18,
System.Func<T19, TResult> callback19,
System.Func<T20, TResult> callback20,
System.Func<T21, TResult> callback21,
System.Func<T22, TResult> callback22,
System.Func<T23, TResult> callback23,
System.Func<T24, TResult> callback24,
System.Func<T25, TResult> callback25,
System.Func<T26, TResult> callback26,
System.Func<T27, TResult> callback27,
System.Func<T28, TResult> callback28,
System.Func<T29, TResult> callback29,
System.Func<T30, TResult> callback30)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(this.item1);
                case 2:
                    return callback2(this.item2);
                case 3:
                    return callback3(this.item3);
                case 4:
                    return callback4(this.item4);
                case 5:
                    return callback5(this.item5);
                case 6:
                    return callback6(this.item6);
                case 7:
                    return callback7(this.item7);
                case 8:
                    return callback8(this.item8);
                case 9:
                    return callback9(this.item9);
                case 10:
                    return callback10(this.item10);
                case 11:
                    return callback11(this.item11);
                case 12:
                    return callback12(this.item12);
                case 13:
                    return callback13(this.item13);
                case 14:
                    return callback14(this.item14);
                case 15:
                    return callback15(this.item15);
                case 16:
                    return callback16(this.item16);
                case 17:
                    return callback17(this.item17);
                case 18:
                    return callback18(this.item18);
                case 19:
                    return callback19(this.item19);
                case 20:
                    return callback20(this.item20);
                case 21:
                    return callback21(this.item21);
                case 22:
                    return callback22(this.item22);
                case 23:
                    return callback23(this.item23);
                case 24:
                    return callback24(this.item24);
                case 25:
                    return callback25(this.item25);
                case 26:
                    return callback26(this.item26);
                case 27:
                    return callback27(this.item27);
                case 28:
                    return callback28(this.item28);
                case 29:
                    return callback29(this.item29);
                case 30:
                    return callback30(this.item30);
                default:
                    return unknownType();
            }
        }

        public TResult Switch<TState, TResult>(
            TState state,
            System.Func<TState, TResult> unknownType,
            System.Func<TState, T1, TResult> callback1,
System.Func<TState, T2, TResult> callback2,
System.Func<TState, T3, TResult> callback3,
System.Func<TState, T4, TResult> callback4,
System.Func<TState, T5, TResult> callback5,
System.Func<TState, T6, TResult> callback6,
System.Func<TState, T7, TResult> callback7,
System.Func<TState, T8, TResult> callback8,
System.Func<TState, T9, TResult> callback9,
System.Func<TState, T10, TResult> callback10,
System.Func<TState, T11, TResult> callback11,
System.Func<TState, T12, TResult> callback12,
System.Func<TState, T13, TResult> callback13,
System.Func<TState, T14, TResult> callback14,
System.Func<TState, T15, TResult> callback15,
System.Func<TState, T16, TResult> callback16,
System.Func<TState, T17, TResult> callback17,
System.Func<TState, T18, TResult> callback18,
System.Func<TState, T19, TResult> callback19,
System.Func<TState, T20, TResult> callback20,
System.Func<TState, T21, TResult> callback21,
System.Func<TState, T22, TResult> callback22,
System.Func<TState, T23, TResult> callback23,
System.Func<TState, T24, TResult> callback24,
System.Func<TState, T25, TResult> callback25,
System.Func<TState, T26, TResult> callback26,
System.Func<TState, T27, TResult> callback27,
System.Func<TState, T28, TResult> callback28,
System.Func<TState, T29, TResult> callback29,
System.Func<TState, T30, TResult> callback30)
        {
            switch (this.discriminator)
            {
                case 1:
                    return callback1(state, this.item1);
                case 2:
                    return callback2(state, this.item2);
                case 3:
                    return callback3(state, this.item3);
                case 4:
                    return callback4(state, this.item4);
                case 5:
                    return callback5(state, this.item5);
                case 6:
                    return callback6(state, this.item6);
                case 7:
                    return callback7(state, this.item7);
                case 8:
                    return callback8(state, this.item8);
                case 9:
                    return callback9(state, this.item9);
                case 10:
                    return callback10(state, this.item10);
                case 11:
                    return callback11(state, this.item11);
                case 12:
                    return callback12(state, this.item12);
                case 13:
                    return callback13(state, this.item13);
                case 14:
                    return callback14(state, this.item14);
                case 15:
                    return callback15(state, this.item15);
                case 16:
                    return callback16(state, this.item16);
                case 17:
                    return callback17(state, this.item17);
                case 18:
                    return callback18(state, this.item18);
                case 19:
                    return callback19(state, this.item19);
                case 20:
                    return callback20(state, this.item20);
                case 21:
                    return callback21(state, this.item21);
                case 22:
                    return callback22(state, this.item22);
                case 23:
                    return callback23(state, this.item23);
                case 24:
                    return callback24(state, this.item24);
                case 25:
                    return callback25(state, this.item25);
                case 26:
                    return callback26(state, this.item26);
                case 27:
                    return callback27(state, this.item27);
                case 28:
                    return callback28(state, this.item28);
                case 29:
                    return callback29(state, this.item29);
                case 30:
                    return callback30(state, this.item30);
                default:
                    return unknownType(state);
            }
        }
    }

}