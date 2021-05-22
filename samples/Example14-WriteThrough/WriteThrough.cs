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

namespace Samples.WriteThrough
{
    using System;
    using System.Diagnostics;
    using FlatSharp;

    /// <summary>
    /// FlatSharp supports Write-Through in limited cases:
    /// - Serialization method is VectorCacheMutable
    /// - Struct field has been opted into write-through.
    /// 
    /// Write Through allows you to make updates to an already-serialized FlatBuffer in-place without a full parse or re-serialize.
    /// This is extremely performant, especially for large buffers as it avoids series of copies and allows in-place updates.
    /// </summary>
    public class WriteThroughSample
    {
        public static void Run()
        {
            Roster roster = new Roster
            {
                Walkers = new[]
                {
                    new DogWalker { Name = "Beth", CurrentDog = new DogReference { Id = 7 }, Position = new Location { Latitude = 1.2, Longitude = 2.1, NoWriteThrough = 10 } },
                    new DogWalker { Name = "Jerry", CurrentDog = new DogReference { Id = 127 }, Position = new Location { Latitude = 12.7, Longitude = 128 } },
                    new DogWalker { Name = "Summer", CurrentDog = new DogReference { Id = 22 }, Position = new Location { Latitude = 36.4, Longitude = 32.1 } },
                }
            };

            // Write the data to the buffer the first time.
            byte[] data = new byte[1024];
            Roster.Serializer.Write(data, roster);

            // Parsed now refers to the buffer. Any changes we make to the fs_writeThrough fields will be reflected back.
            Roster parsed = Roster.Serializer.Parse(data);
            var walker = parsed.Walkers![0];

            // Walking dog 8 in Tokyo now.
            walker.CurrentDog!.Id = 8;
            walker.Position!.Latitude = 35.683;
            walker.Position.Longitude = 139.808;

            // we can update this field, but since writethrough is disabled, the changes won't be reflected in the buffer.
            walker.Position.NoWriteThrough = 1024; 

            // Now let's parse again and see our changes.
            // Notice that we never invoked .Write to rewrite the whole structure back.
            // We only mutated a few fields.
            parsed = Roster.Serializer.Parse(data);

            walker = parsed.Walkers![0];

            Debug.Assert(walker.CurrentDog!.Id == 8, "Dog has been updated. Was 7 originally.");
            Debug.Assert(walker.Position!.Latitude == 35.683, "Latitude has been updated. Was 1.2 originally.");
            Debug.Assert(walker.Position!.Longitude == 139.808, "Longitude has been updated. Was 2.1 originally.");
            Debug.Assert(walker.Position!.NoWriteThrough == 10, "NoWriteThrough was unchanged.");
        }
    }
}
