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

        private MutableString _indent;
        private MutableString _space;
        private MutableString _spaceBefore;
        private MutableString _objectNl;
        private MutableString _arrayNl;

        private List<long> _seen;
        private bool _checkCircular;
        private bool _allowNaN;
        private int _maxNesting;

        #endregion

        public GeneratorState() {
            _indent = MutableString.CreateEmpty();
            _space = MutableString.CreateEmpty();
            _spaceBefore = MutableString.CreateEmpty();
            _objectNl = MutableString.CreateEmpty();
            _arrayNl = MutableString.CreateEmpty();

            _seen = new List<long>();
            _checkCircular = true;
            _allowNaN = false;
            _maxNesting = DEFAULT_MAX_NESTING;
        }

        public static void Configure(RubyContext/*!*/ context, GeneratorState/*!*/ self, Hash/*!*/ configuration) {
            if (configuration.ContainsKey(Helpers.GetGeneratorStateKey(context, "indent"))) {
                self._indent = configuration[Helpers.GetGeneratorStateKey(context, "indent")] as MutableString;
            }
            if (configuration.ContainsKey(Helpers.GetGeneratorStateKey(context, "space"))) {
                self._space = configuration[Helpers.GetGeneratorStateKey(context, "space")] as MutableString;
            }
            if (configuration.ContainsKey(Helpers.GetGeneratorStateKey(context, "space_before"))) {
                self._spaceBefore = configuration[Helpers.GetGeneratorStateKey(context, "space_before")] as MutableString;
            }
            if (configuration.ContainsKey(Helpers.GetGeneratorStateKey(context, "array_nl"))) {
                self._arrayNl = configuration[Helpers.GetGeneratorStateKey(context, "array_nl")] as MutableString;
            }
            if (configuration.ContainsKey(Helpers.GetGeneratorStateKey(context, "object_nl"))) {
                self._objectNl = configuration[Helpers.GetGeneratorStateKey(context, "object_nl")] as MutableString;
            }
            if (configuration.ContainsKey(Helpers.GetGeneratorStateKey(context, "check_circular"))) {
                Object cc = configuration[Helpers.GetGeneratorStateKey(context, "check_circular")];
                self._checkCircular = cc is bool ? (bool)cc : false;
            }
            if (configuration.ContainsKey(Helpers.GetGeneratorStateKey(context, "max_nesting"))) {
                Object mn = configuration[Helpers.GetGeneratorStateKey(context, "max_nesting")];
                self._maxNesting = (mn is int) ? (int)mn : 0;
            }
            if (configuration.ContainsKey(Helpers.GetGeneratorStateKey(context, "allow_nan"))) {
                Object an = configuration[Helpers.GetGeneratorStateKey(context, "allow_nan")];
                self._allowNaN = an is bool ? (bool)an : false;
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
            if (_maxNesting != 0 && depth > _maxNesting) {
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
        
        public MutableString Indent {
            get { return _indent; }
            set { _indent = value; }
        }

        public MutableString Space {
            get { return _space; }
            set { _space = value; }
        }

        public MutableString SpaceBefore {
            get { return _spaceBefore; }
            set { _spaceBefore = value; }
        }

        public MutableString ObjectNl {
            get { return _objectNl; }
            set { _objectNl = value; }
        }

        public MutableString ArrayNl {
            get { return _arrayNl; }
            set { _arrayNl = value; }
        }

        public Boolean CheckCircular {
            get { return _checkCircular; }
            set { _checkCircular = value; }
        }

        public Boolean AllowNaN {
            get { return _allowNaN; }
            set { _allowNaN = value; }
        }

        public Int32 MaxNesting {
            get { return _maxNesting; }
            set { _maxNesting = value; }
        }
    }
}
