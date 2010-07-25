using System;
using IronRuby.Runtime;
using IronRuby.Builtins;

namespace IronRuby.JsonExt {
    public class ParserEngineState {
        public MutableString OriginalSource { get; set; }
        public MutableString Source { get; set; }
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

        public Parser Parser { get; set; }
        public RubyScope Scope { get; set; }

        public RubyContext Context {
            get { return Scope.RubyContext; }
        }
    }
}