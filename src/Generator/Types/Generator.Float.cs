using System;
using System.Globalization;
using System.Runtime.InteropServices;
using IronRuby.Builtins;
using IronRuby.Runtime;

namespace IronRuby.JsonExt {
    public static partial class Generator {
        public static MutableString ToJson(Double self, GeneratorState state) {
            if (Double.IsInfinity(self) || Double.IsNaN(self)) {
                if (state != null && state.AllowNaN == false) {
                    Helpers.ThrowGenerateException(String.Format("{0} not allowed in JSON", self));
                }
                return MutableString.CreateAscii(self.ToString(NumberFormatInfo.InvariantInfo));
            }
            else {
                return MutableString.CreateAscii(self.ToString(NumberFormatInfo.InvariantInfo));
            }
        }
    }

    [RubyClass(Extends = typeof(Double))]
    public static class FloatOps {
        [RubyMethod("to_json")]
        public static MutableString ToJson(Double self, [Optional]GeneratorState state, [Optional]Int32 depth) {
            return Generator.ToJson(self, state);
        }
    }
}
