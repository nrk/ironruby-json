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

        public static RubySymbol GetGeneratorStateKey(RubyContext context, String key) {
            return InitializeGeneratorStateKey(context)[key];
        }

        public static void ThrowCircularDataStructureException(String message, params Object[] args) {
            throw new CircularDatastructureException(String.Format(message, args));
        }

        public static void ThrowGenerateException(String message, params Object[] args) {
            throw new GenerateException(String.Format(message, args));
        }
    }
}
