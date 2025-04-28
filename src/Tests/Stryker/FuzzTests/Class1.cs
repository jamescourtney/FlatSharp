namespace FuzzTests;

using FlatSharp;
using FlatSharpStrykerTests;
using SharpFuzz;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Any(x => x == "--debug"))
        {
            Debugger.Launch();
        }

        if (args.Length > 0)
        {
            if (args[0] == "GenerateData")
            {
                Root root = Root.CreateTestRoot<SimpleListFactory>();
                byte[] temp = new byte[Root.Serializer.GetMaxSize(root)];
                int bytesWritten = Root.Serializer.Write(temp, root);

                Directory.CreateDirectory("TestCases");
                File.WriteAllBytes("TestCases/TestCase.bin", temp.AsSpan()[..bytesWritten]);
            }
            else
            {
                using FileStream fs = File.OpenRead(args[0]);
                Run(fs);
            }
        }
        else
        {
            Fuzzer.OutOfProcess.Run(stream =>
            {
                Run(stream);
            });
        }
    }

    private static void Run(Stream stream)
    {
        using MemoryStream ms = new();
        stream.CopyTo(ms);
        byte[] bytes = ms.ToArray();

        try
        {
            var clone = new Root(Root.Serializer.Parse(bytes, FlatBufferDeserializationOption.GreedyMutable));
        }
        catch (IndexOutOfRangeException)
        {
        }
        catch (ArgumentOutOfRangeException)
        {
        }
        catch (NotMutableException)
        {
        }
        catch (BufferTooSmallException)
        {
        }
        catch (InvalidDataException)
        {
        }
        catch (InvalidOperationException)
        {
        }
        catch (ArgumentException)
        {
        }
        catch (OverflowException)
        {
        }
    }

    private class SimpleListFactory : ICreateCreateListFactory
    {
        public static CreateListCallback<T> GetCallback<T>() => items => new List<T>(items);
    }
}
