using System;
using System.Collections;
using System.Runtime.InteropServices;
using IronRuby.Builtins;
using IronRuby.Runtime;

namespace IronRuby.JsonExt {
    public static partial class Generator {
        public static MutableString ToJson(ConversionStorage<MutableString> toS, IDictionary self, GeneratorState state, int? depth) {
            MutableString result;
            RubyContext context = toS.Context;

            if (state == null) {
                MutableString json;
                result = MutableString.CreateMutable(2 + Math.Max(self.Count * 12, 0), RubyEncoding.Default);

                result.Append('{');
                if (self.Count > 0) {
                    int i = 0;
                    foreach (DictionaryEntry kv in self) {
                        // TODO: added state and depth
                        json = Generator.ToJson(context, Protocols.ConvertToString(toS, kv.Key), null, 0);
                        result.Append(json);
                        context.TaintObjectBy<Object>(result, json);

                        result.Append(':');

                        json = Generator.ToJson(context, kv.Value, null, 0);
                        result.Append(json);
                        context.TaintObjectBy<Object>(result, json);

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

        private static MutableString Transform(ConversionStorage<MutableString> toS, IDictionary self, GeneratorState state, int depth) {
            byte[] objectNl = state.ObjectNl.ToByteArray();
            byte[] indent = Helpers.Repeat(state.Indent.ToByteArray(), depth + 1);
            byte[] spaceBefore = state.SpaceBefore.ToByteArray();
            byte[] space = state.Space.ToByteArray();
            int subDepth = depth + 1;

            MutableString result = MutableString.CreateBinary(2 + self.Count * (12 + indent.Length + spaceBefore.Length + space.Length));
            RubyContext context = toS.Context;

            result.Append((byte)'{');
            result.Append(objectNl);
            if (self.Count > 0) {
                MutableString json;
                int i = 0;
                foreach (DictionaryEntry kv in self) {
                    if (i > 0) {
                        result.Append(objectNl);
                    }
                    if (objectNl.Length != 0) {
                        result.Append(indent);
                    }

                    json = Generator.ToJson(context, Protocols.ConvertToString(toS, kv.Key), state, subDepth);
                    result.Append(json);
                    context.TaintObjectBy<Object>(result, json);

                    result.Append(spaceBefore);
                    result.Append((byte)':');
                    result.Append(space);

                    json = Generator.ToJson(context, kv.Value, state, subDepth);
                    result.Append(json);
                    context.TaintObjectBy<Object>(result, json);

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
    }

    [RubyModule(Extends = typeof(IDictionary))]
    public static class DictionaryOps {
        [RubyMethod("to_json")]
        public static MutableString ToJson(ConversionStorage<MutableString> toS, IDictionary self, 
            [Optional]GeneratorState state, [Optional]Int32 depth) {

            return Generator.ToJson(toS, self, state, depth);
        }
    }

    [RubyClass(Extends = typeof(Hash))]
    public static class HashOps {
        [RubyMethod("to_json")]
        public static MutableString ToJson(ConversionStorage<MutableString> toS, Hash self, 
            [Optional]GeneratorState state, [Optional]Int32 depth) {

            return Generator.ToJson(toS, self, state, depth);
        }
    }
}
