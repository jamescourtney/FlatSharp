## Welcome to FlatSharp!

Thanks for using FlatSharp! FlatSharp is a C# FlatBuffers implementation designed to be secure, fast, and easy to use.

### Quick Resources
- [FlatSharp Samples](https://github.com/jamescourtney/FlatSharp/tree/main/samples)
- [FlatSharp Wiki](https://github.com/jamescourtney/FlatSharp/wiki)
- [FlatBuffers Overview](https://google.github.io/flatbuffers/index.html#flatbuffers_overview)
- [FlatBuffers Schema Authoring](https://google.github.io/flatbuffers/flatbuffers_guide_writing_schema.html)

### Issues, Contributions, and Feedback
FlatSharp is open source. Find it on [GitHub](https://github.com/jamescourtney/FlatSharp)! Issues, contributions, and other feedback are always welcome. Don't be a stranger! If you have feedback to share, please consider filling out the form [here](https://forms.office.com/r/sHkumrr6sK)!

### Sponsorship
FlatSharp is free and always will be. However, the project does take a significant amount of time to maintain. If you or your organization find the project useful, please consider a [Github sponsorship](https://github.com/sponsors/jamescourtney). Any amount is appreciated!

### Quick Start

#### 1. Reference FlatSharp
Reference both `FlatSharp.Runtime` and `FlatSharp.Compiler`. Use the same version for them both.

#### 2. Define a Schema

```idl
// all FlatSharp FBS attributes start with the 'fs_' prefix.
attribute "fs_serializer";

namespace MyNamespace;

enum Color : ubyte { Red = 1, Green, Blue }

table Person (fs_serializer) {
    Id:int;
    Name:string;
    Parent:Person (deprecated);
    Children:[Person];
    FavoriteColor:Color = Blue;
    Position:Location;
}

struct Location {
    Latitude:float;
    Longitude:float;
}
```

#### 3. Update Your csproj
```xml
   <ItemGroup>
     <FlatSharpSchema Include="YourSchema.fbs" />
   </ItemGroup>
```

#### 4. Serialize Your Data
```c#
Person person = new Person(...);
int maxBytesNeeded = Person.Serializer.GetMaxSize(person);
byte[] buffer = new byte[maxBytesNeeded];
int bytesWritten = Person.Serializer.Serialize(buffer, person);
```

#### 5. Parse Your Data

```c#
Person p = Person.Serializer.Parse(data);
```

### License
FlatSharp is licensed under Apache 2.0. Have fun :)
