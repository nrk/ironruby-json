using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using IronRuby.Builtins;
using IronRuby.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;

namespace IronRuby.JsonExt {
    public class GeneratorState {
        #region fields 

        public const Int32 DEFAULT_MAX_NESTING = 19;
        private List<long> _seen;

        #endregion

        public GeneratorState() {
            _seen = new List<long>();

            Indent = MutableString.CreateEmpty();
            Space = MutableString.CreateEmpty();
            SpaceBefore = MutableString.CreateEmpty();
            ObjectNl = MutableString.CreateEmpty();
            ArrayNl = MutableString.CreateEmpty();

            CheckCircular = true;
            AllowNaN = false;
            MaxNesting = DEFAULT_MAX_NESTING;
        }

        public static void Configure(RubyContext/*!*/ context, GeneratorState/*!*/ self, Hash/*!*/ configuration) {
            if (configuration.ContainsKey(Helpers.GetGeneratorStateKey(context, "indent"))) {
                self.Indent = configuration[Helpers.GetGeneratorStateKey(context, "indent")] as MutableString;
            }
            if (configuration.ContainsKey(Helpers.GetGeneratorStateKey(context, "space"))) {
                self.Space = configuration[Helpers.GetGeneratorStateKey(context, "space")] as MutableString;
            }
            if (configuration.ContainsKey(Helpers.GetGeneratorStateKey(context, "space_before"))) {
                self.SpaceBefore = configuration[Helpers.GetGeneratorStateKey(context, "space_before")] as MutableString;
            }
            if (configuration.ContainsKey(Helpers.GetGeneratorStateKey(context, "array_nl"))) {
                self.ArrayNl = configuration[Helpers.GetGeneratorStateKey(context, "array_nl")] as MutableString;
            }
            if (configuration.ContainsKey(Helpers.GetGeneratorStateKey(context, "object_nl"))) {
                self.ObjectNl = configuration[Helpers.GetGeneratorStateKey(context, "object_nl")] as MutableString;
            }
            if (configuration.ContainsKey(Helpers.GetGeneratorStateKey(context, "check_circular"))) {
                Object cc = configuration[Helpers.GetGeneratorStateKey(context, "check_circular")];
                self.CheckCircular = cc is bool ? (bool)cc : false;
            }
            if (configuration.ContainsKey(Helpers.GetGeneratorStateKey(context, "max_nesting"))) {
                Object mn = configuration[Helpers.GetGeneratorStateKey(context, "max_nesting")];
                self.MaxNesting = (mn is int) ? (int)mn : 0;
            }
            if (configuration.ContainsKey(Helpers.GetGeneratorStateKey(context, "allow_nan"))) {
                Object an = configuration[Helpers.GetGeneratorStateKey(context, "allow_nan")];
                self.AllowNaN = an is bool ? (bool)an : false;
            }
        }

        public static GeneratorState Ensure(Object obj) {
            if (obj is GeneratorState) {
                return obj as GeneratorState;
            }
            else {
                // TODO: must investigate
                throw RubyExceptions.CreateTypeError("GeneratorState");
            }
        }

        public void CheckMaxNesting(int depth) {
            if (MaxNesting != 0 && depth > MaxNesting) {
                Helpers.ThrowNestingException("nesting of {0:d} is to deep", depth);
            }
        }

        public void Remember(RubyContext context, Object obj) {
            long objectId = RubyUtils.GetObjectId(context, obj);
            if (!_seen.Contains(objectId)) {
                _seen.Add(objectId);
            }
        }

        public bool Forget(RubyContext context, Object obj) {
            return _seen.Remove(RubyUtils.GetObjectId(context, obj));
        }
        
        public bool Seen(RubyContext context, Object obj) {
            return _seen.Contains(RubyUtils.GetObjectId(context, obj));
        }
        
        public MutableString Indent { get; set; }

        public MutableString Space { get; set; }

        public MutableString SpaceBefore { get; set; }

        public MutableString ObjectNl { get; set; }

        public MutableString ArrayNl { get; set; }

        public Boolean CheckCircular { get; set; }

        public Boolean AllowNaN { get; set; }

        public Int32 MaxNesting { get; set; }
    }
}
