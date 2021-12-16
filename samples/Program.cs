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

namespace Samples;

public static class Program
{
    public static void Main(string[] args)
    {
        MonsterAttributeExample.MonsterAttributeExample.Run();
        SerializerOptions.SerializerOptionsExample.Run();
        SchemaFilesExample.SchemaFilesExample.Run();
        SchemaFilesExample2.SchemaFilesExample2.Run();
        GrpcExample.GrpcExample.Run();
        CopyConstructorsExample.CopyConstructorsExample.Run();
        IncludesExample.IncludesExample.Run();
        SortedVectors.SortedVectorsExample.Run();
        Unions.UnionsExample.Run();
        SharedStrings.SharedStringsExample.Run();
        IndexedVectors.IndexedVectorsExample.Run();
        TypeFacades.TypeFacadesExample.Run();
        StructVectors.StructVectorsSample.Run();
        WriteThrough.WriteThroughSample.Run();
        ValueStructs.ValueStructsSample.Run();
    }
}
