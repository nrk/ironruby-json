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

namespace IronRuby.Libraries.Json {
    using SetBacktraceStorage = CallSiteStorage<Action<CallSite, Exception, RubyArray>>;

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
                    BinaryOpStorage/*!*/ binaryOpStorage, SetBacktraceStorage/*!*/ setBacktraceStorage, 
                    RubyScope/*!*/ scope, Parser/*!*/ self) {

                    // TODO: exceptions are thrown without a backtrace and message string. I still need to better 
                    //       understand how to cleanly throw exceptions in mixed scenarios like this one, where the 
                    //       exception is actually defined in the ruby code of this library. You can read a related 
                    //       discussion I started a while back on the ironruby-core list at the following url: 
                    //       http://www.ruby-forum.com/topic/178446

                    try {
                        return self.Parse(scope);
                    }
                    catch (JSON.ParserException ex) {
                        Helpers.Exception(ex, respondToStorage, unaryOpStorage, binaryOpStorage, setBacktraceStorage, scope, self);
                    }

                    return null;
                }

            }

        }
    }
}