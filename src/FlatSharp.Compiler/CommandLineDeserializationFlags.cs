/*
 * Copyright 2023 James Courtney
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

namespace FlatSharp.Compiler;

[Flags]
public enum CommandLineDeserializationFlags
{
    None = 0,

    Lazy = 1,
    Progressive = 2,
    Greedy = 4,
    GreedyMutable = 8,

    All = Lazy | Progressive | Greedy | GreedyMutable,
}

public static class CommandLineDeserializationFlagsExtensions
{
    public static FlatBufferDeserializationOption ToOption(this CommandLineDeserializationFlags flag)
    {
        return flag switch
        {
            CommandLineDeserializationFlags.Lazy => FlatBufferDeserializationOption.Lazy,
            CommandLineDeserializationFlags.Progressive => FlatBufferDeserializationOption.Progressive,
            CommandLineDeserializationFlags.Greedy => FlatBufferDeserializationOption.Greedy,
            CommandLineDeserializationFlags.GreedyMutable => FlatBufferDeserializationOption.GreedyMutable,
            _ => throw new Exception("Unexpected: " + flag)
        };
    }

    public static IEnumerable<FlatBufferDeserializationOption> ToOptions(this CommandLineDeserializationFlags flag)
    {
        List<FlatBufferDeserializationOption> options = new();

        if (flag.HasFlag(CommandLineDeserializationFlags.Lazy))
        {
            options.Add(FlatBufferDeserializationOption.Lazy);
        }

        if (flag.HasFlag(CommandLineDeserializationFlags.Progressive))
        {
            options.Add(FlatBufferDeserializationOption.Progressive);
        }

        if (flag.HasFlag(CommandLineDeserializationFlags.Greedy))
        {
            options.Add(FlatBufferDeserializationOption.Greedy);
        }

        if (flag.HasFlag(CommandLineDeserializationFlags.GreedyMutable))
        {
            options.Add(FlatBufferDeserializationOption.GreedyMutable);
        }

        return options;
    }
}