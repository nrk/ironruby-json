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

namespace IronRuby.Libraries.Json {
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

        #region constructor 

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

        #endregion

        #region static methods

        public static void Configure(GeneratorState/*!*/ self, Hash/*!*/ configuration) {
            // TODO: convert?

            if (configuration.ContainsKey(Helpers.GetGeneratorStateKey("indent"))) {
                self._indent = configuration[Helpers.GetGeneratorStateKey("indent")] as MutableString;
            }

            if (configuration.ContainsKey(Helpers.GetGeneratorStateKey("space"))) {
                self._space = configuration[Helpers.GetGeneratorStateKey("space")] as MutableString;
            }

            if (configuration.ContainsKey(Helpers.GetGeneratorStateKey("space_before"))) {
                self._spaceBefore = configuration[Helpers.GetGeneratorStateKey("space_before")] as MutableString;
            }

            if (configuration.ContainsKey(Helpers.GetGeneratorStateKey("array_nl"))) {
                self._arrayNl = configuration[Helpers.GetGeneratorStateKey("array_nl")] as MutableString;
            }

            if (configuration.ContainsKey(Helpers.GetGeneratorStateKey("object_nl"))) {
                self._objectNl = configuration[Helpers.GetGeneratorStateKey("object_nl")] as MutableString;
            }

            if (configuration.ContainsKey(Helpers.GetGeneratorStateKey("check_circular"))) {
                Object cc = configuration[Helpers.GetGeneratorStateKey("check_circular")];
                if (cc is Boolean) {
                    self._checkCircular = (bool)cc;
                }
                else {
                    self._checkCircular = false;
                }
            }

            if (configuration.ContainsKey(Helpers.GetGeneratorStateKey("max_nesting"))) {
                Object mn = configuration[Helpers.GetGeneratorStateKey("max_nesting")];
                // TODO: need to inspect
                self._maxNesting = (mn is Boolean && (bool)mn == false) ? 0 : (Int32)mn;
            }

            if (configuration.ContainsKey(Helpers.GetGeneratorStateKey("allow_nan"))) {
                Object an = configuration[Helpers.GetGeneratorStateKey("allow_nan")];
                if (an is Boolean) {
                    self._allowNaN = (bool)an;
                }
                else {
                    self._allowNaN = false;
                }
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

        #endregion

        #region instance methods

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

        #endregion

        #region properties

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

        #endregion
    }
}
