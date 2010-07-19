using System;
using IronRuby.Builtins;
using IronRuby.Runtime;

namespace IronRuby.JsonExt {
    [RubyClass("JSONError", DefineIn = typeof(JsonModule))]
    public class BaseException : SystemException {
        public BaseException(String message) 
            : base(message) { 
        }
    }

    [RubyClass("ParserError", DefineIn = typeof(JsonModule))]
    public class ParserException : BaseException {
        public ParserException(String message) 
            : base(message) { 
        }
    }

    [RubyClass("NestingError", DefineIn = typeof(JsonModule))]
    public class NestingException : ParserException {
        public NestingException(String message) 
            : base(message) { 
        }
    }

    [RubyClass("GeneratorError", DefineIn = typeof(JsonModule))]
    public class GenerateException : BaseException {
        public GenerateException(String message) 
            : base(message) { 
        }
    }

    [RubyClass("CircularDatastructure", DefineIn = typeof(JsonModule))]
    public class CircularDatastructureException : GenerateException {
        public CircularDatastructureException(String message) 
            : base(message) {
        }
    }
}
