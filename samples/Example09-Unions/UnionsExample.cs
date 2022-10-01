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
        Fish brick = new Fish { Kind = FishKind.Coelacanth, Name = "Brick" };

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

        // Similiarly, FlatSharp unions also include a method called "Switch".
        // You can use this instead of a switch statement. The advantage is that
        // if a union element is added later, your code will fail to compile, so
        // it helps you to handle all of the cases.
        string breed = pet.Switch(
            caseDefault: () => "unknown",
            caseDoggo: d => d.Breed.ToString(),
            caseCat: c => c.Breed.ToString(),
            caseFish: f => f.Kind.ToString());
         
        // Finally, each union can accept a visitor. Using structs as visitors
        // will allow you to benefit from devirtualization if performance is a concern.
        string? name = pet.Accept<PetNameVisitor, string?>(new PetNameVisitor());
    }

    /// <summary>
    /// You can also use unions from C#. However, the semantics are not as nice.
    /// </summary>
    [FlatBufferTable]
    public class PersonCS
    {
        [FlatBufferItem(0)]
        public virtual FlatBufferUnion<Dog, Cat, Fish>? Pet { get; set; }

        public void UsePet()
        {
            if (this.Pet is null)
            {
                return;
            }

            var union = this.Pet.Value;

            if (union.TryGet(out Dog? dog))
            {
                // Woof woof
            }
            else if (union.TryGet(out Cat? cat))
            {
                // Meow meow
            }
            else if (union.TryGet(out Fish? fish))
            {
                // gurgle gurgle
            }

            // C# unions still support the switch method above:
            union.Switch(
                defaultCase: () => { },
                case1: (Dog d) => { },
                case2: (Cat c) => { },
                case3: (Fish f) => { });

            string? name = union.Accept<PetNameVisitor, string?>(new PetNameVisitor());
        }
    }

    // Pet.Visitor<string?> implements IFlatBufferUnionVisitor<string?, Dog, Cat, Fish>
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
    }
}
