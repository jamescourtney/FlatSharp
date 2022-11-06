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

namespace Samples.SortedVectors;

/// <summary>
/// This example shows how to use FlatSharp to generated sorted vectors. this can be handy in situations where you
/// expect to read and look up data much more than you expect to write it, since sorting happens each time you 
/// serialize. Only Vectors of Tables are eligible to be sorted, and those tables must have a primitive type key 
/// specified. This example uses C# Attributes to define everything, but there is an equivalent example in this 
/// directory using an FBS file.
/// </summary>
public class SortedVectorsExample : IFlatSharpSample
{
    public void Run()
    {
        UserList userList = new UserList
        {
            Users = new[]
            {
                new User { FirstName = "Jason", LastName = "Bourne", SSN = "234-56-7890"},
                new User { FirstName = "James", LastName = "Bond", SSN = "123-45-6789"},
                new User { FirstName = "Austin", LastName = "Powers", SSN = "International Man of Mystery" }
            }
        };

        byte[] data = new byte[1024];
        int bytesWritten = UserList.Serializer.Write(data, userList);
        UserList parsedList = UserList.Serializer.Parse(data);

        Assert.True(parsedList.Users is not null, "");

        foreach (var u in parsedList.Users)
        {
            Console.WriteLine($"{u.FirstName} {u.LastName} {u.SSN}");
        }

        // Similarly, we can use binary search. You're encouraged to use the FlatSharp binary search method
        // since FlatBuffers uses a different string sorting algorithm than .NET does by default. If you must use
        // your own binary search, use FlatBufferStringComparer.
        User? user = parsedList.Users.BinarySearchByFlatBufferKey("234-56-7890");
        Assert.True(user?.LastName == "Bourne", "Binary search works!");
    }
}
