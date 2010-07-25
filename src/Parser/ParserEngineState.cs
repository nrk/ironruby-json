using System;
using IronRuby.Runtime;
using IronRuby.Builtins;

namespace IronRuby.JsonExt {
    public class ParserEngineState {
        public MutableString vsource { get; set; }
        public MutableString source { get; set; }
        public Int32 len { get; set; }
        public Int32 memo { get; set; }
        public Object create_id { get; set; }
        public Int32 max_nesting { get; set; }
        public Int32 current_nesting { get; set; }
        public Boolean allow_nan { get; set; }
        public Parser Parser { get; set; }
        public RubyScope Scope { get; set; }
        public RubyContext Context {
            get { return Scope.RubyContext; }
        }
    }
}