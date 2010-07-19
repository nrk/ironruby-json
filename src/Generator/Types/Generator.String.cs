using System;
using System.Text;
using System.Runtime.InteropServices;
using IronRuby.Builtins;
using IronRuby.Runtime;

namespace IronRuby.JsonExt {
    public static partial class Generator {
        public static MutableString ToJson(MutableString self) {
            MutableString result = MutableString.CreateMutable(self.Length + 2, RubyEncoding.UTF8);
            char[] chars = Encoding.UTF8.GetChars(self.ToByteArray());
            byte[] escapeSequence = new byte[] { (byte)'\\', 0 };

            result.Append('"');
            foreach (char c in chars) {
                switch (c) {
                    case '"':
                    case '/':
                    case '\\':
                        escapeSequence[1] = (byte)c;
                        result.Append(escapeSequence);
                        break;
                    case '\n':
                        escapeSequence[1] = (byte)'n';
                        result.Append(escapeSequence);
                        break;
                    case '\r':
                        escapeSequence[1] = (byte)'r';
                        result.Append(escapeSequence);
                        break;
                    case '\t':
                        escapeSequence[1] = (byte)'t';
                        result.Append(escapeSequence);
                        break;
                    case '\f':
                        escapeSequence[1] = (byte)'f';
                        result.Append(escapeSequence);
                        break;
                    case '\b':
                        escapeSequence[1] = (byte)'b';
                        result.Append(escapeSequence);
                        break;
                    default:
                        if (c >= 0x20 && c <= 0x7F) {
                            result.Append((byte)c);
                        }
                        else {
                            result.Append(Helpers.EscapeUnicodeChar(c));
                        }
                        break;
                }
            }
            result.Append('"');
            return result;
        }

        public static Hash ToJsonRawObject(RubyScope scope, MutableString self) {
            RubyContext context = scope.RubyContext;
            MutableString createId = Helpers.GetCreateId(scope);

            byte[] selfBuffer = self.ToByteArray();
            RubyArray array = new RubyArray(selfBuffer.Length);
            foreach (byte b in selfBuffer) {
                array.Add(b & 0xFF);
            }

            Hash result = new Hash(context);
            result.Add(createId, MutableString.Create(context.GetClassName(self), RubyEncoding.Binary));
            result.Add(MutableString.CreateAscii("raw"), array);

            return result;
        }

        public static MutableString ToJsonRaw(RubyScope scope, MutableString self) {
            Hash hash = ToJsonRawObject(scope, self);
            return ToJson(scope.RubyContext, hash, null, 0);
        }
    }

    [RubyClass(Extends = typeof(String))]
    public static class ClrStringOps {
        [RubyMethod("to_json")]
        public static MutableString ToJson(String self, [Optional]GeneratorState state, [Optional]Int32 depth) {
            return Generator.ToJson(MutableString.Create(self, RubyEncoding.Binary));
        }
    }

    [RubyClass(Extends = typeof(MutableString))]
    public static partial class MutableStringOps {
        [RubyMethod("to_json")]
        public static MutableString ToJson(MutableString self, [Optional]GeneratorState state, [Optional]Int32 depth) {
            return Generator.ToJson(self);
        }

        [RubyMethod("to_json_raw_object")]
        public static Hash ToJsonRawObject(RubyScope scope, MutableString self) {
            return Generator.ToJsonRawObject(scope, self);
        }

        [RubyMethod("to_json_raw")]
        public static MutableString ToJsonRaw(RubyScope scope, MutableString self) {
            return Generator.ToJsonRaw(scope, self);
        }
    }
}
