using System;

namespace IronRuby.Libraries.Json {
    class JsonNestingException : Exception {
        public JsonNestingException(String message) : base(message) { }
    }

    class JsonParserException : Exception {
        public JsonParserException(String message) : base(message) { }
    }
}