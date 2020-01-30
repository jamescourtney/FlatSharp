

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
				
							}
	
}