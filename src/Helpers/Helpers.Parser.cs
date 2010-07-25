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
            throw new NestingException(String.Format(message, args));
        }

        public static void ThrowParserException(String message, params Object[] args) {
            throw new ParserException(String.Format(message, args));
        }

        public static String GetMessageForException(String source, int p, int pe) {
            return source.Substring(p, Math.Min(20, pe - p));
        }

        public static bool IsJsonClass(Hash hash) {
            return hash.Count > 0 && hash.ContainsKey(_jsonClass);
        }
    }
}
