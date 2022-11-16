/*
 * Copyright 2021 James Courtney
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

namespace Samples.ValueStructs;

/// <summary>
/// Beginning in FlatSharp version 5.7.0, FlatBuffer structs can be declared as value types (ie, C# structs).
/// Value types offer many advantages, such as reduced garbage collection and predictable memory layouts for interop.
/// 
/// However, value structs are not as fully featured as reference structs and can require some care when programming. 
/// In particular, the following are unsupported:
/// - Deserialization modes (value structs are individually deserialized greedily).
/// - IFlatBufferDeserializedObject and other convenience interfaces.
/// 
/// However, Value Structs offer some compelling advantages over reference type structs:
/// - Serialization and parsing can be done with a memcopy operation (on little-endian architectures) instead of field-by-field.
/// - They do not incur any GC allocations, though large structs may take longer to copy.
/// - They can be used for P/Invoke interop with other languages.
/// 
/// The Struct Vectors sample has examples of using struct vectors with value-type structs.
/// </summary>
public class ValueStructsSample : IFlatSharpSample
{
    public void Run()
    {
        Path path = new Path()
        {
            Points = new List<Point>(),
        };

        // Value type struct!
        Assert.True(typeof(Point).IsValueType, "Point is definitely a value type");

        // Add some points to our path.
        for (int i = 0; i < 1000; ++i)
        {
            path.Points.Add(new Point
            {
                X = i,
                Y = i + 1,
                Z = i + 2
            });
        }

        // Try to modify a point.
        PrintPoint(path.Points, 30);  // Prints: (30, 31, 32)
        Point p30 = path.Points[30];
        p30.X = 7;
        PrintPoint(path.Points, 30); // Prints: (30, 31, 32)

        // The value didn't change here. What happened? Remember, Point here is a *value* type, so 'p30' is a
        // full copy of the item in the vector. We modified p30.X to 7, but that only modified our copy.
        // To complete the update, we must do the following:
        path.Points[30] = p30;
        PrintPoint(path.Points, 30); // Prints: (7, 31, 32)

        byte[] data = new byte[Path.Serializer.GetMaxSize(path)];
        Path.Serializer.Write(data, path);

        // Value-types are parsed greedily *when they are accessed*. The serializer here is lazy,
        // which means that we haven't preparsed anything, so these points will be parsed as they are accessed.
        // This means that when parsed.Points[3] is accessed, all of X, Y, and Z will be deserialized at once. However,
        // Points[4] will not be read until that index is accessed.
        // This is one of the major differences between FlatSharp's value and reference type struct implementations.
        // If this were a reference type with virtual properties, X, Y, and Z would be deserialized individually
        // instead of in bulk. This may be good or bad, depending on your use case (and the size of the struct).
        Path parsed = Path.Serializer.Parse(data);

        Assert.True(parsed.Points!.Count == 1000, "The count is as expected");
    }

    private static void PrintPoint(IList<Point> points, int index)
    {
        var point = points[index];
        Console.WriteLine($"Points[{index}]: ({point.X}, {point.Y}, {point.Z})");
    }
}
