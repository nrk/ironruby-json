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

        private RespondToStorage _respondToStorage;

        #endregion

        #region constants 

        private const int JSON_MAX_NESTING = 19;
        private const bool JSON_ALLOW_NAN = false;

        #endregion

        #region constructor 

        public Parser(RubyScope scope, RespondToStorage respondToStorage, MutableString source)
            : this(scope, respondToStorage, source, new Hash(scope.RubyContext)) { }


        public Parser(RubyScope scope, RespondToStorage respondToStorage, MutableString source, Hash options) {
            InitializeLibrary(scope, respondToStorage);

            _json = ParserEngine.InitializeState(this, source);

            if (options.Count > 0) {
                _json.max_nesting = options.ContainsKey(_maxNesting) ? (int)options[_maxNesting] : JSON_MAX_NESTING;
                _json.allow_nan = options.ContainsKey(_allowNan) ? (bool)options[_allowNan] : JSON_ALLOW_NAN;

                if (options.ContainsKey(_createAdditions)) {
                    if ((bool)options[_createAdditions] == true) {
                        //TODO: check needed, create_id could be TrueClass, FalseClass, NilClass or String
                        MutableString createId = Helpers.GetCreateId(scope);
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
                _json.create_id = Helpers.GetCreateId(scope);
            }
        }

        #endregion

        #region methods 

        public void InitializeLibrary(RubyScope scope, RespondToStorage respondToStorage) { 
            KernelOps.Require(scope, this, MutableString.CreateAscii("json/common"));

            _respondToStorage = respondToStorage;
        }

        public Object Parse(RubyScope/*!*/ scope) {
            _json.scope = scope;
            _json.context = scope.RubyContext;
            Object result = ParserEngine.Parse(_json);
            return result;
        }

        #endregion

        #region properties

        public RespondToStorage RespondToStorage {
            get { return _respondToStorage; }
        }

        #endregion
    }
}
