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
public interface IFlatBufferUnion
{
	byte Discriminator { get; }
}


		public interface IFlatBufferUnion<T1> : IFlatBufferUnion
					where T1 : notnull
				{
					T1 Item1 { get; }
				}

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1> : IFlatBufferUnion<T1>

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
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									default:
						return defaultCase(state);
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
		public interface IFlatBufferUnion<T1, T2> : IFlatBufferUnion
					where T1 : notnull
					where T2 : notnull
				{
					T1 Item1 { get; }
					T2 Item2 { get; }
				}

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2> : IFlatBufferUnion<T1, T2>

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
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									default:
						return defaultCase(state);
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
		public interface IFlatBufferUnion<T1, T2, T3> : IFlatBufferUnion
					where T1 : notnull
					where T2 : notnull
					where T3 : notnull
				{
					T1 Item1 { get; }
					T2 Item2 { get; }
					T3 Item3 { get; }
				}

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3> : IFlatBufferUnion<T1, T2, T3>

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
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									default:
						return defaultCase(state);
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
		public interface IFlatBufferUnion<T1, T2, T3, T4> : IFlatBufferUnion
					where T1 : notnull
					where T2 : notnull
					where T3 : notnull
					where T4 : notnull
				{
					T1 Item1 { get; }
					T2 Item2 { get; }
					T3 Item3 { get; }
					T4 Item4 { get; }
				}

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4> : IFlatBufferUnion<T1, T2, T3, T4>

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
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									default:
						return defaultCase(state);
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5> : IFlatBufferUnion
					where T1 : notnull
					where T2 : notnull
					where T3 : notnull
					where T4 : notnull
					where T5 : notnull
				{
					T1 Item1 { get; }
					T2 Item2 { get; }
					T3 Item3 { get; }
					T4 Item4 { get; }
					T5 Item5 { get; }
				}

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5> : IFlatBufferUnion<T1, T2, T3, T4, T5>

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
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									default:
						return defaultCase(state);
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6> : IFlatBufferUnion
					where T1 : notnull
					where T2 : notnull
					where T3 : notnull
					where T4 : notnull
					where T5 : notnull
					where T6 : notnull
				{
					T1 Item1 { get; }
					T2 Item2 { get; }
					T3 Item3 { get; }
					T4 Item4 { get; }
					T5 Item5 { get; }
					T6 Item6 { get; }
				}

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6> : IFlatBufferUnion<T1, T2, T3, T4, T5, T6>

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
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5,
System.Action<T6> case6)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									case 6:
					{
						case6((T6)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5,
System.Action<TState, T6> case6)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									case 6:
					{
						case6(state, (T6)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5,
System.Func<T6, TResult> case6)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									case 6:
					{
						return case6((T6)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5,
System.Func<TState, T6, TResult> case6)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									case 6:
					{
						return case6(state, (T6)this.value);
					}
									default:
						return defaultCase(state);
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7> : IFlatBufferUnion
					where T1 : notnull
					where T2 : notnull
					where T3 : notnull
					where T4 : notnull
					where T5 : notnull
					where T6 : notnull
					where T7 : notnull
				{
					T1 Item1 { get; }
					T2 Item2 { get; }
					T3 Item3 { get; }
					T4 Item4 { get; }
					T5 Item5 { get; }
					T6 Item6 { get; }
					T7 Item7 { get; }
				}

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7> : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7>

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
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5,
System.Action<T6> case6,
System.Action<T7> case7)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									case 6:
					{
						case6((T6)this.value);
						break;
					}
									case 7:
					{
						case7((T7)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5,
System.Action<TState, T6> case6,
System.Action<TState, T7> case7)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									case 6:
					{
						case6(state, (T6)this.value);
						break;
					}
									case 7:
					{
						case7(state, (T7)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5,
System.Func<T6, TResult> case6,
System.Func<T7, TResult> case7)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									case 6:
					{
						return case6((T6)this.value);
					}
									case 7:
					{
						return case7((T7)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5,
System.Func<TState, T6, TResult> case6,
System.Func<TState, T7, TResult> case7)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									case 6:
					{
						return case6(state, (T6)this.value);
					}
									case 7:
					{
						return case7(state, (T7)this.value);
					}
									default:
						return defaultCase(state);
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8> : IFlatBufferUnion
					where T1 : notnull
					where T2 : notnull
					where T3 : notnull
					where T4 : notnull
					where T5 : notnull
					where T6 : notnull
					where T7 : notnull
					where T8 : notnull
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

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8> : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8>

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
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5,
System.Action<T6> case6,
System.Action<T7> case7,
System.Action<T8> case8)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									case 6:
					{
						case6((T6)this.value);
						break;
					}
									case 7:
					{
						case7((T7)this.value);
						break;
					}
									case 8:
					{
						case8((T8)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5,
System.Action<TState, T6> case6,
System.Action<TState, T7> case7,
System.Action<TState, T8> case8)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									case 6:
					{
						case6(state, (T6)this.value);
						break;
					}
									case 7:
					{
						case7(state, (T7)this.value);
						break;
					}
									case 8:
					{
						case8(state, (T8)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5,
System.Func<T6, TResult> case6,
System.Func<T7, TResult> case7,
System.Func<T8, TResult> case8)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									case 6:
					{
						return case6((T6)this.value);
					}
									case 7:
					{
						return case7((T7)this.value);
					}
									case 8:
					{
						return case8((T8)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5,
System.Func<TState, T6, TResult> case6,
System.Func<TState, T7, TResult> case7,
System.Func<TState, T8, TResult> case8)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									case 6:
					{
						return case6(state, (T6)this.value);
					}
									case 7:
					{
						return case7(state, (T7)this.value);
					}
									case 8:
					{
						return case8(state, (T8)this.value);
					}
									default:
						return defaultCase(state);
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IFlatBufferUnion
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

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>

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
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5,
System.Action<T6> case6,
System.Action<T7> case7,
System.Action<T8> case8,
System.Action<T9> case9)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									case 6:
					{
						case6((T6)this.value);
						break;
					}
									case 7:
					{
						case7((T7)this.value);
						break;
					}
									case 8:
					{
						case8((T8)this.value);
						break;
					}
									case 9:
					{
						case9((T9)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5,
System.Action<TState, T6> case6,
System.Action<TState, T7> case7,
System.Action<TState, T8> case8,
System.Action<TState, T9> case9)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									case 6:
					{
						case6(state, (T6)this.value);
						break;
					}
									case 7:
					{
						case7(state, (T7)this.value);
						break;
					}
									case 8:
					{
						case8(state, (T8)this.value);
						break;
					}
									case 9:
					{
						case9(state, (T9)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5,
System.Func<T6, TResult> case6,
System.Func<T7, TResult> case7,
System.Func<T8, TResult> case8,
System.Func<T9, TResult> case9)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									case 6:
					{
						return case6((T6)this.value);
					}
									case 7:
					{
						return case7((T7)this.value);
					}
									case 8:
					{
						return case8((T8)this.value);
					}
									case 9:
					{
						return case9((T9)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5,
System.Func<TState, T6, TResult> case6,
System.Func<TState, T7, TResult> case7,
System.Func<TState, T8, TResult> case8,
System.Func<TState, T9, TResult> case9)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									case 6:
					{
						return case6(state, (T6)this.value);
					}
									case 7:
					{
						return case7(state, (T7)this.value);
					}
									case 8:
					{
						return case8(state, (T8)this.value);
					}
									case 9:
					{
						return case9(state, (T9)this.value);
					}
									default:
						return defaultCase(state);
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IFlatBufferUnion
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

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>

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
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5,
System.Action<T6> case6,
System.Action<T7> case7,
System.Action<T8> case8,
System.Action<T9> case9,
System.Action<T10> case10)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									case 6:
					{
						case6((T6)this.value);
						break;
					}
									case 7:
					{
						case7((T7)this.value);
						break;
					}
									case 8:
					{
						case8((T8)this.value);
						break;
					}
									case 9:
					{
						case9((T9)this.value);
						break;
					}
									case 10:
					{
						case10((T10)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5,
System.Action<TState, T6> case6,
System.Action<TState, T7> case7,
System.Action<TState, T8> case8,
System.Action<TState, T9> case9,
System.Action<TState, T10> case10)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									case 6:
					{
						case6(state, (T6)this.value);
						break;
					}
									case 7:
					{
						case7(state, (T7)this.value);
						break;
					}
									case 8:
					{
						case8(state, (T8)this.value);
						break;
					}
									case 9:
					{
						case9(state, (T9)this.value);
						break;
					}
									case 10:
					{
						case10(state, (T10)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5,
System.Func<T6, TResult> case6,
System.Func<T7, TResult> case7,
System.Func<T8, TResult> case8,
System.Func<T9, TResult> case9,
System.Func<T10, TResult> case10)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									case 6:
					{
						return case6((T6)this.value);
					}
									case 7:
					{
						return case7((T7)this.value);
					}
									case 8:
					{
						return case8((T8)this.value);
					}
									case 9:
					{
						return case9((T9)this.value);
					}
									case 10:
					{
						return case10((T10)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5,
System.Func<TState, T6, TResult> case6,
System.Func<TState, T7, TResult> case7,
System.Func<TState, T8, TResult> case8,
System.Func<TState, T9, TResult> case9,
System.Func<TState, T10, TResult> case10)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									case 6:
					{
						return case6(state, (T6)this.value);
					}
									case 7:
					{
						return case7(state, (T7)this.value);
					}
									case 8:
					{
						return case8(state, (T8)this.value);
					}
									case 9:
					{
						return case9(state, (T9)this.value);
					}
									case 10:
					{
						return case10(state, (T10)this.value);
					}
									default:
						return defaultCase(state);
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : IFlatBufferUnion
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
					where T11 : notnull
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

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>

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
					where T11 : notnull
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
				
			
			public FlatBufferUnion(T11 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 11;
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
				
			
			public T11 Item11
			{
				get 
				{
					if (this.discriminator == 11)
					{
						return (T11)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T11? item)
			{
				if (this.discriminator == 11)
				{
					item = (T11)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5,
System.Action<T6> case6,
System.Action<T7> case7,
System.Action<T8> case8,
System.Action<T9> case9,
System.Action<T10> case10,
System.Action<T11> case11)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									case 6:
					{
						case6((T6)this.value);
						break;
					}
									case 7:
					{
						case7((T7)this.value);
						break;
					}
									case 8:
					{
						case8((T8)this.value);
						break;
					}
									case 9:
					{
						case9((T9)this.value);
						break;
					}
									case 10:
					{
						case10((T10)this.value);
						break;
					}
									case 11:
					{
						case11((T11)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5,
System.Action<TState, T6> case6,
System.Action<TState, T7> case7,
System.Action<TState, T8> case8,
System.Action<TState, T9> case9,
System.Action<TState, T10> case10,
System.Action<TState, T11> case11)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									case 6:
					{
						case6(state, (T6)this.value);
						break;
					}
									case 7:
					{
						case7(state, (T7)this.value);
						break;
					}
									case 8:
					{
						case8(state, (T8)this.value);
						break;
					}
									case 9:
					{
						case9(state, (T9)this.value);
						break;
					}
									case 10:
					{
						case10(state, (T10)this.value);
						break;
					}
									case 11:
					{
						case11(state, (T11)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5,
System.Func<T6, TResult> case6,
System.Func<T7, TResult> case7,
System.Func<T8, TResult> case8,
System.Func<T9, TResult> case9,
System.Func<T10, TResult> case10,
System.Func<T11, TResult> case11)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									case 6:
					{
						return case6((T6)this.value);
					}
									case 7:
					{
						return case7((T7)this.value);
					}
									case 8:
					{
						return case8((T8)this.value);
					}
									case 9:
					{
						return case9((T9)this.value);
					}
									case 10:
					{
						return case10((T10)this.value);
					}
									case 11:
					{
						return case11((T11)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5,
System.Func<TState, T6, TResult> case6,
System.Func<TState, T7, TResult> case7,
System.Func<TState, T8, TResult> case8,
System.Func<TState, T9, TResult> case9,
System.Func<TState, T10, TResult> case10,
System.Func<TState, T11, TResult> case11)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									case 6:
					{
						return case6(state, (T6)this.value);
					}
									case 7:
					{
						return case7(state, (T7)this.value);
					}
									case 8:
					{
						return case8(state, (T8)this.value);
					}
									case 9:
					{
						return case9(state, (T9)this.value);
					}
									case 10:
					{
						return case10(state, (T10)this.value);
					}
									case 11:
					{
						return case11(state, (T11)this.value);
					}
									default:
						return defaultCase(state);
				}
			}

			public override bool Equals(object? other)
			{
				if (other is FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> otherUnion)
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : IFlatBufferUnion
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
					where T11 : notnull
					where T12 : notnull
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

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>

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
					where T11 : notnull
					where T12 : notnull
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
				
			
			public FlatBufferUnion(T11 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 11;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T12 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 12;
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
				
			
			public T11 Item11
			{
				get 
				{
					if (this.discriminator == 11)
					{
						return (T11)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T11? item)
			{
				if (this.discriminator == 11)
				{
					item = (T11)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T12 Item12
			{
				get 
				{
					if (this.discriminator == 12)
					{
						return (T12)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T12? item)
			{
				if (this.discriminator == 12)
				{
					item = (T12)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5,
System.Action<T6> case6,
System.Action<T7> case7,
System.Action<T8> case8,
System.Action<T9> case9,
System.Action<T10> case10,
System.Action<T11> case11,
System.Action<T12> case12)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									case 6:
					{
						case6((T6)this.value);
						break;
					}
									case 7:
					{
						case7((T7)this.value);
						break;
					}
									case 8:
					{
						case8((T8)this.value);
						break;
					}
									case 9:
					{
						case9((T9)this.value);
						break;
					}
									case 10:
					{
						case10((T10)this.value);
						break;
					}
									case 11:
					{
						case11((T11)this.value);
						break;
					}
									case 12:
					{
						case12((T12)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5,
System.Action<TState, T6> case6,
System.Action<TState, T7> case7,
System.Action<TState, T8> case8,
System.Action<TState, T9> case9,
System.Action<TState, T10> case10,
System.Action<TState, T11> case11,
System.Action<TState, T12> case12)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									case 6:
					{
						case6(state, (T6)this.value);
						break;
					}
									case 7:
					{
						case7(state, (T7)this.value);
						break;
					}
									case 8:
					{
						case8(state, (T8)this.value);
						break;
					}
									case 9:
					{
						case9(state, (T9)this.value);
						break;
					}
									case 10:
					{
						case10(state, (T10)this.value);
						break;
					}
									case 11:
					{
						case11(state, (T11)this.value);
						break;
					}
									case 12:
					{
						case12(state, (T12)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5,
System.Func<T6, TResult> case6,
System.Func<T7, TResult> case7,
System.Func<T8, TResult> case8,
System.Func<T9, TResult> case9,
System.Func<T10, TResult> case10,
System.Func<T11, TResult> case11,
System.Func<T12, TResult> case12)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									case 6:
					{
						return case6((T6)this.value);
					}
									case 7:
					{
						return case7((T7)this.value);
					}
									case 8:
					{
						return case8((T8)this.value);
					}
									case 9:
					{
						return case9((T9)this.value);
					}
									case 10:
					{
						return case10((T10)this.value);
					}
									case 11:
					{
						return case11((T11)this.value);
					}
									case 12:
					{
						return case12((T12)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5,
System.Func<TState, T6, TResult> case6,
System.Func<TState, T7, TResult> case7,
System.Func<TState, T8, TResult> case8,
System.Func<TState, T9, TResult> case9,
System.Func<TState, T10, TResult> case10,
System.Func<TState, T11, TResult> case11,
System.Func<TState, T12, TResult> case12)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									case 6:
					{
						return case6(state, (T6)this.value);
					}
									case 7:
					{
						return case7(state, (T7)this.value);
					}
									case 8:
					{
						return case8(state, (T8)this.value);
					}
									case 9:
					{
						return case9(state, (T9)this.value);
					}
									case 10:
					{
						return case10(state, (T10)this.value);
					}
									case 11:
					{
						return case11(state, (T11)this.value);
					}
									case 12:
					{
						return case12(state, (T12)this.value);
					}
									default:
						return defaultCase(state);
				}
			}

			public override bool Equals(object? other)
			{
				if (other is FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> otherUnion)
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : IFlatBufferUnion
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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
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

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>

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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
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
				
			
			public FlatBufferUnion(T11 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 11;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T12 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 12;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T13 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 13;
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
				
			
			public T11 Item11
			{
				get 
				{
					if (this.discriminator == 11)
					{
						return (T11)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T11? item)
			{
				if (this.discriminator == 11)
				{
					item = (T11)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T12 Item12
			{
				get 
				{
					if (this.discriminator == 12)
					{
						return (T12)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T12? item)
			{
				if (this.discriminator == 12)
				{
					item = (T12)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T13 Item13
			{
				get 
				{
					if (this.discriminator == 13)
					{
						return (T13)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T13? item)
			{
				if (this.discriminator == 13)
				{
					item = (T13)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5,
System.Action<T6> case6,
System.Action<T7> case7,
System.Action<T8> case8,
System.Action<T9> case9,
System.Action<T10> case10,
System.Action<T11> case11,
System.Action<T12> case12,
System.Action<T13> case13)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									case 6:
					{
						case6((T6)this.value);
						break;
					}
									case 7:
					{
						case7((T7)this.value);
						break;
					}
									case 8:
					{
						case8((T8)this.value);
						break;
					}
									case 9:
					{
						case9((T9)this.value);
						break;
					}
									case 10:
					{
						case10((T10)this.value);
						break;
					}
									case 11:
					{
						case11((T11)this.value);
						break;
					}
									case 12:
					{
						case12((T12)this.value);
						break;
					}
									case 13:
					{
						case13((T13)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5,
System.Action<TState, T6> case6,
System.Action<TState, T7> case7,
System.Action<TState, T8> case8,
System.Action<TState, T9> case9,
System.Action<TState, T10> case10,
System.Action<TState, T11> case11,
System.Action<TState, T12> case12,
System.Action<TState, T13> case13)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									case 6:
					{
						case6(state, (T6)this.value);
						break;
					}
									case 7:
					{
						case7(state, (T7)this.value);
						break;
					}
									case 8:
					{
						case8(state, (T8)this.value);
						break;
					}
									case 9:
					{
						case9(state, (T9)this.value);
						break;
					}
									case 10:
					{
						case10(state, (T10)this.value);
						break;
					}
									case 11:
					{
						case11(state, (T11)this.value);
						break;
					}
									case 12:
					{
						case12(state, (T12)this.value);
						break;
					}
									case 13:
					{
						case13(state, (T13)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5,
System.Func<T6, TResult> case6,
System.Func<T7, TResult> case7,
System.Func<T8, TResult> case8,
System.Func<T9, TResult> case9,
System.Func<T10, TResult> case10,
System.Func<T11, TResult> case11,
System.Func<T12, TResult> case12,
System.Func<T13, TResult> case13)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									case 6:
					{
						return case6((T6)this.value);
					}
									case 7:
					{
						return case7((T7)this.value);
					}
									case 8:
					{
						return case8((T8)this.value);
					}
									case 9:
					{
						return case9((T9)this.value);
					}
									case 10:
					{
						return case10((T10)this.value);
					}
									case 11:
					{
						return case11((T11)this.value);
					}
									case 12:
					{
						return case12((T12)this.value);
					}
									case 13:
					{
						return case13((T13)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5,
System.Func<TState, T6, TResult> case6,
System.Func<TState, T7, TResult> case7,
System.Func<TState, T8, TResult> case8,
System.Func<TState, T9, TResult> case9,
System.Func<TState, T10, TResult> case10,
System.Func<TState, T11, TResult> case11,
System.Func<TState, T12, TResult> case12,
System.Func<TState, T13, TResult> case13)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									case 6:
					{
						return case6(state, (T6)this.value);
					}
									case 7:
					{
						return case7(state, (T7)this.value);
					}
									case 8:
					{
						return case8(state, (T8)this.value);
					}
									case 9:
					{
						return case9(state, (T9)this.value);
					}
									case 10:
					{
						return case10(state, (T10)this.value);
					}
									case 11:
					{
						return case11(state, (T11)this.value);
					}
									case 12:
					{
						return case12(state, (T12)this.value);
					}
									case 13:
					{
						return case13(state, (T13)this.value);
					}
									default:
						return defaultCase(state);
				}
			}

			public override bool Equals(object? other)
			{
				if (other is FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> otherUnion)
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : IFlatBufferUnion
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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
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

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>

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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
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
				
			
			public FlatBufferUnion(T11 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 11;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T12 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 12;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T13 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 13;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T14 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 14;
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
				
			
			public T11 Item11
			{
				get 
				{
					if (this.discriminator == 11)
					{
						return (T11)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T11? item)
			{
				if (this.discriminator == 11)
				{
					item = (T11)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T12 Item12
			{
				get 
				{
					if (this.discriminator == 12)
					{
						return (T12)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T12? item)
			{
				if (this.discriminator == 12)
				{
					item = (T12)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T13 Item13
			{
				get 
				{
					if (this.discriminator == 13)
					{
						return (T13)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T13? item)
			{
				if (this.discriminator == 13)
				{
					item = (T13)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T14 Item14
			{
				get 
				{
					if (this.discriminator == 14)
					{
						return (T14)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T14? item)
			{
				if (this.discriminator == 14)
				{
					item = (T14)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5,
System.Action<T6> case6,
System.Action<T7> case7,
System.Action<T8> case8,
System.Action<T9> case9,
System.Action<T10> case10,
System.Action<T11> case11,
System.Action<T12> case12,
System.Action<T13> case13,
System.Action<T14> case14)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									case 6:
					{
						case6((T6)this.value);
						break;
					}
									case 7:
					{
						case7((T7)this.value);
						break;
					}
									case 8:
					{
						case8((T8)this.value);
						break;
					}
									case 9:
					{
						case9((T9)this.value);
						break;
					}
									case 10:
					{
						case10((T10)this.value);
						break;
					}
									case 11:
					{
						case11((T11)this.value);
						break;
					}
									case 12:
					{
						case12((T12)this.value);
						break;
					}
									case 13:
					{
						case13((T13)this.value);
						break;
					}
									case 14:
					{
						case14((T14)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5,
System.Action<TState, T6> case6,
System.Action<TState, T7> case7,
System.Action<TState, T8> case8,
System.Action<TState, T9> case9,
System.Action<TState, T10> case10,
System.Action<TState, T11> case11,
System.Action<TState, T12> case12,
System.Action<TState, T13> case13,
System.Action<TState, T14> case14)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									case 6:
					{
						case6(state, (T6)this.value);
						break;
					}
									case 7:
					{
						case7(state, (T7)this.value);
						break;
					}
									case 8:
					{
						case8(state, (T8)this.value);
						break;
					}
									case 9:
					{
						case9(state, (T9)this.value);
						break;
					}
									case 10:
					{
						case10(state, (T10)this.value);
						break;
					}
									case 11:
					{
						case11(state, (T11)this.value);
						break;
					}
									case 12:
					{
						case12(state, (T12)this.value);
						break;
					}
									case 13:
					{
						case13(state, (T13)this.value);
						break;
					}
									case 14:
					{
						case14(state, (T14)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5,
System.Func<T6, TResult> case6,
System.Func<T7, TResult> case7,
System.Func<T8, TResult> case8,
System.Func<T9, TResult> case9,
System.Func<T10, TResult> case10,
System.Func<T11, TResult> case11,
System.Func<T12, TResult> case12,
System.Func<T13, TResult> case13,
System.Func<T14, TResult> case14)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									case 6:
					{
						return case6((T6)this.value);
					}
									case 7:
					{
						return case7((T7)this.value);
					}
									case 8:
					{
						return case8((T8)this.value);
					}
									case 9:
					{
						return case9((T9)this.value);
					}
									case 10:
					{
						return case10((T10)this.value);
					}
									case 11:
					{
						return case11((T11)this.value);
					}
									case 12:
					{
						return case12((T12)this.value);
					}
									case 13:
					{
						return case13((T13)this.value);
					}
									case 14:
					{
						return case14((T14)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5,
System.Func<TState, T6, TResult> case6,
System.Func<TState, T7, TResult> case7,
System.Func<TState, T8, TResult> case8,
System.Func<TState, T9, TResult> case9,
System.Func<TState, T10, TResult> case10,
System.Func<TState, T11, TResult> case11,
System.Func<TState, T12, TResult> case12,
System.Func<TState, T13, TResult> case13,
System.Func<TState, T14, TResult> case14)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									case 6:
					{
						return case6(state, (T6)this.value);
					}
									case 7:
					{
						return case7(state, (T7)this.value);
					}
									case 8:
					{
						return case8(state, (T8)this.value);
					}
									case 9:
					{
						return case9(state, (T9)this.value);
					}
									case 10:
					{
						return case10(state, (T10)this.value);
					}
									case 11:
					{
						return case11(state, (T11)this.value);
					}
									case 12:
					{
						return case12(state, (T12)this.value);
					}
									case 13:
					{
						return case13(state, (T13)this.value);
					}
									case 14:
					{
						return case14(state, (T14)this.value);
					}
									default:
						return defaultCase(state);
				}
			}

			public override bool Equals(object? other)
			{
				if (other is FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> otherUnion)
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : IFlatBufferUnion
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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
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

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>

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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
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
				
			
			public FlatBufferUnion(T11 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 11;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T12 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 12;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T13 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 13;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T14 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 14;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T15 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 15;
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
				
			
			public T11 Item11
			{
				get 
				{
					if (this.discriminator == 11)
					{
						return (T11)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T11? item)
			{
				if (this.discriminator == 11)
				{
					item = (T11)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T12 Item12
			{
				get 
				{
					if (this.discriminator == 12)
					{
						return (T12)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T12? item)
			{
				if (this.discriminator == 12)
				{
					item = (T12)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T13 Item13
			{
				get 
				{
					if (this.discriminator == 13)
					{
						return (T13)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T13? item)
			{
				if (this.discriminator == 13)
				{
					item = (T13)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T14 Item14
			{
				get 
				{
					if (this.discriminator == 14)
					{
						return (T14)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T14? item)
			{
				if (this.discriminator == 14)
				{
					item = (T14)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T15 Item15
			{
				get 
				{
					if (this.discriminator == 15)
					{
						return (T15)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T15? item)
			{
				if (this.discriminator == 15)
				{
					item = (T15)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5,
System.Action<T6> case6,
System.Action<T7> case7,
System.Action<T8> case8,
System.Action<T9> case9,
System.Action<T10> case10,
System.Action<T11> case11,
System.Action<T12> case12,
System.Action<T13> case13,
System.Action<T14> case14,
System.Action<T15> case15)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									case 6:
					{
						case6((T6)this.value);
						break;
					}
									case 7:
					{
						case7((T7)this.value);
						break;
					}
									case 8:
					{
						case8((T8)this.value);
						break;
					}
									case 9:
					{
						case9((T9)this.value);
						break;
					}
									case 10:
					{
						case10((T10)this.value);
						break;
					}
									case 11:
					{
						case11((T11)this.value);
						break;
					}
									case 12:
					{
						case12((T12)this.value);
						break;
					}
									case 13:
					{
						case13((T13)this.value);
						break;
					}
									case 14:
					{
						case14((T14)this.value);
						break;
					}
									case 15:
					{
						case15((T15)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5,
System.Action<TState, T6> case6,
System.Action<TState, T7> case7,
System.Action<TState, T8> case8,
System.Action<TState, T9> case9,
System.Action<TState, T10> case10,
System.Action<TState, T11> case11,
System.Action<TState, T12> case12,
System.Action<TState, T13> case13,
System.Action<TState, T14> case14,
System.Action<TState, T15> case15)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									case 6:
					{
						case6(state, (T6)this.value);
						break;
					}
									case 7:
					{
						case7(state, (T7)this.value);
						break;
					}
									case 8:
					{
						case8(state, (T8)this.value);
						break;
					}
									case 9:
					{
						case9(state, (T9)this.value);
						break;
					}
									case 10:
					{
						case10(state, (T10)this.value);
						break;
					}
									case 11:
					{
						case11(state, (T11)this.value);
						break;
					}
									case 12:
					{
						case12(state, (T12)this.value);
						break;
					}
									case 13:
					{
						case13(state, (T13)this.value);
						break;
					}
									case 14:
					{
						case14(state, (T14)this.value);
						break;
					}
									case 15:
					{
						case15(state, (T15)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5,
System.Func<T6, TResult> case6,
System.Func<T7, TResult> case7,
System.Func<T8, TResult> case8,
System.Func<T9, TResult> case9,
System.Func<T10, TResult> case10,
System.Func<T11, TResult> case11,
System.Func<T12, TResult> case12,
System.Func<T13, TResult> case13,
System.Func<T14, TResult> case14,
System.Func<T15, TResult> case15)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									case 6:
					{
						return case6((T6)this.value);
					}
									case 7:
					{
						return case7((T7)this.value);
					}
									case 8:
					{
						return case8((T8)this.value);
					}
									case 9:
					{
						return case9((T9)this.value);
					}
									case 10:
					{
						return case10((T10)this.value);
					}
									case 11:
					{
						return case11((T11)this.value);
					}
									case 12:
					{
						return case12((T12)this.value);
					}
									case 13:
					{
						return case13((T13)this.value);
					}
									case 14:
					{
						return case14((T14)this.value);
					}
									case 15:
					{
						return case15((T15)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5,
System.Func<TState, T6, TResult> case6,
System.Func<TState, T7, TResult> case7,
System.Func<TState, T8, TResult> case8,
System.Func<TState, T9, TResult> case9,
System.Func<TState, T10, TResult> case10,
System.Func<TState, T11, TResult> case11,
System.Func<TState, T12, TResult> case12,
System.Func<TState, T13, TResult> case13,
System.Func<TState, T14, TResult> case14,
System.Func<TState, T15, TResult> case15)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									case 6:
					{
						return case6(state, (T6)this.value);
					}
									case 7:
					{
						return case7(state, (T7)this.value);
					}
									case 8:
					{
						return case8(state, (T8)this.value);
					}
									case 9:
					{
						return case9(state, (T9)this.value);
					}
									case 10:
					{
						return case10(state, (T10)this.value);
					}
									case 11:
					{
						return case11(state, (T11)this.value);
					}
									case 12:
					{
						return case12(state, (T12)this.value);
					}
									case 13:
					{
						return case13(state, (T13)this.value);
					}
									case 14:
					{
						return case14(state, (T14)this.value);
					}
									case 15:
					{
						return case15(state, (T15)this.value);
					}
									default:
						return defaultCase(state);
				}
			}

			public override bool Equals(object? other)
			{
				if (other is FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> otherUnion)
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : IFlatBufferUnion
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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
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

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>

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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
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
				
			
			public FlatBufferUnion(T11 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 11;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T12 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 12;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T13 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 13;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T14 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 14;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T15 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 15;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T16 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 16;
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
				
			
			public T11 Item11
			{
				get 
				{
					if (this.discriminator == 11)
					{
						return (T11)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T11? item)
			{
				if (this.discriminator == 11)
				{
					item = (T11)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T12 Item12
			{
				get 
				{
					if (this.discriminator == 12)
					{
						return (T12)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T12? item)
			{
				if (this.discriminator == 12)
				{
					item = (T12)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T13 Item13
			{
				get 
				{
					if (this.discriminator == 13)
					{
						return (T13)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T13? item)
			{
				if (this.discriminator == 13)
				{
					item = (T13)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T14 Item14
			{
				get 
				{
					if (this.discriminator == 14)
					{
						return (T14)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T14? item)
			{
				if (this.discriminator == 14)
				{
					item = (T14)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T15 Item15
			{
				get 
				{
					if (this.discriminator == 15)
					{
						return (T15)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T15? item)
			{
				if (this.discriminator == 15)
				{
					item = (T15)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T16 Item16
			{
				get 
				{
					if (this.discriminator == 16)
					{
						return (T16)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T16? item)
			{
				if (this.discriminator == 16)
				{
					item = (T16)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5,
System.Action<T6> case6,
System.Action<T7> case7,
System.Action<T8> case8,
System.Action<T9> case9,
System.Action<T10> case10,
System.Action<T11> case11,
System.Action<T12> case12,
System.Action<T13> case13,
System.Action<T14> case14,
System.Action<T15> case15,
System.Action<T16> case16)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									case 6:
					{
						case6((T6)this.value);
						break;
					}
									case 7:
					{
						case7((T7)this.value);
						break;
					}
									case 8:
					{
						case8((T8)this.value);
						break;
					}
									case 9:
					{
						case9((T9)this.value);
						break;
					}
									case 10:
					{
						case10((T10)this.value);
						break;
					}
									case 11:
					{
						case11((T11)this.value);
						break;
					}
									case 12:
					{
						case12((T12)this.value);
						break;
					}
									case 13:
					{
						case13((T13)this.value);
						break;
					}
									case 14:
					{
						case14((T14)this.value);
						break;
					}
									case 15:
					{
						case15((T15)this.value);
						break;
					}
									case 16:
					{
						case16((T16)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5,
System.Action<TState, T6> case6,
System.Action<TState, T7> case7,
System.Action<TState, T8> case8,
System.Action<TState, T9> case9,
System.Action<TState, T10> case10,
System.Action<TState, T11> case11,
System.Action<TState, T12> case12,
System.Action<TState, T13> case13,
System.Action<TState, T14> case14,
System.Action<TState, T15> case15,
System.Action<TState, T16> case16)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									case 6:
					{
						case6(state, (T6)this.value);
						break;
					}
									case 7:
					{
						case7(state, (T7)this.value);
						break;
					}
									case 8:
					{
						case8(state, (T8)this.value);
						break;
					}
									case 9:
					{
						case9(state, (T9)this.value);
						break;
					}
									case 10:
					{
						case10(state, (T10)this.value);
						break;
					}
									case 11:
					{
						case11(state, (T11)this.value);
						break;
					}
									case 12:
					{
						case12(state, (T12)this.value);
						break;
					}
									case 13:
					{
						case13(state, (T13)this.value);
						break;
					}
									case 14:
					{
						case14(state, (T14)this.value);
						break;
					}
									case 15:
					{
						case15(state, (T15)this.value);
						break;
					}
									case 16:
					{
						case16(state, (T16)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5,
System.Func<T6, TResult> case6,
System.Func<T7, TResult> case7,
System.Func<T8, TResult> case8,
System.Func<T9, TResult> case9,
System.Func<T10, TResult> case10,
System.Func<T11, TResult> case11,
System.Func<T12, TResult> case12,
System.Func<T13, TResult> case13,
System.Func<T14, TResult> case14,
System.Func<T15, TResult> case15,
System.Func<T16, TResult> case16)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									case 6:
					{
						return case6((T6)this.value);
					}
									case 7:
					{
						return case7((T7)this.value);
					}
									case 8:
					{
						return case8((T8)this.value);
					}
									case 9:
					{
						return case9((T9)this.value);
					}
									case 10:
					{
						return case10((T10)this.value);
					}
									case 11:
					{
						return case11((T11)this.value);
					}
									case 12:
					{
						return case12((T12)this.value);
					}
									case 13:
					{
						return case13((T13)this.value);
					}
									case 14:
					{
						return case14((T14)this.value);
					}
									case 15:
					{
						return case15((T15)this.value);
					}
									case 16:
					{
						return case16((T16)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5,
System.Func<TState, T6, TResult> case6,
System.Func<TState, T7, TResult> case7,
System.Func<TState, T8, TResult> case8,
System.Func<TState, T9, TResult> case9,
System.Func<TState, T10, TResult> case10,
System.Func<TState, T11, TResult> case11,
System.Func<TState, T12, TResult> case12,
System.Func<TState, T13, TResult> case13,
System.Func<TState, T14, TResult> case14,
System.Func<TState, T15, TResult> case15,
System.Func<TState, T16, TResult> case16)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									case 6:
					{
						return case6(state, (T6)this.value);
					}
									case 7:
					{
						return case7(state, (T7)this.value);
					}
									case 8:
					{
						return case8(state, (T8)this.value);
					}
									case 9:
					{
						return case9(state, (T9)this.value);
					}
									case 10:
					{
						return case10(state, (T10)this.value);
					}
									case 11:
					{
						return case11(state, (T11)this.value);
					}
									case 12:
					{
						return case12(state, (T12)this.value);
					}
									case 13:
					{
						return case13(state, (T13)this.value);
					}
									case 14:
					{
						return case14(state, (T14)this.value);
					}
									case 15:
					{
						return case15(state, (T15)this.value);
					}
									case 16:
					{
						return case16(state, (T16)this.value);
					}
									default:
						return defaultCase(state);
				}
			}

			public override bool Equals(object? other)
			{
				if (other is FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> otherUnion)
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> : IFlatBufferUnion
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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
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

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>

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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
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
				
			
			public FlatBufferUnion(T11 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 11;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T12 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 12;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T13 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 13;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T14 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 14;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T15 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 15;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T16 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 16;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T17 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 17;
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
				
			
			public T11 Item11
			{
				get 
				{
					if (this.discriminator == 11)
					{
						return (T11)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T11? item)
			{
				if (this.discriminator == 11)
				{
					item = (T11)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T12 Item12
			{
				get 
				{
					if (this.discriminator == 12)
					{
						return (T12)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T12? item)
			{
				if (this.discriminator == 12)
				{
					item = (T12)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T13 Item13
			{
				get 
				{
					if (this.discriminator == 13)
					{
						return (T13)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T13? item)
			{
				if (this.discriminator == 13)
				{
					item = (T13)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T14 Item14
			{
				get 
				{
					if (this.discriminator == 14)
					{
						return (T14)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T14? item)
			{
				if (this.discriminator == 14)
				{
					item = (T14)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T15 Item15
			{
				get 
				{
					if (this.discriminator == 15)
					{
						return (T15)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T15? item)
			{
				if (this.discriminator == 15)
				{
					item = (T15)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T16 Item16
			{
				get 
				{
					if (this.discriminator == 16)
					{
						return (T16)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T16? item)
			{
				if (this.discriminator == 16)
				{
					item = (T16)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T17 Item17
			{
				get 
				{
					if (this.discriminator == 17)
					{
						return (T17)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T17? item)
			{
				if (this.discriminator == 17)
				{
					item = (T17)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5,
System.Action<T6> case6,
System.Action<T7> case7,
System.Action<T8> case8,
System.Action<T9> case9,
System.Action<T10> case10,
System.Action<T11> case11,
System.Action<T12> case12,
System.Action<T13> case13,
System.Action<T14> case14,
System.Action<T15> case15,
System.Action<T16> case16,
System.Action<T17> case17)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									case 6:
					{
						case6((T6)this.value);
						break;
					}
									case 7:
					{
						case7((T7)this.value);
						break;
					}
									case 8:
					{
						case8((T8)this.value);
						break;
					}
									case 9:
					{
						case9((T9)this.value);
						break;
					}
									case 10:
					{
						case10((T10)this.value);
						break;
					}
									case 11:
					{
						case11((T11)this.value);
						break;
					}
									case 12:
					{
						case12((T12)this.value);
						break;
					}
									case 13:
					{
						case13((T13)this.value);
						break;
					}
									case 14:
					{
						case14((T14)this.value);
						break;
					}
									case 15:
					{
						case15((T15)this.value);
						break;
					}
									case 16:
					{
						case16((T16)this.value);
						break;
					}
									case 17:
					{
						case17((T17)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5,
System.Action<TState, T6> case6,
System.Action<TState, T7> case7,
System.Action<TState, T8> case8,
System.Action<TState, T9> case9,
System.Action<TState, T10> case10,
System.Action<TState, T11> case11,
System.Action<TState, T12> case12,
System.Action<TState, T13> case13,
System.Action<TState, T14> case14,
System.Action<TState, T15> case15,
System.Action<TState, T16> case16,
System.Action<TState, T17> case17)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									case 6:
					{
						case6(state, (T6)this.value);
						break;
					}
									case 7:
					{
						case7(state, (T7)this.value);
						break;
					}
									case 8:
					{
						case8(state, (T8)this.value);
						break;
					}
									case 9:
					{
						case9(state, (T9)this.value);
						break;
					}
									case 10:
					{
						case10(state, (T10)this.value);
						break;
					}
									case 11:
					{
						case11(state, (T11)this.value);
						break;
					}
									case 12:
					{
						case12(state, (T12)this.value);
						break;
					}
									case 13:
					{
						case13(state, (T13)this.value);
						break;
					}
									case 14:
					{
						case14(state, (T14)this.value);
						break;
					}
									case 15:
					{
						case15(state, (T15)this.value);
						break;
					}
									case 16:
					{
						case16(state, (T16)this.value);
						break;
					}
									case 17:
					{
						case17(state, (T17)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5,
System.Func<T6, TResult> case6,
System.Func<T7, TResult> case7,
System.Func<T8, TResult> case8,
System.Func<T9, TResult> case9,
System.Func<T10, TResult> case10,
System.Func<T11, TResult> case11,
System.Func<T12, TResult> case12,
System.Func<T13, TResult> case13,
System.Func<T14, TResult> case14,
System.Func<T15, TResult> case15,
System.Func<T16, TResult> case16,
System.Func<T17, TResult> case17)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									case 6:
					{
						return case6((T6)this.value);
					}
									case 7:
					{
						return case7((T7)this.value);
					}
									case 8:
					{
						return case8((T8)this.value);
					}
									case 9:
					{
						return case9((T9)this.value);
					}
									case 10:
					{
						return case10((T10)this.value);
					}
									case 11:
					{
						return case11((T11)this.value);
					}
									case 12:
					{
						return case12((T12)this.value);
					}
									case 13:
					{
						return case13((T13)this.value);
					}
									case 14:
					{
						return case14((T14)this.value);
					}
									case 15:
					{
						return case15((T15)this.value);
					}
									case 16:
					{
						return case16((T16)this.value);
					}
									case 17:
					{
						return case17((T17)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5,
System.Func<TState, T6, TResult> case6,
System.Func<TState, T7, TResult> case7,
System.Func<TState, T8, TResult> case8,
System.Func<TState, T9, TResult> case9,
System.Func<TState, T10, TResult> case10,
System.Func<TState, T11, TResult> case11,
System.Func<TState, T12, TResult> case12,
System.Func<TState, T13, TResult> case13,
System.Func<TState, T14, TResult> case14,
System.Func<TState, T15, TResult> case15,
System.Func<TState, T16, TResult> case16,
System.Func<TState, T17, TResult> case17)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									case 6:
					{
						return case6(state, (T6)this.value);
					}
									case 7:
					{
						return case7(state, (T7)this.value);
					}
									case 8:
					{
						return case8(state, (T8)this.value);
					}
									case 9:
					{
						return case9(state, (T9)this.value);
					}
									case 10:
					{
						return case10(state, (T10)this.value);
					}
									case 11:
					{
						return case11(state, (T11)this.value);
					}
									case 12:
					{
						return case12(state, (T12)this.value);
					}
									case 13:
					{
						return case13(state, (T13)this.value);
					}
									case 14:
					{
						return case14(state, (T14)this.value);
					}
									case 15:
					{
						return case15(state, (T15)this.value);
					}
									case 16:
					{
						return case16(state, (T16)this.value);
					}
									case 17:
					{
						return case17(state, (T17)this.value);
					}
									default:
						return defaultCase(state);
				}
			}

			public override bool Equals(object? other)
			{
				if (other is FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> otherUnion)
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> : IFlatBufferUnion
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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
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

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>

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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
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
				
			
			public FlatBufferUnion(T11 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 11;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T12 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 12;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T13 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 13;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T14 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 14;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T15 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 15;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T16 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 16;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T17 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 17;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T18 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 18;
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
				
			
			public T11 Item11
			{
				get 
				{
					if (this.discriminator == 11)
					{
						return (T11)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T11? item)
			{
				if (this.discriminator == 11)
				{
					item = (T11)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T12 Item12
			{
				get 
				{
					if (this.discriminator == 12)
					{
						return (T12)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T12? item)
			{
				if (this.discriminator == 12)
				{
					item = (T12)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T13 Item13
			{
				get 
				{
					if (this.discriminator == 13)
					{
						return (T13)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T13? item)
			{
				if (this.discriminator == 13)
				{
					item = (T13)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T14 Item14
			{
				get 
				{
					if (this.discriminator == 14)
					{
						return (T14)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T14? item)
			{
				if (this.discriminator == 14)
				{
					item = (T14)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T15 Item15
			{
				get 
				{
					if (this.discriminator == 15)
					{
						return (T15)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T15? item)
			{
				if (this.discriminator == 15)
				{
					item = (T15)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T16 Item16
			{
				get 
				{
					if (this.discriminator == 16)
					{
						return (T16)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T16? item)
			{
				if (this.discriminator == 16)
				{
					item = (T16)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T17 Item17
			{
				get 
				{
					if (this.discriminator == 17)
					{
						return (T17)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T17? item)
			{
				if (this.discriminator == 17)
				{
					item = (T17)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T18 Item18
			{
				get 
				{
					if (this.discriminator == 18)
					{
						return (T18)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T18? item)
			{
				if (this.discriminator == 18)
				{
					item = (T18)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5,
System.Action<T6> case6,
System.Action<T7> case7,
System.Action<T8> case8,
System.Action<T9> case9,
System.Action<T10> case10,
System.Action<T11> case11,
System.Action<T12> case12,
System.Action<T13> case13,
System.Action<T14> case14,
System.Action<T15> case15,
System.Action<T16> case16,
System.Action<T17> case17,
System.Action<T18> case18)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									case 6:
					{
						case6((T6)this.value);
						break;
					}
									case 7:
					{
						case7((T7)this.value);
						break;
					}
									case 8:
					{
						case8((T8)this.value);
						break;
					}
									case 9:
					{
						case9((T9)this.value);
						break;
					}
									case 10:
					{
						case10((T10)this.value);
						break;
					}
									case 11:
					{
						case11((T11)this.value);
						break;
					}
									case 12:
					{
						case12((T12)this.value);
						break;
					}
									case 13:
					{
						case13((T13)this.value);
						break;
					}
									case 14:
					{
						case14((T14)this.value);
						break;
					}
									case 15:
					{
						case15((T15)this.value);
						break;
					}
									case 16:
					{
						case16((T16)this.value);
						break;
					}
									case 17:
					{
						case17((T17)this.value);
						break;
					}
									case 18:
					{
						case18((T18)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5,
System.Action<TState, T6> case6,
System.Action<TState, T7> case7,
System.Action<TState, T8> case8,
System.Action<TState, T9> case9,
System.Action<TState, T10> case10,
System.Action<TState, T11> case11,
System.Action<TState, T12> case12,
System.Action<TState, T13> case13,
System.Action<TState, T14> case14,
System.Action<TState, T15> case15,
System.Action<TState, T16> case16,
System.Action<TState, T17> case17,
System.Action<TState, T18> case18)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									case 6:
					{
						case6(state, (T6)this.value);
						break;
					}
									case 7:
					{
						case7(state, (T7)this.value);
						break;
					}
									case 8:
					{
						case8(state, (T8)this.value);
						break;
					}
									case 9:
					{
						case9(state, (T9)this.value);
						break;
					}
									case 10:
					{
						case10(state, (T10)this.value);
						break;
					}
									case 11:
					{
						case11(state, (T11)this.value);
						break;
					}
									case 12:
					{
						case12(state, (T12)this.value);
						break;
					}
									case 13:
					{
						case13(state, (T13)this.value);
						break;
					}
									case 14:
					{
						case14(state, (T14)this.value);
						break;
					}
									case 15:
					{
						case15(state, (T15)this.value);
						break;
					}
									case 16:
					{
						case16(state, (T16)this.value);
						break;
					}
									case 17:
					{
						case17(state, (T17)this.value);
						break;
					}
									case 18:
					{
						case18(state, (T18)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5,
System.Func<T6, TResult> case6,
System.Func<T7, TResult> case7,
System.Func<T8, TResult> case8,
System.Func<T9, TResult> case9,
System.Func<T10, TResult> case10,
System.Func<T11, TResult> case11,
System.Func<T12, TResult> case12,
System.Func<T13, TResult> case13,
System.Func<T14, TResult> case14,
System.Func<T15, TResult> case15,
System.Func<T16, TResult> case16,
System.Func<T17, TResult> case17,
System.Func<T18, TResult> case18)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									case 6:
					{
						return case6((T6)this.value);
					}
									case 7:
					{
						return case7((T7)this.value);
					}
									case 8:
					{
						return case8((T8)this.value);
					}
									case 9:
					{
						return case9((T9)this.value);
					}
									case 10:
					{
						return case10((T10)this.value);
					}
									case 11:
					{
						return case11((T11)this.value);
					}
									case 12:
					{
						return case12((T12)this.value);
					}
									case 13:
					{
						return case13((T13)this.value);
					}
									case 14:
					{
						return case14((T14)this.value);
					}
									case 15:
					{
						return case15((T15)this.value);
					}
									case 16:
					{
						return case16((T16)this.value);
					}
									case 17:
					{
						return case17((T17)this.value);
					}
									case 18:
					{
						return case18((T18)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5,
System.Func<TState, T6, TResult> case6,
System.Func<TState, T7, TResult> case7,
System.Func<TState, T8, TResult> case8,
System.Func<TState, T9, TResult> case9,
System.Func<TState, T10, TResult> case10,
System.Func<TState, T11, TResult> case11,
System.Func<TState, T12, TResult> case12,
System.Func<TState, T13, TResult> case13,
System.Func<TState, T14, TResult> case14,
System.Func<TState, T15, TResult> case15,
System.Func<TState, T16, TResult> case16,
System.Func<TState, T17, TResult> case17,
System.Func<TState, T18, TResult> case18)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									case 6:
					{
						return case6(state, (T6)this.value);
					}
									case 7:
					{
						return case7(state, (T7)this.value);
					}
									case 8:
					{
						return case8(state, (T8)this.value);
					}
									case 9:
					{
						return case9(state, (T9)this.value);
					}
									case 10:
					{
						return case10(state, (T10)this.value);
					}
									case 11:
					{
						return case11(state, (T11)this.value);
					}
									case 12:
					{
						return case12(state, (T12)this.value);
					}
									case 13:
					{
						return case13(state, (T13)this.value);
					}
									case 14:
					{
						return case14(state, (T14)this.value);
					}
									case 15:
					{
						return case15(state, (T15)this.value);
					}
									case 16:
					{
						return case16(state, (T16)this.value);
					}
									case 17:
					{
						return case17(state, (T17)this.value);
					}
									case 18:
					{
						return case18(state, (T18)this.value);
					}
									default:
						return defaultCase(state);
				}
			}

			public override bool Equals(object? other)
			{
				if (other is FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> otherUnion)
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> : IFlatBufferUnion
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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
					where T19 : notnull
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

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>

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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
					where T19 : notnull
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
				
			
			public FlatBufferUnion(T11 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 11;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T12 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 12;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T13 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 13;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T14 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 14;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T15 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 15;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T16 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 16;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T17 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 17;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T18 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 18;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T19 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 19;
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
				
			
			public T11 Item11
			{
				get 
				{
					if (this.discriminator == 11)
					{
						return (T11)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T11? item)
			{
				if (this.discriminator == 11)
				{
					item = (T11)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T12 Item12
			{
				get 
				{
					if (this.discriminator == 12)
					{
						return (T12)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T12? item)
			{
				if (this.discriminator == 12)
				{
					item = (T12)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T13 Item13
			{
				get 
				{
					if (this.discriminator == 13)
					{
						return (T13)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T13? item)
			{
				if (this.discriminator == 13)
				{
					item = (T13)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T14 Item14
			{
				get 
				{
					if (this.discriminator == 14)
					{
						return (T14)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T14? item)
			{
				if (this.discriminator == 14)
				{
					item = (T14)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T15 Item15
			{
				get 
				{
					if (this.discriminator == 15)
					{
						return (T15)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T15? item)
			{
				if (this.discriminator == 15)
				{
					item = (T15)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T16 Item16
			{
				get 
				{
					if (this.discriminator == 16)
					{
						return (T16)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T16? item)
			{
				if (this.discriminator == 16)
				{
					item = (T16)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T17 Item17
			{
				get 
				{
					if (this.discriminator == 17)
					{
						return (T17)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T17? item)
			{
				if (this.discriminator == 17)
				{
					item = (T17)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T18 Item18
			{
				get 
				{
					if (this.discriminator == 18)
					{
						return (T18)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T18? item)
			{
				if (this.discriminator == 18)
				{
					item = (T18)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T19 Item19
			{
				get 
				{
					if (this.discriminator == 19)
					{
						return (T19)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T19? item)
			{
				if (this.discriminator == 19)
				{
					item = (T19)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5,
System.Action<T6> case6,
System.Action<T7> case7,
System.Action<T8> case8,
System.Action<T9> case9,
System.Action<T10> case10,
System.Action<T11> case11,
System.Action<T12> case12,
System.Action<T13> case13,
System.Action<T14> case14,
System.Action<T15> case15,
System.Action<T16> case16,
System.Action<T17> case17,
System.Action<T18> case18,
System.Action<T19> case19)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									case 6:
					{
						case6((T6)this.value);
						break;
					}
									case 7:
					{
						case7((T7)this.value);
						break;
					}
									case 8:
					{
						case8((T8)this.value);
						break;
					}
									case 9:
					{
						case9((T9)this.value);
						break;
					}
									case 10:
					{
						case10((T10)this.value);
						break;
					}
									case 11:
					{
						case11((T11)this.value);
						break;
					}
									case 12:
					{
						case12((T12)this.value);
						break;
					}
									case 13:
					{
						case13((T13)this.value);
						break;
					}
									case 14:
					{
						case14((T14)this.value);
						break;
					}
									case 15:
					{
						case15((T15)this.value);
						break;
					}
									case 16:
					{
						case16((T16)this.value);
						break;
					}
									case 17:
					{
						case17((T17)this.value);
						break;
					}
									case 18:
					{
						case18((T18)this.value);
						break;
					}
									case 19:
					{
						case19((T19)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5,
System.Action<TState, T6> case6,
System.Action<TState, T7> case7,
System.Action<TState, T8> case8,
System.Action<TState, T9> case9,
System.Action<TState, T10> case10,
System.Action<TState, T11> case11,
System.Action<TState, T12> case12,
System.Action<TState, T13> case13,
System.Action<TState, T14> case14,
System.Action<TState, T15> case15,
System.Action<TState, T16> case16,
System.Action<TState, T17> case17,
System.Action<TState, T18> case18,
System.Action<TState, T19> case19)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									case 6:
					{
						case6(state, (T6)this.value);
						break;
					}
									case 7:
					{
						case7(state, (T7)this.value);
						break;
					}
									case 8:
					{
						case8(state, (T8)this.value);
						break;
					}
									case 9:
					{
						case9(state, (T9)this.value);
						break;
					}
									case 10:
					{
						case10(state, (T10)this.value);
						break;
					}
									case 11:
					{
						case11(state, (T11)this.value);
						break;
					}
									case 12:
					{
						case12(state, (T12)this.value);
						break;
					}
									case 13:
					{
						case13(state, (T13)this.value);
						break;
					}
									case 14:
					{
						case14(state, (T14)this.value);
						break;
					}
									case 15:
					{
						case15(state, (T15)this.value);
						break;
					}
									case 16:
					{
						case16(state, (T16)this.value);
						break;
					}
									case 17:
					{
						case17(state, (T17)this.value);
						break;
					}
									case 18:
					{
						case18(state, (T18)this.value);
						break;
					}
									case 19:
					{
						case19(state, (T19)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5,
System.Func<T6, TResult> case6,
System.Func<T7, TResult> case7,
System.Func<T8, TResult> case8,
System.Func<T9, TResult> case9,
System.Func<T10, TResult> case10,
System.Func<T11, TResult> case11,
System.Func<T12, TResult> case12,
System.Func<T13, TResult> case13,
System.Func<T14, TResult> case14,
System.Func<T15, TResult> case15,
System.Func<T16, TResult> case16,
System.Func<T17, TResult> case17,
System.Func<T18, TResult> case18,
System.Func<T19, TResult> case19)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									case 6:
					{
						return case6((T6)this.value);
					}
									case 7:
					{
						return case7((T7)this.value);
					}
									case 8:
					{
						return case8((T8)this.value);
					}
									case 9:
					{
						return case9((T9)this.value);
					}
									case 10:
					{
						return case10((T10)this.value);
					}
									case 11:
					{
						return case11((T11)this.value);
					}
									case 12:
					{
						return case12((T12)this.value);
					}
									case 13:
					{
						return case13((T13)this.value);
					}
									case 14:
					{
						return case14((T14)this.value);
					}
									case 15:
					{
						return case15((T15)this.value);
					}
									case 16:
					{
						return case16((T16)this.value);
					}
									case 17:
					{
						return case17((T17)this.value);
					}
									case 18:
					{
						return case18((T18)this.value);
					}
									case 19:
					{
						return case19((T19)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5,
System.Func<TState, T6, TResult> case6,
System.Func<TState, T7, TResult> case7,
System.Func<TState, T8, TResult> case8,
System.Func<TState, T9, TResult> case9,
System.Func<TState, T10, TResult> case10,
System.Func<TState, T11, TResult> case11,
System.Func<TState, T12, TResult> case12,
System.Func<TState, T13, TResult> case13,
System.Func<TState, T14, TResult> case14,
System.Func<TState, T15, TResult> case15,
System.Func<TState, T16, TResult> case16,
System.Func<TState, T17, TResult> case17,
System.Func<TState, T18, TResult> case18,
System.Func<TState, T19, TResult> case19)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									case 6:
					{
						return case6(state, (T6)this.value);
					}
									case 7:
					{
						return case7(state, (T7)this.value);
					}
									case 8:
					{
						return case8(state, (T8)this.value);
					}
									case 9:
					{
						return case9(state, (T9)this.value);
					}
									case 10:
					{
						return case10(state, (T10)this.value);
					}
									case 11:
					{
						return case11(state, (T11)this.value);
					}
									case 12:
					{
						return case12(state, (T12)this.value);
					}
									case 13:
					{
						return case13(state, (T13)this.value);
					}
									case 14:
					{
						return case14(state, (T14)this.value);
					}
									case 15:
					{
						return case15(state, (T15)this.value);
					}
									case 16:
					{
						return case16(state, (T16)this.value);
					}
									case 17:
					{
						return case17(state, (T17)this.value);
					}
									case 18:
					{
						return case18(state, (T18)this.value);
					}
									case 19:
					{
						return case19(state, (T19)this.value);
					}
									default:
						return defaultCase(state);
				}
			}

			public override bool Equals(object? other)
			{
				if (other is FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> otherUnion)
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20> : IFlatBufferUnion
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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
					where T19 : notnull
					where T20 : notnull
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

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20> : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>

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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
					where T19 : notnull
					where T20 : notnull
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
				
			
			public FlatBufferUnion(T11 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 11;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T12 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 12;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T13 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 13;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T14 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 14;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T15 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 15;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T16 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 16;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T17 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 17;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T18 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 18;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T19 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 19;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T20 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 20;
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
				
			
			public T11 Item11
			{
				get 
				{
					if (this.discriminator == 11)
					{
						return (T11)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T11? item)
			{
				if (this.discriminator == 11)
				{
					item = (T11)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T12 Item12
			{
				get 
				{
					if (this.discriminator == 12)
					{
						return (T12)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T12? item)
			{
				if (this.discriminator == 12)
				{
					item = (T12)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T13 Item13
			{
				get 
				{
					if (this.discriminator == 13)
					{
						return (T13)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T13? item)
			{
				if (this.discriminator == 13)
				{
					item = (T13)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T14 Item14
			{
				get 
				{
					if (this.discriminator == 14)
					{
						return (T14)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T14? item)
			{
				if (this.discriminator == 14)
				{
					item = (T14)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T15 Item15
			{
				get 
				{
					if (this.discriminator == 15)
					{
						return (T15)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T15? item)
			{
				if (this.discriminator == 15)
				{
					item = (T15)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T16 Item16
			{
				get 
				{
					if (this.discriminator == 16)
					{
						return (T16)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T16? item)
			{
				if (this.discriminator == 16)
				{
					item = (T16)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T17 Item17
			{
				get 
				{
					if (this.discriminator == 17)
					{
						return (T17)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T17? item)
			{
				if (this.discriminator == 17)
				{
					item = (T17)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T18 Item18
			{
				get 
				{
					if (this.discriminator == 18)
					{
						return (T18)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T18? item)
			{
				if (this.discriminator == 18)
				{
					item = (T18)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T19 Item19
			{
				get 
				{
					if (this.discriminator == 19)
					{
						return (T19)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T19? item)
			{
				if (this.discriminator == 19)
				{
					item = (T19)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T20 Item20
			{
				get 
				{
					if (this.discriminator == 20)
					{
						return (T20)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T20? item)
			{
				if (this.discriminator == 20)
				{
					item = (T20)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5,
System.Action<T6> case6,
System.Action<T7> case7,
System.Action<T8> case8,
System.Action<T9> case9,
System.Action<T10> case10,
System.Action<T11> case11,
System.Action<T12> case12,
System.Action<T13> case13,
System.Action<T14> case14,
System.Action<T15> case15,
System.Action<T16> case16,
System.Action<T17> case17,
System.Action<T18> case18,
System.Action<T19> case19,
System.Action<T20> case20)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									case 6:
					{
						case6((T6)this.value);
						break;
					}
									case 7:
					{
						case7((T7)this.value);
						break;
					}
									case 8:
					{
						case8((T8)this.value);
						break;
					}
									case 9:
					{
						case9((T9)this.value);
						break;
					}
									case 10:
					{
						case10((T10)this.value);
						break;
					}
									case 11:
					{
						case11((T11)this.value);
						break;
					}
									case 12:
					{
						case12((T12)this.value);
						break;
					}
									case 13:
					{
						case13((T13)this.value);
						break;
					}
									case 14:
					{
						case14((T14)this.value);
						break;
					}
									case 15:
					{
						case15((T15)this.value);
						break;
					}
									case 16:
					{
						case16((T16)this.value);
						break;
					}
									case 17:
					{
						case17((T17)this.value);
						break;
					}
									case 18:
					{
						case18((T18)this.value);
						break;
					}
									case 19:
					{
						case19((T19)this.value);
						break;
					}
									case 20:
					{
						case20((T20)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5,
System.Action<TState, T6> case6,
System.Action<TState, T7> case7,
System.Action<TState, T8> case8,
System.Action<TState, T9> case9,
System.Action<TState, T10> case10,
System.Action<TState, T11> case11,
System.Action<TState, T12> case12,
System.Action<TState, T13> case13,
System.Action<TState, T14> case14,
System.Action<TState, T15> case15,
System.Action<TState, T16> case16,
System.Action<TState, T17> case17,
System.Action<TState, T18> case18,
System.Action<TState, T19> case19,
System.Action<TState, T20> case20)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									case 6:
					{
						case6(state, (T6)this.value);
						break;
					}
									case 7:
					{
						case7(state, (T7)this.value);
						break;
					}
									case 8:
					{
						case8(state, (T8)this.value);
						break;
					}
									case 9:
					{
						case9(state, (T9)this.value);
						break;
					}
									case 10:
					{
						case10(state, (T10)this.value);
						break;
					}
									case 11:
					{
						case11(state, (T11)this.value);
						break;
					}
									case 12:
					{
						case12(state, (T12)this.value);
						break;
					}
									case 13:
					{
						case13(state, (T13)this.value);
						break;
					}
									case 14:
					{
						case14(state, (T14)this.value);
						break;
					}
									case 15:
					{
						case15(state, (T15)this.value);
						break;
					}
									case 16:
					{
						case16(state, (T16)this.value);
						break;
					}
									case 17:
					{
						case17(state, (T17)this.value);
						break;
					}
									case 18:
					{
						case18(state, (T18)this.value);
						break;
					}
									case 19:
					{
						case19(state, (T19)this.value);
						break;
					}
									case 20:
					{
						case20(state, (T20)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5,
System.Func<T6, TResult> case6,
System.Func<T7, TResult> case7,
System.Func<T8, TResult> case8,
System.Func<T9, TResult> case9,
System.Func<T10, TResult> case10,
System.Func<T11, TResult> case11,
System.Func<T12, TResult> case12,
System.Func<T13, TResult> case13,
System.Func<T14, TResult> case14,
System.Func<T15, TResult> case15,
System.Func<T16, TResult> case16,
System.Func<T17, TResult> case17,
System.Func<T18, TResult> case18,
System.Func<T19, TResult> case19,
System.Func<T20, TResult> case20)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									case 6:
					{
						return case6((T6)this.value);
					}
									case 7:
					{
						return case7((T7)this.value);
					}
									case 8:
					{
						return case8((T8)this.value);
					}
									case 9:
					{
						return case9((T9)this.value);
					}
									case 10:
					{
						return case10((T10)this.value);
					}
									case 11:
					{
						return case11((T11)this.value);
					}
									case 12:
					{
						return case12((T12)this.value);
					}
									case 13:
					{
						return case13((T13)this.value);
					}
									case 14:
					{
						return case14((T14)this.value);
					}
									case 15:
					{
						return case15((T15)this.value);
					}
									case 16:
					{
						return case16((T16)this.value);
					}
									case 17:
					{
						return case17((T17)this.value);
					}
									case 18:
					{
						return case18((T18)this.value);
					}
									case 19:
					{
						return case19((T19)this.value);
					}
									case 20:
					{
						return case20((T20)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5,
System.Func<TState, T6, TResult> case6,
System.Func<TState, T7, TResult> case7,
System.Func<TState, T8, TResult> case8,
System.Func<TState, T9, TResult> case9,
System.Func<TState, T10, TResult> case10,
System.Func<TState, T11, TResult> case11,
System.Func<TState, T12, TResult> case12,
System.Func<TState, T13, TResult> case13,
System.Func<TState, T14, TResult> case14,
System.Func<TState, T15, TResult> case15,
System.Func<TState, T16, TResult> case16,
System.Func<TState, T17, TResult> case17,
System.Func<TState, T18, TResult> case18,
System.Func<TState, T19, TResult> case19,
System.Func<TState, T20, TResult> case20)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									case 6:
					{
						return case6(state, (T6)this.value);
					}
									case 7:
					{
						return case7(state, (T7)this.value);
					}
									case 8:
					{
						return case8(state, (T8)this.value);
					}
									case 9:
					{
						return case9(state, (T9)this.value);
					}
									case 10:
					{
						return case10(state, (T10)this.value);
					}
									case 11:
					{
						return case11(state, (T11)this.value);
					}
									case 12:
					{
						return case12(state, (T12)this.value);
					}
									case 13:
					{
						return case13(state, (T13)this.value);
					}
									case 14:
					{
						return case14(state, (T14)this.value);
					}
									case 15:
					{
						return case15(state, (T15)this.value);
					}
									case 16:
					{
						return case16(state, (T16)this.value);
					}
									case 17:
					{
						return case17(state, (T17)this.value);
					}
									case 18:
					{
						return case18(state, (T18)this.value);
					}
									case 19:
					{
						return case19(state, (T19)this.value);
					}
									case 20:
					{
						return case20(state, (T20)this.value);
					}
									default:
						return defaultCase(state);
				}
			}

			public override bool Equals(object? other)
			{
				if (other is FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20> otherUnion)
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21> : IFlatBufferUnion
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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
					where T19 : notnull
					where T20 : notnull
					where T21 : notnull
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

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21> : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>

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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
					where T19 : notnull
					where T20 : notnull
					where T21 : notnull
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
				
			
			public FlatBufferUnion(T11 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 11;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T12 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 12;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T13 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 13;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T14 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 14;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T15 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 15;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T16 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 16;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T17 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 17;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T18 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 18;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T19 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 19;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T20 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 20;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T21 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 21;
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
				
			
			public T11 Item11
			{
				get 
				{
					if (this.discriminator == 11)
					{
						return (T11)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T11? item)
			{
				if (this.discriminator == 11)
				{
					item = (T11)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T12 Item12
			{
				get 
				{
					if (this.discriminator == 12)
					{
						return (T12)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T12? item)
			{
				if (this.discriminator == 12)
				{
					item = (T12)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T13 Item13
			{
				get 
				{
					if (this.discriminator == 13)
					{
						return (T13)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T13? item)
			{
				if (this.discriminator == 13)
				{
					item = (T13)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T14 Item14
			{
				get 
				{
					if (this.discriminator == 14)
					{
						return (T14)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T14? item)
			{
				if (this.discriminator == 14)
				{
					item = (T14)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T15 Item15
			{
				get 
				{
					if (this.discriminator == 15)
					{
						return (T15)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T15? item)
			{
				if (this.discriminator == 15)
				{
					item = (T15)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T16 Item16
			{
				get 
				{
					if (this.discriminator == 16)
					{
						return (T16)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T16? item)
			{
				if (this.discriminator == 16)
				{
					item = (T16)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T17 Item17
			{
				get 
				{
					if (this.discriminator == 17)
					{
						return (T17)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T17? item)
			{
				if (this.discriminator == 17)
				{
					item = (T17)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T18 Item18
			{
				get 
				{
					if (this.discriminator == 18)
					{
						return (T18)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T18? item)
			{
				if (this.discriminator == 18)
				{
					item = (T18)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T19 Item19
			{
				get 
				{
					if (this.discriminator == 19)
					{
						return (T19)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T19? item)
			{
				if (this.discriminator == 19)
				{
					item = (T19)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T20 Item20
			{
				get 
				{
					if (this.discriminator == 20)
					{
						return (T20)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T20? item)
			{
				if (this.discriminator == 20)
				{
					item = (T20)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T21 Item21
			{
				get 
				{
					if (this.discriminator == 21)
					{
						return (T21)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T21? item)
			{
				if (this.discriminator == 21)
				{
					item = (T21)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5,
System.Action<T6> case6,
System.Action<T7> case7,
System.Action<T8> case8,
System.Action<T9> case9,
System.Action<T10> case10,
System.Action<T11> case11,
System.Action<T12> case12,
System.Action<T13> case13,
System.Action<T14> case14,
System.Action<T15> case15,
System.Action<T16> case16,
System.Action<T17> case17,
System.Action<T18> case18,
System.Action<T19> case19,
System.Action<T20> case20,
System.Action<T21> case21)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									case 6:
					{
						case6((T6)this.value);
						break;
					}
									case 7:
					{
						case7((T7)this.value);
						break;
					}
									case 8:
					{
						case8((T8)this.value);
						break;
					}
									case 9:
					{
						case9((T9)this.value);
						break;
					}
									case 10:
					{
						case10((T10)this.value);
						break;
					}
									case 11:
					{
						case11((T11)this.value);
						break;
					}
									case 12:
					{
						case12((T12)this.value);
						break;
					}
									case 13:
					{
						case13((T13)this.value);
						break;
					}
									case 14:
					{
						case14((T14)this.value);
						break;
					}
									case 15:
					{
						case15((T15)this.value);
						break;
					}
									case 16:
					{
						case16((T16)this.value);
						break;
					}
									case 17:
					{
						case17((T17)this.value);
						break;
					}
									case 18:
					{
						case18((T18)this.value);
						break;
					}
									case 19:
					{
						case19((T19)this.value);
						break;
					}
									case 20:
					{
						case20((T20)this.value);
						break;
					}
									case 21:
					{
						case21((T21)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5,
System.Action<TState, T6> case6,
System.Action<TState, T7> case7,
System.Action<TState, T8> case8,
System.Action<TState, T9> case9,
System.Action<TState, T10> case10,
System.Action<TState, T11> case11,
System.Action<TState, T12> case12,
System.Action<TState, T13> case13,
System.Action<TState, T14> case14,
System.Action<TState, T15> case15,
System.Action<TState, T16> case16,
System.Action<TState, T17> case17,
System.Action<TState, T18> case18,
System.Action<TState, T19> case19,
System.Action<TState, T20> case20,
System.Action<TState, T21> case21)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									case 6:
					{
						case6(state, (T6)this.value);
						break;
					}
									case 7:
					{
						case7(state, (T7)this.value);
						break;
					}
									case 8:
					{
						case8(state, (T8)this.value);
						break;
					}
									case 9:
					{
						case9(state, (T9)this.value);
						break;
					}
									case 10:
					{
						case10(state, (T10)this.value);
						break;
					}
									case 11:
					{
						case11(state, (T11)this.value);
						break;
					}
									case 12:
					{
						case12(state, (T12)this.value);
						break;
					}
									case 13:
					{
						case13(state, (T13)this.value);
						break;
					}
									case 14:
					{
						case14(state, (T14)this.value);
						break;
					}
									case 15:
					{
						case15(state, (T15)this.value);
						break;
					}
									case 16:
					{
						case16(state, (T16)this.value);
						break;
					}
									case 17:
					{
						case17(state, (T17)this.value);
						break;
					}
									case 18:
					{
						case18(state, (T18)this.value);
						break;
					}
									case 19:
					{
						case19(state, (T19)this.value);
						break;
					}
									case 20:
					{
						case20(state, (T20)this.value);
						break;
					}
									case 21:
					{
						case21(state, (T21)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5,
System.Func<T6, TResult> case6,
System.Func<T7, TResult> case7,
System.Func<T8, TResult> case8,
System.Func<T9, TResult> case9,
System.Func<T10, TResult> case10,
System.Func<T11, TResult> case11,
System.Func<T12, TResult> case12,
System.Func<T13, TResult> case13,
System.Func<T14, TResult> case14,
System.Func<T15, TResult> case15,
System.Func<T16, TResult> case16,
System.Func<T17, TResult> case17,
System.Func<T18, TResult> case18,
System.Func<T19, TResult> case19,
System.Func<T20, TResult> case20,
System.Func<T21, TResult> case21)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									case 6:
					{
						return case6((T6)this.value);
					}
									case 7:
					{
						return case7((T7)this.value);
					}
									case 8:
					{
						return case8((T8)this.value);
					}
									case 9:
					{
						return case9((T9)this.value);
					}
									case 10:
					{
						return case10((T10)this.value);
					}
									case 11:
					{
						return case11((T11)this.value);
					}
									case 12:
					{
						return case12((T12)this.value);
					}
									case 13:
					{
						return case13((T13)this.value);
					}
									case 14:
					{
						return case14((T14)this.value);
					}
									case 15:
					{
						return case15((T15)this.value);
					}
									case 16:
					{
						return case16((T16)this.value);
					}
									case 17:
					{
						return case17((T17)this.value);
					}
									case 18:
					{
						return case18((T18)this.value);
					}
									case 19:
					{
						return case19((T19)this.value);
					}
									case 20:
					{
						return case20((T20)this.value);
					}
									case 21:
					{
						return case21((T21)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5,
System.Func<TState, T6, TResult> case6,
System.Func<TState, T7, TResult> case7,
System.Func<TState, T8, TResult> case8,
System.Func<TState, T9, TResult> case9,
System.Func<TState, T10, TResult> case10,
System.Func<TState, T11, TResult> case11,
System.Func<TState, T12, TResult> case12,
System.Func<TState, T13, TResult> case13,
System.Func<TState, T14, TResult> case14,
System.Func<TState, T15, TResult> case15,
System.Func<TState, T16, TResult> case16,
System.Func<TState, T17, TResult> case17,
System.Func<TState, T18, TResult> case18,
System.Func<TState, T19, TResult> case19,
System.Func<TState, T20, TResult> case20,
System.Func<TState, T21, TResult> case21)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									case 6:
					{
						return case6(state, (T6)this.value);
					}
									case 7:
					{
						return case7(state, (T7)this.value);
					}
									case 8:
					{
						return case8(state, (T8)this.value);
					}
									case 9:
					{
						return case9(state, (T9)this.value);
					}
									case 10:
					{
						return case10(state, (T10)this.value);
					}
									case 11:
					{
						return case11(state, (T11)this.value);
					}
									case 12:
					{
						return case12(state, (T12)this.value);
					}
									case 13:
					{
						return case13(state, (T13)this.value);
					}
									case 14:
					{
						return case14(state, (T14)this.value);
					}
									case 15:
					{
						return case15(state, (T15)this.value);
					}
									case 16:
					{
						return case16(state, (T16)this.value);
					}
									case 17:
					{
						return case17(state, (T17)this.value);
					}
									case 18:
					{
						return case18(state, (T18)this.value);
					}
									case 19:
					{
						return case19(state, (T19)this.value);
					}
									case 20:
					{
						return case20(state, (T20)this.value);
					}
									case 21:
					{
						return case21(state, (T21)this.value);
					}
									default:
						return defaultCase(state);
				}
			}

			public override bool Equals(object? other)
			{
				if (other is FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21> otherUnion)
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22> : IFlatBufferUnion
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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
					where T19 : notnull
					where T20 : notnull
					where T21 : notnull
					where T22 : notnull
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

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22> : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>

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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
					where T19 : notnull
					where T20 : notnull
					where T21 : notnull
					where T22 : notnull
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
				
			
			public FlatBufferUnion(T11 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 11;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T12 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 12;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T13 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 13;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T14 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 14;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T15 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 15;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T16 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 16;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T17 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 17;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T18 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 18;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T19 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 19;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T20 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 20;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T21 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 21;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T22 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 22;
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
				
			
			public T11 Item11
			{
				get 
				{
					if (this.discriminator == 11)
					{
						return (T11)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T11? item)
			{
				if (this.discriminator == 11)
				{
					item = (T11)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T12 Item12
			{
				get 
				{
					if (this.discriminator == 12)
					{
						return (T12)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T12? item)
			{
				if (this.discriminator == 12)
				{
					item = (T12)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T13 Item13
			{
				get 
				{
					if (this.discriminator == 13)
					{
						return (T13)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T13? item)
			{
				if (this.discriminator == 13)
				{
					item = (T13)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T14 Item14
			{
				get 
				{
					if (this.discriminator == 14)
					{
						return (T14)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T14? item)
			{
				if (this.discriminator == 14)
				{
					item = (T14)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T15 Item15
			{
				get 
				{
					if (this.discriminator == 15)
					{
						return (T15)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T15? item)
			{
				if (this.discriminator == 15)
				{
					item = (T15)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T16 Item16
			{
				get 
				{
					if (this.discriminator == 16)
					{
						return (T16)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T16? item)
			{
				if (this.discriminator == 16)
				{
					item = (T16)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T17 Item17
			{
				get 
				{
					if (this.discriminator == 17)
					{
						return (T17)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T17? item)
			{
				if (this.discriminator == 17)
				{
					item = (T17)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T18 Item18
			{
				get 
				{
					if (this.discriminator == 18)
					{
						return (T18)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T18? item)
			{
				if (this.discriminator == 18)
				{
					item = (T18)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T19 Item19
			{
				get 
				{
					if (this.discriminator == 19)
					{
						return (T19)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T19? item)
			{
				if (this.discriminator == 19)
				{
					item = (T19)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T20 Item20
			{
				get 
				{
					if (this.discriminator == 20)
					{
						return (T20)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T20? item)
			{
				if (this.discriminator == 20)
				{
					item = (T20)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T21 Item21
			{
				get 
				{
					if (this.discriminator == 21)
					{
						return (T21)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T21? item)
			{
				if (this.discriminator == 21)
				{
					item = (T21)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T22 Item22
			{
				get 
				{
					if (this.discriminator == 22)
					{
						return (T22)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T22? item)
			{
				if (this.discriminator == 22)
				{
					item = (T22)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5,
System.Action<T6> case6,
System.Action<T7> case7,
System.Action<T8> case8,
System.Action<T9> case9,
System.Action<T10> case10,
System.Action<T11> case11,
System.Action<T12> case12,
System.Action<T13> case13,
System.Action<T14> case14,
System.Action<T15> case15,
System.Action<T16> case16,
System.Action<T17> case17,
System.Action<T18> case18,
System.Action<T19> case19,
System.Action<T20> case20,
System.Action<T21> case21,
System.Action<T22> case22)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									case 6:
					{
						case6((T6)this.value);
						break;
					}
									case 7:
					{
						case7((T7)this.value);
						break;
					}
									case 8:
					{
						case8((T8)this.value);
						break;
					}
									case 9:
					{
						case9((T9)this.value);
						break;
					}
									case 10:
					{
						case10((T10)this.value);
						break;
					}
									case 11:
					{
						case11((T11)this.value);
						break;
					}
									case 12:
					{
						case12((T12)this.value);
						break;
					}
									case 13:
					{
						case13((T13)this.value);
						break;
					}
									case 14:
					{
						case14((T14)this.value);
						break;
					}
									case 15:
					{
						case15((T15)this.value);
						break;
					}
									case 16:
					{
						case16((T16)this.value);
						break;
					}
									case 17:
					{
						case17((T17)this.value);
						break;
					}
									case 18:
					{
						case18((T18)this.value);
						break;
					}
									case 19:
					{
						case19((T19)this.value);
						break;
					}
									case 20:
					{
						case20((T20)this.value);
						break;
					}
									case 21:
					{
						case21((T21)this.value);
						break;
					}
									case 22:
					{
						case22((T22)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5,
System.Action<TState, T6> case6,
System.Action<TState, T7> case7,
System.Action<TState, T8> case8,
System.Action<TState, T9> case9,
System.Action<TState, T10> case10,
System.Action<TState, T11> case11,
System.Action<TState, T12> case12,
System.Action<TState, T13> case13,
System.Action<TState, T14> case14,
System.Action<TState, T15> case15,
System.Action<TState, T16> case16,
System.Action<TState, T17> case17,
System.Action<TState, T18> case18,
System.Action<TState, T19> case19,
System.Action<TState, T20> case20,
System.Action<TState, T21> case21,
System.Action<TState, T22> case22)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									case 6:
					{
						case6(state, (T6)this.value);
						break;
					}
									case 7:
					{
						case7(state, (T7)this.value);
						break;
					}
									case 8:
					{
						case8(state, (T8)this.value);
						break;
					}
									case 9:
					{
						case9(state, (T9)this.value);
						break;
					}
									case 10:
					{
						case10(state, (T10)this.value);
						break;
					}
									case 11:
					{
						case11(state, (T11)this.value);
						break;
					}
									case 12:
					{
						case12(state, (T12)this.value);
						break;
					}
									case 13:
					{
						case13(state, (T13)this.value);
						break;
					}
									case 14:
					{
						case14(state, (T14)this.value);
						break;
					}
									case 15:
					{
						case15(state, (T15)this.value);
						break;
					}
									case 16:
					{
						case16(state, (T16)this.value);
						break;
					}
									case 17:
					{
						case17(state, (T17)this.value);
						break;
					}
									case 18:
					{
						case18(state, (T18)this.value);
						break;
					}
									case 19:
					{
						case19(state, (T19)this.value);
						break;
					}
									case 20:
					{
						case20(state, (T20)this.value);
						break;
					}
									case 21:
					{
						case21(state, (T21)this.value);
						break;
					}
									case 22:
					{
						case22(state, (T22)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5,
System.Func<T6, TResult> case6,
System.Func<T7, TResult> case7,
System.Func<T8, TResult> case8,
System.Func<T9, TResult> case9,
System.Func<T10, TResult> case10,
System.Func<T11, TResult> case11,
System.Func<T12, TResult> case12,
System.Func<T13, TResult> case13,
System.Func<T14, TResult> case14,
System.Func<T15, TResult> case15,
System.Func<T16, TResult> case16,
System.Func<T17, TResult> case17,
System.Func<T18, TResult> case18,
System.Func<T19, TResult> case19,
System.Func<T20, TResult> case20,
System.Func<T21, TResult> case21,
System.Func<T22, TResult> case22)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									case 6:
					{
						return case6((T6)this.value);
					}
									case 7:
					{
						return case7((T7)this.value);
					}
									case 8:
					{
						return case8((T8)this.value);
					}
									case 9:
					{
						return case9((T9)this.value);
					}
									case 10:
					{
						return case10((T10)this.value);
					}
									case 11:
					{
						return case11((T11)this.value);
					}
									case 12:
					{
						return case12((T12)this.value);
					}
									case 13:
					{
						return case13((T13)this.value);
					}
									case 14:
					{
						return case14((T14)this.value);
					}
									case 15:
					{
						return case15((T15)this.value);
					}
									case 16:
					{
						return case16((T16)this.value);
					}
									case 17:
					{
						return case17((T17)this.value);
					}
									case 18:
					{
						return case18((T18)this.value);
					}
									case 19:
					{
						return case19((T19)this.value);
					}
									case 20:
					{
						return case20((T20)this.value);
					}
									case 21:
					{
						return case21((T21)this.value);
					}
									case 22:
					{
						return case22((T22)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5,
System.Func<TState, T6, TResult> case6,
System.Func<TState, T7, TResult> case7,
System.Func<TState, T8, TResult> case8,
System.Func<TState, T9, TResult> case9,
System.Func<TState, T10, TResult> case10,
System.Func<TState, T11, TResult> case11,
System.Func<TState, T12, TResult> case12,
System.Func<TState, T13, TResult> case13,
System.Func<TState, T14, TResult> case14,
System.Func<TState, T15, TResult> case15,
System.Func<TState, T16, TResult> case16,
System.Func<TState, T17, TResult> case17,
System.Func<TState, T18, TResult> case18,
System.Func<TState, T19, TResult> case19,
System.Func<TState, T20, TResult> case20,
System.Func<TState, T21, TResult> case21,
System.Func<TState, T22, TResult> case22)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									case 6:
					{
						return case6(state, (T6)this.value);
					}
									case 7:
					{
						return case7(state, (T7)this.value);
					}
									case 8:
					{
						return case8(state, (T8)this.value);
					}
									case 9:
					{
						return case9(state, (T9)this.value);
					}
									case 10:
					{
						return case10(state, (T10)this.value);
					}
									case 11:
					{
						return case11(state, (T11)this.value);
					}
									case 12:
					{
						return case12(state, (T12)this.value);
					}
									case 13:
					{
						return case13(state, (T13)this.value);
					}
									case 14:
					{
						return case14(state, (T14)this.value);
					}
									case 15:
					{
						return case15(state, (T15)this.value);
					}
									case 16:
					{
						return case16(state, (T16)this.value);
					}
									case 17:
					{
						return case17(state, (T17)this.value);
					}
									case 18:
					{
						return case18(state, (T18)this.value);
					}
									case 19:
					{
						return case19(state, (T19)this.value);
					}
									case 20:
					{
						return case20(state, (T20)this.value);
					}
									case 21:
					{
						return case21(state, (T21)this.value);
					}
									case 22:
					{
						return case22(state, (T22)this.value);
					}
									default:
						return defaultCase(state);
				}
			}

			public override bool Equals(object? other)
			{
				if (other is FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22> otherUnion)
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23> : IFlatBufferUnion
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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
					where T19 : notnull
					where T20 : notnull
					where T21 : notnull
					where T22 : notnull
					where T23 : notnull
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

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23> : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>

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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
					where T19 : notnull
					where T20 : notnull
					where T21 : notnull
					where T22 : notnull
					where T23 : notnull
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
				
			
			public FlatBufferUnion(T11 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 11;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T12 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 12;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T13 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 13;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T14 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 14;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T15 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 15;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T16 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 16;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T17 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 17;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T18 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 18;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T19 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 19;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T20 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 20;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T21 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 21;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T22 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 22;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T23 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 23;
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
				
			
			public T11 Item11
			{
				get 
				{
					if (this.discriminator == 11)
					{
						return (T11)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T11? item)
			{
				if (this.discriminator == 11)
				{
					item = (T11)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T12 Item12
			{
				get 
				{
					if (this.discriminator == 12)
					{
						return (T12)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T12? item)
			{
				if (this.discriminator == 12)
				{
					item = (T12)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T13 Item13
			{
				get 
				{
					if (this.discriminator == 13)
					{
						return (T13)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T13? item)
			{
				if (this.discriminator == 13)
				{
					item = (T13)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T14 Item14
			{
				get 
				{
					if (this.discriminator == 14)
					{
						return (T14)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T14? item)
			{
				if (this.discriminator == 14)
				{
					item = (T14)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T15 Item15
			{
				get 
				{
					if (this.discriminator == 15)
					{
						return (T15)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T15? item)
			{
				if (this.discriminator == 15)
				{
					item = (T15)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T16 Item16
			{
				get 
				{
					if (this.discriminator == 16)
					{
						return (T16)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T16? item)
			{
				if (this.discriminator == 16)
				{
					item = (T16)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T17 Item17
			{
				get 
				{
					if (this.discriminator == 17)
					{
						return (T17)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T17? item)
			{
				if (this.discriminator == 17)
				{
					item = (T17)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T18 Item18
			{
				get 
				{
					if (this.discriminator == 18)
					{
						return (T18)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T18? item)
			{
				if (this.discriminator == 18)
				{
					item = (T18)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T19 Item19
			{
				get 
				{
					if (this.discriminator == 19)
					{
						return (T19)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T19? item)
			{
				if (this.discriminator == 19)
				{
					item = (T19)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T20 Item20
			{
				get 
				{
					if (this.discriminator == 20)
					{
						return (T20)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T20? item)
			{
				if (this.discriminator == 20)
				{
					item = (T20)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T21 Item21
			{
				get 
				{
					if (this.discriminator == 21)
					{
						return (T21)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T21? item)
			{
				if (this.discriminator == 21)
				{
					item = (T21)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T22 Item22
			{
				get 
				{
					if (this.discriminator == 22)
					{
						return (T22)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T22? item)
			{
				if (this.discriminator == 22)
				{
					item = (T22)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T23 Item23
			{
				get 
				{
					if (this.discriminator == 23)
					{
						return (T23)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T23? item)
			{
				if (this.discriminator == 23)
				{
					item = (T23)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5,
System.Action<T6> case6,
System.Action<T7> case7,
System.Action<T8> case8,
System.Action<T9> case9,
System.Action<T10> case10,
System.Action<T11> case11,
System.Action<T12> case12,
System.Action<T13> case13,
System.Action<T14> case14,
System.Action<T15> case15,
System.Action<T16> case16,
System.Action<T17> case17,
System.Action<T18> case18,
System.Action<T19> case19,
System.Action<T20> case20,
System.Action<T21> case21,
System.Action<T22> case22,
System.Action<T23> case23)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									case 6:
					{
						case6((T6)this.value);
						break;
					}
									case 7:
					{
						case7((T7)this.value);
						break;
					}
									case 8:
					{
						case8((T8)this.value);
						break;
					}
									case 9:
					{
						case9((T9)this.value);
						break;
					}
									case 10:
					{
						case10((T10)this.value);
						break;
					}
									case 11:
					{
						case11((T11)this.value);
						break;
					}
									case 12:
					{
						case12((T12)this.value);
						break;
					}
									case 13:
					{
						case13((T13)this.value);
						break;
					}
									case 14:
					{
						case14((T14)this.value);
						break;
					}
									case 15:
					{
						case15((T15)this.value);
						break;
					}
									case 16:
					{
						case16((T16)this.value);
						break;
					}
									case 17:
					{
						case17((T17)this.value);
						break;
					}
									case 18:
					{
						case18((T18)this.value);
						break;
					}
									case 19:
					{
						case19((T19)this.value);
						break;
					}
									case 20:
					{
						case20((T20)this.value);
						break;
					}
									case 21:
					{
						case21((T21)this.value);
						break;
					}
									case 22:
					{
						case22((T22)this.value);
						break;
					}
									case 23:
					{
						case23((T23)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5,
System.Action<TState, T6> case6,
System.Action<TState, T7> case7,
System.Action<TState, T8> case8,
System.Action<TState, T9> case9,
System.Action<TState, T10> case10,
System.Action<TState, T11> case11,
System.Action<TState, T12> case12,
System.Action<TState, T13> case13,
System.Action<TState, T14> case14,
System.Action<TState, T15> case15,
System.Action<TState, T16> case16,
System.Action<TState, T17> case17,
System.Action<TState, T18> case18,
System.Action<TState, T19> case19,
System.Action<TState, T20> case20,
System.Action<TState, T21> case21,
System.Action<TState, T22> case22,
System.Action<TState, T23> case23)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									case 6:
					{
						case6(state, (T6)this.value);
						break;
					}
									case 7:
					{
						case7(state, (T7)this.value);
						break;
					}
									case 8:
					{
						case8(state, (T8)this.value);
						break;
					}
									case 9:
					{
						case9(state, (T9)this.value);
						break;
					}
									case 10:
					{
						case10(state, (T10)this.value);
						break;
					}
									case 11:
					{
						case11(state, (T11)this.value);
						break;
					}
									case 12:
					{
						case12(state, (T12)this.value);
						break;
					}
									case 13:
					{
						case13(state, (T13)this.value);
						break;
					}
									case 14:
					{
						case14(state, (T14)this.value);
						break;
					}
									case 15:
					{
						case15(state, (T15)this.value);
						break;
					}
									case 16:
					{
						case16(state, (T16)this.value);
						break;
					}
									case 17:
					{
						case17(state, (T17)this.value);
						break;
					}
									case 18:
					{
						case18(state, (T18)this.value);
						break;
					}
									case 19:
					{
						case19(state, (T19)this.value);
						break;
					}
									case 20:
					{
						case20(state, (T20)this.value);
						break;
					}
									case 21:
					{
						case21(state, (T21)this.value);
						break;
					}
									case 22:
					{
						case22(state, (T22)this.value);
						break;
					}
									case 23:
					{
						case23(state, (T23)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5,
System.Func<T6, TResult> case6,
System.Func<T7, TResult> case7,
System.Func<T8, TResult> case8,
System.Func<T9, TResult> case9,
System.Func<T10, TResult> case10,
System.Func<T11, TResult> case11,
System.Func<T12, TResult> case12,
System.Func<T13, TResult> case13,
System.Func<T14, TResult> case14,
System.Func<T15, TResult> case15,
System.Func<T16, TResult> case16,
System.Func<T17, TResult> case17,
System.Func<T18, TResult> case18,
System.Func<T19, TResult> case19,
System.Func<T20, TResult> case20,
System.Func<T21, TResult> case21,
System.Func<T22, TResult> case22,
System.Func<T23, TResult> case23)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									case 6:
					{
						return case6((T6)this.value);
					}
									case 7:
					{
						return case7((T7)this.value);
					}
									case 8:
					{
						return case8((T8)this.value);
					}
									case 9:
					{
						return case9((T9)this.value);
					}
									case 10:
					{
						return case10((T10)this.value);
					}
									case 11:
					{
						return case11((T11)this.value);
					}
									case 12:
					{
						return case12((T12)this.value);
					}
									case 13:
					{
						return case13((T13)this.value);
					}
									case 14:
					{
						return case14((T14)this.value);
					}
									case 15:
					{
						return case15((T15)this.value);
					}
									case 16:
					{
						return case16((T16)this.value);
					}
									case 17:
					{
						return case17((T17)this.value);
					}
									case 18:
					{
						return case18((T18)this.value);
					}
									case 19:
					{
						return case19((T19)this.value);
					}
									case 20:
					{
						return case20((T20)this.value);
					}
									case 21:
					{
						return case21((T21)this.value);
					}
									case 22:
					{
						return case22((T22)this.value);
					}
									case 23:
					{
						return case23((T23)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5,
System.Func<TState, T6, TResult> case6,
System.Func<TState, T7, TResult> case7,
System.Func<TState, T8, TResult> case8,
System.Func<TState, T9, TResult> case9,
System.Func<TState, T10, TResult> case10,
System.Func<TState, T11, TResult> case11,
System.Func<TState, T12, TResult> case12,
System.Func<TState, T13, TResult> case13,
System.Func<TState, T14, TResult> case14,
System.Func<TState, T15, TResult> case15,
System.Func<TState, T16, TResult> case16,
System.Func<TState, T17, TResult> case17,
System.Func<TState, T18, TResult> case18,
System.Func<TState, T19, TResult> case19,
System.Func<TState, T20, TResult> case20,
System.Func<TState, T21, TResult> case21,
System.Func<TState, T22, TResult> case22,
System.Func<TState, T23, TResult> case23)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									case 6:
					{
						return case6(state, (T6)this.value);
					}
									case 7:
					{
						return case7(state, (T7)this.value);
					}
									case 8:
					{
						return case8(state, (T8)this.value);
					}
									case 9:
					{
						return case9(state, (T9)this.value);
					}
									case 10:
					{
						return case10(state, (T10)this.value);
					}
									case 11:
					{
						return case11(state, (T11)this.value);
					}
									case 12:
					{
						return case12(state, (T12)this.value);
					}
									case 13:
					{
						return case13(state, (T13)this.value);
					}
									case 14:
					{
						return case14(state, (T14)this.value);
					}
									case 15:
					{
						return case15(state, (T15)this.value);
					}
									case 16:
					{
						return case16(state, (T16)this.value);
					}
									case 17:
					{
						return case17(state, (T17)this.value);
					}
									case 18:
					{
						return case18(state, (T18)this.value);
					}
									case 19:
					{
						return case19(state, (T19)this.value);
					}
									case 20:
					{
						return case20(state, (T20)this.value);
					}
									case 21:
					{
						return case21(state, (T21)this.value);
					}
									case 22:
					{
						return case22(state, (T22)this.value);
					}
									case 23:
					{
						return case23(state, (T23)this.value);
					}
									default:
						return defaultCase(state);
				}
			}

			public override bool Equals(object? other)
			{
				if (other is FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23> otherUnion)
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24> : IFlatBufferUnion
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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
					where T19 : notnull
					where T20 : notnull
					where T21 : notnull
					where T22 : notnull
					where T23 : notnull
					where T24 : notnull
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

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24> : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>

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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
					where T19 : notnull
					where T20 : notnull
					where T21 : notnull
					where T22 : notnull
					where T23 : notnull
					where T24 : notnull
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
				
			
			public FlatBufferUnion(T11 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 11;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T12 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 12;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T13 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 13;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T14 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 14;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T15 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 15;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T16 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 16;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T17 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 17;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T18 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 18;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T19 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 19;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T20 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 20;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T21 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 21;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T22 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 22;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T23 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 23;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T24 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 24;
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
				
			
			public T11 Item11
			{
				get 
				{
					if (this.discriminator == 11)
					{
						return (T11)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T11? item)
			{
				if (this.discriminator == 11)
				{
					item = (T11)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T12 Item12
			{
				get 
				{
					if (this.discriminator == 12)
					{
						return (T12)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T12? item)
			{
				if (this.discriminator == 12)
				{
					item = (T12)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T13 Item13
			{
				get 
				{
					if (this.discriminator == 13)
					{
						return (T13)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T13? item)
			{
				if (this.discriminator == 13)
				{
					item = (T13)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T14 Item14
			{
				get 
				{
					if (this.discriminator == 14)
					{
						return (T14)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T14? item)
			{
				if (this.discriminator == 14)
				{
					item = (T14)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T15 Item15
			{
				get 
				{
					if (this.discriminator == 15)
					{
						return (T15)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T15? item)
			{
				if (this.discriminator == 15)
				{
					item = (T15)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T16 Item16
			{
				get 
				{
					if (this.discriminator == 16)
					{
						return (T16)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T16? item)
			{
				if (this.discriminator == 16)
				{
					item = (T16)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T17 Item17
			{
				get 
				{
					if (this.discriminator == 17)
					{
						return (T17)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T17? item)
			{
				if (this.discriminator == 17)
				{
					item = (T17)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T18 Item18
			{
				get 
				{
					if (this.discriminator == 18)
					{
						return (T18)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T18? item)
			{
				if (this.discriminator == 18)
				{
					item = (T18)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T19 Item19
			{
				get 
				{
					if (this.discriminator == 19)
					{
						return (T19)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T19? item)
			{
				if (this.discriminator == 19)
				{
					item = (T19)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T20 Item20
			{
				get 
				{
					if (this.discriminator == 20)
					{
						return (T20)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T20? item)
			{
				if (this.discriminator == 20)
				{
					item = (T20)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T21 Item21
			{
				get 
				{
					if (this.discriminator == 21)
					{
						return (T21)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T21? item)
			{
				if (this.discriminator == 21)
				{
					item = (T21)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T22 Item22
			{
				get 
				{
					if (this.discriminator == 22)
					{
						return (T22)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T22? item)
			{
				if (this.discriminator == 22)
				{
					item = (T22)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T23 Item23
			{
				get 
				{
					if (this.discriminator == 23)
					{
						return (T23)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T23? item)
			{
				if (this.discriminator == 23)
				{
					item = (T23)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T24 Item24
			{
				get 
				{
					if (this.discriminator == 24)
					{
						return (T24)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T24? item)
			{
				if (this.discriminator == 24)
				{
					item = (T24)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5,
System.Action<T6> case6,
System.Action<T7> case7,
System.Action<T8> case8,
System.Action<T9> case9,
System.Action<T10> case10,
System.Action<T11> case11,
System.Action<T12> case12,
System.Action<T13> case13,
System.Action<T14> case14,
System.Action<T15> case15,
System.Action<T16> case16,
System.Action<T17> case17,
System.Action<T18> case18,
System.Action<T19> case19,
System.Action<T20> case20,
System.Action<T21> case21,
System.Action<T22> case22,
System.Action<T23> case23,
System.Action<T24> case24)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									case 6:
					{
						case6((T6)this.value);
						break;
					}
									case 7:
					{
						case7((T7)this.value);
						break;
					}
									case 8:
					{
						case8((T8)this.value);
						break;
					}
									case 9:
					{
						case9((T9)this.value);
						break;
					}
									case 10:
					{
						case10((T10)this.value);
						break;
					}
									case 11:
					{
						case11((T11)this.value);
						break;
					}
									case 12:
					{
						case12((T12)this.value);
						break;
					}
									case 13:
					{
						case13((T13)this.value);
						break;
					}
									case 14:
					{
						case14((T14)this.value);
						break;
					}
									case 15:
					{
						case15((T15)this.value);
						break;
					}
									case 16:
					{
						case16((T16)this.value);
						break;
					}
									case 17:
					{
						case17((T17)this.value);
						break;
					}
									case 18:
					{
						case18((T18)this.value);
						break;
					}
									case 19:
					{
						case19((T19)this.value);
						break;
					}
									case 20:
					{
						case20((T20)this.value);
						break;
					}
									case 21:
					{
						case21((T21)this.value);
						break;
					}
									case 22:
					{
						case22((T22)this.value);
						break;
					}
									case 23:
					{
						case23((T23)this.value);
						break;
					}
									case 24:
					{
						case24((T24)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5,
System.Action<TState, T6> case6,
System.Action<TState, T7> case7,
System.Action<TState, T8> case8,
System.Action<TState, T9> case9,
System.Action<TState, T10> case10,
System.Action<TState, T11> case11,
System.Action<TState, T12> case12,
System.Action<TState, T13> case13,
System.Action<TState, T14> case14,
System.Action<TState, T15> case15,
System.Action<TState, T16> case16,
System.Action<TState, T17> case17,
System.Action<TState, T18> case18,
System.Action<TState, T19> case19,
System.Action<TState, T20> case20,
System.Action<TState, T21> case21,
System.Action<TState, T22> case22,
System.Action<TState, T23> case23,
System.Action<TState, T24> case24)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									case 6:
					{
						case6(state, (T6)this.value);
						break;
					}
									case 7:
					{
						case7(state, (T7)this.value);
						break;
					}
									case 8:
					{
						case8(state, (T8)this.value);
						break;
					}
									case 9:
					{
						case9(state, (T9)this.value);
						break;
					}
									case 10:
					{
						case10(state, (T10)this.value);
						break;
					}
									case 11:
					{
						case11(state, (T11)this.value);
						break;
					}
									case 12:
					{
						case12(state, (T12)this.value);
						break;
					}
									case 13:
					{
						case13(state, (T13)this.value);
						break;
					}
									case 14:
					{
						case14(state, (T14)this.value);
						break;
					}
									case 15:
					{
						case15(state, (T15)this.value);
						break;
					}
									case 16:
					{
						case16(state, (T16)this.value);
						break;
					}
									case 17:
					{
						case17(state, (T17)this.value);
						break;
					}
									case 18:
					{
						case18(state, (T18)this.value);
						break;
					}
									case 19:
					{
						case19(state, (T19)this.value);
						break;
					}
									case 20:
					{
						case20(state, (T20)this.value);
						break;
					}
									case 21:
					{
						case21(state, (T21)this.value);
						break;
					}
									case 22:
					{
						case22(state, (T22)this.value);
						break;
					}
									case 23:
					{
						case23(state, (T23)this.value);
						break;
					}
									case 24:
					{
						case24(state, (T24)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5,
System.Func<T6, TResult> case6,
System.Func<T7, TResult> case7,
System.Func<T8, TResult> case8,
System.Func<T9, TResult> case9,
System.Func<T10, TResult> case10,
System.Func<T11, TResult> case11,
System.Func<T12, TResult> case12,
System.Func<T13, TResult> case13,
System.Func<T14, TResult> case14,
System.Func<T15, TResult> case15,
System.Func<T16, TResult> case16,
System.Func<T17, TResult> case17,
System.Func<T18, TResult> case18,
System.Func<T19, TResult> case19,
System.Func<T20, TResult> case20,
System.Func<T21, TResult> case21,
System.Func<T22, TResult> case22,
System.Func<T23, TResult> case23,
System.Func<T24, TResult> case24)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									case 6:
					{
						return case6((T6)this.value);
					}
									case 7:
					{
						return case7((T7)this.value);
					}
									case 8:
					{
						return case8((T8)this.value);
					}
									case 9:
					{
						return case9((T9)this.value);
					}
									case 10:
					{
						return case10((T10)this.value);
					}
									case 11:
					{
						return case11((T11)this.value);
					}
									case 12:
					{
						return case12((T12)this.value);
					}
									case 13:
					{
						return case13((T13)this.value);
					}
									case 14:
					{
						return case14((T14)this.value);
					}
									case 15:
					{
						return case15((T15)this.value);
					}
									case 16:
					{
						return case16((T16)this.value);
					}
									case 17:
					{
						return case17((T17)this.value);
					}
									case 18:
					{
						return case18((T18)this.value);
					}
									case 19:
					{
						return case19((T19)this.value);
					}
									case 20:
					{
						return case20((T20)this.value);
					}
									case 21:
					{
						return case21((T21)this.value);
					}
									case 22:
					{
						return case22((T22)this.value);
					}
									case 23:
					{
						return case23((T23)this.value);
					}
									case 24:
					{
						return case24((T24)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5,
System.Func<TState, T6, TResult> case6,
System.Func<TState, T7, TResult> case7,
System.Func<TState, T8, TResult> case8,
System.Func<TState, T9, TResult> case9,
System.Func<TState, T10, TResult> case10,
System.Func<TState, T11, TResult> case11,
System.Func<TState, T12, TResult> case12,
System.Func<TState, T13, TResult> case13,
System.Func<TState, T14, TResult> case14,
System.Func<TState, T15, TResult> case15,
System.Func<TState, T16, TResult> case16,
System.Func<TState, T17, TResult> case17,
System.Func<TState, T18, TResult> case18,
System.Func<TState, T19, TResult> case19,
System.Func<TState, T20, TResult> case20,
System.Func<TState, T21, TResult> case21,
System.Func<TState, T22, TResult> case22,
System.Func<TState, T23, TResult> case23,
System.Func<TState, T24, TResult> case24)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									case 6:
					{
						return case6(state, (T6)this.value);
					}
									case 7:
					{
						return case7(state, (T7)this.value);
					}
									case 8:
					{
						return case8(state, (T8)this.value);
					}
									case 9:
					{
						return case9(state, (T9)this.value);
					}
									case 10:
					{
						return case10(state, (T10)this.value);
					}
									case 11:
					{
						return case11(state, (T11)this.value);
					}
									case 12:
					{
						return case12(state, (T12)this.value);
					}
									case 13:
					{
						return case13(state, (T13)this.value);
					}
									case 14:
					{
						return case14(state, (T14)this.value);
					}
									case 15:
					{
						return case15(state, (T15)this.value);
					}
									case 16:
					{
						return case16(state, (T16)this.value);
					}
									case 17:
					{
						return case17(state, (T17)this.value);
					}
									case 18:
					{
						return case18(state, (T18)this.value);
					}
									case 19:
					{
						return case19(state, (T19)this.value);
					}
									case 20:
					{
						return case20(state, (T20)this.value);
					}
									case 21:
					{
						return case21(state, (T21)this.value);
					}
									case 22:
					{
						return case22(state, (T22)this.value);
					}
									case 23:
					{
						return case23(state, (T23)this.value);
					}
									case 24:
					{
						return case24(state, (T24)this.value);
					}
									default:
						return defaultCase(state);
				}
			}

			public override bool Equals(object? other)
			{
				if (other is FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24> otherUnion)
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25> : IFlatBufferUnion
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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
					where T19 : notnull
					where T20 : notnull
					where T21 : notnull
					where T22 : notnull
					where T23 : notnull
					where T24 : notnull
					where T25 : notnull
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

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25> : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25>

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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
					where T19 : notnull
					where T20 : notnull
					where T21 : notnull
					where T22 : notnull
					where T23 : notnull
					where T24 : notnull
					where T25 : notnull
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
				
			
			public FlatBufferUnion(T11 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 11;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T12 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 12;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T13 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 13;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T14 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 14;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T15 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 15;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T16 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 16;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T17 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 17;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T18 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 18;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T19 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 19;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T20 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 20;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T21 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 21;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T22 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 22;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T23 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 23;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T24 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 24;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T25 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 25;
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
				
			
			public T11 Item11
			{
				get 
				{
					if (this.discriminator == 11)
					{
						return (T11)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T11? item)
			{
				if (this.discriminator == 11)
				{
					item = (T11)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T12 Item12
			{
				get 
				{
					if (this.discriminator == 12)
					{
						return (T12)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T12? item)
			{
				if (this.discriminator == 12)
				{
					item = (T12)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T13 Item13
			{
				get 
				{
					if (this.discriminator == 13)
					{
						return (T13)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T13? item)
			{
				if (this.discriminator == 13)
				{
					item = (T13)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T14 Item14
			{
				get 
				{
					if (this.discriminator == 14)
					{
						return (T14)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T14? item)
			{
				if (this.discriminator == 14)
				{
					item = (T14)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T15 Item15
			{
				get 
				{
					if (this.discriminator == 15)
					{
						return (T15)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T15? item)
			{
				if (this.discriminator == 15)
				{
					item = (T15)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T16 Item16
			{
				get 
				{
					if (this.discriminator == 16)
					{
						return (T16)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T16? item)
			{
				if (this.discriminator == 16)
				{
					item = (T16)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T17 Item17
			{
				get 
				{
					if (this.discriminator == 17)
					{
						return (T17)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T17? item)
			{
				if (this.discriminator == 17)
				{
					item = (T17)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T18 Item18
			{
				get 
				{
					if (this.discriminator == 18)
					{
						return (T18)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T18? item)
			{
				if (this.discriminator == 18)
				{
					item = (T18)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T19 Item19
			{
				get 
				{
					if (this.discriminator == 19)
					{
						return (T19)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T19? item)
			{
				if (this.discriminator == 19)
				{
					item = (T19)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T20 Item20
			{
				get 
				{
					if (this.discriminator == 20)
					{
						return (T20)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T20? item)
			{
				if (this.discriminator == 20)
				{
					item = (T20)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T21 Item21
			{
				get 
				{
					if (this.discriminator == 21)
					{
						return (T21)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T21? item)
			{
				if (this.discriminator == 21)
				{
					item = (T21)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T22 Item22
			{
				get 
				{
					if (this.discriminator == 22)
					{
						return (T22)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T22? item)
			{
				if (this.discriminator == 22)
				{
					item = (T22)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T23 Item23
			{
				get 
				{
					if (this.discriminator == 23)
					{
						return (T23)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T23? item)
			{
				if (this.discriminator == 23)
				{
					item = (T23)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T24 Item24
			{
				get 
				{
					if (this.discriminator == 24)
					{
						return (T24)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T24? item)
			{
				if (this.discriminator == 24)
				{
					item = (T24)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T25 Item25
			{
				get 
				{
					if (this.discriminator == 25)
					{
						return (T25)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T25? item)
			{
				if (this.discriminator == 25)
				{
					item = (T25)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5,
System.Action<T6> case6,
System.Action<T7> case7,
System.Action<T8> case8,
System.Action<T9> case9,
System.Action<T10> case10,
System.Action<T11> case11,
System.Action<T12> case12,
System.Action<T13> case13,
System.Action<T14> case14,
System.Action<T15> case15,
System.Action<T16> case16,
System.Action<T17> case17,
System.Action<T18> case18,
System.Action<T19> case19,
System.Action<T20> case20,
System.Action<T21> case21,
System.Action<T22> case22,
System.Action<T23> case23,
System.Action<T24> case24,
System.Action<T25> case25)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									case 6:
					{
						case6((T6)this.value);
						break;
					}
									case 7:
					{
						case7((T7)this.value);
						break;
					}
									case 8:
					{
						case8((T8)this.value);
						break;
					}
									case 9:
					{
						case9((T9)this.value);
						break;
					}
									case 10:
					{
						case10((T10)this.value);
						break;
					}
									case 11:
					{
						case11((T11)this.value);
						break;
					}
									case 12:
					{
						case12((T12)this.value);
						break;
					}
									case 13:
					{
						case13((T13)this.value);
						break;
					}
									case 14:
					{
						case14((T14)this.value);
						break;
					}
									case 15:
					{
						case15((T15)this.value);
						break;
					}
									case 16:
					{
						case16((T16)this.value);
						break;
					}
									case 17:
					{
						case17((T17)this.value);
						break;
					}
									case 18:
					{
						case18((T18)this.value);
						break;
					}
									case 19:
					{
						case19((T19)this.value);
						break;
					}
									case 20:
					{
						case20((T20)this.value);
						break;
					}
									case 21:
					{
						case21((T21)this.value);
						break;
					}
									case 22:
					{
						case22((T22)this.value);
						break;
					}
									case 23:
					{
						case23((T23)this.value);
						break;
					}
									case 24:
					{
						case24((T24)this.value);
						break;
					}
									case 25:
					{
						case25((T25)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5,
System.Action<TState, T6> case6,
System.Action<TState, T7> case7,
System.Action<TState, T8> case8,
System.Action<TState, T9> case9,
System.Action<TState, T10> case10,
System.Action<TState, T11> case11,
System.Action<TState, T12> case12,
System.Action<TState, T13> case13,
System.Action<TState, T14> case14,
System.Action<TState, T15> case15,
System.Action<TState, T16> case16,
System.Action<TState, T17> case17,
System.Action<TState, T18> case18,
System.Action<TState, T19> case19,
System.Action<TState, T20> case20,
System.Action<TState, T21> case21,
System.Action<TState, T22> case22,
System.Action<TState, T23> case23,
System.Action<TState, T24> case24,
System.Action<TState, T25> case25)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									case 6:
					{
						case6(state, (T6)this.value);
						break;
					}
									case 7:
					{
						case7(state, (T7)this.value);
						break;
					}
									case 8:
					{
						case8(state, (T8)this.value);
						break;
					}
									case 9:
					{
						case9(state, (T9)this.value);
						break;
					}
									case 10:
					{
						case10(state, (T10)this.value);
						break;
					}
									case 11:
					{
						case11(state, (T11)this.value);
						break;
					}
									case 12:
					{
						case12(state, (T12)this.value);
						break;
					}
									case 13:
					{
						case13(state, (T13)this.value);
						break;
					}
									case 14:
					{
						case14(state, (T14)this.value);
						break;
					}
									case 15:
					{
						case15(state, (T15)this.value);
						break;
					}
									case 16:
					{
						case16(state, (T16)this.value);
						break;
					}
									case 17:
					{
						case17(state, (T17)this.value);
						break;
					}
									case 18:
					{
						case18(state, (T18)this.value);
						break;
					}
									case 19:
					{
						case19(state, (T19)this.value);
						break;
					}
									case 20:
					{
						case20(state, (T20)this.value);
						break;
					}
									case 21:
					{
						case21(state, (T21)this.value);
						break;
					}
									case 22:
					{
						case22(state, (T22)this.value);
						break;
					}
									case 23:
					{
						case23(state, (T23)this.value);
						break;
					}
									case 24:
					{
						case24(state, (T24)this.value);
						break;
					}
									case 25:
					{
						case25(state, (T25)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5,
System.Func<T6, TResult> case6,
System.Func<T7, TResult> case7,
System.Func<T8, TResult> case8,
System.Func<T9, TResult> case9,
System.Func<T10, TResult> case10,
System.Func<T11, TResult> case11,
System.Func<T12, TResult> case12,
System.Func<T13, TResult> case13,
System.Func<T14, TResult> case14,
System.Func<T15, TResult> case15,
System.Func<T16, TResult> case16,
System.Func<T17, TResult> case17,
System.Func<T18, TResult> case18,
System.Func<T19, TResult> case19,
System.Func<T20, TResult> case20,
System.Func<T21, TResult> case21,
System.Func<T22, TResult> case22,
System.Func<T23, TResult> case23,
System.Func<T24, TResult> case24,
System.Func<T25, TResult> case25)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									case 6:
					{
						return case6((T6)this.value);
					}
									case 7:
					{
						return case7((T7)this.value);
					}
									case 8:
					{
						return case8((T8)this.value);
					}
									case 9:
					{
						return case9((T9)this.value);
					}
									case 10:
					{
						return case10((T10)this.value);
					}
									case 11:
					{
						return case11((T11)this.value);
					}
									case 12:
					{
						return case12((T12)this.value);
					}
									case 13:
					{
						return case13((T13)this.value);
					}
									case 14:
					{
						return case14((T14)this.value);
					}
									case 15:
					{
						return case15((T15)this.value);
					}
									case 16:
					{
						return case16((T16)this.value);
					}
									case 17:
					{
						return case17((T17)this.value);
					}
									case 18:
					{
						return case18((T18)this.value);
					}
									case 19:
					{
						return case19((T19)this.value);
					}
									case 20:
					{
						return case20((T20)this.value);
					}
									case 21:
					{
						return case21((T21)this.value);
					}
									case 22:
					{
						return case22((T22)this.value);
					}
									case 23:
					{
						return case23((T23)this.value);
					}
									case 24:
					{
						return case24((T24)this.value);
					}
									case 25:
					{
						return case25((T25)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5,
System.Func<TState, T6, TResult> case6,
System.Func<TState, T7, TResult> case7,
System.Func<TState, T8, TResult> case8,
System.Func<TState, T9, TResult> case9,
System.Func<TState, T10, TResult> case10,
System.Func<TState, T11, TResult> case11,
System.Func<TState, T12, TResult> case12,
System.Func<TState, T13, TResult> case13,
System.Func<TState, T14, TResult> case14,
System.Func<TState, T15, TResult> case15,
System.Func<TState, T16, TResult> case16,
System.Func<TState, T17, TResult> case17,
System.Func<TState, T18, TResult> case18,
System.Func<TState, T19, TResult> case19,
System.Func<TState, T20, TResult> case20,
System.Func<TState, T21, TResult> case21,
System.Func<TState, T22, TResult> case22,
System.Func<TState, T23, TResult> case23,
System.Func<TState, T24, TResult> case24,
System.Func<TState, T25, TResult> case25)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									case 6:
					{
						return case6(state, (T6)this.value);
					}
									case 7:
					{
						return case7(state, (T7)this.value);
					}
									case 8:
					{
						return case8(state, (T8)this.value);
					}
									case 9:
					{
						return case9(state, (T9)this.value);
					}
									case 10:
					{
						return case10(state, (T10)this.value);
					}
									case 11:
					{
						return case11(state, (T11)this.value);
					}
									case 12:
					{
						return case12(state, (T12)this.value);
					}
									case 13:
					{
						return case13(state, (T13)this.value);
					}
									case 14:
					{
						return case14(state, (T14)this.value);
					}
									case 15:
					{
						return case15(state, (T15)this.value);
					}
									case 16:
					{
						return case16(state, (T16)this.value);
					}
									case 17:
					{
						return case17(state, (T17)this.value);
					}
									case 18:
					{
						return case18(state, (T18)this.value);
					}
									case 19:
					{
						return case19(state, (T19)this.value);
					}
									case 20:
					{
						return case20(state, (T20)this.value);
					}
									case 21:
					{
						return case21(state, (T21)this.value);
					}
									case 22:
					{
						return case22(state, (T22)this.value);
					}
									case 23:
					{
						return case23(state, (T23)this.value);
					}
									case 24:
					{
						return case24(state, (T24)this.value);
					}
									case 25:
					{
						return case25(state, (T25)this.value);
					}
									default:
						return defaultCase(state);
				}
			}

			public override bool Equals(object? other)
			{
				if (other is FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25> otherUnion)
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26> : IFlatBufferUnion
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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
					where T19 : notnull
					where T20 : notnull
					where T21 : notnull
					where T22 : notnull
					where T23 : notnull
					where T24 : notnull
					where T25 : notnull
					where T26 : notnull
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

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26> : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26>

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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
					where T19 : notnull
					where T20 : notnull
					where T21 : notnull
					where T22 : notnull
					where T23 : notnull
					where T24 : notnull
					where T25 : notnull
					where T26 : notnull
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
				
			
			public FlatBufferUnion(T11 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 11;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T12 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 12;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T13 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 13;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T14 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 14;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T15 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 15;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T16 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 16;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T17 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 17;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T18 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 18;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T19 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 19;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T20 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 20;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T21 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 21;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T22 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 22;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T23 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 23;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T24 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 24;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T25 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 25;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T26 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 26;
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
				
			
			public T11 Item11
			{
				get 
				{
					if (this.discriminator == 11)
					{
						return (T11)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T11? item)
			{
				if (this.discriminator == 11)
				{
					item = (T11)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T12 Item12
			{
				get 
				{
					if (this.discriminator == 12)
					{
						return (T12)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T12? item)
			{
				if (this.discriminator == 12)
				{
					item = (T12)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T13 Item13
			{
				get 
				{
					if (this.discriminator == 13)
					{
						return (T13)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T13? item)
			{
				if (this.discriminator == 13)
				{
					item = (T13)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T14 Item14
			{
				get 
				{
					if (this.discriminator == 14)
					{
						return (T14)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T14? item)
			{
				if (this.discriminator == 14)
				{
					item = (T14)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T15 Item15
			{
				get 
				{
					if (this.discriminator == 15)
					{
						return (T15)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T15? item)
			{
				if (this.discriminator == 15)
				{
					item = (T15)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T16 Item16
			{
				get 
				{
					if (this.discriminator == 16)
					{
						return (T16)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T16? item)
			{
				if (this.discriminator == 16)
				{
					item = (T16)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T17 Item17
			{
				get 
				{
					if (this.discriminator == 17)
					{
						return (T17)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T17? item)
			{
				if (this.discriminator == 17)
				{
					item = (T17)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T18 Item18
			{
				get 
				{
					if (this.discriminator == 18)
					{
						return (T18)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T18? item)
			{
				if (this.discriminator == 18)
				{
					item = (T18)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T19 Item19
			{
				get 
				{
					if (this.discriminator == 19)
					{
						return (T19)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T19? item)
			{
				if (this.discriminator == 19)
				{
					item = (T19)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T20 Item20
			{
				get 
				{
					if (this.discriminator == 20)
					{
						return (T20)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T20? item)
			{
				if (this.discriminator == 20)
				{
					item = (T20)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T21 Item21
			{
				get 
				{
					if (this.discriminator == 21)
					{
						return (T21)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T21? item)
			{
				if (this.discriminator == 21)
				{
					item = (T21)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T22 Item22
			{
				get 
				{
					if (this.discriminator == 22)
					{
						return (T22)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T22? item)
			{
				if (this.discriminator == 22)
				{
					item = (T22)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T23 Item23
			{
				get 
				{
					if (this.discriminator == 23)
					{
						return (T23)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T23? item)
			{
				if (this.discriminator == 23)
				{
					item = (T23)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T24 Item24
			{
				get 
				{
					if (this.discriminator == 24)
					{
						return (T24)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T24? item)
			{
				if (this.discriminator == 24)
				{
					item = (T24)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T25 Item25
			{
				get 
				{
					if (this.discriminator == 25)
					{
						return (T25)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T25? item)
			{
				if (this.discriminator == 25)
				{
					item = (T25)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T26 Item26
			{
				get 
				{
					if (this.discriminator == 26)
					{
						return (T26)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T26? item)
			{
				if (this.discriminator == 26)
				{
					item = (T26)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5,
System.Action<T6> case6,
System.Action<T7> case7,
System.Action<T8> case8,
System.Action<T9> case9,
System.Action<T10> case10,
System.Action<T11> case11,
System.Action<T12> case12,
System.Action<T13> case13,
System.Action<T14> case14,
System.Action<T15> case15,
System.Action<T16> case16,
System.Action<T17> case17,
System.Action<T18> case18,
System.Action<T19> case19,
System.Action<T20> case20,
System.Action<T21> case21,
System.Action<T22> case22,
System.Action<T23> case23,
System.Action<T24> case24,
System.Action<T25> case25,
System.Action<T26> case26)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									case 6:
					{
						case6((T6)this.value);
						break;
					}
									case 7:
					{
						case7((T7)this.value);
						break;
					}
									case 8:
					{
						case8((T8)this.value);
						break;
					}
									case 9:
					{
						case9((T9)this.value);
						break;
					}
									case 10:
					{
						case10((T10)this.value);
						break;
					}
									case 11:
					{
						case11((T11)this.value);
						break;
					}
									case 12:
					{
						case12((T12)this.value);
						break;
					}
									case 13:
					{
						case13((T13)this.value);
						break;
					}
									case 14:
					{
						case14((T14)this.value);
						break;
					}
									case 15:
					{
						case15((T15)this.value);
						break;
					}
									case 16:
					{
						case16((T16)this.value);
						break;
					}
									case 17:
					{
						case17((T17)this.value);
						break;
					}
									case 18:
					{
						case18((T18)this.value);
						break;
					}
									case 19:
					{
						case19((T19)this.value);
						break;
					}
									case 20:
					{
						case20((T20)this.value);
						break;
					}
									case 21:
					{
						case21((T21)this.value);
						break;
					}
									case 22:
					{
						case22((T22)this.value);
						break;
					}
									case 23:
					{
						case23((T23)this.value);
						break;
					}
									case 24:
					{
						case24((T24)this.value);
						break;
					}
									case 25:
					{
						case25((T25)this.value);
						break;
					}
									case 26:
					{
						case26((T26)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5,
System.Action<TState, T6> case6,
System.Action<TState, T7> case7,
System.Action<TState, T8> case8,
System.Action<TState, T9> case9,
System.Action<TState, T10> case10,
System.Action<TState, T11> case11,
System.Action<TState, T12> case12,
System.Action<TState, T13> case13,
System.Action<TState, T14> case14,
System.Action<TState, T15> case15,
System.Action<TState, T16> case16,
System.Action<TState, T17> case17,
System.Action<TState, T18> case18,
System.Action<TState, T19> case19,
System.Action<TState, T20> case20,
System.Action<TState, T21> case21,
System.Action<TState, T22> case22,
System.Action<TState, T23> case23,
System.Action<TState, T24> case24,
System.Action<TState, T25> case25,
System.Action<TState, T26> case26)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									case 6:
					{
						case6(state, (T6)this.value);
						break;
					}
									case 7:
					{
						case7(state, (T7)this.value);
						break;
					}
									case 8:
					{
						case8(state, (T8)this.value);
						break;
					}
									case 9:
					{
						case9(state, (T9)this.value);
						break;
					}
									case 10:
					{
						case10(state, (T10)this.value);
						break;
					}
									case 11:
					{
						case11(state, (T11)this.value);
						break;
					}
									case 12:
					{
						case12(state, (T12)this.value);
						break;
					}
									case 13:
					{
						case13(state, (T13)this.value);
						break;
					}
									case 14:
					{
						case14(state, (T14)this.value);
						break;
					}
									case 15:
					{
						case15(state, (T15)this.value);
						break;
					}
									case 16:
					{
						case16(state, (T16)this.value);
						break;
					}
									case 17:
					{
						case17(state, (T17)this.value);
						break;
					}
									case 18:
					{
						case18(state, (T18)this.value);
						break;
					}
									case 19:
					{
						case19(state, (T19)this.value);
						break;
					}
									case 20:
					{
						case20(state, (T20)this.value);
						break;
					}
									case 21:
					{
						case21(state, (T21)this.value);
						break;
					}
									case 22:
					{
						case22(state, (T22)this.value);
						break;
					}
									case 23:
					{
						case23(state, (T23)this.value);
						break;
					}
									case 24:
					{
						case24(state, (T24)this.value);
						break;
					}
									case 25:
					{
						case25(state, (T25)this.value);
						break;
					}
									case 26:
					{
						case26(state, (T26)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5,
System.Func<T6, TResult> case6,
System.Func<T7, TResult> case7,
System.Func<T8, TResult> case8,
System.Func<T9, TResult> case9,
System.Func<T10, TResult> case10,
System.Func<T11, TResult> case11,
System.Func<T12, TResult> case12,
System.Func<T13, TResult> case13,
System.Func<T14, TResult> case14,
System.Func<T15, TResult> case15,
System.Func<T16, TResult> case16,
System.Func<T17, TResult> case17,
System.Func<T18, TResult> case18,
System.Func<T19, TResult> case19,
System.Func<T20, TResult> case20,
System.Func<T21, TResult> case21,
System.Func<T22, TResult> case22,
System.Func<T23, TResult> case23,
System.Func<T24, TResult> case24,
System.Func<T25, TResult> case25,
System.Func<T26, TResult> case26)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									case 6:
					{
						return case6((T6)this.value);
					}
									case 7:
					{
						return case7((T7)this.value);
					}
									case 8:
					{
						return case8((T8)this.value);
					}
									case 9:
					{
						return case9((T9)this.value);
					}
									case 10:
					{
						return case10((T10)this.value);
					}
									case 11:
					{
						return case11((T11)this.value);
					}
									case 12:
					{
						return case12((T12)this.value);
					}
									case 13:
					{
						return case13((T13)this.value);
					}
									case 14:
					{
						return case14((T14)this.value);
					}
									case 15:
					{
						return case15((T15)this.value);
					}
									case 16:
					{
						return case16((T16)this.value);
					}
									case 17:
					{
						return case17((T17)this.value);
					}
									case 18:
					{
						return case18((T18)this.value);
					}
									case 19:
					{
						return case19((T19)this.value);
					}
									case 20:
					{
						return case20((T20)this.value);
					}
									case 21:
					{
						return case21((T21)this.value);
					}
									case 22:
					{
						return case22((T22)this.value);
					}
									case 23:
					{
						return case23((T23)this.value);
					}
									case 24:
					{
						return case24((T24)this.value);
					}
									case 25:
					{
						return case25((T25)this.value);
					}
									case 26:
					{
						return case26((T26)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5,
System.Func<TState, T6, TResult> case6,
System.Func<TState, T7, TResult> case7,
System.Func<TState, T8, TResult> case8,
System.Func<TState, T9, TResult> case9,
System.Func<TState, T10, TResult> case10,
System.Func<TState, T11, TResult> case11,
System.Func<TState, T12, TResult> case12,
System.Func<TState, T13, TResult> case13,
System.Func<TState, T14, TResult> case14,
System.Func<TState, T15, TResult> case15,
System.Func<TState, T16, TResult> case16,
System.Func<TState, T17, TResult> case17,
System.Func<TState, T18, TResult> case18,
System.Func<TState, T19, TResult> case19,
System.Func<TState, T20, TResult> case20,
System.Func<TState, T21, TResult> case21,
System.Func<TState, T22, TResult> case22,
System.Func<TState, T23, TResult> case23,
System.Func<TState, T24, TResult> case24,
System.Func<TState, T25, TResult> case25,
System.Func<TState, T26, TResult> case26)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									case 6:
					{
						return case6(state, (T6)this.value);
					}
									case 7:
					{
						return case7(state, (T7)this.value);
					}
									case 8:
					{
						return case8(state, (T8)this.value);
					}
									case 9:
					{
						return case9(state, (T9)this.value);
					}
									case 10:
					{
						return case10(state, (T10)this.value);
					}
									case 11:
					{
						return case11(state, (T11)this.value);
					}
									case 12:
					{
						return case12(state, (T12)this.value);
					}
									case 13:
					{
						return case13(state, (T13)this.value);
					}
									case 14:
					{
						return case14(state, (T14)this.value);
					}
									case 15:
					{
						return case15(state, (T15)this.value);
					}
									case 16:
					{
						return case16(state, (T16)this.value);
					}
									case 17:
					{
						return case17(state, (T17)this.value);
					}
									case 18:
					{
						return case18(state, (T18)this.value);
					}
									case 19:
					{
						return case19(state, (T19)this.value);
					}
									case 20:
					{
						return case20(state, (T20)this.value);
					}
									case 21:
					{
						return case21(state, (T21)this.value);
					}
									case 22:
					{
						return case22(state, (T22)this.value);
					}
									case 23:
					{
						return case23(state, (T23)this.value);
					}
									case 24:
					{
						return case24(state, (T24)this.value);
					}
									case 25:
					{
						return case25(state, (T25)this.value);
					}
									case 26:
					{
						return case26(state, (T26)this.value);
					}
									default:
						return defaultCase(state);
				}
			}

			public override bool Equals(object? other)
			{
				if (other is FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26> otherUnion)
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27> : IFlatBufferUnion
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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
					where T19 : notnull
					where T20 : notnull
					where T21 : notnull
					where T22 : notnull
					where T23 : notnull
					where T24 : notnull
					where T25 : notnull
					where T26 : notnull
					where T27 : notnull
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

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27> : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27>

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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
					where T19 : notnull
					where T20 : notnull
					where T21 : notnull
					where T22 : notnull
					where T23 : notnull
					where T24 : notnull
					where T25 : notnull
					where T26 : notnull
					where T27 : notnull
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
				
			
			public FlatBufferUnion(T11 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 11;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T12 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 12;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T13 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 13;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T14 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 14;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T15 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 15;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T16 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 16;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T17 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 17;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T18 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 18;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T19 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 19;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T20 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 20;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T21 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 21;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T22 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 22;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T23 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 23;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T24 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 24;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T25 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 25;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T26 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 26;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T27 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 27;
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
				
			
			public T11 Item11
			{
				get 
				{
					if (this.discriminator == 11)
					{
						return (T11)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T11? item)
			{
				if (this.discriminator == 11)
				{
					item = (T11)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T12 Item12
			{
				get 
				{
					if (this.discriminator == 12)
					{
						return (T12)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T12? item)
			{
				if (this.discriminator == 12)
				{
					item = (T12)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T13 Item13
			{
				get 
				{
					if (this.discriminator == 13)
					{
						return (T13)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T13? item)
			{
				if (this.discriminator == 13)
				{
					item = (T13)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T14 Item14
			{
				get 
				{
					if (this.discriminator == 14)
					{
						return (T14)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T14? item)
			{
				if (this.discriminator == 14)
				{
					item = (T14)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T15 Item15
			{
				get 
				{
					if (this.discriminator == 15)
					{
						return (T15)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T15? item)
			{
				if (this.discriminator == 15)
				{
					item = (T15)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T16 Item16
			{
				get 
				{
					if (this.discriminator == 16)
					{
						return (T16)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T16? item)
			{
				if (this.discriminator == 16)
				{
					item = (T16)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T17 Item17
			{
				get 
				{
					if (this.discriminator == 17)
					{
						return (T17)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T17? item)
			{
				if (this.discriminator == 17)
				{
					item = (T17)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T18 Item18
			{
				get 
				{
					if (this.discriminator == 18)
					{
						return (T18)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T18? item)
			{
				if (this.discriminator == 18)
				{
					item = (T18)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T19 Item19
			{
				get 
				{
					if (this.discriminator == 19)
					{
						return (T19)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T19? item)
			{
				if (this.discriminator == 19)
				{
					item = (T19)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T20 Item20
			{
				get 
				{
					if (this.discriminator == 20)
					{
						return (T20)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T20? item)
			{
				if (this.discriminator == 20)
				{
					item = (T20)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T21 Item21
			{
				get 
				{
					if (this.discriminator == 21)
					{
						return (T21)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T21? item)
			{
				if (this.discriminator == 21)
				{
					item = (T21)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T22 Item22
			{
				get 
				{
					if (this.discriminator == 22)
					{
						return (T22)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T22? item)
			{
				if (this.discriminator == 22)
				{
					item = (T22)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T23 Item23
			{
				get 
				{
					if (this.discriminator == 23)
					{
						return (T23)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T23? item)
			{
				if (this.discriminator == 23)
				{
					item = (T23)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T24 Item24
			{
				get 
				{
					if (this.discriminator == 24)
					{
						return (T24)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T24? item)
			{
				if (this.discriminator == 24)
				{
					item = (T24)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T25 Item25
			{
				get 
				{
					if (this.discriminator == 25)
					{
						return (T25)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T25? item)
			{
				if (this.discriminator == 25)
				{
					item = (T25)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T26 Item26
			{
				get 
				{
					if (this.discriminator == 26)
					{
						return (T26)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T26? item)
			{
				if (this.discriminator == 26)
				{
					item = (T26)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T27 Item27
			{
				get 
				{
					if (this.discriminator == 27)
					{
						return (T27)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T27? item)
			{
				if (this.discriminator == 27)
				{
					item = (T27)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5,
System.Action<T6> case6,
System.Action<T7> case7,
System.Action<T8> case8,
System.Action<T9> case9,
System.Action<T10> case10,
System.Action<T11> case11,
System.Action<T12> case12,
System.Action<T13> case13,
System.Action<T14> case14,
System.Action<T15> case15,
System.Action<T16> case16,
System.Action<T17> case17,
System.Action<T18> case18,
System.Action<T19> case19,
System.Action<T20> case20,
System.Action<T21> case21,
System.Action<T22> case22,
System.Action<T23> case23,
System.Action<T24> case24,
System.Action<T25> case25,
System.Action<T26> case26,
System.Action<T27> case27)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									case 6:
					{
						case6((T6)this.value);
						break;
					}
									case 7:
					{
						case7((T7)this.value);
						break;
					}
									case 8:
					{
						case8((T8)this.value);
						break;
					}
									case 9:
					{
						case9((T9)this.value);
						break;
					}
									case 10:
					{
						case10((T10)this.value);
						break;
					}
									case 11:
					{
						case11((T11)this.value);
						break;
					}
									case 12:
					{
						case12((T12)this.value);
						break;
					}
									case 13:
					{
						case13((T13)this.value);
						break;
					}
									case 14:
					{
						case14((T14)this.value);
						break;
					}
									case 15:
					{
						case15((T15)this.value);
						break;
					}
									case 16:
					{
						case16((T16)this.value);
						break;
					}
									case 17:
					{
						case17((T17)this.value);
						break;
					}
									case 18:
					{
						case18((T18)this.value);
						break;
					}
									case 19:
					{
						case19((T19)this.value);
						break;
					}
									case 20:
					{
						case20((T20)this.value);
						break;
					}
									case 21:
					{
						case21((T21)this.value);
						break;
					}
									case 22:
					{
						case22((T22)this.value);
						break;
					}
									case 23:
					{
						case23((T23)this.value);
						break;
					}
									case 24:
					{
						case24((T24)this.value);
						break;
					}
									case 25:
					{
						case25((T25)this.value);
						break;
					}
									case 26:
					{
						case26((T26)this.value);
						break;
					}
									case 27:
					{
						case27((T27)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5,
System.Action<TState, T6> case6,
System.Action<TState, T7> case7,
System.Action<TState, T8> case8,
System.Action<TState, T9> case9,
System.Action<TState, T10> case10,
System.Action<TState, T11> case11,
System.Action<TState, T12> case12,
System.Action<TState, T13> case13,
System.Action<TState, T14> case14,
System.Action<TState, T15> case15,
System.Action<TState, T16> case16,
System.Action<TState, T17> case17,
System.Action<TState, T18> case18,
System.Action<TState, T19> case19,
System.Action<TState, T20> case20,
System.Action<TState, T21> case21,
System.Action<TState, T22> case22,
System.Action<TState, T23> case23,
System.Action<TState, T24> case24,
System.Action<TState, T25> case25,
System.Action<TState, T26> case26,
System.Action<TState, T27> case27)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									case 6:
					{
						case6(state, (T6)this.value);
						break;
					}
									case 7:
					{
						case7(state, (T7)this.value);
						break;
					}
									case 8:
					{
						case8(state, (T8)this.value);
						break;
					}
									case 9:
					{
						case9(state, (T9)this.value);
						break;
					}
									case 10:
					{
						case10(state, (T10)this.value);
						break;
					}
									case 11:
					{
						case11(state, (T11)this.value);
						break;
					}
									case 12:
					{
						case12(state, (T12)this.value);
						break;
					}
									case 13:
					{
						case13(state, (T13)this.value);
						break;
					}
									case 14:
					{
						case14(state, (T14)this.value);
						break;
					}
									case 15:
					{
						case15(state, (T15)this.value);
						break;
					}
									case 16:
					{
						case16(state, (T16)this.value);
						break;
					}
									case 17:
					{
						case17(state, (T17)this.value);
						break;
					}
									case 18:
					{
						case18(state, (T18)this.value);
						break;
					}
									case 19:
					{
						case19(state, (T19)this.value);
						break;
					}
									case 20:
					{
						case20(state, (T20)this.value);
						break;
					}
									case 21:
					{
						case21(state, (T21)this.value);
						break;
					}
									case 22:
					{
						case22(state, (T22)this.value);
						break;
					}
									case 23:
					{
						case23(state, (T23)this.value);
						break;
					}
									case 24:
					{
						case24(state, (T24)this.value);
						break;
					}
									case 25:
					{
						case25(state, (T25)this.value);
						break;
					}
									case 26:
					{
						case26(state, (T26)this.value);
						break;
					}
									case 27:
					{
						case27(state, (T27)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5,
System.Func<T6, TResult> case6,
System.Func<T7, TResult> case7,
System.Func<T8, TResult> case8,
System.Func<T9, TResult> case9,
System.Func<T10, TResult> case10,
System.Func<T11, TResult> case11,
System.Func<T12, TResult> case12,
System.Func<T13, TResult> case13,
System.Func<T14, TResult> case14,
System.Func<T15, TResult> case15,
System.Func<T16, TResult> case16,
System.Func<T17, TResult> case17,
System.Func<T18, TResult> case18,
System.Func<T19, TResult> case19,
System.Func<T20, TResult> case20,
System.Func<T21, TResult> case21,
System.Func<T22, TResult> case22,
System.Func<T23, TResult> case23,
System.Func<T24, TResult> case24,
System.Func<T25, TResult> case25,
System.Func<T26, TResult> case26,
System.Func<T27, TResult> case27)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									case 6:
					{
						return case6((T6)this.value);
					}
									case 7:
					{
						return case7((T7)this.value);
					}
									case 8:
					{
						return case8((T8)this.value);
					}
									case 9:
					{
						return case9((T9)this.value);
					}
									case 10:
					{
						return case10((T10)this.value);
					}
									case 11:
					{
						return case11((T11)this.value);
					}
									case 12:
					{
						return case12((T12)this.value);
					}
									case 13:
					{
						return case13((T13)this.value);
					}
									case 14:
					{
						return case14((T14)this.value);
					}
									case 15:
					{
						return case15((T15)this.value);
					}
									case 16:
					{
						return case16((T16)this.value);
					}
									case 17:
					{
						return case17((T17)this.value);
					}
									case 18:
					{
						return case18((T18)this.value);
					}
									case 19:
					{
						return case19((T19)this.value);
					}
									case 20:
					{
						return case20((T20)this.value);
					}
									case 21:
					{
						return case21((T21)this.value);
					}
									case 22:
					{
						return case22((T22)this.value);
					}
									case 23:
					{
						return case23((T23)this.value);
					}
									case 24:
					{
						return case24((T24)this.value);
					}
									case 25:
					{
						return case25((T25)this.value);
					}
									case 26:
					{
						return case26((T26)this.value);
					}
									case 27:
					{
						return case27((T27)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5,
System.Func<TState, T6, TResult> case6,
System.Func<TState, T7, TResult> case7,
System.Func<TState, T8, TResult> case8,
System.Func<TState, T9, TResult> case9,
System.Func<TState, T10, TResult> case10,
System.Func<TState, T11, TResult> case11,
System.Func<TState, T12, TResult> case12,
System.Func<TState, T13, TResult> case13,
System.Func<TState, T14, TResult> case14,
System.Func<TState, T15, TResult> case15,
System.Func<TState, T16, TResult> case16,
System.Func<TState, T17, TResult> case17,
System.Func<TState, T18, TResult> case18,
System.Func<TState, T19, TResult> case19,
System.Func<TState, T20, TResult> case20,
System.Func<TState, T21, TResult> case21,
System.Func<TState, T22, TResult> case22,
System.Func<TState, T23, TResult> case23,
System.Func<TState, T24, TResult> case24,
System.Func<TState, T25, TResult> case25,
System.Func<TState, T26, TResult> case26,
System.Func<TState, T27, TResult> case27)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									case 6:
					{
						return case6(state, (T6)this.value);
					}
									case 7:
					{
						return case7(state, (T7)this.value);
					}
									case 8:
					{
						return case8(state, (T8)this.value);
					}
									case 9:
					{
						return case9(state, (T9)this.value);
					}
									case 10:
					{
						return case10(state, (T10)this.value);
					}
									case 11:
					{
						return case11(state, (T11)this.value);
					}
									case 12:
					{
						return case12(state, (T12)this.value);
					}
									case 13:
					{
						return case13(state, (T13)this.value);
					}
									case 14:
					{
						return case14(state, (T14)this.value);
					}
									case 15:
					{
						return case15(state, (T15)this.value);
					}
									case 16:
					{
						return case16(state, (T16)this.value);
					}
									case 17:
					{
						return case17(state, (T17)this.value);
					}
									case 18:
					{
						return case18(state, (T18)this.value);
					}
									case 19:
					{
						return case19(state, (T19)this.value);
					}
									case 20:
					{
						return case20(state, (T20)this.value);
					}
									case 21:
					{
						return case21(state, (T21)this.value);
					}
									case 22:
					{
						return case22(state, (T22)this.value);
					}
									case 23:
					{
						return case23(state, (T23)this.value);
					}
									case 24:
					{
						return case24(state, (T24)this.value);
					}
									case 25:
					{
						return case25(state, (T25)this.value);
					}
									case 26:
					{
						return case26(state, (T26)this.value);
					}
									case 27:
					{
						return case27(state, (T27)this.value);
					}
									default:
						return defaultCase(state);
				}
			}

			public override bool Equals(object? other)
			{
				if (other is FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27> otherUnion)
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28> : IFlatBufferUnion
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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
					where T19 : notnull
					where T20 : notnull
					where T21 : notnull
					where T22 : notnull
					where T23 : notnull
					where T24 : notnull
					where T25 : notnull
					where T26 : notnull
					where T27 : notnull
					where T28 : notnull
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

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28> : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28>

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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
					where T19 : notnull
					where T20 : notnull
					where T21 : notnull
					where T22 : notnull
					where T23 : notnull
					where T24 : notnull
					where T25 : notnull
					where T26 : notnull
					where T27 : notnull
					where T28 : notnull
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
				
			
			public FlatBufferUnion(T11 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 11;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T12 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 12;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T13 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 13;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T14 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 14;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T15 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 15;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T16 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 16;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T17 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 17;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T18 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 18;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T19 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 19;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T20 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 20;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T21 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 21;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T22 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 22;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T23 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 23;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T24 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 24;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T25 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 25;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T26 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 26;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T27 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 27;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T28 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 28;
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
				
			
			public T11 Item11
			{
				get 
				{
					if (this.discriminator == 11)
					{
						return (T11)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T11? item)
			{
				if (this.discriminator == 11)
				{
					item = (T11)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T12 Item12
			{
				get 
				{
					if (this.discriminator == 12)
					{
						return (T12)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T12? item)
			{
				if (this.discriminator == 12)
				{
					item = (T12)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T13 Item13
			{
				get 
				{
					if (this.discriminator == 13)
					{
						return (T13)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T13? item)
			{
				if (this.discriminator == 13)
				{
					item = (T13)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T14 Item14
			{
				get 
				{
					if (this.discriminator == 14)
					{
						return (T14)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T14? item)
			{
				if (this.discriminator == 14)
				{
					item = (T14)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T15 Item15
			{
				get 
				{
					if (this.discriminator == 15)
					{
						return (T15)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T15? item)
			{
				if (this.discriminator == 15)
				{
					item = (T15)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T16 Item16
			{
				get 
				{
					if (this.discriminator == 16)
					{
						return (T16)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T16? item)
			{
				if (this.discriminator == 16)
				{
					item = (T16)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T17 Item17
			{
				get 
				{
					if (this.discriminator == 17)
					{
						return (T17)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T17? item)
			{
				if (this.discriminator == 17)
				{
					item = (T17)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T18 Item18
			{
				get 
				{
					if (this.discriminator == 18)
					{
						return (T18)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T18? item)
			{
				if (this.discriminator == 18)
				{
					item = (T18)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T19 Item19
			{
				get 
				{
					if (this.discriminator == 19)
					{
						return (T19)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T19? item)
			{
				if (this.discriminator == 19)
				{
					item = (T19)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T20 Item20
			{
				get 
				{
					if (this.discriminator == 20)
					{
						return (T20)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T20? item)
			{
				if (this.discriminator == 20)
				{
					item = (T20)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T21 Item21
			{
				get 
				{
					if (this.discriminator == 21)
					{
						return (T21)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T21? item)
			{
				if (this.discriminator == 21)
				{
					item = (T21)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T22 Item22
			{
				get 
				{
					if (this.discriminator == 22)
					{
						return (T22)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T22? item)
			{
				if (this.discriminator == 22)
				{
					item = (T22)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T23 Item23
			{
				get 
				{
					if (this.discriminator == 23)
					{
						return (T23)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T23? item)
			{
				if (this.discriminator == 23)
				{
					item = (T23)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T24 Item24
			{
				get 
				{
					if (this.discriminator == 24)
					{
						return (T24)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T24? item)
			{
				if (this.discriminator == 24)
				{
					item = (T24)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T25 Item25
			{
				get 
				{
					if (this.discriminator == 25)
					{
						return (T25)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T25? item)
			{
				if (this.discriminator == 25)
				{
					item = (T25)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T26 Item26
			{
				get 
				{
					if (this.discriminator == 26)
					{
						return (T26)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T26? item)
			{
				if (this.discriminator == 26)
				{
					item = (T26)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T27 Item27
			{
				get 
				{
					if (this.discriminator == 27)
					{
						return (T27)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T27? item)
			{
				if (this.discriminator == 27)
				{
					item = (T27)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T28 Item28
			{
				get 
				{
					if (this.discriminator == 28)
					{
						return (T28)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T28? item)
			{
				if (this.discriminator == 28)
				{
					item = (T28)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5,
System.Action<T6> case6,
System.Action<T7> case7,
System.Action<T8> case8,
System.Action<T9> case9,
System.Action<T10> case10,
System.Action<T11> case11,
System.Action<T12> case12,
System.Action<T13> case13,
System.Action<T14> case14,
System.Action<T15> case15,
System.Action<T16> case16,
System.Action<T17> case17,
System.Action<T18> case18,
System.Action<T19> case19,
System.Action<T20> case20,
System.Action<T21> case21,
System.Action<T22> case22,
System.Action<T23> case23,
System.Action<T24> case24,
System.Action<T25> case25,
System.Action<T26> case26,
System.Action<T27> case27,
System.Action<T28> case28)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									case 6:
					{
						case6((T6)this.value);
						break;
					}
									case 7:
					{
						case7((T7)this.value);
						break;
					}
									case 8:
					{
						case8((T8)this.value);
						break;
					}
									case 9:
					{
						case9((T9)this.value);
						break;
					}
									case 10:
					{
						case10((T10)this.value);
						break;
					}
									case 11:
					{
						case11((T11)this.value);
						break;
					}
									case 12:
					{
						case12((T12)this.value);
						break;
					}
									case 13:
					{
						case13((T13)this.value);
						break;
					}
									case 14:
					{
						case14((T14)this.value);
						break;
					}
									case 15:
					{
						case15((T15)this.value);
						break;
					}
									case 16:
					{
						case16((T16)this.value);
						break;
					}
									case 17:
					{
						case17((T17)this.value);
						break;
					}
									case 18:
					{
						case18((T18)this.value);
						break;
					}
									case 19:
					{
						case19((T19)this.value);
						break;
					}
									case 20:
					{
						case20((T20)this.value);
						break;
					}
									case 21:
					{
						case21((T21)this.value);
						break;
					}
									case 22:
					{
						case22((T22)this.value);
						break;
					}
									case 23:
					{
						case23((T23)this.value);
						break;
					}
									case 24:
					{
						case24((T24)this.value);
						break;
					}
									case 25:
					{
						case25((T25)this.value);
						break;
					}
									case 26:
					{
						case26((T26)this.value);
						break;
					}
									case 27:
					{
						case27((T27)this.value);
						break;
					}
									case 28:
					{
						case28((T28)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5,
System.Action<TState, T6> case6,
System.Action<TState, T7> case7,
System.Action<TState, T8> case8,
System.Action<TState, T9> case9,
System.Action<TState, T10> case10,
System.Action<TState, T11> case11,
System.Action<TState, T12> case12,
System.Action<TState, T13> case13,
System.Action<TState, T14> case14,
System.Action<TState, T15> case15,
System.Action<TState, T16> case16,
System.Action<TState, T17> case17,
System.Action<TState, T18> case18,
System.Action<TState, T19> case19,
System.Action<TState, T20> case20,
System.Action<TState, T21> case21,
System.Action<TState, T22> case22,
System.Action<TState, T23> case23,
System.Action<TState, T24> case24,
System.Action<TState, T25> case25,
System.Action<TState, T26> case26,
System.Action<TState, T27> case27,
System.Action<TState, T28> case28)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									case 6:
					{
						case6(state, (T6)this.value);
						break;
					}
									case 7:
					{
						case7(state, (T7)this.value);
						break;
					}
									case 8:
					{
						case8(state, (T8)this.value);
						break;
					}
									case 9:
					{
						case9(state, (T9)this.value);
						break;
					}
									case 10:
					{
						case10(state, (T10)this.value);
						break;
					}
									case 11:
					{
						case11(state, (T11)this.value);
						break;
					}
									case 12:
					{
						case12(state, (T12)this.value);
						break;
					}
									case 13:
					{
						case13(state, (T13)this.value);
						break;
					}
									case 14:
					{
						case14(state, (T14)this.value);
						break;
					}
									case 15:
					{
						case15(state, (T15)this.value);
						break;
					}
									case 16:
					{
						case16(state, (T16)this.value);
						break;
					}
									case 17:
					{
						case17(state, (T17)this.value);
						break;
					}
									case 18:
					{
						case18(state, (T18)this.value);
						break;
					}
									case 19:
					{
						case19(state, (T19)this.value);
						break;
					}
									case 20:
					{
						case20(state, (T20)this.value);
						break;
					}
									case 21:
					{
						case21(state, (T21)this.value);
						break;
					}
									case 22:
					{
						case22(state, (T22)this.value);
						break;
					}
									case 23:
					{
						case23(state, (T23)this.value);
						break;
					}
									case 24:
					{
						case24(state, (T24)this.value);
						break;
					}
									case 25:
					{
						case25(state, (T25)this.value);
						break;
					}
									case 26:
					{
						case26(state, (T26)this.value);
						break;
					}
									case 27:
					{
						case27(state, (T27)this.value);
						break;
					}
									case 28:
					{
						case28(state, (T28)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5,
System.Func<T6, TResult> case6,
System.Func<T7, TResult> case7,
System.Func<T8, TResult> case8,
System.Func<T9, TResult> case9,
System.Func<T10, TResult> case10,
System.Func<T11, TResult> case11,
System.Func<T12, TResult> case12,
System.Func<T13, TResult> case13,
System.Func<T14, TResult> case14,
System.Func<T15, TResult> case15,
System.Func<T16, TResult> case16,
System.Func<T17, TResult> case17,
System.Func<T18, TResult> case18,
System.Func<T19, TResult> case19,
System.Func<T20, TResult> case20,
System.Func<T21, TResult> case21,
System.Func<T22, TResult> case22,
System.Func<T23, TResult> case23,
System.Func<T24, TResult> case24,
System.Func<T25, TResult> case25,
System.Func<T26, TResult> case26,
System.Func<T27, TResult> case27,
System.Func<T28, TResult> case28)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									case 6:
					{
						return case6((T6)this.value);
					}
									case 7:
					{
						return case7((T7)this.value);
					}
									case 8:
					{
						return case8((T8)this.value);
					}
									case 9:
					{
						return case9((T9)this.value);
					}
									case 10:
					{
						return case10((T10)this.value);
					}
									case 11:
					{
						return case11((T11)this.value);
					}
									case 12:
					{
						return case12((T12)this.value);
					}
									case 13:
					{
						return case13((T13)this.value);
					}
									case 14:
					{
						return case14((T14)this.value);
					}
									case 15:
					{
						return case15((T15)this.value);
					}
									case 16:
					{
						return case16((T16)this.value);
					}
									case 17:
					{
						return case17((T17)this.value);
					}
									case 18:
					{
						return case18((T18)this.value);
					}
									case 19:
					{
						return case19((T19)this.value);
					}
									case 20:
					{
						return case20((T20)this.value);
					}
									case 21:
					{
						return case21((T21)this.value);
					}
									case 22:
					{
						return case22((T22)this.value);
					}
									case 23:
					{
						return case23((T23)this.value);
					}
									case 24:
					{
						return case24((T24)this.value);
					}
									case 25:
					{
						return case25((T25)this.value);
					}
									case 26:
					{
						return case26((T26)this.value);
					}
									case 27:
					{
						return case27((T27)this.value);
					}
									case 28:
					{
						return case28((T28)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5,
System.Func<TState, T6, TResult> case6,
System.Func<TState, T7, TResult> case7,
System.Func<TState, T8, TResult> case8,
System.Func<TState, T9, TResult> case9,
System.Func<TState, T10, TResult> case10,
System.Func<TState, T11, TResult> case11,
System.Func<TState, T12, TResult> case12,
System.Func<TState, T13, TResult> case13,
System.Func<TState, T14, TResult> case14,
System.Func<TState, T15, TResult> case15,
System.Func<TState, T16, TResult> case16,
System.Func<TState, T17, TResult> case17,
System.Func<TState, T18, TResult> case18,
System.Func<TState, T19, TResult> case19,
System.Func<TState, T20, TResult> case20,
System.Func<TState, T21, TResult> case21,
System.Func<TState, T22, TResult> case22,
System.Func<TState, T23, TResult> case23,
System.Func<TState, T24, TResult> case24,
System.Func<TState, T25, TResult> case25,
System.Func<TState, T26, TResult> case26,
System.Func<TState, T27, TResult> case27,
System.Func<TState, T28, TResult> case28)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									case 6:
					{
						return case6(state, (T6)this.value);
					}
									case 7:
					{
						return case7(state, (T7)this.value);
					}
									case 8:
					{
						return case8(state, (T8)this.value);
					}
									case 9:
					{
						return case9(state, (T9)this.value);
					}
									case 10:
					{
						return case10(state, (T10)this.value);
					}
									case 11:
					{
						return case11(state, (T11)this.value);
					}
									case 12:
					{
						return case12(state, (T12)this.value);
					}
									case 13:
					{
						return case13(state, (T13)this.value);
					}
									case 14:
					{
						return case14(state, (T14)this.value);
					}
									case 15:
					{
						return case15(state, (T15)this.value);
					}
									case 16:
					{
						return case16(state, (T16)this.value);
					}
									case 17:
					{
						return case17(state, (T17)this.value);
					}
									case 18:
					{
						return case18(state, (T18)this.value);
					}
									case 19:
					{
						return case19(state, (T19)this.value);
					}
									case 20:
					{
						return case20(state, (T20)this.value);
					}
									case 21:
					{
						return case21(state, (T21)this.value);
					}
									case 22:
					{
						return case22(state, (T22)this.value);
					}
									case 23:
					{
						return case23(state, (T23)this.value);
					}
									case 24:
					{
						return case24(state, (T24)this.value);
					}
									case 25:
					{
						return case25(state, (T25)this.value);
					}
									case 26:
					{
						return case26(state, (T26)this.value);
					}
									case 27:
					{
						return case27(state, (T27)this.value);
					}
									case 28:
					{
						return case28(state, (T28)this.value);
					}
									default:
						return defaultCase(state);
				}
			}

			public override bool Equals(object? other)
			{
				if (other is FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28> otherUnion)
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29> : IFlatBufferUnion
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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
					where T19 : notnull
					where T20 : notnull
					where T21 : notnull
					where T22 : notnull
					where T23 : notnull
					where T24 : notnull
					where T25 : notnull
					where T26 : notnull
					where T27 : notnull
					where T28 : notnull
					where T29 : notnull
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

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29> : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29>

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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
					where T19 : notnull
					where T20 : notnull
					where T21 : notnull
					where T22 : notnull
					where T23 : notnull
					where T24 : notnull
					where T25 : notnull
					where T26 : notnull
					where T27 : notnull
					where T28 : notnull
					where T29 : notnull
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
				
			
			public FlatBufferUnion(T11 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 11;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T12 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 12;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T13 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 13;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T14 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 14;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T15 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 15;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T16 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 16;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T17 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 17;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T18 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 18;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T19 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 19;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T20 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 20;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T21 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 21;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T22 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 22;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T23 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 23;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T24 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 24;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T25 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 25;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T26 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 26;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T27 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 27;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T28 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 28;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T29 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 29;
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
				
			
			public T11 Item11
			{
				get 
				{
					if (this.discriminator == 11)
					{
						return (T11)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T11? item)
			{
				if (this.discriminator == 11)
				{
					item = (T11)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T12 Item12
			{
				get 
				{
					if (this.discriminator == 12)
					{
						return (T12)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T12? item)
			{
				if (this.discriminator == 12)
				{
					item = (T12)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T13 Item13
			{
				get 
				{
					if (this.discriminator == 13)
					{
						return (T13)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T13? item)
			{
				if (this.discriminator == 13)
				{
					item = (T13)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T14 Item14
			{
				get 
				{
					if (this.discriminator == 14)
					{
						return (T14)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T14? item)
			{
				if (this.discriminator == 14)
				{
					item = (T14)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T15 Item15
			{
				get 
				{
					if (this.discriminator == 15)
					{
						return (T15)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T15? item)
			{
				if (this.discriminator == 15)
				{
					item = (T15)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T16 Item16
			{
				get 
				{
					if (this.discriminator == 16)
					{
						return (T16)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T16? item)
			{
				if (this.discriminator == 16)
				{
					item = (T16)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T17 Item17
			{
				get 
				{
					if (this.discriminator == 17)
					{
						return (T17)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T17? item)
			{
				if (this.discriminator == 17)
				{
					item = (T17)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T18 Item18
			{
				get 
				{
					if (this.discriminator == 18)
					{
						return (T18)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T18? item)
			{
				if (this.discriminator == 18)
				{
					item = (T18)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T19 Item19
			{
				get 
				{
					if (this.discriminator == 19)
					{
						return (T19)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T19? item)
			{
				if (this.discriminator == 19)
				{
					item = (T19)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T20 Item20
			{
				get 
				{
					if (this.discriminator == 20)
					{
						return (T20)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T20? item)
			{
				if (this.discriminator == 20)
				{
					item = (T20)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T21 Item21
			{
				get 
				{
					if (this.discriminator == 21)
					{
						return (T21)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T21? item)
			{
				if (this.discriminator == 21)
				{
					item = (T21)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T22 Item22
			{
				get 
				{
					if (this.discriminator == 22)
					{
						return (T22)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T22? item)
			{
				if (this.discriminator == 22)
				{
					item = (T22)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T23 Item23
			{
				get 
				{
					if (this.discriminator == 23)
					{
						return (T23)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T23? item)
			{
				if (this.discriminator == 23)
				{
					item = (T23)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T24 Item24
			{
				get 
				{
					if (this.discriminator == 24)
					{
						return (T24)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T24? item)
			{
				if (this.discriminator == 24)
				{
					item = (T24)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T25 Item25
			{
				get 
				{
					if (this.discriminator == 25)
					{
						return (T25)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T25? item)
			{
				if (this.discriminator == 25)
				{
					item = (T25)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T26 Item26
			{
				get 
				{
					if (this.discriminator == 26)
					{
						return (T26)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T26? item)
			{
				if (this.discriminator == 26)
				{
					item = (T26)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T27 Item27
			{
				get 
				{
					if (this.discriminator == 27)
					{
						return (T27)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T27? item)
			{
				if (this.discriminator == 27)
				{
					item = (T27)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T28 Item28
			{
				get 
				{
					if (this.discriminator == 28)
					{
						return (T28)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T28? item)
			{
				if (this.discriminator == 28)
				{
					item = (T28)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T29 Item29
			{
				get 
				{
					if (this.discriminator == 29)
					{
						return (T29)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T29? item)
			{
				if (this.discriminator == 29)
				{
					item = (T29)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5,
System.Action<T6> case6,
System.Action<T7> case7,
System.Action<T8> case8,
System.Action<T9> case9,
System.Action<T10> case10,
System.Action<T11> case11,
System.Action<T12> case12,
System.Action<T13> case13,
System.Action<T14> case14,
System.Action<T15> case15,
System.Action<T16> case16,
System.Action<T17> case17,
System.Action<T18> case18,
System.Action<T19> case19,
System.Action<T20> case20,
System.Action<T21> case21,
System.Action<T22> case22,
System.Action<T23> case23,
System.Action<T24> case24,
System.Action<T25> case25,
System.Action<T26> case26,
System.Action<T27> case27,
System.Action<T28> case28,
System.Action<T29> case29)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									case 6:
					{
						case6((T6)this.value);
						break;
					}
									case 7:
					{
						case7((T7)this.value);
						break;
					}
									case 8:
					{
						case8((T8)this.value);
						break;
					}
									case 9:
					{
						case9((T9)this.value);
						break;
					}
									case 10:
					{
						case10((T10)this.value);
						break;
					}
									case 11:
					{
						case11((T11)this.value);
						break;
					}
									case 12:
					{
						case12((T12)this.value);
						break;
					}
									case 13:
					{
						case13((T13)this.value);
						break;
					}
									case 14:
					{
						case14((T14)this.value);
						break;
					}
									case 15:
					{
						case15((T15)this.value);
						break;
					}
									case 16:
					{
						case16((T16)this.value);
						break;
					}
									case 17:
					{
						case17((T17)this.value);
						break;
					}
									case 18:
					{
						case18((T18)this.value);
						break;
					}
									case 19:
					{
						case19((T19)this.value);
						break;
					}
									case 20:
					{
						case20((T20)this.value);
						break;
					}
									case 21:
					{
						case21((T21)this.value);
						break;
					}
									case 22:
					{
						case22((T22)this.value);
						break;
					}
									case 23:
					{
						case23((T23)this.value);
						break;
					}
									case 24:
					{
						case24((T24)this.value);
						break;
					}
									case 25:
					{
						case25((T25)this.value);
						break;
					}
									case 26:
					{
						case26((T26)this.value);
						break;
					}
									case 27:
					{
						case27((T27)this.value);
						break;
					}
									case 28:
					{
						case28((T28)this.value);
						break;
					}
									case 29:
					{
						case29((T29)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5,
System.Action<TState, T6> case6,
System.Action<TState, T7> case7,
System.Action<TState, T8> case8,
System.Action<TState, T9> case9,
System.Action<TState, T10> case10,
System.Action<TState, T11> case11,
System.Action<TState, T12> case12,
System.Action<TState, T13> case13,
System.Action<TState, T14> case14,
System.Action<TState, T15> case15,
System.Action<TState, T16> case16,
System.Action<TState, T17> case17,
System.Action<TState, T18> case18,
System.Action<TState, T19> case19,
System.Action<TState, T20> case20,
System.Action<TState, T21> case21,
System.Action<TState, T22> case22,
System.Action<TState, T23> case23,
System.Action<TState, T24> case24,
System.Action<TState, T25> case25,
System.Action<TState, T26> case26,
System.Action<TState, T27> case27,
System.Action<TState, T28> case28,
System.Action<TState, T29> case29)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									case 6:
					{
						case6(state, (T6)this.value);
						break;
					}
									case 7:
					{
						case7(state, (T7)this.value);
						break;
					}
									case 8:
					{
						case8(state, (T8)this.value);
						break;
					}
									case 9:
					{
						case9(state, (T9)this.value);
						break;
					}
									case 10:
					{
						case10(state, (T10)this.value);
						break;
					}
									case 11:
					{
						case11(state, (T11)this.value);
						break;
					}
									case 12:
					{
						case12(state, (T12)this.value);
						break;
					}
									case 13:
					{
						case13(state, (T13)this.value);
						break;
					}
									case 14:
					{
						case14(state, (T14)this.value);
						break;
					}
									case 15:
					{
						case15(state, (T15)this.value);
						break;
					}
									case 16:
					{
						case16(state, (T16)this.value);
						break;
					}
									case 17:
					{
						case17(state, (T17)this.value);
						break;
					}
									case 18:
					{
						case18(state, (T18)this.value);
						break;
					}
									case 19:
					{
						case19(state, (T19)this.value);
						break;
					}
									case 20:
					{
						case20(state, (T20)this.value);
						break;
					}
									case 21:
					{
						case21(state, (T21)this.value);
						break;
					}
									case 22:
					{
						case22(state, (T22)this.value);
						break;
					}
									case 23:
					{
						case23(state, (T23)this.value);
						break;
					}
									case 24:
					{
						case24(state, (T24)this.value);
						break;
					}
									case 25:
					{
						case25(state, (T25)this.value);
						break;
					}
									case 26:
					{
						case26(state, (T26)this.value);
						break;
					}
									case 27:
					{
						case27(state, (T27)this.value);
						break;
					}
									case 28:
					{
						case28(state, (T28)this.value);
						break;
					}
									case 29:
					{
						case29(state, (T29)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5,
System.Func<T6, TResult> case6,
System.Func<T7, TResult> case7,
System.Func<T8, TResult> case8,
System.Func<T9, TResult> case9,
System.Func<T10, TResult> case10,
System.Func<T11, TResult> case11,
System.Func<T12, TResult> case12,
System.Func<T13, TResult> case13,
System.Func<T14, TResult> case14,
System.Func<T15, TResult> case15,
System.Func<T16, TResult> case16,
System.Func<T17, TResult> case17,
System.Func<T18, TResult> case18,
System.Func<T19, TResult> case19,
System.Func<T20, TResult> case20,
System.Func<T21, TResult> case21,
System.Func<T22, TResult> case22,
System.Func<T23, TResult> case23,
System.Func<T24, TResult> case24,
System.Func<T25, TResult> case25,
System.Func<T26, TResult> case26,
System.Func<T27, TResult> case27,
System.Func<T28, TResult> case28,
System.Func<T29, TResult> case29)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									case 6:
					{
						return case6((T6)this.value);
					}
									case 7:
					{
						return case7((T7)this.value);
					}
									case 8:
					{
						return case8((T8)this.value);
					}
									case 9:
					{
						return case9((T9)this.value);
					}
									case 10:
					{
						return case10((T10)this.value);
					}
									case 11:
					{
						return case11((T11)this.value);
					}
									case 12:
					{
						return case12((T12)this.value);
					}
									case 13:
					{
						return case13((T13)this.value);
					}
									case 14:
					{
						return case14((T14)this.value);
					}
									case 15:
					{
						return case15((T15)this.value);
					}
									case 16:
					{
						return case16((T16)this.value);
					}
									case 17:
					{
						return case17((T17)this.value);
					}
									case 18:
					{
						return case18((T18)this.value);
					}
									case 19:
					{
						return case19((T19)this.value);
					}
									case 20:
					{
						return case20((T20)this.value);
					}
									case 21:
					{
						return case21((T21)this.value);
					}
									case 22:
					{
						return case22((T22)this.value);
					}
									case 23:
					{
						return case23((T23)this.value);
					}
									case 24:
					{
						return case24((T24)this.value);
					}
									case 25:
					{
						return case25((T25)this.value);
					}
									case 26:
					{
						return case26((T26)this.value);
					}
									case 27:
					{
						return case27((T27)this.value);
					}
									case 28:
					{
						return case28((T28)this.value);
					}
									case 29:
					{
						return case29((T29)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5,
System.Func<TState, T6, TResult> case6,
System.Func<TState, T7, TResult> case7,
System.Func<TState, T8, TResult> case8,
System.Func<TState, T9, TResult> case9,
System.Func<TState, T10, TResult> case10,
System.Func<TState, T11, TResult> case11,
System.Func<TState, T12, TResult> case12,
System.Func<TState, T13, TResult> case13,
System.Func<TState, T14, TResult> case14,
System.Func<TState, T15, TResult> case15,
System.Func<TState, T16, TResult> case16,
System.Func<TState, T17, TResult> case17,
System.Func<TState, T18, TResult> case18,
System.Func<TState, T19, TResult> case19,
System.Func<TState, T20, TResult> case20,
System.Func<TState, T21, TResult> case21,
System.Func<TState, T22, TResult> case22,
System.Func<TState, T23, TResult> case23,
System.Func<TState, T24, TResult> case24,
System.Func<TState, T25, TResult> case25,
System.Func<TState, T26, TResult> case26,
System.Func<TState, T27, TResult> case27,
System.Func<TState, T28, TResult> case28,
System.Func<TState, T29, TResult> case29)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									case 6:
					{
						return case6(state, (T6)this.value);
					}
									case 7:
					{
						return case7(state, (T7)this.value);
					}
									case 8:
					{
						return case8(state, (T8)this.value);
					}
									case 9:
					{
						return case9(state, (T9)this.value);
					}
									case 10:
					{
						return case10(state, (T10)this.value);
					}
									case 11:
					{
						return case11(state, (T11)this.value);
					}
									case 12:
					{
						return case12(state, (T12)this.value);
					}
									case 13:
					{
						return case13(state, (T13)this.value);
					}
									case 14:
					{
						return case14(state, (T14)this.value);
					}
									case 15:
					{
						return case15(state, (T15)this.value);
					}
									case 16:
					{
						return case16(state, (T16)this.value);
					}
									case 17:
					{
						return case17(state, (T17)this.value);
					}
									case 18:
					{
						return case18(state, (T18)this.value);
					}
									case 19:
					{
						return case19(state, (T19)this.value);
					}
									case 20:
					{
						return case20(state, (T20)this.value);
					}
									case 21:
					{
						return case21(state, (T21)this.value);
					}
									case 22:
					{
						return case22(state, (T22)this.value);
					}
									case 23:
					{
						return case23(state, (T23)this.value);
					}
									case 24:
					{
						return case24(state, (T24)this.value);
					}
									case 25:
					{
						return case25(state, (T25)this.value);
					}
									case 26:
					{
						return case26(state, (T26)this.value);
					}
									case 27:
					{
						return case27(state, (T27)this.value);
					}
									case 28:
					{
						return case28(state, (T28)this.value);
					}
									case 29:
					{
						return case29(state, (T29)this.value);
					}
									default:
						return defaultCase(state);
				}
			}

			public override bool Equals(object? other)
			{
				if (other is FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29> otherUnion)
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
		public interface IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30> : IFlatBufferUnion
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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
					where T19 : notnull
					where T20 : notnull
					where T21 : notnull
					where T22 : notnull
					where T23 : notnull
					where T24 : notnull
					where T25 : notnull
					where T26 : notnull
					where T27 : notnull
					where T28 : notnull
					where T29 : notnull
					where T30 : notnull
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

		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public struct FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30> : IFlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30>

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
					where T11 : notnull
					where T12 : notnull
					where T13 : notnull
					where T14 : notnull
					where T15 : notnull
					where T16 : notnull
					where T17 : notnull
					where T18 : notnull
					where T19 : notnull
					where T20 : notnull
					where T21 : notnull
					where T22 : notnull
					where T23 : notnull
					where T24 : notnull
					where T25 : notnull
					where T26 : notnull
					where T27 : notnull
					where T28 : notnull
					where T29 : notnull
					where T30 : notnull
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
				
			
			public FlatBufferUnion(T11 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 11;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T12 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 12;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T13 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 13;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T14 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 14;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T15 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 15;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T16 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 16;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T17 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 17;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T18 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 18;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T19 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 19;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T20 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 20;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T21 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 21;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T22 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 22;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T23 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 23;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T24 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 24;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T25 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 25;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T26 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 26;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T27 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 27;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T28 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 28;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T29 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 29;
				this.value = item;
			}
				
			
			public FlatBufferUnion(T30 item)
			{
				if (item is null)
				{
					throw new System.ArgumentNullException(nameof(item), "FlatBuffer unions do not accept null items. If you wish to use a null value, simply null out the union on the class.");
				}

				this.discriminator = 30;
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
				
			
			public T11 Item11
			{
				get 
				{
					if (this.discriminator == 11)
					{
						return (T11)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T11? item)
			{
				if (this.discriminator == 11)
				{
					item = (T11)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T12 Item12
			{
				get 
				{
					if (this.discriminator == 12)
					{
						return (T12)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T12? item)
			{
				if (this.discriminator == 12)
				{
					item = (T12)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T13 Item13
			{
				get 
				{
					if (this.discriminator == 13)
					{
						return (T13)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T13? item)
			{
				if (this.discriminator == 13)
				{
					item = (T13)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T14 Item14
			{
				get 
				{
					if (this.discriminator == 14)
					{
						return (T14)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T14? item)
			{
				if (this.discriminator == 14)
				{
					item = (T14)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T15 Item15
			{
				get 
				{
					if (this.discriminator == 15)
					{
						return (T15)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T15? item)
			{
				if (this.discriminator == 15)
				{
					item = (T15)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T16 Item16
			{
				get 
				{
					if (this.discriminator == 16)
					{
						return (T16)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T16? item)
			{
				if (this.discriminator == 16)
				{
					item = (T16)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T17 Item17
			{
				get 
				{
					if (this.discriminator == 17)
					{
						return (T17)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T17? item)
			{
				if (this.discriminator == 17)
				{
					item = (T17)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T18 Item18
			{
				get 
				{
					if (this.discriminator == 18)
					{
						return (T18)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T18? item)
			{
				if (this.discriminator == 18)
				{
					item = (T18)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T19 Item19
			{
				get 
				{
					if (this.discriminator == 19)
					{
						return (T19)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T19? item)
			{
				if (this.discriminator == 19)
				{
					item = (T19)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T20 Item20
			{
				get 
				{
					if (this.discriminator == 20)
					{
						return (T20)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T20? item)
			{
				if (this.discriminator == 20)
				{
					item = (T20)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T21 Item21
			{
				get 
				{
					if (this.discriminator == 21)
					{
						return (T21)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T21? item)
			{
				if (this.discriminator == 21)
				{
					item = (T21)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T22 Item22
			{
				get 
				{
					if (this.discriminator == 22)
					{
						return (T22)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T22? item)
			{
				if (this.discriminator == 22)
				{
					item = (T22)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T23 Item23
			{
				get 
				{
					if (this.discriminator == 23)
					{
						return (T23)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T23? item)
			{
				if (this.discriminator == 23)
				{
					item = (T23)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T24 Item24
			{
				get 
				{
					if (this.discriminator == 24)
					{
						return (T24)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T24? item)
			{
				if (this.discriminator == 24)
				{
					item = (T24)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T25 Item25
			{
				get 
				{
					if (this.discriminator == 25)
					{
						return (T25)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T25? item)
			{
				if (this.discriminator == 25)
				{
					item = (T25)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T26 Item26
			{
				get 
				{
					if (this.discriminator == 26)
					{
						return (T26)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T26? item)
			{
				if (this.discriminator == 26)
				{
					item = (T26)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T27 Item27
			{
				get 
				{
					if (this.discriminator == 27)
					{
						return (T27)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T27? item)
			{
				if (this.discriminator == 27)
				{
					item = (T27)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T28 Item28
			{
				get 
				{
					if (this.discriminator == 28)
					{
						return (T28)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T28? item)
			{
				if (this.discriminator == 28)
				{
					item = (T28)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T29 Item29
			{
				get 
				{
					if (this.discriminator == 29)
					{
						return (T29)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T29? item)
			{
				if (this.discriminator == 29)
				{
					item = (T29)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public T30 Item30
			{
				get 
				{
					if (this.discriminator == 30)
					{
						return (T30)this.value;
					}
					else
					{
						throw new System.InvalidOperationException();
					}
				}
			}

			public bool TryGet([NotNullWhen(true)] out T30? item)
			{
				if (this.discriminator == 30)
				{
					item = (T30)this.value;
					return true;
				}
				else
				{
					item = default;
					return false;
				}
			}
				
			
			public void Switch(
				System.Action defaultCase,
				System.Action<T1> case1,
System.Action<T2> case2,
System.Action<T3> case3,
System.Action<T4> case4,
System.Action<T5> case5,
System.Action<T6> case6,
System.Action<T7> case7,
System.Action<T8> case8,
System.Action<T9> case9,
System.Action<T10> case10,
System.Action<T11> case11,
System.Action<T12> case12,
System.Action<T13> case13,
System.Action<T14> case14,
System.Action<T15> case15,
System.Action<T16> case16,
System.Action<T17> case17,
System.Action<T18> case18,
System.Action<T19> case19,
System.Action<T20> case20,
System.Action<T21> case21,
System.Action<T22> case22,
System.Action<T23> case23,
System.Action<T24> case24,
System.Action<T25> case25,
System.Action<T26> case26,
System.Action<T27> case27,
System.Action<T28> case28,
System.Action<T29> case29,
System.Action<T30> case30)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1((T1)this.value);
						break;
					}
									case 2:
					{
						case2((T2)this.value);
						break;
					}
									case 3:
					{
						case3((T3)this.value);
						break;
					}
									case 4:
					{
						case4((T4)this.value);
						break;
					}
									case 5:
					{
						case5((T5)this.value);
						break;
					}
									case 6:
					{
						case6((T6)this.value);
						break;
					}
									case 7:
					{
						case7((T7)this.value);
						break;
					}
									case 8:
					{
						case8((T8)this.value);
						break;
					}
									case 9:
					{
						case9((T9)this.value);
						break;
					}
									case 10:
					{
						case10((T10)this.value);
						break;
					}
									case 11:
					{
						case11((T11)this.value);
						break;
					}
									case 12:
					{
						case12((T12)this.value);
						break;
					}
									case 13:
					{
						case13((T13)this.value);
						break;
					}
									case 14:
					{
						case14((T14)this.value);
						break;
					}
									case 15:
					{
						case15((T15)this.value);
						break;
					}
									case 16:
					{
						case16((T16)this.value);
						break;
					}
									case 17:
					{
						case17((T17)this.value);
						break;
					}
									case 18:
					{
						case18((T18)this.value);
						break;
					}
									case 19:
					{
						case19((T19)this.value);
						break;
					}
									case 20:
					{
						case20((T20)this.value);
						break;
					}
									case 21:
					{
						case21((T21)this.value);
						break;
					}
									case 22:
					{
						case22((T22)this.value);
						break;
					}
									case 23:
					{
						case23((T23)this.value);
						break;
					}
									case 24:
					{
						case24((T24)this.value);
						break;
					}
									case 25:
					{
						case25((T25)this.value);
						break;
					}
									case 26:
					{
						case26((T26)this.value);
						break;
					}
									case 27:
					{
						case27((T27)this.value);
						break;
					}
									case 28:
					{
						case28((T28)this.value);
						break;
					}
									case 29:
					{
						case29((T29)this.value);
						break;
					}
									case 30:
					{
						case30((T30)this.value);
						break;
					}
									default:
						defaultCase();
						break;
				}
			}

			public void Switch<TState>(
				TState state,
				System.Action<TState> defaultCase,
				System.Action<TState, T1> case1,
System.Action<TState, T2> case2,
System.Action<TState, T3> case3,
System.Action<TState, T4> case4,
System.Action<TState, T5> case5,
System.Action<TState, T6> case6,
System.Action<TState, T7> case7,
System.Action<TState, T8> case8,
System.Action<TState, T9> case9,
System.Action<TState, T10> case10,
System.Action<TState, T11> case11,
System.Action<TState, T12> case12,
System.Action<TState, T13> case13,
System.Action<TState, T14> case14,
System.Action<TState, T15> case15,
System.Action<TState, T16> case16,
System.Action<TState, T17> case17,
System.Action<TState, T18> case18,
System.Action<TState, T19> case19,
System.Action<TState, T20> case20,
System.Action<TState, T21> case21,
System.Action<TState, T22> case22,
System.Action<TState, T23> case23,
System.Action<TState, T24> case24,
System.Action<TState, T25> case25,
System.Action<TState, T26> case26,
System.Action<TState, T27> case27,
System.Action<TState, T28> case28,
System.Action<TState, T29> case29,
System.Action<TState, T30> case30)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						case1(state, (T1)this.value);
						break;
					}
									case 2:
					{
						case2(state, (T2)this.value);
						break;
					}
									case 3:
					{
						case3(state, (T3)this.value);
						break;
					}
									case 4:
					{
						case4(state, (T4)this.value);
						break;
					}
									case 5:
					{
						case5(state, (T5)this.value);
						break;
					}
									case 6:
					{
						case6(state, (T6)this.value);
						break;
					}
									case 7:
					{
						case7(state, (T7)this.value);
						break;
					}
									case 8:
					{
						case8(state, (T8)this.value);
						break;
					}
									case 9:
					{
						case9(state, (T9)this.value);
						break;
					}
									case 10:
					{
						case10(state, (T10)this.value);
						break;
					}
									case 11:
					{
						case11(state, (T11)this.value);
						break;
					}
									case 12:
					{
						case12(state, (T12)this.value);
						break;
					}
									case 13:
					{
						case13(state, (T13)this.value);
						break;
					}
									case 14:
					{
						case14(state, (T14)this.value);
						break;
					}
									case 15:
					{
						case15(state, (T15)this.value);
						break;
					}
									case 16:
					{
						case16(state, (T16)this.value);
						break;
					}
									case 17:
					{
						case17(state, (T17)this.value);
						break;
					}
									case 18:
					{
						case18(state, (T18)this.value);
						break;
					}
									case 19:
					{
						case19(state, (T19)this.value);
						break;
					}
									case 20:
					{
						case20(state, (T20)this.value);
						break;
					}
									case 21:
					{
						case21(state, (T21)this.value);
						break;
					}
									case 22:
					{
						case22(state, (T22)this.value);
						break;
					}
									case 23:
					{
						case23(state, (T23)this.value);
						break;
					}
									case 24:
					{
						case24(state, (T24)this.value);
						break;
					}
									case 25:
					{
						case25(state, (T25)this.value);
						break;
					}
									case 26:
					{
						case26(state, (T26)this.value);
						break;
					}
									case 27:
					{
						case27(state, (T27)this.value);
						break;
					}
									case 28:
					{
						case28(state, (T28)this.value);
						break;
					}
									case 29:
					{
						case29(state, (T29)this.value);
						break;
					}
									case 30:
					{
						case30(state, (T30)this.value);
						break;
					}
									default:
						defaultCase(state);
						break;
				}
			}

				
			public TResult Switch<TResult>(
				System.Func<TResult> defaultCase,
				System.Func<T1, TResult> case1,
System.Func<T2, TResult> case2,
System.Func<T3, TResult> case3,
System.Func<T4, TResult> case4,
System.Func<T5, TResult> case5,
System.Func<T6, TResult> case6,
System.Func<T7, TResult> case7,
System.Func<T8, TResult> case8,
System.Func<T9, TResult> case9,
System.Func<T10, TResult> case10,
System.Func<T11, TResult> case11,
System.Func<T12, TResult> case12,
System.Func<T13, TResult> case13,
System.Func<T14, TResult> case14,
System.Func<T15, TResult> case15,
System.Func<T16, TResult> case16,
System.Func<T17, TResult> case17,
System.Func<T18, TResult> case18,
System.Func<T19, TResult> case19,
System.Func<T20, TResult> case20,
System.Func<T21, TResult> case21,
System.Func<T22, TResult> case22,
System.Func<T23, TResult> case23,
System.Func<T24, TResult> case24,
System.Func<T25, TResult> case25,
System.Func<T26, TResult> case26,
System.Func<T27, TResult> case27,
System.Func<T28, TResult> case28,
System.Func<T29, TResult> case29,
System.Func<T30, TResult> case30)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1((T1)this.value);
					}
									case 2:
					{
						return case2((T2)this.value);
					}
									case 3:
					{
						return case3((T3)this.value);
					}
									case 4:
					{
						return case4((T4)this.value);
					}
									case 5:
					{
						return case5((T5)this.value);
					}
									case 6:
					{
						return case6((T6)this.value);
					}
									case 7:
					{
						return case7((T7)this.value);
					}
									case 8:
					{
						return case8((T8)this.value);
					}
									case 9:
					{
						return case9((T9)this.value);
					}
									case 10:
					{
						return case10((T10)this.value);
					}
									case 11:
					{
						return case11((T11)this.value);
					}
									case 12:
					{
						return case12((T12)this.value);
					}
									case 13:
					{
						return case13((T13)this.value);
					}
									case 14:
					{
						return case14((T14)this.value);
					}
									case 15:
					{
						return case15((T15)this.value);
					}
									case 16:
					{
						return case16((T16)this.value);
					}
									case 17:
					{
						return case17((T17)this.value);
					}
									case 18:
					{
						return case18((T18)this.value);
					}
									case 19:
					{
						return case19((T19)this.value);
					}
									case 20:
					{
						return case20((T20)this.value);
					}
									case 21:
					{
						return case21((T21)this.value);
					}
									case 22:
					{
						return case22((T22)this.value);
					}
									case 23:
					{
						return case23((T23)this.value);
					}
									case 24:
					{
						return case24((T24)this.value);
					}
									case 25:
					{
						return case25((T25)this.value);
					}
									case 26:
					{
						return case26((T26)this.value);
					}
									case 27:
					{
						return case27((T27)this.value);
					}
									case 28:
					{
						return case28((T28)this.value);
					}
									case 29:
					{
						return case29((T29)this.value);
					}
									case 30:
					{
						return case30((T30)this.value);
					}
									default:
						return defaultCase();
				}
			}

			public TResult Switch<TState, TResult>(
				TState state,
				System.Func<TState, TResult> defaultCase,
				System.Func<TState, T1, TResult> case1,
System.Func<TState, T2, TResult> case2,
System.Func<TState, T3, TResult> case3,
System.Func<TState, T4, TResult> case4,
System.Func<TState, T5, TResult> case5,
System.Func<TState, T6, TResult> case6,
System.Func<TState, T7, TResult> case7,
System.Func<TState, T8, TResult> case8,
System.Func<TState, T9, TResult> case9,
System.Func<TState, T10, TResult> case10,
System.Func<TState, T11, TResult> case11,
System.Func<TState, T12, TResult> case12,
System.Func<TState, T13, TResult> case13,
System.Func<TState, T14, TResult> case14,
System.Func<TState, T15, TResult> case15,
System.Func<TState, T16, TResult> case16,
System.Func<TState, T17, TResult> case17,
System.Func<TState, T18, TResult> case18,
System.Func<TState, T19, TResult> case19,
System.Func<TState, T20, TResult> case20,
System.Func<TState, T21, TResult> case21,
System.Func<TState, T22, TResult> case22,
System.Func<TState, T23, TResult> case23,
System.Func<TState, T24, TResult> case24,
System.Func<TState, T25, TResult> case25,
System.Func<TState, T26, TResult> case26,
System.Func<TState, T27, TResult> case27,
System.Func<TState, T28, TResult> case28,
System.Func<TState, T29, TResult> case29,
System.Func<TState, T30, TResult> case30)
			{
				switch (this.discriminator)
				{
									case 1:
					{
						return case1(state, (T1)this.value);
					}
									case 2:
					{
						return case2(state, (T2)this.value);
					}
									case 3:
					{
						return case3(state, (T3)this.value);
					}
									case 4:
					{
						return case4(state, (T4)this.value);
					}
									case 5:
					{
						return case5(state, (T5)this.value);
					}
									case 6:
					{
						return case6(state, (T6)this.value);
					}
									case 7:
					{
						return case7(state, (T7)this.value);
					}
									case 8:
					{
						return case8(state, (T8)this.value);
					}
									case 9:
					{
						return case9(state, (T9)this.value);
					}
									case 10:
					{
						return case10(state, (T10)this.value);
					}
									case 11:
					{
						return case11(state, (T11)this.value);
					}
									case 12:
					{
						return case12(state, (T12)this.value);
					}
									case 13:
					{
						return case13(state, (T13)this.value);
					}
									case 14:
					{
						return case14(state, (T14)this.value);
					}
									case 15:
					{
						return case15(state, (T15)this.value);
					}
									case 16:
					{
						return case16(state, (T16)this.value);
					}
									case 17:
					{
						return case17(state, (T17)this.value);
					}
									case 18:
					{
						return case18(state, (T18)this.value);
					}
									case 19:
					{
						return case19(state, (T19)this.value);
					}
									case 20:
					{
						return case20(state, (T20)this.value);
					}
									case 21:
					{
						return case21(state, (T21)this.value);
					}
									case 22:
					{
						return case22(state, (T22)this.value);
					}
									case 23:
					{
						return case23(state, (T23)this.value);
					}
									case 24:
					{
						return case24(state, (T24)this.value);
					}
									case 25:
					{
						return case25(state, (T25)this.value);
					}
									case 26:
					{
						return case26(state, (T26)this.value);
					}
									case 27:
					{
						return case27(state, (T27)this.value);
					}
									case 28:
					{
						return case28(state, (T28)this.value);
					}
									case 29:
					{
						return case29(state, (T29)this.value);
					}
									case 30:
					{
						return case30(state, (T30)this.value);
					}
									default:
						return defaultCase(state);
				}
			}

			public override bool Equals(object? other)
			{
				if (other is FlatBufferUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30> otherUnion)
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
