using System;
using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using IronRuby.Builtins;
using IronRuby.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Utils;
using Microsoft.Scripting.Runtime;

namespace IronRuby.JsonExt {
    [RubyClass("Parser", Extends = typeof(Parser), DefineIn = typeof(ExtModule))]
    public class ParserOps {
        [RubyConstructor]
        public static Parser CreateParser(RubyScope/*!*/ scope, RubyClass/*!*/ self, MutableString/*!*/ source, 
            [DefaultParameterValue(null)]Hash options) {

            return new Parser(scope, source, options != null ? options : new Hash(scope.RubyContext));
        }

        [RubyMethod("parse")]
        public static Object Parse(RespondToStorage/*!*/ respondToStorage, UnaryOpStorage/*!*/ unaryOpStorage,
            BinaryOpStorage/*!*/ binaryOpStorage, RubyScope/*!*/ scope, Parser/*!*/ self) {

            return self.Parse(scope);
        }
    }
}
