/*
 * Copyright 2025 James Courtney
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

namespace FlatSharpEndToEndTests.PartialProperties;

/*
 * These "tests" simply exist to ensure the code compiles.
 * We cannot use reflection to determine if a property is partial or not.
 */

public partial class Table
{
    public virtual partial Struct? S { get; set; }
}

public partial class Struct
{
    public virtual partial int A { get; set; }
}

public partial class PartialTableDefaultOn
{
    public virtual partial int X { get; set; }
}

public partial class PartialStructDefaultOn
{
    public virtual partial int X { get; set; }
}
