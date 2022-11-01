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

namespace Samples.Unions;

/// <summary>
/// This example shows how to use FlatSharp with unions. FlatBuffer unions are 
/// discriminated unions, which means exactly one of the fields may be set.
/// 
/// FlatSharp suppo
/// </summary>
public class UnionsExample
{
    public static void Run()
    {
        Cat simon = new Cat { Breed = CatBreed.Bengal, Name = "Simon" };
        Dog george = new Dog { Breed = DogBreed.Corgi, Name = "George" };
        Fish brick = new Fish { Kind = FishKind.Puffer, Name = "Brick" };

        // A person can only have one pet. It can be a cat, dog, or fish.
        // This person has a fish named brick. You could model a person with multiple
        // pets as a vector of unions.
        Person person = new Person
        {
            Pet = new Pet(brick)
        };

        Pet pet = person.Pet.Value;

        // Each union has an internal enum with the general cases.
        if (pet.Kind == Pet.ItemKind.Doggo)
        {
            Dog dog = pet.Doggo;
            string? lowerName = dog.Name?.ToLowerInvariant();
        }

        // We can also use .TryGet:
        if (pet.TryGet(out Cat? cat))
        {
            // this was a cat.
        }

        // Accessing .Cat directly will throw because this is a Fish.
        try
        {
            Console.WriteLine(pet.Cat.Name);
            Debug.Assert(false);
        }
        catch (InvalidOperationException)
        {
        }

        // Finally, each union can accept a visitor. Using structs as visitors
        // will allow you to benefit from devirtualization if performance is a concern.
        string? name = pet.Accept<PetNameVisitor, string?>(new PetNameVisitor());
    }

    // Pet.Visitor<string?> implements IFlatBufferUnionVisitor<string?, Dog, Cat, Fish, string>
    public struct PetNameVisitor : Pet.Visitor<string?>
    {
        public string? Visit(Dog item)
        {
            return item.Name;
        }

        public string? Visit(Cat item)
        {
            return item.Name;
        }

        public string? Visit(Fish item)
        {
            return item.Name;
        }

        public string? Visit(string item)
        {
            return item;
        }
    }
}
