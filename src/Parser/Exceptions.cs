using System;
using IronRuby.Builtins;
using IronRuby.Runtime;

namespace IronRuby.Libraries.Json {
    public static partial class JSON {
        [RubyClass("JSONError")]
        public class BaseException : SystemException {
            public BaseException(String message) : base(message) { }
        }

        [RubyClass("ParserError")]
        public class ParserException : BaseException {
            public ParserException(String message) : base(message) { }
        }

        [RubyClass("NestingError")]
        public class NestingException : ParserException {
            public NestingException(String message) : base(message) { }
        }

        [RubyClass("GeneratorError")]
        public class GenerateException : BaseException {
            public GenerateException(String message) : base(message) { }
        }

        [RubyClass("CircularDatastructure")]
        public class CircularDatastructureException : GenerateException {
            public CircularDatastructureException(String message) : base(message) { }
        }
    }
}