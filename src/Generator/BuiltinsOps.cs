using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using IronRuby.Builtins;
using IronRuby.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;

namespace IronRuby.Libraries.Json.Builtins {
    using SetBacktraceStorage = CallSiteStorage<Action<CallSite, Exception, RubyArray>>;

    [RubyClass(Extends = typeof(Object))]
    public static class ObjectOps {
        [RubyMethod("to_json")]
        public static MutableString ToJson(RespondToStorage/*!*/ respondToStorage, UnaryOpStorage/*!*/ unaryOpStorage,
            BinaryOpStorage/*!*/ binaryOpStorage, SetBacktraceStorage/*!*/ setBacktraceStorage, RubyScope/*!*/ scope, 
            Object self, [Optional]GeneratorState state, [Optional]Int32 depth) {
            
            try {
                return Generator.ToJson(scope.RubyContext, self, state, depth);
            }
            catch (JSON.GenerateException ex) {
                Helpers.Exception(ex, respondToStorage, unaryOpStorage, binaryOpStorage, setBacktraceStorage, scope, self);
            }
            return null;
        }
    }

    [RubyClass(Extends = typeof(RubyArray))]
    public static class ArrayOps {
        [RubyMethod("to_json")]
        public static MutableString ToJson(RespondToStorage/*!*/ respondToStorage, UnaryOpStorage/*!*/ unaryOpStorage,
            BinaryOpStorage/*!*/ binaryOpStorage, SetBacktraceStorage/*!*/ setBacktraceStorage, RubyScope/*!*/ scope, 
            RubyArray self, [Optional]GeneratorState state, [Optional]Int32 depth) {

            try {
                return Generator.ToJson(scope.RubyContext, self, state, depth);
            }
            catch (JSON.GenerateException ex) {
                Helpers.Exception(ex, respondToStorage, unaryOpStorage, binaryOpStorage, setBacktraceStorage, scope, self);
            }
            return null;
        }
    }

    [RubyClass(Extends = typeof(Hash))]
    public static class HashOps {
        [RubyMethod("to_json")]
        public static MutableString ToJson(ConversionStorage<MutableString> toS, RespondToStorage/*!*/ respondToStorage, 
            UnaryOpStorage/*!*/ unaryOpStorage, BinaryOpStorage/*!*/ binaryOpStorage, SetBacktraceStorage/*!*/ setBacktraceStorage, 
            RubyScope/*!*/ scope, Hash self, [Optional]GeneratorState state, [Optional]Int32 depth) {

            try {
                return Generator.ToJson(toS, self, state, depth);
            }
            catch (JSON.GenerateException ex) {
                Helpers.Exception(ex, respondToStorage, unaryOpStorage, binaryOpStorage, setBacktraceStorage, scope, self);
            }
            return null;
        }
    }

    [RubyClass(Extends = typeof(MutableString))]
    public static class MutableStringOps {
        [RubyMethod("to_json")]
        public static MutableString ToJson(MutableString self, [Optional]GeneratorState state, [Optional]Int32 depth) {
            return Generator.ToJson(self);
        }

        [RubyMethod("to_json_raw_object")]
        public static Hash ToJsonRawObject(RubyScope scope, MutableString self) {
            return Generator.ToJsonRawObject(scope, self);
        }

        [RubyMethod("to_json_raw")]
        public static MutableString ToJsonRaw(RubyScope scope, MutableString self) {
            return Generator.ToJsonRaw(scope, self);
        }
    }

    [RubyClass(Extends = typeof(Int32))]
    public static class FixnumOps {
        [RubyMethod("to_json")]
        public static MutableString ToJson(Int32 self, [Optional]GeneratorState state, [Optional]Int32 depth) {
            return Generator.ToJson(self);
        }
    }

    [RubyClass(Extends = typeof(Double))]
    public static class FloatOps {
        [RubyMethod("to_json")]
        public static MutableString ToJson(Double self, [Optional]GeneratorState state, [Optional]Int32 depth) {
            return Generator.ToJson(self, state);
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

    [RubyClass(Extends = typeof(DynamicNull))]
    public static class DynamicNullOps {
        [RubyMethod("to_json")]
        public static MutableString ToJson(DynamicNull self, [Optional]GeneratorState state, [Optional]Int32 depth) {
            return Generator.ToJson(self);
        }
    }
}
