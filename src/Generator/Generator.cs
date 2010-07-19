using System;
using System.Runtime.CompilerServices;
using IronRuby.Builtins;
using IronRuby.Runtime;
using IronRuby.Runtime.Calls;
using Microsoft.Scripting.Utils;

namespace IronRuby.JsonExt {
    using ToJsonStateCallSite = CallSite<Func<CallSite, RubyContext, Object, GeneratorState, Int32, Object>>;

    public static partial class Generator {
        private static readonly MutableString JSON_NULL = MutableString.CreateAscii("null");
        private static readonly MutableString JSON_TRUE = MutableString.CreateAscii("true");
        private static readonly MutableString JSON_FALSE = MutableString.CreateAscii("false");

        private static readonly ToJsonStateCallSite toJsonCallSite = ToJsonStateCallSite.Create(RubyCallAction.MakeShared("to_json", RubyCallSignature.Simple(2)));
    }
}
