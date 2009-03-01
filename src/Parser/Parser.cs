using System;
using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using IronRuby.Builtins;
using IronRuby.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;

namespace IronRuby.Libraries.Json {
    public class Parser {
        #region symbols

        private static SymbolId _maxNesting = SymbolTable.StringToId("max_nesting");
        private static SymbolId _allowNan = SymbolTable.StringToId("allow_nan");
        private static SymbolId _jsonCreatable = SymbolTable.StringToId("json_creatable?");
        private static SymbolId _jsonCreate = SymbolTable.StringToId("json_create");
        private static SymbolId _createId = SymbolTable.StringToId("create_id");
        private static SymbolId _createAdditions = SymbolTable.StringToId("create_additions");
        private static SymbolId _chr = SymbolTable.StringToId("chr");

        #endregion

        #region fields

        private ParserEngineState _json;

        private RubyModule _eParserError;
        private RubyModule _eNestingError;

        #endregion

        #region constants 

        private const int JSON_MAX_NESTING = 19;
        private const bool JSON_ALLOW_NAN = false;

        #endregion

        #region constructor 

        public Parser(RubyScope scope, MutableString source)
            : this(scope, source, new Hash(scope.RubyContext)) { }


        public Parser(RubyScope scope, MutableString source, Hash options) {
            InitializeLibrary(scope);

            _json = ParserEngine.InitializeState(this, source);

            if (options.Count > 0) {
                _json.max_nesting = options.ContainsKey(_maxNesting) ? (int)options[_maxNesting] : JSON_MAX_NESTING;
                _json.allow_nan = options.ContainsKey(_allowNan) ? (bool)options[_allowNan] : JSON_ALLOW_NAN;

                if (options.ContainsKey(_createAdditions)) {
                    if ((bool)options[_createAdditions] == true) {
                        //TODO: check needed, create_id could be TrueClass, FalseClass, NilClass or String
                        MutableString createId = Helpers.GetCreateId(scope.RubyContext);
                        _json.create_id = createId;
                    }
                    else {
                        _json.create_id = null;
                    }
                }
            }
            else {
                _json.max_nesting = JSON_MAX_NESTING;
                _json.allow_nan = JSON_ALLOW_NAN;
                _json.create_id = Helpers.GetCreateId(scope.RubyContext);
            }
        }

        #endregion

        #region methods 

        public void InitializeLibrary(RubyScope scope) { 
            KernelOps.Require(scope, this, MutableString.Create("json/common"));

            bool loadParseError = scope.RubyContext.TryGetModule(scope.GlobalScope, "JSON::ParserError", out _eParserError);
            bool loadNestingError = scope.RubyContext.TryGetModule(scope.GlobalScope, "JSON::NestingError", out _eNestingError);
            if (!loadParseError || !loadNestingError) {
                // TODO: I am going to change this. Seriously.
                throw RubyExceptions.CreateNameError("Missing JSON::ParserError and/or JSON::NestingError");
            }
        }

        public Object Parse(RubyContext/*!*/ context) {
            _json.context = context;
            Object result = ParserEngine.Parse(_json);
            return result;
        }

        #endregion

        #region properties

        public RubyModule ParserError {
            get { return _eParserError; }
        }

        public RubyModule NestingError {
            get { return _eNestingError; }
        }

        #endregion
    }
}
