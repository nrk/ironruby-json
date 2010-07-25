using System;
using System.Globalization;
using System.Runtime.InteropServices;
using IronRuby.Builtins;
using IronRuby.Runtime;

namespace IronRuby.JsonExt {
    public static partial class Generator {
        public static MutableString ToJson(Int32 self) {
            return MutableString.CreateAscii(self.ToString(NumberFormatInfo.InvariantInfo));
        }
    }

    [RubyClass(Extends = typeof(Int32))]
    public static class FixnumOps {
        [RubyMethod("to_json")]
        public static MutableString ToJson(Int32 self, [Optional]GeneratorState state, [Optional]Int32 depth) {
            return Generator.ToJson(self);
        }
    }
}
