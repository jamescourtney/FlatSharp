// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace FlatSharpEndToEndTests.Oracle.Flatc
{

using global::System;
using global::System.Collections.Generic;
using global::Google.FlatBuffers;

public struct AlignmentTestOuterHoder : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_23_5_26(); }
  public static AlignmentTestOuterHoder GetRootAsAlignmentTestOuterHoder(ByteBuffer _bb) { return GetRootAsAlignmentTestOuterHoder(_bb, new AlignmentTestOuterHoder()); }
  public static AlignmentTestOuterHoder GetRootAsAlignmentTestOuterHoder(ByteBuffer _bb, AlignmentTestOuterHoder obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public AlignmentTestOuterHoder __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public FlatSharpEndToEndTests.Oracle.Flatc.AlignmentTestOuter? Value { get { int o = __p.__offset(4); return o != 0 ? (FlatSharpEndToEndTests.Oracle.Flatc.AlignmentTestOuter?)(new FlatSharpEndToEndTests.Oracle.Flatc.AlignmentTestOuter()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartAlignmentTestOuterHoder(FlatBufferBuilder builder) { builder.StartTable(1); }
  public static void AddValue(FlatBufferBuilder builder, Offset<FlatSharpEndToEndTests.Oracle.Flatc.AlignmentTestOuter> valueOffset) { builder.AddStruct(0, valueOffset.Value, 0); }
  public static Offset<FlatSharpEndToEndTests.Oracle.Flatc.AlignmentTestOuterHoder> EndAlignmentTestOuterHoder(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<FlatSharpEndToEndTests.Oracle.Flatc.AlignmentTestOuterHoder>(o);
  }
  public AlignmentTestOuterHoderT UnPack() {
    var _o = new AlignmentTestOuterHoderT();
    this.UnPackTo(_o);
    return _o;
  }
  public void UnPackTo(AlignmentTestOuterHoderT _o) {
    _o.Value = this.Value.HasValue ? this.Value.Value.UnPack() : null;
  }
  public static Offset<FlatSharpEndToEndTests.Oracle.Flatc.AlignmentTestOuterHoder> Pack(FlatBufferBuilder builder, AlignmentTestOuterHoderT _o) {
    if (_o == null) return default(Offset<FlatSharpEndToEndTests.Oracle.Flatc.AlignmentTestOuterHoder>);
    StartAlignmentTestOuterHoder(builder);
    AddValue(builder, FlatSharpEndToEndTests.Oracle.Flatc.AlignmentTestOuter.Pack(builder, _o.Value));
    return EndAlignmentTestOuterHoder(builder);
  }
}

public class AlignmentTestOuterHoderT
{
  public FlatSharpEndToEndTests.Oracle.Flatc.AlignmentTestOuterT Value { get; set; }

  public AlignmentTestOuterHoderT() {
    this.Value = new FlatSharpEndToEndTests.Oracle.Flatc.AlignmentTestOuterT();
  }
}


static public class AlignmentTestOuterHoderVerify
{
  static public bool Verify(Google.FlatBuffers.Verifier verifier, uint tablePos)
  {
    return verifier.VerifyTableStart(tablePos)
      && verifier.VerifyField(tablePos, 4 /*Value*/, 40 /*FlatSharpEndToEndTests.Oracle.Flatc.AlignmentTestOuter*/, 8, false)
      && verifier.VerifyTableEnd(tablePos);
  }
}

}
