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

using FlatSharp;
using FlatSharp.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Samples.Unions
{
    /// <summary>
    /// This example shows how to use FlatSharp with unions. FlatBuffer unions are discriminated unions, which means exactly one of the fields may be set.
    /// FlatSharp can 
    /// </summary>
    public class UnionsExample
    {
        public static void Run()
        {
            Cat simon = new Cat { Breed = CatBreed.Bengal, Name = "Simon" };
            Dog george = new Dog { Breed = DogBreed.Corgi, Name = "George" };
            Fish brick = new Fish { Kind = FishKind.Coelacanth, Name = "Brick" };

            Person person = new Person
            {
                // The difference between Person.Pet and Person.BasicPet is that Person.Pet is
                // a custom convenience class that derives from FlatBufferUnion. In 
                Pet = new Pet(brick)
            };

            UsePet(person.Pet);
        }

        // These two methods do the same thing. They are coded separately to
        // demonstrate the usability you get with FlatSharp's compiled union
        // types.

        public static void UsePet(Pet pet)
        {
            // Enum members
            if (pet.Kind == Pet.ItemKind.Doggo)
            {
                Dog dog = pet.Doggo;
                string? lowerName = dog.Name?.ToLowerInvariant();
            }

            string breed = pet.Switch(
                caseDefault: () => "unknown",
                caseDoggo: d => d.Breed.ToString(),
                caseCat: c => c.Breed.ToString(),
                caseFish: f => f.Kind.ToString());
        }
    }
}
