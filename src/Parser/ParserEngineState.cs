using System;
using IronRuby.Runtime;

namespace IronRuby.Libraries.Json {
    public class ParserEngineState {
        public Object vsource;
        public String source;
        public Int32 len;
        public Int32 memo;
        public Object create_id;
        public Int32 max_nesting;
        public Int32 current_nesting;
        public Boolean allow_nan;
        public RubyContext context;
        public Parser parser;
    }
}