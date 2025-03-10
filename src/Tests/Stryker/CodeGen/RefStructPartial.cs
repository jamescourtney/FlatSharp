﻿using FlatSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatSharpStrykerTests;

public interface IReferenceItem
{
    static abstract bool IsStaticInitialized { get; }

    bool IsInitialized { get; }
}

public partial class RefStruct : IReferenceItem
{
    public static bool IsStaticInitialized { get; set; }

    public bool IsInitialized { get; set; }

    partial void OnInitialized(FlatBufferDeserializationContext? context) => this.IsInitialized = true;

    static partial void OnStaticInitialize() => IsStaticInitialized = true;
}
