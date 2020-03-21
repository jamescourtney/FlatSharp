namespace Samples.IncludesExample
{
    using FlatSharp;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// This example shows how different flatbuffer files can include each other. In this example, IncludesExample.fbs references A, which references B.
    /// We can create a complete serializer package and gRPC service just from the declarations in IncludesExample.cs.
    /// </summary>
    public class IncludesExample
    {
        public static void Run()
        {
        }
    }
}
