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
