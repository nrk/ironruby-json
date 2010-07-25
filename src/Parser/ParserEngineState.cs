using System;
using IronRuby.Runtime;
using IronRuby.Builtins;

namespace IronRuby.JsonExt {
    public class ParserEngineState {
        public const Int32 DEFAULT_MAX_NESTING = 19;
        public const bool DEFAULT_ALLOW_NAN = false;

        public ParserEngineState(Parser parser, RubyScope scope, MutableString source) {
            Parser = parser;
            Scope = scope;
            OriginalSource = source;
            Source = source;

            CreateID = Helpers.GetCreateId(scope);
            AllowNaN = DEFAULT_ALLOW_NAN;
            MaxNesting = DEFAULT_MAX_NESTING;
            CurrentNesting = 0;
            Memo = 0;
        }

        public Parser Parser { get; private set; }
        public RubyScope Scope { get; private set; }
        public MutableString OriginalSource { get; private set; }
        public MutableString Source { get; private set; }

        public Int32 Memo { get; set; }
        public Object CreateID { get; set; }
        public Int32 MaxNesting { get; set; }
        public Int32 CurrentNesting { get; set; }
        public Boolean AllowNaN { get; set; }

        public Int32 Length {
            get {
                return OriginalSource.Length;
            }
        }

        public RubyContext Context {
            get { return Scope.RubyContext; }
        }
    }
}
