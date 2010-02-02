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

namespace IronRuby.Libraries.Json {
    using CreateIdCallSite = CallSite<Func<CallSite, RubyContext, Object, Object>>;
    using ExceptionCreateCallSite = CallSite<Func<CallSite, RubyContext, Object, MutableString, Exception>>;
    using SetBacktraceStorage = CallSiteStorage<Action<CallSite, Exception, RubyArray>>;

    public static class Helpers {
        #region static fields

        private static readonly byte[] HEX = new byte[] {
                    (byte)'0', (byte)'1', (byte)'2', (byte)'3', (byte)'4', (byte)'5', (byte)'6', (byte)'7',
                    (byte)'8', (byte)'9', (byte)'a', (byte)'b', (byte)'c', (byte)'d', (byte)'e', (byte)'f'
                };

        private static readonly Dictionary<String, SymbolId> _generatorStateKeyMappings;

        private static readonly MutableString _jsonClass = MutableString.CreateAscii("json_class");

        #endregion

        #region static constructor

        static Helpers() {
            // TODO: I do not really like how I implemented this...
            _generatorStateKeyMappings = new Dictionary<String, SymbolId>();
            _generatorStateKeyMappings.Add("indent", SymbolTable.StringToId("indent"));
            _generatorStateKeyMappings.Add("space", SymbolTable.StringToId("space"));
            _generatorStateKeyMappings.Add("space_before", SymbolTable.StringToId("space_before"));
            _generatorStateKeyMappings.Add("array_nl", SymbolTable.StringToId("array_nl"));
            _generatorStateKeyMappings.Add("object_nl", SymbolTable.StringToId("object_nl"));
            _generatorStateKeyMappings.Add("check_circular", SymbolTable.StringToId("check_circular"));
            _generatorStateKeyMappings.Add("max_nesting", SymbolTable.StringToId("max_nesting"));
            _generatorStateKeyMappings.Add("allow_nan", SymbolTable.StringToId("allow_nan"));
        }

        #endregion

        #region generator helpers 

        public static byte[] EscapeUnicodeChar(char c) {
            return new byte[] {
                (byte)'\\', 
                (byte)'u', 
                HEX[((uint)c >> 12) & 0xF], 
                HEX[((uint)c >> 8) & 0xF],
                HEX[((uint)c >> 4) & 0xF], 
                HEX[(uint)c & 0xF]
            };
        }

        public static byte[] Repeat(byte[] a, int n) {
            return Repeat(a, 0, a.Length, n);
        }

        public static byte[] Repeat(byte[] a, int begin, int length, int n) {
            int resultLen = length * n;
            byte[] result = new byte[resultLen];
            for (int pos = 0; pos < resultLen; pos += length) {
                Array.Copy(a, begin, result, pos, length);
            }
            return result;
        }

        public static MutableString GetCreateId(RubyScope scope) {
            return RubyOps.GetInstanceVariable(scope, GetModule(scope, "JSON"), "@create_id") as MutableString;
        }

        public static SymbolId GetGeneratorStateKey(String key) {
            return _generatorStateKeyMappings[key];
        }

        public static void InheritsFlags(RubyContext context, Object target, Object inheritsFrom) {
            // TODO: need to FreezeObjectBy too?
            context.TaintObjectBy<Object>(target, inheritsFrom);
        }

        public static void ThrowCircularDataStructureException(String message, params Object[] args) {
            throw new JSON.CircularDatastructureException(String.Format(message, args));
        }

        public static void ThrowGenerateException(String message, params Object[] args) {
            throw new JSON.GenerateException(String.Format(message, args));
        }

        #endregion

        #region parser helpers

        public static MutableString CreateMutableString(String value, int len) {
            return MutableString.Create(value, RubyEncoding.Binary);
        }

        public static Object ToInteger(String str) {
            return IronRuby.Compiler.Tokenizer.ParseInteger(str, 10).ToObject();
        }

        public static Object ToFloat(String str) {
            //NOTE: this terrible hack is for huge floats, e.g. 23456789012E666
            double res;
            if (!Double.TryParse(str, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out res)) {
                return str.StartsWith("-") ? Double.NegativeInfinity : Double.PositiveInfinity;
            }
            return res;
        }

        public static void ThrowNestingException(String message, params Object[] args) {
            throw new JSON.NestingException(String.Format(message, args));
        }

        public static void ThrowParserException(String message, params Object[] args) {
            throw new JSON.ParserException(String.Format(message, args));
        }

        public static String GetMessageForException(String source, int p, int pe) {
            return source.Substring(p, Math.Min(20, pe - p));
        }

        public static bool IsJsonClass(Hash hash) {
            return hash.Count > 0 && hash.ContainsKey(_jsonClass);
        }

        #endregion

        private static RubyModule GetModule(RubyScope scope, String className)  {
            RubyModule module;
            if (!scope.RubyContext.TryGetModule(scope.GlobalScope, className, out module)) {
                throw RubyExceptions.CreateNameError(className);
            }
            return module;
        }
    }
}
