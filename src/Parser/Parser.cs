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

            _json = new ParserEngineState(this, scope, source);
            if (options.Count > 0) {
                if (options.ContainsKey(_maxNesting)) {
                    _json.MaxNesting = options[_maxNesting] is int ? (int)options[_maxNesting] : 0;
                }
                _json.AllowNaN = options.ContainsKey(_allowNan) ? (bool)options[_allowNan] : ParserEngineState.DEFAULT_ALLOW_NAN;
                // TODO: check needed, create_id could be TrueClass, FalseClass, NilClass or String
                _json.CreateID = options.ContainsKey(_createAdditions) && (bool)options[_createAdditions] ? Helpers.GetCreateId(scope) : null;
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

        public MutableString Source {
            get { return _json.OriginalSource; }
        }
    }
}
