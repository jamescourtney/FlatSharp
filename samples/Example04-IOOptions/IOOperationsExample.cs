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

namespace Samples.IOOptionsExample
{
    using System;
    using System.Collections.Generic;
    using FlatSharp;

    /// <summary>
    /// This sample shows some different IO Options when using FlatSharp.
    /// </summary>
    public class IOOperationsExample
    {
        public static void Run()
        {
            Dog tony = new Dog
            {
                Breed = DogBreed.BostonTerrier,
                Vitals = new AnimalVitals { Age = 11, Gender = Gender.Male, Name = "Tony" }
            };
            Dog rocket = new Dog
            {
                Breed = DogBreed.GoldenRetriever,
                Vitals = new AnimalVitals { Age = 8, Gender = Gender.Female, Name = "Rocket" }
            };
            Dog peaches = new Dog
            {
                Breed = DogBreed.GermanShepard,
                Vitals = new AnimalVitals { Age = 14, Gender = Gender.Female, Name = "Peaches" }
            };

            Cat grumpyCat = new Cat
            {
                Breed = CatBreed.GrumpyCat,
                Vitals = new AnimalVitals { Age = 17, Gender = Gender.Female, Name = "Tardar Sauce" }
            };

            Person person = new Person
            {
                Age = 24,
                Cats = new[] { grumpyCat },
                Dogs = new[] { tony, rocket, peaches },
                FavoritePet = new FavoritePet(rocket),
                Name = "Nikola Tesla"
            };

            // SpanWriter is the core code that writes data to a span. Flatsharp provides one:
            // However, you can always implement ISpanWriter yourself.
            SpanWriter spanWriter = default(SpanWriter);

            byte[] buffer = new byte[Person.Serializer.GetMaxSize(person)];

            int bytesWritten = Person.Serializer.Write(spanWriter, buffer, person);

            // For reading data, we use InputBuffer. There are more options here:

            // Array and Memory input buffers are general purpose and support all scenarios.
            var p1 = Person.Serializer.Parse(new ArrayInputBuffer(buffer));
            var p2 = Person.Serializer.Parse(new MemoryInputBuffer(new Memory<byte>(buffer)));

            // ReadOnlyMemory input buffer will fail to Parse any objects that have Memory<T> in them (that is -- non read only memory).
            var p3 = Person.Serializer.Parse(new ReadOnlyMemoryInputBuffer(new ReadOnlyMemory<byte>(buffer)));
        }
    }
}
