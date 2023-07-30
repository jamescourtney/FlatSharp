// See https://aka.ms/new-console-template for more information
using FlatSharp;
using FlatSharpEndToEndTests.FileIdentifiers;

Console.WriteLine("Hello, World!");

FlatSharpEndToEndTests.FileIdentifiers.HasId id = new() { Foo = 32 };
FlatSharpEndToEndTests.FileIdentifiers.HasId.Serializer.Write(new byte[100], id);

FlatSharpEndToEndTests.FileIdentifiers.NoId noId = new();

NoId.Serializer.Write(new byte[100], new());