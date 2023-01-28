using FlatSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlatSharp.Internal;

public static class MockBitConverter
{
    private static readonly ThreadLocal<bool> isLE = new(() => true);

    public static bool IsLittleEndian
    {
        get => isLE.Value;
        set => isLE.Value = value;
    }
}
