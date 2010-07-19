using System;
using System.Runtime.InteropServices;
using IronRuby.Builtins;
using IronRuby.Runtime;

namespace IronRuby.JsonExt {
    public static partial class Generator {
        public static MutableString ToJson(RubyContext context, Object self, params Object[] args) {
            return toJsonCallSite.Target(toJsonCallSite, context, self, args[0] as GeneratorState, (int)args[1]) as MutableString;
        }
    }

    [RubyClass(Extends = typeof(Object))]
    public static class ObjectOps {
        [RubyMethod("to_json")]
        public static MutableString ToJson(ConversionStorage<MutableString>/*!*/ toS, 
            Object/*!*/ self, [Optional]GeneratorState state, [Optional]Int32 depth) {

            return Generator.ToJson(Protocols.ConvertToString(toS, self));
        }
    }
}
