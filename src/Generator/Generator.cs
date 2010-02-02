using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using IronRuby.Builtins;
using IronRuby.Runtime;
using IronRuby.Runtime.Calls;
using Microsoft.Scripting;
using Microsoft.Scripting.Utils;
using Microsoft.Scripting.Runtime;

namespace IronRuby.Libraries.Json {
    using ToJsonStateCallSite = CallSite<Func<CallSite, RubyContext, Object, GeneratorState, Int32, Object>>;

    public static class Generator {

        #region fields

        private static readonly MutableString JSON_NULL = MutableString.CreateAscii("null");
        private static readonly MutableString JSON_TRUE = MutableString.CreateAscii("true");
        private static readonly MutableString JSON_FALSE = MutableString.CreateAscii("false");

        private static readonly ToJsonStateCallSite toJsonCallSite = ToJsonStateCallSite.Create(RubyCallAction.MakeShared("to_json", RubyCallSignature.Simple(2)));

        #endregion
        

        #region Object

        public static MutableString ToJson(RubyContext context, Object self, params Object[] args) {
            return toJsonCallSite.Target(toJsonCallSite, context, self, args[0] as GeneratorState, (int)args[1]) as MutableString;
        }

        #endregion

        #region RubyArray, IList

        public static MutableString ToJson(RubyContext context, IList self, GeneratorState state, int? depth) {
            MutableString result;

            if (state == null) {
                result = MutableString.CreateMutable(2 + Math.Max(self.Count * 4, 0), RubyEncoding.Default);
                // TODO: inherits flags?
                result.Append('[');

                if (self.Count > 0) {
                    for (int i = 0; i < self.Count; i++) {
                        result.Append(Generator.ToJson(context, self[i], null, 0));
                        Helpers.InheritsFlags(context, result, self);
                        // TODO: inherits flags?
                        if (i < self.Count - 1) {
                            result.Append(',');
                        }
                    }
                }

                result.Append(']');
            }
            else {
                result = Transform(context, self, state, depth.HasValue ? depth.Value : 0);
            }

            // TODO: inherits flags?
            return result;
        }

        private static MutableString Transform(RubyContext context, IList self, GeneratorState state, int depth) {
            MutableString result = MutableString.CreateMutable(2 + Math.Max(self.Count * 4, 0), RubyEncoding.Default);

            byte[] indentUnit = state.Indent.ToByteArray();
            byte[] shift = Helpers.Repeat(indentUnit, depth + 1);

            // TODO: inherits flags?

            byte[] arrayNl = state.ArrayNl.ToByteArray();
            byte[] delim = new byte[1 + arrayNl.Length];
            delim[0] = (byte)',';
            Array.Copy(arrayNl, 0, delim, 1, arrayNl.Length);

            state.CheckMaxNesting(depth + 1);

            if (state.CheckCircular) {
                state.Remember(context, self);

                result.Append('[');
                result.Append(arrayNl);

                if (self.Count > 0) {
                    for (int i = 0; i < self.Count; i++) {
                        Object element = self[i];
                        if (state.Seen(context, element)) {
                            Helpers.ThrowCircularDataStructureException("circular data structures not supported!");
                        }

                        // TODO: inherits flags?

                        if (i > 0) {
                            result.Append(delim);
                        }

                        result.Append(shift);
                        result.Append(Generator.ToJson(context, element, state, depth + 1));
                    }

                    // TODO: this might be needed outside of the count check for compat
                    if (arrayNl.Length != 0) {
                        result.Append(arrayNl);
                        result.Append(shift, 0, depth * indentUnit.Length);
                    }
                }

                result.Append(']');

                state.Forget(context, self);
            }
            else {
                result.Append('[');
                result.Append(arrayNl);

                if (self.Count > 0) {
                    for (int i = 0; i < self.Count; i++) {
                        Object element = self[i];
                        // TODO: inherits flags?

                        if (i > 0) {
                            result.Append(delim);
                        }

                        result.Append(shift);
                        result.Append(Generator.ToJson(context, element, state, depth + 1));
                    }

                    // TODO: this might be needed outside of the count check for compatibility
                    if (arrayNl.Length != 0) {
                        result.Append(arrayNl);
                        result.Append(shift, 0, depth * indentUnit.Length);
                    }
                }

                result.Append(']');
            }

            return result;
        }

        #endregion

        #region Hash

        public static MutableString ToJson(ConversionStorage<MutableString> toS, Hash self, GeneratorState state, int? depth) {
            MutableString result;
            RubyContext context = toS.Context;

            if (state == null) {
                result = MutableString.CreateMutable(2 + Math.Max(self.Count * 12, 0), RubyEncoding.Default);
                result.Append('{');

                if (self.Count > 0) {
                    int i = 0;
                    foreach (KeyValuePair<Object, Object> kv in self) {
                        // TODO: added state and depth
                        result.Append(Generator.ToJson(context, Protocols.CastToString(toS, kv.Key), null, 0));
                        result.Append(':');
                        result.Append(Generator.ToJson(context, kv.Value, null, 0));
                        if (++i < self.Count) {
                            result.Append(',');
                        }
                    }
                }

                result.Append('}');
            }
            else {
                GeneratorState.Ensure(state);

                if (state.CheckCircular) {
                    if (state.Seen(context, self)) {
                        Helpers.ThrowCircularDataStructureException("circular data structures not supported!");
                    }

                    state.Remember(context, self);
                    result = Transform(toS, self, state, depth.HasValue ? depth.Value : 0);
                    state.Forget(context, self);
                }
                else {
                    result = Transform(toS, self, state, depth.HasValue ? depth.Value : 0);
                }
            }

            return result;
        }

        private static MutableString Transform(ConversionStorage<MutableString> toS, Hash self, GeneratorState state, int depth) {
            byte[] objectNl = state.ObjectNl.ToByteArray();
            byte[] indent = Helpers.Repeat(state.Indent.ToByteArray(), depth + 1);
            byte[] spaceBefore = state.SpaceBefore.ToByteArray();
            byte[] space = state.Space.ToByteArray();
            int subDepth = depth + 1;

            MutableString result = MutableString.CreateBinary(2 + self.Count * (12 + indent.Length + spaceBefore.Length + space.Length));

            result.Append((byte)'{');
            result.Append(objectNl);

            if (self.Count > 0) {
                int i = 0;
                foreach (KeyValuePair<Object, Object> kv in self) {
                    if (i > 0) {
                        result.Append(objectNl);
                    }

                    if (objectNl.Length != 0) {
                        result.Append(indent);
                    }

                    result.Append(Generator.ToJson(toS.Context, Protocols.ConvertToString(toS, kv.Key), state, subDepth));
                    result.Append(spaceBefore);
                    result.Append((byte)':');
                    result.Append(space);
                    // TODO: inherits flags?

                    result.Append(Generator.ToJson(toS.Context, kv.Value, state, subDepth));
                    // TODO: inherits flags?

                    if (++i < self.Count) {
                        result.Append(',');
                    }
                }

                if (objectNl.Length != 0) {
                    result.Append(objectNl);
                    if (indent.Length != 0) {
                        for (int n = 0; n < depth; n++) {
                            result.Append(indent);
                        }
                    }
                }
            }

            result.Append((byte)'}');

            return result;
        }

        #endregion

        #region MutableString

        public static MutableString ToJson(MutableString self) {
            MutableString result = MutableString.CreateBinary(self.Length + 2);
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

        #endregion

        #region Int32

        public static MutableString ToJson(Int32 self) {
            return MutableString.CreateAscii(self.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
        }

        #endregion

        #region Double

        public static MutableString ToJson(Double self, GeneratorState state) {
            if (Double.IsInfinity(self) || Double.IsNaN(self)) {
                if (state != null) {
                    if (state.AllowNaN == false) {
                        //TODO: self.ToString() is not correct
                        throw new JSON.GenerateException(String.Format("{0} not allowed in JSON", self));
                    }
                }
                return MutableString.CreateAscii(self.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            }
            else {
                return MutableString.CreateAscii(self.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
            }
        }

        #endregion

        #region Boolean

        public static MutableString ToJson(Boolean self) {
            return MutableString.Create(self ? JSON_TRUE : JSON_FALSE);
        }

        #endregion

        #region DynamicNull

        public static MutableString ToJson(DynamicNull self) {
            return JSON_NULL;
        }

        #endregion

    }
}
