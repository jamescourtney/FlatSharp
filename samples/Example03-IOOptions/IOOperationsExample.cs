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

namespace Samples.IOOptionsExample;

/// <summary>
/// This sample shows some different IO Options when using FlatSharp.
/// </summary>
public class IOOperationsExample : IFlatSharpSample
{
    public bool HasConsoleOutput => false;

    public void Run()
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

        // ISpanWriter is the core code that writes data to a span. Flatsharp provides one: SpanWriter
        // However, you can always implement ISpanWriter yourself.
        SpanWriter spanWriter = new SpanWriter();

        // Allocate a new buffer that can hold this person.
        byte[] buffer = new byte[Person.Serializer.GetMaxSize(person)];

        // Write the person into the buffer using the given spanwriter.
        int bytesWritten = Person.Serializer.Write(spanWriter, buffer, person);

        // For reading data, we use InputBuffer. There are more options here:
        // - ArrayInputBuffer for regular arrays
        // - ArraySegmentInputBuffer for ArraySegment<byte>
        // - MemoryInputBuffer for Memory<byte>
        // - ReadOnlyMemoryInputBuffer for ReadOnlyMemory<byte>. This one won't work for WriteThrough
        //   or objects that have a Memory<byte> vector in them.

        var p1 = Person.Serializer.Parse(new ArrayInputBuffer(buffer));
        var p2 = Person.Serializer.Parse(new ArraySegmentInputBuffer(new ArraySegment<byte>(buffer)));
        var p3 = Person.Serializer.Parse(new MemoryInputBuffer(new Memory<byte>(buffer)));
        var p4 = Person.Serializer.Parse(new ReadOnlyMemoryInputBuffer(new ReadOnlyMemory<byte>(buffer)));
    }
}
