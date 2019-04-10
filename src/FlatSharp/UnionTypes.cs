

namespace FlatSharp
{
	internal interface IUnion
	{
	}


				
			public sealed class FlatBufferUnion<T1, T2> : IUnion
			{
				private readonly byte discriminator;
				
				
				private T1 item1;
				
				
				private T2 item2;
				
								
				
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 1);
						this.item1 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 2);
						this.item2 = value;
					}
				}
				
							}
				
			public sealed class FlatBufferUnion<T1, T2, T3> : IUnion
			{
				private readonly byte discriminator;
				
				
				private T1 item1;
				
				
				private T2 item2;
				
				
				private T3 item3;
				
								
				
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 1);
						this.item1 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 2);
						this.item2 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 3);
						this.item3 = value;
					}
				}
				
							}
				
			public sealed class FlatBufferUnion<T1, T2, T3, T4> : IUnion
			{
				private readonly byte discriminator;
				
				
				private T1 item1;
				
				
				private T2 item2;
				
				
				private T3 item3;
				
				
				private T4 item4;
				
								
				
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 1);
						this.item1 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 2);
						this.item2 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 3);
						this.item3 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 4);
						this.item4 = value;
					}
				}
				
							}
				
			public sealed class FlatBufferUnion<T1, T2, T3, T4, T5> : IUnion
			{
				private readonly byte discriminator;
				
				
				private T1 item1;
				
				
				private T2 item2;
				
				
				private T3 item3;
				
				
				private T4 item4;
				
				
				private T5 item5;
				
								
				
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 1);
						this.item1 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 2);
						this.item2 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 3);
						this.item3 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 4);
						this.item4 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 5);
						this.item5 = value;
					}
				}
				
							}
				
			public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6> : IUnion
			{
				private readonly byte discriminator;
				
				
				private T1 item1;
				
				
				private T2 item2;
				
				
				private T3 item3;
				
				
				private T4 item4;
				
				
				private T5 item5;
				
				
				private T6 item6;
				
								
				
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 1);
						this.item1 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 2);
						this.item2 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 3);
						this.item3 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 4);
						this.item4 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 5);
						this.item5 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 6);
						this.item6 = value;
					}
				}
				
							}
				
			public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7> : IUnion
			{
				private readonly byte discriminator;
				
				
				private T1 item1;
				
				
				private T2 item2;
				
				
				private T3 item3;
				
				
				private T4 item4;
				
				
				private T5 item5;
				
				
				private T6 item6;
				
				
				private T7 item7;
				
								
				
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 1);
						this.item1 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 2);
						this.item2 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 3);
						this.item3 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 4);
						this.item4 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 5);
						this.item5 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 6);
						this.item6 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 7);
						this.item7 = value;
					}
				}
				
							}
				
			public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8> : IUnion
			{
				private readonly byte discriminator;
				
				
				private T1 item1;
				
				
				private T2 item2;
				
				
				private T3 item3;
				
				
				private T4 item4;
				
				
				private T5 item5;
				
				
				private T6 item6;
				
				
				private T7 item7;
				
				
				private T8 item8;
				
								
				
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 1);
						this.item1 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 2);
						this.item2 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 3);
						this.item3 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 4);
						this.item4 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 5);
						this.item5 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 6);
						this.item6 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 7);
						this.item7 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 8);
						this.item8 = value;
					}
				}
				
							}
				
			public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IUnion
			{
				private readonly byte discriminator;
				
				
				private T1 item1;
				
				
				private T2 item2;
				
				
				private T3 item3;
				
				
				private T4 item4;
				
				
				private T5 item5;
				
				
				private T6 item6;
				
				
				private T7 item7;
				
				
				private T8 item8;
				
				
				private T9 item9;
				
								
				
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 1);
						this.item1 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 2);
						this.item2 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 3);
						this.item3 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 4);
						this.item4 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 5);
						this.item5 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 6);
						this.item6 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 7);
						this.item7 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 8);
						this.item8 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 9);
						this.item9 = value;
					}
				}
				
							}
				
			public sealed class FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IUnion
			{
				private readonly byte discriminator;
				
				
				private T1 item1;
				
				
				private T2 item2;
				
				
				private T3 item3;
				
				
				private T4 item4;
				
				
				private T5 item5;
				
				
				private T6 item6;
				
				
				private T7 item7;
				
				
				private T8 item8;
				
				
				private T9 item9;
				
				
				private T10 item10;
				
								
				
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 1);
						this.item1 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 2);
						this.item2 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 3);
						this.item3 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 4);
						this.item4 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 5);
						this.item5 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 6);
						this.item6 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 7);
						this.item7 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 8);
						this.item8 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 9);
						this.item9 = value;
					}
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
							throw new System.Exception();
						}
					}

					internal set
					{
						System.Diagnostics.Debug.Assert(this.discriminator == 10);
						this.item10 = value;
					}
				}
				
							}
	
}