namespace Samples.IOOptionsExample
{
    using System;
    using System.Collections.Generic;
    using FlatSharp;
    using FlatSharp.Unsafe;

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

            // RIP
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
                FavoritePet = new FlatBufferUnion<Dog, Cat>(rocket),
                Name = "Nikola Tesla"
            };

            // SpanWriter is the core code that writes data to a span. Flatsharp provides a couple:
            SpanWriter spanWriter = new SpanWriter();
            SpanWriter unsafeSpanWriter = new UnsafeSpanWriter();

            byte[] buffer = new byte[Person.Serializer.GetMaxSize(person)];

            int bytesWritten = Person.Serializer.Write(spanWriter, buffer, person);
            bytesWritten = Person.Serializer.Write(unsafeSpanWriter, buffer, person);

            // For reading data, we use InputBuffer. There are more options here:
            var p1 = Person.Serializer.Parse(new ArrayInputBuffer(buffer));
            var p2 = Person.Serializer.Parse(new UnsafeArrayInputBuffer(buffer));
            var p3 = Person.Serializer.Parse(new MemoryInputBuffer(new Memory<byte>(buffer)));
            using (var unsafeMemoryInput = new UnsafeMemoryInputBuffer(new Memory<byte>(buffer)))
            {
                // Unsafe memory input buffer must be disposed of, otherwise it'll end up on the finalizer queue.
                var p4 = Person.Serializer.Parse(unsafeMemoryInput);
            }
        }
    }
}
