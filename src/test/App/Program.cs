// See https://aka.ms/new-console-template for more information
using FlatSharp;

Console.WriteLine("Hello, World!");

FlatSharpEndToEndTests.FileIdentifiers.HasId id = new() { Foo = 32 };
FlatSharpEndToEndTests.FileIdentifiers.HasId.Serializer.Write(new byte[100], id);