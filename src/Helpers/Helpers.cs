using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IronRuby.Builtins;
using IronRuby.Runtime;
using IronRuby.Runtime.Calls;
using Microsoft.Scripting;
using Microsoft.Scripting.Utils;
using Microsoft.Scripting.Runtime;

namespace IronRuby.JsonExt {
    public static partial class Helpers {
        private static readonly byte[] HEX = new byte[] {
            (byte)'0', (byte)'1', (byte)'2', (byte)'3', (byte)'4', (byte)'5', (byte)'6', (byte)'7',
            (byte)'8', (byte)'9', (byte)'a', (byte)'b', (byte)'c', (byte)'d', (byte)'e', (byte)'f'
        };

        private static Dictionary<String, RubySymbol> _generatorStateKeyMappings;
        private static readonly MutableString _jsonClass = MutableString.CreateAscii("json_class");

        private static IDictionary<String, RubySymbol> InitializeGeneratorStateKey(RubyContext context) {
            // TODO: I do not really like how I implemented this...
            if (_generatorStateKeyMappings == null) {
                _generatorStateKeyMappings = new Dictionary<String, RubySymbol>();
                _generatorStateKeyMappings.Add("indent", context.CreateAsciiSymbol("indent"));
                _generatorStateKeyMappings.Add("space", context.CreateAsciiSymbol("space"));
                _generatorStateKeyMappings.Add("space_before", context.CreateAsciiSymbol("space_before"));
                _generatorStateKeyMappings.Add("array_nl", context.CreateAsciiSymbol("array_nl"));
                _generatorStateKeyMappings.Add("object_nl", context.CreateAsciiSymbol("object_nl"));
                _generatorStateKeyMappings.Add("check_circular", context.CreateAsciiSymbol("check_circular"));
                _generatorStateKeyMappings.Add("max_nesting", context.CreateAsciiSymbol("max_nesting"));
                _generatorStateKeyMappings.Add("allow_nan", context.CreateAsciiSymbol("allow_nan"));
            }
            return _generatorStateKeyMappings;
        }
        
        private static RubyModule GetModule(RubyScope scope, String className) {
            RubyModule module;
            if (!scope.RubyContext.TryGetModule(scope.GlobalScope, className, out module)) {
                throw RubyExceptions.CreateNameError(className);
            }
            return module;
        }
    }
}
