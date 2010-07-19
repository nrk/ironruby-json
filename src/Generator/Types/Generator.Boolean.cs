using System;
using System.Runtime.InteropServices;
using IronRuby.Builtins;
using IronRuby.Runtime;

namespace IronRuby.JsonExt {
    public static partial class Generator {
        public static MutableString ToJson(Boolean self) {
            return MutableString.Create(self ? JSON_TRUE : JSON_FALSE);
        }
    }

    [RubyClass(Extends = typeof(TrueClass))]
    public static class TrueOps {
        [RubyMethod("to_json")]
        public static MutableString ToJson(Boolean self, [Optional]GeneratorState state, [Optional]Int32 depth) {
            return Generator.ToJson(self);
        }
    }

    [RubyClass(Extends = typeof(FalseClass))]
    public static class FalseOps {
        [RubyMethod("to_json")]
        public static MutableString ToJson(Boolean self, [Optional]GeneratorState state, [Optional]Int32 depth) {
            return Generator.ToJson(self);
        }
    }
}
