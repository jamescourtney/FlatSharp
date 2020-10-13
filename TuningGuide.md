# FlatSharp Performance Tuning Guide

If you're using FlatSharp, you probably care about performance. While the samples project covers aspects of performance, it's also useful to consolidate
recommendations into one place. FlatSharp offers lots of configuration knobs for you to choose from. These will each have impacts on how your code performs. 

## FlatSharp: How it works
Before we get into a full breakdown of the different performance options, it's useful to have some context on how FlatSharp actually works.
FlatSharp treats tables and structs similiarly internally. Let's pretend we have this struct:

```c#
[FlatBufferStruct]
public class Location
{
  [FlatBufferItem(0)] public virtual float X { get; set; }
  [FlatBufferItem(1)] public virtual float Y { get; set; }
  [FlatBufferItem(2)] public virtual float Z { get; set; }
}

```

When serializing, FlatSharp will generate some code that looks approximately like this:
```c#
public static void WriteLocation<TSpanWriter>(
  TSpanWriter spanWriter, 
  Span<byte> span, 
  Location value, 
  int offset, 
  SerializationContext context) where TSpanWriter : ISpanWriter
{
  spanWriter.WriteFloat(span, value.X, (offset + 0), context);
  spanWriter.WriteFloat(span, value.Y, (offset + 4), context);
  spanWriter.WriteFloat(span, value.Z, (offset + 8), context);
}
```
Reasonably simple: we're writing each field of the struct at the predefined offset relative to the base offset.

Deserializing is more interesting. When deserializing, FlatSharp will generate a subclass of ```Location``` that overrides ```X```, ```Y```, and ```Z```:

```c#
public class LocationReader<TInputBuffer> : Location where TInputBuffer : IInputBuffer
{
  ...

  public LocationReader(TInputBuffer buffer, int offset) { ... }
  
  public override float X
  {
    get => ...
    set => ...
  }
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static float ReadIndex0Value(TInputBuffer buffer, int offset) => buffer.ReadFloat(offset);
}
```
The deserialization code is generated differently depending on which Deserialization option is selected. However, when you parse an object with FlatSharp, you will get back 
a subclass of the type you requested. How that subclass is implemented depends upon the deserialization option that you select.

### ```GreedyMutable``` Deserialization
GreedyMutable deserialization is the simplest to understand. The full object graph is deserialized at once, and the input buffer is not needed after the fact. 
Code for a GreedyMutable deserializer looks like this:

```c#
public class LocationReader<TInputBuffer> : Location where TInputBuffer : IInputBuffer
{
  private float index0Value;

  public LocationReader(TInputBuffer buffer, int offset)
  {
    this.index0Value = ReadIndex0Value(buffer, (offset + 0));
  }
  
  public override float X
  {
    get => this.index0Value;
    
    // When using Greedy instead of GreedyMutable, setters throw a NotMutableException.
    set => this.index0Value = value;
  }
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static float ReadIndex0Value(TInputBuffer buffer, int offset) => buffer.ReadFloat(offset);
}
```
Notably, the ```buffer``` parameter is not retained after the constructor has finished, which means you are free to reuse it immediately after
the deserialization operation has concluded.

### ```Lazy``` Deserialization
```Lazy``` deserialization is the opposite of ```Greedy```. In ```Greedy``` mode, everything is preallocated and stored. In Lazy mode, nothing is preallocated or stored:

```c#
public class LocationReader<TInputBuffer> : Location where TInputBuffer : IInputBuffer
{
  private readonly TInputBuffer buffer;
  private readonly int offset;

  public LocationReader(TInputBuffer buffer, int offset)
  {
    this.buffer = buffer;
    this.offset = offset;
  }
  
  public override float X
  {
    get => ReadIndex0Value(this.buffer, this.offset + 0);
    
    // Lazy is always immutable.
    set => throw new NotMutableException();
  }
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static float ReadIndex0Value(TInputBuffer buffer, int offset) => buffer.ReadFloat(offset);
}
```

As we see here, ```Lazy``` is as advertised. Properties will only be read as they are accessed. Repeated accesses of the same property result in repeated trips
to the ```InputBuffer```. Crucially, ```Lazy``` maintains a reference to the ```InputBuffer```. If your access patterns are sparse, ```Lazy``` deserialization can
be very effective, since cycles are not wasted reading data that isn't used.

### ```PropertyCache``` Deserialization
PropertyCache can be thought of as Lazy-with-caching. The difference between ```Lazy``` and ```PropertyCache``` mode is that ```PropertyCache``` will 
[memoize](https://en.wikipedia.org/wiki/Memoization) the results of the reads from the underlying buffer.

```c#
public class LocationReader<TInputBuffer> : Location where TInputBuffer : IInputBuffer
{
  private readonly TInputBuffer buffer;
  private readonly int offset;
  
  private bool hasIndex0Value;
  private float index0Value;

  public LocationReader(TInputBuffer buffer, int offset)
  {
    this.buffer = buffer;
    this.offset = offset;
  }
  
  public override float X
  {
    get
    {
      if (!this.hasIndex0Value)
      {
        this.index0Value = ReadIndex0Value(this.buffer, this.offset + 0);
        this.hasIndex0Value = true;
      }
      
      return this.index0Value;
    }
    
    // PropertyCache is always immutable.
    set => throw new NotMutableException();
  }
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static float ReadIndex0Value(TInputBuffer buffer, int offset) => buffer.ReadFloat(offset);
}
```
So we see the primary difference between PropertyCache and Lazy is the addition of two fields in the generated class, as well as an ```if``` statement inside the getter.
For repeated accesses, ```PropertyCache``` is faster than lazy at the expense of more memory. For situations where fields are accessed at most once, ```PropertyCache``` will be 
slower than ```Lazy```.

Not shown in this example, ```PropertyCache``` will never preallocate vectors. If you need ```PropertyCache``` behavior with preallocated vectors, then look into the ```VectorCache``` option.

### Virtual / Non-Virtual Properties
Beginning in version 4.1.0, FlatSharp supports non-virtual properties:

```c#
[FlatBufferStruct]
public class 2DLocation
{
  [FlatBufferItem(0)] public virtual float X { get; set; }
  [FlatBufferItem(1)] public float Y { get; set; }
}
```

How does FlatSharp generate code for this scenario? The rules are:
- Any non-virtual properties are deserialized greedily. There is no way around this since FlatSharp cannot override these properties.
- Any virtual properties are deserialized according to the deserialization option. In the example below, we're assuming ```PropertyCache``` was used.

```c#
public class LocationReader<TInputBuffer> : 2DLocation where TInputBuffer : IInputBuffer
{
  private readonly TInputBuffer buffer;
  private readonly int offset;
  
  private bool hasIndex0Value;
  private float index0Value;

  public LocationReader(TInputBuffer buffer, int offset)
  {
    this.buffer = buffer;
    this.offset = offset;
    
    // Flatsharp just sets the base value. No extra fields or properties generated since the Y is non-virtual.
    base.Y = ReadIndex1Value(buffer, (offset + 4));
  }
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static float ReadIndex0Value(TInputBuffer buffer, int offset) => buffer.ReadFloat(offset);
  
  public override float X
  {
    get
    {
      if (!this.hasIndex0Value)
      {
        this.index0Value = ReadIndex0Value(this.buffer, this.offset + 0);
        this.hasIndex0Value = true;
      }
      
      return this.index0Value;
    }
    
    // PropertyCache is always immutable.
    set => throw new NotMutableException();
  }
  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static float ReadIndex1Value(TInputBuffer buffer, int offset) => buffer.ReadFloat(offset);
}
```

As you can see here, it's entirely possible to mix and match virtual and non-virtual properties. In addition to performance benefits of non-virtual methods, it also allows you to
mix-and-match greedy vs non-greedy deserialization.

## Performance Implications
So, we've seen what kind of code Flatsharp will generate for you depending on your configuration. When should you use which options? The best answer is, of course, to benchmark. 
However, answers to the following should help inform your choices.

#### Question 1: Are the default settings not fast enough?
FlatSharp is really fast, even with the default ```Greedy``` settings. Don't preemptively optimize. ```Greedy``` also works well
because it guarantees you can immediately recycle your ```InputBuffer``` object. Using ```greedy``` deserialization on buffers with lots of data can cause spikes in the
Garbage Collection since all of the objects are allocated at once, rather than getting amortized out as you use the buffer. ```Greedy``` is left as the default
because it is the most straightforward and most like other serialization libraries.

#### Question 2: Do you serialize or parse more often?
Some services are read-mostly, and some are write-mostly. If you're doing more serializing than parsing, consider making your properites non-virtual:
```c#
public int Foobar { get; set; }
```
Non-Virtual properties are faster to access than virtual ones. There is some overhead to virtual dispatches, so using non-virtual properties will be faster for serialization.
However, non-virtual properties by their nature can only be deserialized Greedily. Using non-virtual properties is an interesting way to mix lazy and greedy deserialization.

#### Question 3: When should I consider using ```Lazy``` deserialization?
Lazy is great when your access patterns are sparse and at-most-once. If you're touching individual properties more than once, then lazy will likely be slower than other options. 
Lazy also means that the deserialized objects carry references to the source buffer.

#### Question 4: What about ```PropertyCache``` and ```VectorCache```?
These two options represent a halfway point between ```Lazy``` and ```Greedy```. Data is read at-most-once, which is nice when access patterns cannot be anticipated, but full greedy
mode is not appropriate.

