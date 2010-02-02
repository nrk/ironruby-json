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
using Microsoft.Scripting.Utils;
using Microsoft.Scripting.Runtime;

namespace IronRuby.Libraries.Json.Builtins {
    [RubyClass(Extends = typeof(Object))]
    public static class ObjectOps {
        [RubyMethod("to_json")]
        public static MutableString ToJson(UnaryOpStorage/*!*/ inspectStorage, 
            ConversionStorage<MutableString>/*!*/ tosConversion, Object/*!*/ self) {

            return Generator.ToJson(KernelOps.Inspect(inspectStorage, tosConversion, self));
        }
    }

    [RubyClass(Extends = typeof(RubyArray))]
    public static class ArrayOps {
        [RubyMethod("to_json")]
        public static MutableString ToJson(RubyScope/*!*/ scope, RubyArray self, 
            [Optional]GeneratorState state, [Optional]Int32 depth) {

            return Generator.ToJson(scope.RubyContext, self, state, depth);
        }
    }

    [RubyModule(Extends = typeof(IList))]
    public static class IListOps {
        [RubyMethod("to_json")]
        public static MutableString ToJson(RubyScope/*!*/ scope, IList self,
            [Optional]GeneratorState state, [Optional]Int32 depth) {

            return Generator.ToJson(scope.RubyContext, self, state, depth);
        }
    }

    [RubyClass(Extends = typeof(Hash))]
    public static class HashOps {
        [RubyMethod("to_json")]
        public static MutableString ToJson(ConversionStorage<MutableString> toS, RubyScope/*!*/ scope,
            Hash self, [Optional]GeneratorState state, [Optional]Int32 depth) {

            return Generator.ToJson(toS, self, state, depth);
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

    [RubyClass(Extends = typeof(String))]
    public static class ClrStringOps {
        [RubyMethod("to_json")]
        public static MutableString ToJson(String self, [Optional]GeneratorState state, [Optional]Int32 depth) {
            return Generator.ToJson(MutableString.Create(self, RubyEncoding.Binary));
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
