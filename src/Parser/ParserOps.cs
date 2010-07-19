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
    public static partial class JSON {
        public static partial class Ext {

            [RubyClass("Parser", Extends = typeof(Parser))]
            public class ParserOps {

                [RubyConstructor]
                public static Parser CreateParser(RespondToStorage/*!*/ respondToStorage, RubyScope/*!*/ scope, 
                    RubyClass/*!*/ self, MutableString/*!*/ source, [DefaultParameterValue(null)]Hash options) {
                    return new Parser(scope, respondToStorage, source, options != null ? options : new Hash(scope.RubyContext));
                }

                [RubyMethod("parse")]
                public static Object Parse(RespondToStorage/*!*/ respondToStorage, UnaryOpStorage/*!*/ unaryOpStorage,
                    BinaryOpStorage/*!*/ binaryOpStorage, RubyScope/*!*/ scope, Parser/*!*/ self) {

                    return self.Parse(scope);
                }

            }

        }
    }
}