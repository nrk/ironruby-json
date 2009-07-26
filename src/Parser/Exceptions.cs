using System;

namespace IronRuby.Libraries.Json {
    class JsonNestingException : Exception {
        public JsonNestingException(String message) : base(message) { }
    }

    class JsonParserException : Exception {
        public JsonParserException(String message) : base(message) { }
    }

    class JsonGenerateException : Exception {
        public JsonGenerateException(String message) : base(message) { }
    }

    class JsonCircularDatastructureException : Exception {
        public JsonCircularDatastructureException(String message) : base(message) { }
    }
}