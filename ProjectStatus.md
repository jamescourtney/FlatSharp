### Status of FlatSharp

Briefly, FlatSharp is a mature project and is actively supported. This means that while the author is not continually adding new features, the project is monitored, questions are answered, and issues are addressed.

To expand on this, the FlatBuffers format does not undergo frequent updates. As FlatSharp has gotten closer and closer to feature parity with the canonical library, there are really two things to work on:
1) New usability features, such as `IIndexedVector`, value structs, etc.
2) Performance improvements

FlatSharp's main goal is to make a FlatBuffers implementation that is safe, idiomatic, and performant (in roughly that order). These restrictions also limit the amount of work there is to do in FlatSharp.

#### Usability Improvements
These are hard to find. Not to say that FlatSharp is perfect (it isn't), but it is difficult to identity areas for improvement without feedback and customer usage data. Many of the recent features in FlatSharp have been driven by feedback from users (notably value-type structs). If you have a suggestion for how to make FlatSharp better, please open an issue or submit an idea. Contributions are also very welcome!

#### Performance Improvements
FlatSharp is generally well optimized, though you can find performance improvements if you're willing to dig around.
Unfortunately, many of these involve relaxing the "safe code" goal. Without other contributors who can provide meaningful code reviews, FlatSharp needs to continue to run in "safe" mode with overflow checks as a way to ensure that the library is not responsible for security issues.
Much of the attention FlatSharp receives today is around prototyping prospective performance improvements, which are only merged into the main branch if they meet the safety criteria and improve performance. To give a flavor of things that have been tried:
- Vectorizing certain operations in `GetMaxSize` (~4 nanosecond improvement, questionable safety regarding overflow checks. Rejected.)
- Using bitflags to serialize small tables to reduce the number of branches (code size increase cancels out branches, roughly same performance. Rejected.)
- Pre-reading small vtables to remove a branch on the deserialize path (Modest improvement in the case where the removed branches were not well predicted. Merged.)

FlatSharp is mostly at the point now where the best way to get a performance uplift is to move to the next version of .NET, since improvements in the JIT will help FlatSharp across the board. However, if you do see an area for improvement that doesn't violate the safety restriction, please contribute it back!

#### What about Extensions to FlatBuffers?
FlatSharp has taken a consistent position against extending FlatBuffers. There are two risks to extending the format:
1) FlatSharp adds something (ie, `DateTime` support) that FlatBuffers adds later, but in a different way. Imagine FlatSharp serialzes `DateTime`s as strings but FlatBuffers uses UTC milliseconds.
2) FlatSharp ends up diverging from the format significantly. This means that FlatSharp would be a separate binary format that looks a bit like FlatBuffers. Such a change would reduce FlatSharp's usefulness.

If you're reading this and are intererested in extending the FlatBuffers format, you should start with the official FlatBuffer repository. Get signoff there and get C# support merged, and then FlatSharp will be happy to accept a contribution.



