using System;
using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using IronRuby.Builtins;
using IronRuby.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;

namespace IronRuby.Libraries.Json {
    public static partial class JSON {
        public static partial class Ext {

            [RubyClass("Parser", Extends = typeof(Parser))]
            public class ParserOps {

                [RubyConstructor]
                public static Parser CreateParser(RubyScope/*!*/ scope, RubyClass/*!*/ self, MutableString/*!*/ source, 
                    [DefaultParameterValue(null)]Hash options) {
                    return new Parser(scope, source, options != null ? options : new Hash(scope.RubyContext));
                }

                [RubyMethod("parse")]
                public static Object Parse(RespondToStorage/*!*/ respondToStorage, UnaryOpStorage/*!*/ unaryOpStorage,
                    BinaryOpStorage/*!*/ binaryOpStorage, SetBacktraceStorage/*!*/ setBacktraceStorage, 
                    RubyContext/*!*/ context, Parser/*!*/ self) {

                    // TODO: exceptions are thrown without a backtrace and message string. I still need to better 
                    //       understand how to cleanly throw exceptions in mixed scenarios like this one, where the 
                    //       exception is actually defined in the ruby code of this library. You can read a related 
                    //       discussion I started a while back on the ironruby-core list at the following url: 
                    //       http://www.ruby-forum.com/topic/178446

                    try {
                        return self.Parse(context);
                    }
                    catch (JsonParserException ex) {
                        KernelOps.RaiseException(respondToStorage, unaryOpStorage, binaryOpStorage, setBacktraceStorage, 
                            context, self, self.ParserError, ex.Message, null);
                    }
                    catch (JsonNestingException ex) {
                        KernelOps.RaiseException(respondToStorage, unaryOpStorage, binaryOpStorage, setBacktraceStorage, 
                            context, self, self.NestingError, ex.Message, null);
                    }

                    return null;
                }

            }

        }
    }
}