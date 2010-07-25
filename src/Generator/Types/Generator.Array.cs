using System;
using System.Collections;
using System.Runtime.InteropServices;
using IronRuby.Builtins;
using IronRuby.Runtime;

namespace IronRuby.JsonExt {
    public static partial class Generator {
        public static MutableString ToJson(RubyContext context, IList self, GeneratorState state, int? depth) {
            MutableString result;

            if (state == null) {
                result = MutableString.CreateMutable(2 + Math.Max(self.Count * 4, 0), RubyEncoding.UTF8);
                result.Append('[');
                context.TaintObjectBy<Object>(result, self);

                if (self.Count > 0) {
                    for (int i = 0; i < self.Count; i++) {
                        Object element = self[i];
                        result.Append(Generator.ToJson(context, element, null, 0));
                        context.TaintObjectBy<Object>(result, element);
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

            context.TaintObjectBy<Object>(result, self);
            return result;
        }

        private static MutableString Transform(RubyContext context, IList self, GeneratorState state, int depth) {
            MutableString result = MutableString.CreateMutable(2 + Math.Max(self.Count * 4, 0), RubyEncoding.UTF8);

            byte[] indentUnit = state.Indent.ToByteArray();
            byte[] shift = Helpers.Repeat(indentUnit, depth + 1);

            byte[] arrayNl = state.ArrayNl.ToByteArray();
            byte[] delim = new byte[1 + arrayNl.Length];
            delim[0] = (byte)',';
            Array.Copy(arrayNl, 0, delim, 1, arrayNl.Length);

            state.CheckMaxNesting(depth + 1);
            context.TaintObjectBy<Object>(result, self);

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
                        context.TaintObjectBy<Object>(result, element);

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
                        context.TaintObjectBy<Object>(result, element);

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
    }

    [RubyModule(Extends = typeof(IList))]
    public static class IListOps {
        [RubyMethod("to_json")]
        public static MutableString ToJson(RubyContext/*!*/ context, IList self,
            [Optional]GeneratorState state, [Optional]Int32 depth) {

            return Generator.ToJson(context, self, state, depth);
        }
    }
    
    [RubyClass(Extends = typeof(RubyArray))]
    public static class ArrayOps {
        [RubyMethod("to_json")]
        public static MutableString ToJson(RubyContext/*!*/ context, RubyArray self,
            [Optional]GeneratorState state, [Optional]Int32 depth) {

            return Generator.ToJson(context, self, state, depth);
        }
    }
}
