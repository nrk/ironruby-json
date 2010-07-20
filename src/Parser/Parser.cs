using System;
using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using IronRuby.Builtins;
using IronRuby.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;

namespace IronRuby.JsonExt {
    public class Parser {
        #region fields

        private const int JSON_MAX_NESTING = 19;
        private const bool JSON_ALLOW_NAN = false;

        private static RubySymbol _maxNesting;
        private static RubySymbol _allowNan;
        private static RubySymbol _jsonCreatable;
        private static RubySymbol _jsonCreate;
        private static RubySymbol _createId;
        private static RubySymbol _createAdditions;
        private static RubySymbol _chr;

        private ParserEngineState _json;

        #endregion

        public Parser(RubyScope scope, MutableString source)
            : this(scope, source, new Hash(scope.RubyContext)) { 
        }
        
        public Parser(RubyScope scope, MutableString source, Hash options) {
            InitializeLibrary(scope);

            _json = ParserEngine.InitializeState(this, source);
            _json.Scope = scope;
            _json.Context = scope.RubyContext;

            if (options.Count > 0) {
                if (options.ContainsKey(_maxNesting)) {
                    object maxNesting = options[_maxNesting];
                    if (maxNesting is int) {
                        _json.max_nesting = (int)maxNesting;
                    }
                    else {
                        // TODO: verify the actual behaviour JSON::Parser when passing a 
                        //       :max_nesting value different than false or nil.
                        _json.max_nesting = Int32.MaxValue;
                    }
                }
                else {
                    _json.max_nesting = JSON_MAX_NESTING;
                }
                _json.allow_nan = options.ContainsKey(_allowNan) ? (bool)(options[_allowNan] ?? JSON_ALLOW_NAN) : JSON_ALLOW_NAN;

                if (options.ContainsKey(_createAdditions)) {
                    //TODO: check needed, create_id could be TrueClass, FalseClass, NilClass or String
                    _json.create_id = (bool)options[_createAdditions] ? Helpers.GetCreateId(scope) : null;
                }
            }
            else {
                _json.max_nesting = JSON_MAX_NESTING;
                _json.allow_nan = JSON_ALLOW_NAN;
                _json.create_id = Helpers.GetCreateId(scope);
            }
        }

        public void InitializeLibrary(RubyScope scope) { 
            KernelOps.Require(scope, this, MutableString.CreateAscii("json/common"));

            _maxNesting = scope.RubyContext.CreateAsciiSymbol("max_nesting");
            _allowNan = scope.RubyContext.CreateAsciiSymbol("allow_nan");
            _jsonCreatable = scope.RubyContext.CreateAsciiSymbol("json_creatable?");
            _jsonCreate = scope.RubyContext.CreateAsciiSymbol("json_create");
            _createId = scope.RubyContext.CreateAsciiSymbol("create_id");
            _createAdditions = scope.RubyContext.CreateAsciiSymbol("create_additions");
            _chr = scope.RubyContext.CreateAsciiSymbol("chr");
        }

        public Object Parse() {
            return ParserEngine.Parse(_json);
        }

        public Object Source {
            get { return _json.source; }
        }
    }
}
