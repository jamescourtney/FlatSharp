

using System.Collections.Generic;
using System.Runtime.CompilerServices;

public static partial class BenchmarkUtilities
{
    
    
    public static void Prepare(int length, out Benchmark.FBBench.FS.FooBarContainer container)
    {
        var foobars = new List<Benchmark.FBBench.FS.FooBar>();
        for (int i = 0; i < length; ++i)
        {
            var foo = new Benchmark.FBBench.FS.Foo
            {
                Id = 0xABADCAFEABADCAFE + (ulong)i,
                Count = (short)(10000 + i),
                Prefix = (sbyte)('@' + i),
                Length = (uint)(1000000 + i)
            };

            
            var bar = new Benchmark.FBBench.FS.Bar
            {
                Parent = foo,
                Ratio = 3.14159f + i,
                Size = (ushort)(10000 + i),
                Time = 123456 + i
            };

            
            var fooBar = new Benchmark.FBBench.FS.FooBar
            {
                Name = System.Guid.NewGuid().ToString(),
                Postfix = (byte)('!' + i),
                Rating = 3.1415432432445543543 + i,
                Sibling = bar,
            };

            foobars.Add(fooBar);
        }

        container = new Benchmark.FBBench.FS.FooBarContainer
        {
            Fruit = 123,
            Initialized = true,
            Location = "http://google.com/flatbuffers/",
            List = foobars,
        };
    }

        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TraverseFooBarContainer(Benchmark.FBBench.FS.FooBarContainer foobar, int iterations)
    {
        int sum = 0;

        for (int loop = 0; loop < iterations; ++loop)
        {
            sum += foobar.Initialized ? 1 : 0;
            sum += foobar.Location.Length;
            sum += foobar.Fruit;

            var list = foobar.List;
            int count = list.Count;

            for (int i = 0; i < count; ++i)
            {
                var item = list[i]; 
                sum += item.Name.Length;
                sum += (int)item.Postfix;
                sum += (int)item.Rating;

                var bar = item.Sibling;
                sum += (int)bar.Ratio;
                sum += (int)bar.Size;
                sum += bar.Time;

                var parent = bar.Parent;
                sum += parent.Count;
                sum += (int)parent.Id;
                sum += (int)parent.Length;
                sum += parent.Prefix;
            }
        }

        return sum;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TraverseFooBarContainerPartial(Benchmark.FBBench.FS.FooBarContainer foobar, int iterations)
    {
        int sum = 0;

        for (int loop = 0; loop < iterations; ++loop)
        {
            sum += foobar.Initialized ? 1 : 0;
            sum += foobar.Location.Length;
            sum += foobar.Fruit;

            var list = foobar.List;
            int count = list.Count;

            for (int i = 0; i < count; ++i)
            {
                var item = list[i];
                sum += item.Name.Length;

                var bar = item.Sibling;
                sum += (int)bar.Ratio;

                var parent = bar.Parent;
                sum += parent.Count;
            }
        }

        return sum;
    }

    
    
    public static void Prepare(int length, out Benchmark.FBBench.FS.FooBarContainerValue container)
    {
        var foobars = new List<Benchmark.FBBench.FS.FooBarValue>();
        for (int i = 0; i < length; ++i)
        {
            var foo = new Benchmark.FBBench.FS.FooValue
            {
                Id = 0xABADCAFEABADCAFE + (ulong)i,
                Count = (short)(10000 + i),
                Prefix = (sbyte)('@' + i),
                Length = (uint)(1000000 + i)
            };

            
            var bar = new Benchmark.FBBench.FS.BarValue
            {
                Parent = foo,
                Ratio = 3.14159f + i,
                Size = (ushort)(10000 + i),
                Time = 123456 + i
            };

            
            var fooBar = new Benchmark.FBBench.FS.FooBarValue
            {
                Name = System.Guid.NewGuid().ToString(),
                Postfix = (byte)('!' + i),
                Rating = 3.1415432432445543543 + i,
                Sibling = bar,
            };

            foobars.Add(fooBar);
        }

        container = new Benchmark.FBBench.FS.FooBarContainerValue
        {
            Fruit = 123,
            Initialized = true,
            Location = "http://google.com/flatbuffers/",
            List = foobars,
        };
    }

        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TraverseFooBarContainer(Benchmark.FBBench.FS.FooBarContainerValue foobar, int iterations)
    {
        int sum = 0;

        for (int loop = 0; loop < iterations; ++loop)
        {
            sum += foobar.Initialized ? 1 : 0;
            sum += foobar.Location.Length;
            sum += foobar.Fruit;

            var list = foobar.List;
            int count = list.Count;

            for (int i = 0; i < count; ++i)
            {
                var item = list[i]; 
                sum += item.Name.Length;
                sum += (int)item.Postfix;
                sum += (int)item.Rating;

                var bar = item.Sibling;
                sum += (int)bar.Ratio;
                sum += (int)bar.Size;
                sum += bar.Time;

                var parent = bar.Parent;
                sum += parent.Count;
                sum += (int)parent.Id;
                sum += (int)parent.Length;
                sum += parent.Prefix;
            }
        }

        return sum;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TraverseFooBarContainerPartial(Benchmark.FBBench.FS.FooBarContainerValue foobar, int iterations)
    {
        int sum = 0;

        for (int loop = 0; loop < iterations; ++loop)
        {
            sum += foobar.Initialized ? 1 : 0;
            sum += foobar.Location.Length;
            sum += foobar.Fruit;

            var list = foobar.List;
            int count = list.Count;

            for (int i = 0; i < count; ++i)
            {
                var item = list[i];
                sum += item.Name.Length;

                var bar = item.Sibling;
                sum += (int)bar.Ratio;

                var parent = bar.Parent;
                sum += parent.Count;
            }
        }

        return sum;
    }

    
    
    public static void Prepare(int length, out Benchmark.FBBench.ReflectionBased.FooBarListContainer container)
    {
        var foobars = new List<Benchmark.FBBench.ReflectionBased.FooBar>();
        for (int i = 0; i < length; ++i)
        {
            var foo = new Benchmark.FBBench.ReflectionBased.Foo
            {
                Id = 0xABADCAFEABADCAFE + (ulong)i,
                Count = (short)(10000 + i),
                Prefix = (sbyte)('@' + i),
                Length = (uint)(1000000 + i)
            };

            
            var bar = new Benchmark.FBBench.ReflectionBased.Bar
            {
                Parent = foo,
                Ratio = 3.14159f + i,
                Size = (ushort)(10000 + i),
                Time = 123456 + i
            };

            
            var fooBar = new Benchmark.FBBench.ReflectionBased.FooBar
            {
                Name = System.Guid.NewGuid().ToString(),
                Postfix = (byte)('!' + i),
                Rating = 3.1415432432445543543 + i,
                Sibling = bar,
            };

            foobars.Add(fooBar);
        }

        container = new Benchmark.FBBench.ReflectionBased.FooBarListContainer
        {
            Fruit = 123,
            Initialized = true,
            Location = "http://google.com/flatbuffers/",
            List = foobars,
        };
    }

        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TraverseFooBarContainer(Benchmark.FBBench.ReflectionBased.FooBarListContainer foobar, int iterations)
    {
        int sum = 0;

        for (int loop = 0; loop < iterations; ++loop)
        {
            sum += foobar.Initialized ? 1 : 0;
            sum += foobar.Location.Length;
            sum += foobar.Fruit;

            var list = foobar.List;
            int count = list.Count;

            for (int i = 0; i < count; ++i)
            {
                var item = list[i]; 
                sum += item.Name.Length;
                sum += (int)item.Postfix;
                sum += (int)item.Rating;

                var bar = item.Sibling;
                sum += (int)bar.Ratio;
                sum += (int)bar.Size;
                sum += bar.Time;

                var parent = bar.Parent;
                sum += parent.Count;
                sum += (int)parent.Id;
                sum += (int)parent.Length;
                sum += parent.Prefix;
            }
        }

        return sum;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TraverseFooBarContainerPartial(Benchmark.FBBench.ReflectionBased.FooBarListContainer foobar, int iterations)
    {
        int sum = 0;

        for (int loop = 0; loop < iterations; ++loop)
        {
            sum += foobar.Initialized ? 1 : 0;
            sum += foobar.Location.Length;
            sum += foobar.Fruit;

            var list = foobar.List;
            int count = list.Count;

            for (int i = 0; i < count; ++i)
            {
                var item = list[i];
                sum += item.Name.Length;

                var bar = item.Sibling;
                sum += (int)bar.Ratio;

                var parent = bar.Parent;
                sum += parent.Count;
            }
        }

        return sum;
    }

    
    
    public static void Prepare(int length, out Benchmark.FBBench.GFB.FooBarContainerT container)
    {
        var foobars = new List<Benchmark.FBBench.GFB.FooBarT>();
        for (int i = 0; i < length; ++i)
        {
            var foo = new Benchmark.FBBench.GFB.FooT
            {
                Id = 0xABADCAFEABADCAFE + (ulong)i,
                Count = (short)(10000 + i),
                Prefix = (sbyte)('@' + i),
                Length = (uint)(1000000 + i)
            };

            
            var bar = new Benchmark.FBBench.GFB.BarT
            {
                Parent = foo,
                Ratio = 3.14159f + i,
                Size = (ushort)(10000 + i),
                Time = 123456 + i
            };

            
            var fooBar = new Benchmark.FBBench.GFB.FooBarT
            {
                Name = System.Guid.NewGuid().ToString(),
                Postfix = (byte)('!' + i),
                Rating = 3.1415432432445543543 + i,
                Sibling = bar,
            };

            foobars.Add(fooBar);
        }

        container = new Benchmark.FBBench.GFB.FooBarContainerT
        {
            Fruit = 123,
            Initialized = true,
            Location = "http://google.com/flatbuffers/",
            List = foobars,
        };
    }

        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TraverseFooBarContainer(Benchmark.FBBench.GFB.FooBarContainerT foobar, int iterations)
    {
        int sum = 0;

        for (int loop = 0; loop < iterations; ++loop)
        {
            sum += foobar.Initialized ? 1 : 0;
            sum += foobar.Location.Length;
            sum += foobar.Fruit;

            var list = foobar.List;
            int count = list.Count;

            for (int i = 0; i < count; ++i)
            {
                var item = list[i]; 
                sum += item.Name.Length;
                sum += (int)item.Postfix;
                sum += (int)item.Rating;

                var bar = item.Sibling;
                sum += (int)bar.Ratio;
                sum += (int)bar.Size;
                sum += bar.Time;

                var parent = bar.Parent;
                sum += parent.Count;
                sum += (int)parent.Id;
                sum += (int)parent.Length;
                sum += parent.Prefix;
            }
        }

        return sum;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TraverseFooBarContainerPartial(Benchmark.FBBench.GFB.FooBarContainerT foobar, int iterations)
    {
        int sum = 0;

        for (int loop = 0; loop < iterations; ++loop)
        {
            sum += foobar.Initialized ? 1 : 0;
            sum += foobar.Location.Length;
            sum += foobar.Fruit;

            var list = foobar.List;
            int count = list.Count;

            for (int i = 0; i < count; ++i)
            {
                var item = list[i];
                sum += item.Name.Length;

                var bar = item.Sibling;
                sum += (int)bar.Ratio;

                var parent = bar.Parent;
                sum += parent.Count;
            }
        }

        return sum;
    }

    
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TraverseFooBarContainer(Benchmark.FBBench.PB.FooBarContainer foobar, int iterations)
    {
        int sum = 0;

        for (int loop = 0; loop < iterations; ++loop)
        {
            sum += foobar.Initialized ? 1 : 0;
            sum += foobar.Location.Length;
            sum += foobar.Fruit;

            var list = foobar.List;
            int count = list.Count;

            for (int i = 0; i < count; ++i)
            {
                var item = list[i]; 
                sum += item.Name.Length;
                sum += (int)item.Postfix;
                sum += (int)item.Rating;

                var bar = item.Sibling;
                sum += (int)bar.Ratio;
                sum += (int)bar.Size;
                sum += bar.Time;

                var parent = bar.Parent;
                sum += parent.Count;
                sum += (int)parent.Id;
                sum += (int)parent.Length;
                sum += parent.Prefix;
            }
        }

        return sum;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TraverseFooBarContainerPartial(Benchmark.FBBench.PB.FooBarContainer foobar, int iterations)
    {
        int sum = 0;

        for (int loop = 0; loop < iterations; ++loop)
        {
            sum += foobar.Initialized ? 1 : 0;
            sum += foobar.Location.Length;
            sum += foobar.Fruit;

            var list = foobar.List;
            int count = list.Count;

            for (int i = 0; i < count; ++i)
            {
                var item = list[i];
                sum += item.Name.Length;

                var bar = item.Sibling;
                sum += (int)bar.Ratio;

                var parent = bar.Parent;
                sum += parent.Count;
            }
        }

        return sum;
    }

    }
