using System;
using System.Runtime.InteropServices;
using IronRuby.Builtins;
using IronRuby.Runtime;
using Microsoft.Scripting.Runtime;

namespace IronRuby.JsonExt {
    public static partial class Generator {
        public static MutableString ToJson(DynamicNull self) {
            return JSON_NULL;
        }
    }

    [RubyClass(Extends = typeof(DynamicNull))]
    public static class DynamicNullOps {
        [RubyMethod("to_json")]
        public static MutableString ToJson(DynamicNull self, [Optional]GeneratorState state, [Optional]Int32 depth) {
            return Generator.ToJson(self);
        }
    }
}
