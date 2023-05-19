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

namespace FlatSharp.Internal;

/// <summary>
/// Error messages for buffer validation.
/// </summary>
public static class ValidationErrors
{
    public const string BufferLessThanMinLength = "The buffer is too small to be valid";

    public const string WrongFileIdentifier = "The file identifier was mismatched or incorrect.";

    public const string InvalidOffset = "The offset points beyond the end of the buffer.";

    public const string InvalidLength = "The length points beyond the end of the buffer.";

    public const string NoNullTerminator = "The string was not null-terminated.";

    public const string NullItemInVector = "The vector contained a null item.";

    public const string InvalidUOffset = "The UOffset was invalid. Expected to be at least 4.";
}
