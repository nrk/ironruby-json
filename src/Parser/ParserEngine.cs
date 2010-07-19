using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Scripting;
using Microsoft.Scripting.Utils;
using Microsoft.Scripting.Runtime;
using IronRuby.Builtins;
using IronRuby.Runtime;
using IronRuby.Runtime.Calls;

namespace IronRuby.JsonExt {
    using JSONCreatableCallSite = CallSite<Func<CallSite, RubyContext, Object, Object>>;
    using JSONCreateCallSite = CallSite<Func<CallSite, RubyContext, Object, Object, Object>>;

    static class ParserEngine {
        /* Just a quick note about the current implementation of the JSON_parse_* methods in 
         * this class. As you can see there are ref ints in the argument list of each methods, 
         * this is actually needed to maximize the compatibility with the original C code of 
         * the native json ext as "p" and "pe" are pointers to a char[] and they are passed 
         * back and forth across the functions calls to retain the current position when 
         * parsing "source". Honestly speaking, it does not look so good from a OO point of 
         * view. Ideally, "source", "p" and "pe" could be merged in the ParserEngineState 
         * instance which retains a few other states anyway. ParserEngineState could be made 
         * just a little more intelligent too, but this is going to need some rework so 
         * I am thinking of postponing this until everything is stable.
         */

        #region constants

        private const Int32 EVIL = 0x666;

        private const Int32 DEFAULT_MAX_NESTING = 19;

        private const String JSON_MINUS_INFINITY = "-Infinity";
        private const Double CONSTANT_NAN = Double.NaN;
        private const Double CONSTANT_INFINITY = Double.PositiveInfinity;
        private const Double CONSTANT_MINUS_INFINITY = Double.NegativeInfinity;

        #endregion

        #region callsites 

        private static readonly JSONCreatableCallSite _jsonCreatableCallSite = JSONCreatableCallSite.Create(RubyCallAction.MakeShared("json_creatable?", RubyCallSignature.Simple(0)));
        private static readonly JSONCreateCallSite _jsonCreateCallSite = JSONCreateCallSite.Create(RubyCallAction.MakeShared("json_create", RubyCallSignature.Simple(0)));

        #endregion



        #region ** ragel generated code **

        static readonly sbyte[] _JSON_object_actions = new sbyte[] {
	        0, 1, 0, 1, 1, 1, 2
        };

        static readonly sbyte[] _JSON_object_key_offsets = new sbyte[] {
	        0, 0, 1, 8, 14, 16, 17, 19, 
	        20, 36, 43, 49, 51, 52, 54, 55, 
	        57, 58, 60, 61, 63, 64, 66, 67, 
	        69, 70, 72, 73
        };

        static readonly char[] _JSON_object_trans_keys = new char[] {
	        '\u007b', '\u000d', '\u0020', '\u0022', '\u002f', '\u007d', '\u0009', '\u000a', 
	        '\u000d', '\u0020', '\u002f', '\u003a', '\u0009', '\u000a', '\u002a', '\u002f', 
	        '\u002a', '\u002a', '\u002f', '\u000a', '\u000d', '\u0020', '\u0022', '\u002d', 
	        '\u002f', '\u0049', '\u004e', '\u005b', '\u0066', '\u006e', '\u0074', '\u007b', 
	        '\u0009', '\u000a', '\u0030', '\u0039', '\u000d', '\u0020', '\u002c', '\u002f', 
	        '\u007d', '\u0009', '\u000a', '\u000d', '\u0020', '\u0022', '\u002f', '\u0009', 
	        '\u000a', '\u002a', '\u002f', '\u002a', '\u002a', '\u002f', '\u000a', '\u002a', 
	        '\u002f', '\u002a', '\u002a', '\u002f', '\u000a', '\u002a', '\u002f', '\u002a', 
	        '\u002a', '\u002f', '\u000a', '\u002a', '\u002f', '\u002a', '\u002a', '\u002f', 
	        '\u000a', (char) 0
        };

        static readonly sbyte[] _JSON_object_single_lengths = new sbyte[] {
	        0, 1, 5, 4, 2, 1, 2, 1, 
	        12, 5, 4, 2, 1, 2, 1, 2, 
	        1, 2, 1, 2, 1, 2, 1, 2, 
	        1, 2, 1, 0
        };

        static readonly sbyte[] _JSON_object_range_lengths = new sbyte[] {
	        0, 0, 1, 1, 0, 0, 0, 0, 
	        2, 1, 1, 0, 0, 0, 0, 0, 
	        0, 0, 0, 0, 0, 0, 0, 0, 
	        0, 0, 0, 0
        };

        static readonly sbyte[] _JSON_object_index_offsets = new sbyte[] {
	        0, 0, 2, 9, 15, 18, 20, 23, 
	        25, 40, 47, 53, 56, 58, 61, 63, 
	        66, 68, 71, 73, 76, 78, 81, 83, 
	        86, 88, 91, 93
        };

        static readonly sbyte[] _JSON_object_indicies = new sbyte[] {
	        0, 1, 0, 0, 2, 3, 4, 0, 
	        1, 5, 5, 6, 7, 5, 1, 8, 
	        9, 1, 10, 8, 10, 5, 8, 5, 
	        9, 7, 7, 11, 11, 12, 11, 11, 
	        11, 11, 11, 11, 11, 7, 11, 1, 
	        13, 13, 14, 15, 4, 13, 1, 14, 
	        14, 2, 16, 14, 1, 17, 18, 1, 
	        19, 17, 19, 14, 17, 14, 18, 20, 
	        21, 1, 22, 20, 22, 13, 20, 13, 
	        21, 23, 24, 1, 25, 23, 25, 7, 
	        23, 7, 24, 26, 27, 1, 28, 26, 
	        28, 0, 26, 0, 27, 1, 0
        };

        static readonly sbyte[] _JSON_object_trans_targs = new sbyte[] {
	        2, 0, 3, 23, 27, 3, 4, 8, 
	        5, 7, 6, 9, 19, 9, 10, 15, 
	        11, 12, 14, 13, 16, 18, 17, 20, 
	        22, 21, 24, 26, 25
        };

        static readonly sbyte[] _JSON_object_trans_actions = new sbyte[] {
	        0, 0, 3, 0, 5, 0, 0, 0, 
	        0, 0, 0, 1, 0, 0, 0, 0, 
	        0, 0, 0, 0, 0, 0, 0, 0, 
	        0, 0, 0, 0, 0
        };

        const int JSON_object_start = 1;
        const int JSON_object_first_final = 27;
        const int JSON_object_error = 0;

        const int JSON_object_en_main = 1;

        #endregion


        #region JSON_parse_object

        static int? JSON_parse_object(ParserEngineState json, String source, ref int p, ref int pe, ref Object result) {
            int cs = EVIL;
            Object last_name = null;

            if (json.max_nesting > 0 && json.current_nesting > json.max_nesting) {
                Helpers.ThrowNestingException("nesting of {0:d} is to deep", json.current_nesting);
            }

            result = new Hash(json.context);

            #region ** ragel generated code **

            {
                cs = JSON_object_start;
            }

            {
                sbyte _klen;
                sbyte _trans;
                sbyte _acts;
                sbyte _nacts;
                sbyte _keys;

                if (p == pe)
                    goto _test_eof;
                if (cs == 0)
                    goto _out;
            _resume:
                _keys = _JSON_object_key_offsets[cs];
                _trans = (sbyte)_JSON_object_index_offsets[cs];

                _klen = _JSON_object_single_lengths[cs];
                if (_klen > 0) {
                    sbyte _lower = _keys;
                    sbyte _mid;
                    sbyte _upper = (sbyte)(_keys + _klen - 1);
                    while (true) {
                        if (_upper < _lower)
                            break;

                        _mid = (sbyte)(_lower + ((_upper - _lower) >> 1));
                        if (source[p] < _JSON_object_trans_keys[_mid])
                            _upper = (sbyte)(_mid - 1);
                        else if (source[p] > _JSON_object_trans_keys[_mid])
                            _lower = (sbyte)(_mid + 1);
                        else {
                            _trans += (sbyte)(_mid - _keys);
                            goto _match;
                        }
                    }
                    _keys += (sbyte)_klen;
                    _trans += (sbyte)_klen;
                }

                _klen = _JSON_object_range_lengths[cs];
                if (_klen > 0) {
                    sbyte _lower = _keys;
                    sbyte _mid;
                    sbyte _upper = (sbyte)(_keys + (_klen << 1) - 2);
                    while (true) {
                        if (_upper < _lower)
                            break;

                        _mid = (sbyte)(_lower + (((_upper - _lower) >> 1) & ~1));
                        if (source[p] < _JSON_object_trans_keys[_mid])
                            _upper = (sbyte)(_mid - 2);
                        else if (source[p] > _JSON_object_trans_keys[_mid + 1])
                            _lower = (sbyte)(_mid + 2);
                        else {
                            _trans += (sbyte)((_mid - _keys) >> 1);
                            goto _match;
                        }
                    }
                    _trans += (sbyte)_klen;
                }

            _match:
                _trans = (sbyte)_JSON_object_indicies[_trans];
                cs = _JSON_object_trans_targs[_trans];

                if (_JSON_object_trans_actions[_trans] == 0)
                    goto _again;

                _acts = _JSON_object_trans_actions[_trans];
                _nacts = _JSON_object_actions[_acts++];
                while (_nacts-- > 0) {
                    switch (_JSON_object_actions[_acts++]) {
                        case 0: {
                                Object v = null;
                                int? np = JSON_parse_value(json, source, ref p, ref pe, ref v);
                                if (!np.HasValue) {
                                    p--; { p++; if (true) goto _out; }
                                }
                                else {
                                    (result as Hash).Add(last_name, v); { p = ((np.Value)) - 1; }
                                }
                            }
                            break;
                        case 1: {
                                int? np = JSON_parse_string(json, source, ref p, ref pe, ref last_name);
                                if (!np.HasValue) { p--; { p++; if (true) goto _out; } } else { p = ((np.Value)) - 1; }
                            }
                            break;
                        case 2: { p--; { p++; if (true) goto _out; } }
                            break;
                        default: break;
                    }
                }

            _again:
                if (cs == 0)
                    goto _out;
                if (++p != pe)
                    goto _resume;
            _test_eof: { }
            _out: { }
            }

            #endregion

            if (cs >= JSON_object_first_final) {
                if (json.create_id != null && Helpers.IsJsonClass(result as Hash)) {
                    Object classNameT = (result as Hash)[json.create_id];
                    if (classNameT != null) {
                        RubyModule classClass;
                        String className = (classNameT as MutableString).ToString();
                        if (!json.context.TryGetModule(json.scope.GlobalScope, className, out classClass)) {
                            throw RubyExceptions.CreateArgumentError(String.Format("can't find const {0}", className));
                        }

                        if (Protocols.RespondTo(json.parser.RespondToStorage, classClass, "json_creatable?")) {
                            bool creatable = (bool) _jsonCreatableCallSite.Target(_jsonCreatableCallSite, json.context, classClass);
                            if (creatable) {
                                result = _jsonCreateCallSite.Target(_jsonCreateCallSite, json.context, classClass, result);
                            }
                        }
                    }
                }

                return p + 1;
            }
            else {
                return null;
            }
        }

        #endregion


        #region ** ragel generated code **

        static readonly sbyte[] _JSON_value_actions = new sbyte[] {
	        0, 1, 0, 1, 1, 1, 2, 1, 
	        3, 1, 4, 1, 5, 1, 6, 1, 
	        7, 1, 8, 1, 9
        };

        static readonly sbyte[] _JSON_value_key_offsets = new sbyte[] {
	        0, 0, 11, 12, 13, 14, 15, 16, 
	        17, 18, 19, 20, 21, 22, 23, 24, 
	        25, 26, 27, 28, 29, 30
        };

        static readonly char[] _JSON_value_trans_keys = new char[] {
	        '\u0022', '\u002d', '\u0049', '\u004e', '\u005b', '\u0066', '\u006e', '\u0074', 
	        '\u007b', '\u0030', '\u0039', '\u006e', '\u0066', '\u0069', '\u006e', '\u0069', 
	        '\u0074', '\u0079', '\u0061', '\u004e', '\u0061', '\u006c', '\u0073', '\u0065', 
	        '\u0075', '\u006c', '\u006c', '\u0072', '\u0075', '\u0065', (char) 0
        };

        static readonly sbyte[] _JSON_value_single_lengths = new sbyte[] {
	        0, 9, 1, 1, 1, 1, 1, 1, 
	        1, 1, 1, 1, 1, 1, 1, 1, 
	        1, 1, 1, 1, 1, 0
        };

        static readonly sbyte[] _JSON_value_range_lengths = new sbyte[] {
	        0, 1, 0, 0, 0, 0, 0, 0, 
	        0, 0, 0, 0, 0, 0, 0, 0, 
	        0, 0, 0, 0, 0, 0
        };

        static readonly sbyte[] _JSON_value_index_offsets = new sbyte[] {
	        0, 0, 11, 13, 15, 17, 19, 21, 
	        23, 25, 27, 29, 31, 33, 35, 37, 
	        39, 41, 43, 45, 47, 49
        };

        static readonly sbyte[] _JSON_value_trans_targs = new sbyte[] {
	        21, 21, 2, 9, 21, 11, 15, 18, 
	        21, 21, 0, 3, 0, 4, 0, 5, 
	        0, 6, 0, 7, 0, 8, 0, 21, 
	        0, 10, 0, 21, 0, 12, 0, 13, 
	        0, 14, 0, 21, 0, 16, 0, 17, 
	        0, 21, 0, 19, 0, 20, 0, 21, 
	        0, 0, 0
        };

        static readonly sbyte[] _JSON_value_trans_actions = new sbyte[] {
	        11, 13, 0, 0, 15, 0, 0, 0, 
	        17, 13, 0, 0, 0, 0, 0, 0, 
	        0, 0, 0, 0, 0, 0, 0, 9, 
	        0, 0, 0, 7, 0, 0, 0, 0, 
	        0, 0, 0, 3, 0, 0, 0, 0, 
	        0, 1, 0, 0, 0, 0, 0, 5, 
	        0, 0, 0
        };

        static readonly sbyte[] _JSON_value_from_state_actions = new sbyte[] {
	        0, 0, 0, 0, 0, 0, 0, 0, 
	        0, 0, 0, 0, 0, 0, 0, 0, 
	        0, 0, 0, 0, 0, 19
        };

        const int JSON_value_start = 1;
        const int JSON_value_first_final = 21;
        const int JSON_value_error = 0;

        const int JSON_value_en_main = 1;

        #endregion


        #region JSON_parse_value

        static int? JSON_parse_value(ParserEngineState json, String source, ref int p, ref int pe, ref Object result) {
            int cs = EVIL;

            #region ** ragel generated code **

            {
                cs = JSON_value_start;
            }

            {
                sbyte _klen;
                sbyte _trans;
                sbyte _acts;
                sbyte _nacts;
                sbyte _keys;

                if (p == pe)
                    goto _test_eof;
                if (cs == 0)
                    goto _out;
            _resume:
                _acts = _JSON_value_from_state_actions[cs];
                _nacts = _JSON_value_actions[_acts++];
                while (_nacts-- > 0) {
                    switch (_JSON_value_actions[_acts++]) {
                        case 9: { p--; { p++; if (true) goto _out; } }
                            break;
                        default: break;
                    }
                }

                _keys = _JSON_value_key_offsets[cs];
                _trans = (sbyte)_JSON_value_index_offsets[cs];

                _klen = _JSON_value_single_lengths[cs];
                if (_klen > 0) {
                    sbyte _lower = _keys;
                    sbyte _mid;
                    sbyte _upper = (sbyte)(_keys + _klen - 1);
                    while (true) {
                        if (_upper < _lower)
                            break;

                        _mid = (sbyte)(_lower + ((_upper - _lower) >> 1));
                        if (source[p] < _JSON_value_trans_keys[_mid])
                            _upper = (sbyte)(_mid - 1);
                        else if (source[p] > _JSON_value_trans_keys[_mid])
                            _lower = (sbyte)(_mid + 1);
                        else {
                            _trans += (sbyte)(_mid - _keys);
                            goto _match;
                        }
                    }
                    _keys += (sbyte)_klen;
                    _trans += (sbyte)_klen;
                }

                _klen = _JSON_value_range_lengths[cs];
                if (_klen > 0) {
                    sbyte _lower = _keys;
                    sbyte _mid;
                    sbyte _upper = (sbyte)(_keys + (_klen << 1) - 2);
                    while (true) {
                        if (_upper < _lower)
                            break;

                        _mid = (sbyte)(_lower + (((_upper - _lower) >> 1) & ~1));
                        if (source[p] < _JSON_value_trans_keys[_mid])
                            _upper = (sbyte)(_mid - 2);
                        else if (source[p] > _JSON_value_trans_keys[_mid + 1])
                            _lower = (sbyte)(_mid + 2);
                        else {
                            _trans += (sbyte)((_mid - _keys) >> 1);
                            goto _match;
                        }
                    }
                    _trans += (sbyte)_klen;
                }

            _match:
                cs = _JSON_value_trans_targs[_trans];

                if (_JSON_value_trans_actions[_trans] == 0)
                    goto _again;

                _acts = _JSON_value_trans_actions[_trans];
                _nacts = _JSON_value_actions[_acts++];
                while (_nacts-- > 0) {
                    switch (_JSON_value_actions[_acts++]) {
                        case 0: {
                                result = null;
                            }
                            break;
                        case 1: {
                                result = false;
                            }
                            break;
                        case 2: {
                                result = true;
                            }
                            break;
                        case 3: {
                                if (json.allow_nan) {
                                    result = CONSTANT_NAN;
                                }
                                else {
                                    Helpers.ThrowParserException("unexpected token at '{0}'", Helpers.GetMessageForException(source, p - 2, pe));
                                }
                            }
                            break;
                        case 4: {
                                if (json.allow_nan) {
                                    result = CONSTANT_INFINITY;
                                }
                                else {
                                    Helpers.ThrowParserException("unexpected token at '{0}'", Helpers.GetMessageForException(source, p - 8, pe));
                                }
                            }
                            break;
                        case 5: {
                                int? np = JSON_parse_string(json, source, ref p, ref pe, ref result);
                                if (!np.HasValue) { p--; { p++; if (true) goto _out; } } else { p = ((np.Value)) - 1; }
                            }
                            break;
                        case 6: {
                                int? np;
                                if (pe > p + 9 && String.CompareOrdinal(JSON_MINUS_INFINITY, 0, source, p, 9) == 0) {
                                    if (json.allow_nan) {
                                        result = CONSTANT_MINUS_INFINITY; { p = ((p + 10)) - 1; }
                                        p--; { p++; if (true) goto _out; }
                                    }
                                    else {
                                        Helpers.ThrowParserException("unexpected token at '{0}'", Helpers.GetMessageForException(source, p, pe));
                                    }
                                }
                                np = JSON_parse_float(json, source, ref p, ref pe, ref result);
                                if (np.HasValue) { p = ((np.Value)) - 1; }
                                np = JSON_parse_integer(json, source, ref p, ref pe, ref result);
                                if (np.HasValue) { p = ((np.Value)) - 1; }
                                p--; { p++; if (true) goto _out; }
                            }
                            break;
                        case 7: {
                                int? np;
                                json.current_nesting++;
                                np = JSON_parse_array(json, source, ref p, ref pe, ref result);
                                json.current_nesting--;
                                if (!np.HasValue) { p--; { p++; if (true) goto _out; } } else { p = ((np.Value)) - 1; }
                            }
                            break;
                        case 8: {
                                int? np;
                                json.current_nesting++;
                                np = JSON_parse_object(json, source, ref p, ref pe, ref result);
                                json.current_nesting--;
                                if (!np.HasValue) { p--; { p++; if (true) goto _out; } } else { p = ((np.Value)) - 1; }
                            }
                            break;
                        default: break;
                    }
                }

            _again:
                if (cs == 0)
                    goto _out;
                if (++p != pe)
                    goto _resume;
            _test_eof: { }
            _out: { }
            }
            #endregion

            if (cs >= JSON_value_first_final) {
                return p;
            }
            else {
                return null;
            }
        }

        #endregion
        

        #region ** ragel generated code **


        static readonly sbyte[] _JSON_integer_actions = new sbyte[] {
	        0, 1, 0
        };

        static readonly sbyte[] _JSON_integer_key_offsets = new sbyte[] {
	        0, 0, 4, 7, 9, 11
        };

        static readonly char[] _JSON_integer_trans_keys = new char[] {
	        '\u002d', '\u0030', '\u0031', '\u0039', '\u0030', '\u0031', '\u0039', '\u0030', 
	        '\u0039', '\u0030', '\u0039', (char) 0
        };

        static readonly sbyte[] _JSON_integer_single_lengths = new sbyte[] {
	        0, 2, 1, 0, 0, 0
        };

        static readonly sbyte[] _JSON_integer_range_lengths = new sbyte[] {
	        0, 1, 1, 1, 1, 0
        };

        static readonly sbyte[] _JSON_integer_index_offsets = new sbyte[] {
	        0, 0, 4, 7, 9, 11
        };

        static readonly sbyte[] _JSON_integer_indicies = new sbyte[] {
	        0, 2, 3, 1, 2, 3, 1, 1, 
	        4, 3, 4, 1, 0
        };

        static readonly sbyte[] _JSON_integer_trans_targs = new sbyte[] {
	        2, 0, 3, 4, 5
        };

        static readonly sbyte[] _JSON_integer_trans_actions = new sbyte[] {
	        0, 0, 0, 0, 1
        };

        const int JSON_integer_start = 1;
        const int JSON_integer_first_final = 5;
        const int JSON_integer_error = 0;

        const int JSON_integer_en_main = 1;

        #endregion


        #region JSON_parse_integer

        static int? JSON_parse_integer(ParserEngineState json, String source, ref int p, ref int pe, ref Object result) {
            int cs = EVIL;

            #region ** ragel generated code **
            {
                cs = JSON_integer_start;
            }
            #endregion

            json.memo = p;

            #region ** ragel generated code **
            {
                sbyte _klen;
                sbyte _trans;
                sbyte _acts;
                sbyte _nacts;
                sbyte _keys;

                if (p == pe)
                    goto _test_eof;
                if (cs == 0)
                    goto _out;
            _resume:
                _keys = _JSON_integer_key_offsets[cs];
                _trans = (sbyte)_JSON_integer_index_offsets[cs];

                _klen = _JSON_integer_single_lengths[cs];
                if (_klen > 0) {
                    sbyte _lower = _keys;
                    sbyte _mid;
                    sbyte _upper = (sbyte)(_keys + _klen - 1);
                    while (true) {
                        if (_upper < _lower)
                            break;

                        _mid = (sbyte)(_lower + ((_upper - _lower) >> 1));
                        if (source[p] < _JSON_integer_trans_keys[_mid])
                            _upper = (sbyte)(_mid - 1);
                        else if (source[p] > _JSON_integer_trans_keys[_mid])
                            _lower = (sbyte)(_mid + 1);
                        else {
                            _trans += (sbyte)(_mid - _keys);
                            goto _match;
                        }
                    }
                    _keys += (sbyte)_klen;
                    _trans += (sbyte)_klen;
                }

                _klen = _JSON_integer_range_lengths[cs];
                if (_klen > 0) {
                    sbyte _lower = _keys;
                    sbyte _mid;
                    sbyte _upper = (sbyte)(_keys + (_klen << 1) - 2);
                    while (true) {
                        if (_upper < _lower)
                            break;

                        _mid = (sbyte)(_lower + (((_upper - _lower) >> 1) & ~1));
                        if (source[p] < _JSON_integer_trans_keys[_mid])
                            _upper = (sbyte)(_mid - 2);
                        else if (source[p] > _JSON_integer_trans_keys[_mid + 1])
                            _lower = (sbyte)(_mid + 2);
                        else {
                            _trans += (sbyte)((_mid - _keys) >> 1);
                            goto _match;
                        }
                    }
                    _trans += (sbyte)_klen;
                }

            _match:
                _trans = (sbyte)_JSON_integer_indicies[_trans];
                cs = _JSON_integer_trans_targs[_trans];

                if (_JSON_integer_trans_actions[_trans] == 0)
                    goto _again;

                _acts = _JSON_integer_trans_actions[_trans];
                _nacts = _JSON_integer_actions[_acts++];
                while (_nacts-- > 0) {
                    switch (_JSON_integer_actions[_acts++]) {
                        case 0: { p--; { p++; if (true) goto _out; } }
                            break;
                        default: break;
                    }
                }

            _again:
                if (cs == 0)
                    goto _out;
                if (++p != pe)
                    goto _resume;
            _test_eof: { }
            _out: { }
            }

            #endregion

            if (cs >= JSON_integer_first_final) {
                int len = p - json.memo;
                result = Helpers.ToInteger(source.Substring(json.memo, len));
                return p + 1;
            }
            else {
                return null;
            }
        }

        #endregion


        #region ** ragel generated code **

        static readonly sbyte[] _JSON_float_actions = new sbyte[] {
	        0, 1, 0
        };

        static readonly sbyte[] _JSON_float_key_offsets = new sbyte[] {
	        0, 0, 4, 7, 10, 12, 18, 22, 
	        24, 30, 35
        };

        static readonly char[] _JSON_float_trans_keys = new char[] {
	        '\u002d', '\u0030', '\u0031', '\u0039', '\u0030', '\u0031', '\u0039', '\u002e', 
	        '\u0045', '\u0065', '\u0030', '\u0039', '\u0045', '\u0065', '\u002d', '\u002e', 
	        '\u0030', '\u0039', '\u002b', '\u002d', '\u0030', '\u0039', '\u0030', '\u0039', 
	        '\u0045', '\u0065', '\u002d', '\u002e', '\u0030', '\u0039', '\u002e', '\u0045', 
	        '\u0065', '\u0030', '\u0039', (char) 0
        };

        static readonly sbyte[] _JSON_float_single_lengths = new sbyte[] {
	        0, 2, 1, 3, 0, 2, 2, 0, 
	        2, 3, 0
        };

        static readonly sbyte[] _JSON_float_range_lengths = new sbyte[] {
	        0, 1, 1, 0, 1, 2, 1, 1, 
	        2, 1, 0
        };

        static readonly sbyte[] _JSON_float_index_offsets = new sbyte[] {
	        0, 0, 4, 7, 11, 13, 18, 22, 
	        24, 29, 34
        };

        static readonly sbyte[] _JSON_float_indicies = new sbyte[] {
	        0, 2, 3, 1, 2, 3, 1, 4, 
	        5, 5, 1, 6, 1, 5, 5, 1, 
	        6, 7, 8, 8, 9, 1, 9, 1, 
	        1, 1, 1, 9, 7, 4, 5, 5, 
	        3, 1, 1, 0
        };

        static readonly sbyte[] _JSON_float_trans_targs = new sbyte[] {
	        2, 0, 3, 9, 4, 6, 5, 10, 
	        7, 8
        };

        static readonly sbyte[] _JSON_float_trans_actions = new sbyte[] {
	        0, 0, 0, 0, 0, 0, 0, 1, 
	        0, 0
        };

        const int JSON_float_start = 1;
        const int JSON_float_first_final = 10;
        const int JSON_float_error = 0;

        const int JSON_float_en_main = 1;

        #endregion


        #region JSON_parse_float

        static int? JSON_parse_float(ParserEngineState json, String source, ref int p, ref int pe, ref Object result) {
            int cs = EVIL;

            #region ** ragel generated code **

            {
                cs = JSON_float_start;
            }

            #endregion

            json.memo = p;

            #region ** ragel generated code **

            {
                sbyte _klen;
                sbyte _trans;
                sbyte _acts;
                sbyte _nacts;
                sbyte _keys;

                if (p == pe)
                    goto _test_eof;
                if (cs == 0)
                    goto _out;
            _resume:
                _keys = _JSON_float_key_offsets[cs];
                _trans = (sbyte)_JSON_float_index_offsets[cs];

                _klen = _JSON_float_single_lengths[cs];
                if (_klen > 0) {
                    sbyte _lower = _keys;
                    sbyte _mid;
                    sbyte _upper = (sbyte)(_keys + _klen - 1);
                    while (true) {
                        if (_upper < _lower)
                            break;

                        _mid = (sbyte)(_lower + ((_upper - _lower) >> 1));
                        if (source[p] < _JSON_float_trans_keys[_mid])
                            _upper = (sbyte)(_mid - 1);
                        else if (source[p] > _JSON_float_trans_keys[_mid])
                            _lower = (sbyte)(_mid + 1);
                        else {
                            _trans += (sbyte)(_mid - _keys);
                            goto _match;
                        }
                    }
                    _keys += (sbyte)_klen;
                    _trans += (sbyte)_klen;
                }

                _klen = _JSON_float_range_lengths[cs];
                if (_klen > 0) {
                    sbyte _lower = _keys;
                    sbyte _mid;
                    sbyte _upper = (sbyte)(_keys + (_klen << 1) - 2);
                    while (true) {
                        if (_upper < _lower)
                            break;

                        _mid = (sbyte)(_lower + (((_upper - _lower) >> 1) & ~1));
                        if (source[p] < _JSON_float_trans_keys[_mid])
                            _upper = (sbyte)(_mid - 2);
                        else if (source[p] > _JSON_float_trans_keys[_mid + 1])
                            _lower = (sbyte)(_mid + 2);
                        else {
                            _trans += (sbyte)((_mid - _keys) >> 1);
                            goto _match;
                        }
                    }
                    _trans += (sbyte)_klen;
                }

            _match:
                _trans = (sbyte)_JSON_float_indicies[_trans];
                cs = _JSON_float_trans_targs[_trans];

                if (_JSON_float_trans_actions[_trans] == 0)
                    goto _again;

                _acts = _JSON_float_trans_actions[_trans];
                _nacts = _JSON_float_actions[_acts++];
                while (_nacts-- > 0) {
                    switch (_JSON_float_actions[_acts++]) {
                        case 0: { p--; { p++; if (true) goto _out; } }
                            break;
                        default: break;
                    }
                }

            _again:
                if (cs == 0)
                    goto _out;
                if (++p != pe)
                    goto _resume;
            _test_eof: { }
            _out: { }
            }

            #endregion

            if (cs >= JSON_float_first_final) {
                int len = p - json.memo;
                result = Helpers.ToFloat(source.Substring(json.memo, len));
                return p + 1;
            }
            else {
                p = json.memo;
                return null;
            }
        }

        #endregion


        #region ** ragel generated code **

        static readonly sbyte[] _JSON_array_actions = new sbyte[] {
	        0, 1, 0, 1, 1
        };

        static readonly sbyte[] _JSON_array_key_offsets = new sbyte[] {
	        0, 0, 1, 18, 25, 41, 43, 44, 
	        46, 47, 49, 50, 52, 53, 55, 56, 
	        58, 59
        };

        static readonly char[] _JSON_array_trans_keys = new char[] {
	        '\u005b', '\u000d', '\u0020', '\u0022', '\u002d', '\u002f', '\u0049', '\u004e', 
	        '\u005b', '\u005d', '\u0066', '\u006e', '\u0074', '\u007b', '\u0009', '\u000a', 
	        '\u0030', '\u0039', '\u000d', '\u0020', '\u002c', '\u002f', '\u005d', '\u0009', 
	        '\u000a', '\u000d', '\u0020', '\u0022', '\u002d', '\u002f', '\u0049', '\u004e', 
	        '\u005b', '\u0066', '\u006e', '\u0074', '\u007b', '\u0009', '\u000a', '\u0030', 
	        '\u0039', '\u002a', '\u002f', '\u002a', '\u002a', '\u002f', '\u000a', '\u002a', 
	        '\u002f', '\u002a', '\u002a', '\u002f', '\u000a', '\u002a', '\u002f', '\u002a', 
	        '\u002a', '\u002f', '\u000a', (char) 0
        };

        static readonly sbyte[] _JSON_array_single_lengths = new sbyte[] {
	        0, 1, 13, 5, 12, 2, 1, 2, 
	        1, 2, 1, 2, 1, 2, 1, 2, 
	        1, 0
        };

        static readonly sbyte[] _JSON_array_range_lengths = new sbyte[] {
	        0, 0, 2, 1, 2, 0, 0, 0, 
	        0, 0, 0, 0, 0, 0, 0, 0, 
	        0, 0
        };

        static readonly sbyte[] _JSON_array_index_offsets = new sbyte[] {
	        0, 0, 2, 18, 25, 40, 43, 45, 
	        48, 50, 53, 55, 58, 60, 63, 65, 
	        68, 70
        };

        static readonly sbyte[] _JSON_array_indicies = new sbyte[] {
	        0, 1, 0, 0, 2, 2, 3, 2, 
	        2, 2, 4, 2, 2, 2, 2, 0, 
	        2, 1, 5, 5, 6, 7, 4, 5, 
	        1, 6, 6, 2, 2, 8, 2, 2, 
	        2, 2, 2, 2, 2, 6, 2, 1, 
	        9, 10, 1, 11, 9, 11, 6, 9, 
	        6, 10, 12, 13, 1, 14, 12, 14, 
	        5, 12, 5, 13, 15, 16, 1, 17, 
	        15, 17, 0, 15, 0, 16, 1, 0
        };

        static readonly sbyte[] _JSON_array_trans_targs = new sbyte[] {
	        2, 0, 3, 13, 17, 3, 4, 9, 
	        5, 6, 8, 7, 10, 12, 11, 14, 
	        16, 15
        };

        static readonly sbyte[] _JSON_array_trans_actions = new sbyte[] {
	        0, 0, 1, 0, 3, 0, 0, 0, 
	        0, 0, 0, 0, 0, 0, 0, 0, 
	        0, 0
        };

        const int JSON_array_start = 1;
        const int JSON_array_first_final = 17;
        const int JSON_array_error = 0;

        const int JSON_array_en_main = 1;

        #endregion


        #region JSON_parse_array

        static int? JSON_parse_array(ParserEngineState json, String source, ref int p, ref int pe, ref Object result) {
            int cs = EVIL;

            if (json.max_nesting > 0 && json.current_nesting > json.max_nesting) {
                Helpers.ThrowNestingException("nesting of {0:d} is to deep", json.current_nesting);
            }
            result = new RubyArray();

            #region ** ragel generated code **

            {
                cs = JSON_array_start;
            }

            {
                sbyte _klen;
                sbyte _trans;
                sbyte _acts;
                sbyte _nacts;
                sbyte _keys;

                if (p == pe)
                    goto _test_eof;
                if (cs == 0)
                    goto _out;
            _resume:
                _keys = _JSON_array_key_offsets[cs];
                _trans = (sbyte)_JSON_array_index_offsets[cs];

                _klen = _JSON_array_single_lengths[cs];
                if (_klen > 0) {
                    sbyte _lower = _keys;
                    sbyte _mid;
                    sbyte _upper = (sbyte)(_keys + _klen - 1);
                    while (true) {
                        if (_upper < _lower)
                            break;

                        _mid = (sbyte)(_lower + ((_upper - _lower) >> 1));
                        if (source[p] < _JSON_array_trans_keys[_mid])
                            _upper = (sbyte)(_mid - 1);
                        else if (source[p] > _JSON_array_trans_keys[_mid])
                            _lower = (sbyte)(_mid + 1);
                        else {
                            _trans += (sbyte)(_mid - _keys);
                            goto _match;
                        }
                    }
                    _keys += (sbyte)_klen;
                    _trans += (sbyte)_klen;
                }

                _klen = _JSON_array_range_lengths[cs];
                if (_klen > 0) {
                    sbyte _lower = _keys;
                    sbyte _mid;
                    sbyte _upper = (sbyte)(_keys + (_klen << 1) - 2);
                    while (true) {
                        if (_upper < _lower)
                            break;

                        _mid = (sbyte)(_lower + (((_upper - _lower) >> 1) & ~1));
                        if (source[p] < _JSON_array_trans_keys[_mid])
                            _upper = (sbyte)(_mid - 2);
                        else if (source[p] > _JSON_array_trans_keys[_mid + 1])
                            _lower = (sbyte)(_mid + 2);
                        else {
                            _trans += (sbyte)((_mid - _keys) >> 1);
                            goto _match;
                        }
                    }
                    _trans += (sbyte)_klen;
                }

            _match:
                _trans = (sbyte)_JSON_array_indicies[_trans];
                cs = _JSON_array_trans_targs[_trans];

                if (_JSON_array_trans_actions[_trans] == 0)
                    goto _again;

                _acts = _JSON_array_trans_actions[_trans];
                _nacts = _JSON_array_actions[_acts++];
                while (_nacts-- > 0) {
                    switch (_JSON_array_actions[_acts++]) {
                        case 0: {
                                Object v = null;
                                int? np = JSON_parse_value(json, source, ref p, ref pe, ref v);
                                if (!np.HasValue) {
                                    p--; { p++; if (true) goto _out; }
                                }
                                else {
                                    (result as IList<Object>).Add(v); { p = ((np.Value)) - 1; }
                                }
                            }
                            break;
                        case 1: { p--; { p++; if (true) goto _out; } }
                            break;
                        default: break;
                    }
                }

            _again:
                if (cs == 0)
                    goto _out;
                if (++p != pe)
                    goto _resume;
            _test_eof: { }
            _out: { }
            }

            #endregion

            if (cs >= JSON_array_first_final) {
                return p + 1;
            }
            else {
                Helpers.ThrowParserException("unexpected token at '{0}'", Helpers.GetMessageForException(source, p, pe));
            }
            return null;
        }

        #endregion


        #region json_string_unescape

        static Object UnescapeJsonString(String source, ref int p, ref int pe) {
            JsonStringUnescaper unescaper = new JsonStringUnescaper(source.ToCharArray(p, pe - p));
            p = pe;
            return unescaper.Unescape();
        }

        #endregion


        #region ** ragel generated code **

        static readonly sbyte[] _JSON_string_actions = new sbyte[] {
	        0, 2, 0, 1
        };

        static readonly sbyte[] _JSON_string_key_offsets = new sbyte[] {
	        0, 0, 1, 5, 8, 14, 20, 26, 
	        32
        };

        static readonly char[] _JSON_string_trans_keys = new char[] {
	        '\u0022', '\u0022', '\u005c', '\u0000', '\u001f', '\u0075', '\u0000', '\u001f', 
	        '\u0030', '\u0039', '\u0041', '\u0046', '\u0061', '\u0066', '\u0030', '\u0039', 
	        '\u0041', '\u0046', '\u0061', '\u0066', '\u0030', '\u0039', '\u0041', '\u0046', 
	        '\u0061', '\u0066', '\u0030', '\u0039', '\u0041', '\u0046', '\u0061', '\u0066', 
	        (char) 0
        };

        static readonly sbyte[] _JSON_string_single_lengths = new sbyte[] {
	        0, 1, 2, 1, 0, 0, 0, 0, 
	        0
        };

        static readonly sbyte[] _JSON_string_range_lengths = new sbyte[] {
	        0, 0, 1, 1, 3, 3, 3, 3, 
	        0
        };

        static readonly sbyte[] _JSON_string_index_offsets = new sbyte[] {
	        0, 0, 2, 6, 9, 13, 17, 21, 
	        25
        };

        static readonly sbyte[] _JSON_string_indicies = new sbyte[] {
	        0, 1, 2, 3, 1, 0, 4, 1, 
	        0, 5, 5, 5, 1, 6, 6, 6, 
	        1, 7, 7, 7, 1, 0, 0, 0, 
	        1, 1, 0
        };

        static readonly sbyte[] _JSON_string_trans_targs = new sbyte[] {
	        2, 0, 8, 3, 4, 5, 6, 7
        };

        static readonly sbyte[] _JSON_string_trans_actions = new sbyte[] {
	        0, 0, 1, 0, 0, 0, 0, 0
        };

        const int JSON_string_start = 1;
        const int JSON_string_first_final = 8;
        const int JSON_string_error = 0;

        const int JSON_string_en_main = 1;

        #endregion


        #region JSON_parse_string

        static int? JSON_parse_string(ParserEngineState json, String source, ref int p, ref int pe, ref Object result) {
            int cs = EVIL;

            //result = rb_str_new("", 0);
            result = MutableString.CreateEmpty();

            #region ** ragel generated code **

            {
                cs = JSON_string_start;
            }

            #endregion

            json.memo = p;

            #region ** ragel generated code **

            {
                sbyte _klen;
                sbyte _trans;
                sbyte _acts;
                sbyte _nacts;
                sbyte _keys;

                if (p == pe)
                    goto _test_eof;
                if (cs == 0)
                    goto _out;
            _resume:
                _keys = _JSON_string_key_offsets[cs];
                _trans = (sbyte)_JSON_string_index_offsets[cs];

                _klen = _JSON_string_single_lengths[cs];
                if (_klen > 0) {
                    sbyte _lower = _keys;
                    sbyte _mid;
                    sbyte _upper = (sbyte)(_keys + _klen - 1);
                    while (true) {
                        if (_upper < _lower)
                            break;

                        _mid = (sbyte)(_lower + ((_upper - _lower) >> 1));
                        if (source[p] < _JSON_string_trans_keys[_mid])
                            _upper = (sbyte)(_mid - 1);
                        else if (source[p] > _JSON_string_trans_keys[_mid])
                            _lower = (sbyte)(_mid + 1);
                        else {
                            _trans += (sbyte)(_mid - _keys);
                            goto _match;
                        }
                    }
                    _keys += (sbyte)_klen;
                    _trans += (sbyte)_klen;
                }

                _klen = _JSON_string_range_lengths[cs];
                if (_klen > 0) {
                    sbyte _lower = _keys;
                    sbyte _mid;
                    sbyte _upper = (sbyte)(_keys + (_klen << 1) - 2);
                    while (true) {
                        if (_upper < _lower)
                            break;

                        _mid = (sbyte)(_lower + (((_upper - _lower) >> 1) & ~1));
                        if (source[p] < _JSON_string_trans_keys[_mid])
                            _upper = (sbyte)(_mid - 2);
                        else if (source[p] > _JSON_string_trans_keys[_mid + 1])
                            _lower = (sbyte)(_mid + 2);
                        else {
                            _trans += (sbyte)((_mid - _keys) >> 1);
                            goto _match;
                        }
                    }
                    _trans += (sbyte)_klen;
                }

            _match:
                _trans = (sbyte)_JSON_string_indicies[_trans];
                cs = _JSON_string_trans_targs[_trans];

                if (_JSON_string_trans_actions[_trans] == 0)
                    goto _again;

                _acts = _JSON_string_trans_actions[_trans];
                _nacts = _JSON_string_actions[_acts++];
                while (_nacts-- > 0) {
                    switch (_JSON_string_actions[_acts++]) {
                        case 0: {
                                int curmemo = json.memo + 1;
                                result = UnescapeJsonString(source, ref curmemo, ref p);
                                json.memo = curmemo;
                                if (result == null) { p--; { p++; if (true) goto _out; } } else { p = ((p + 1)) - 1; }
                            }
                            break;
                        case 1: { p--; { p++; if (true) goto _out; } }
                            break;
                        default: break;
                    }
                }

            _again:
                if (cs == 0)
                    goto _out;
                if (++p != pe)
                    goto _resume;
            _test_eof: { }
            _out: { }
            }

            #endregion

            if (cs >= JSON_string_first_final) {
                return p + 1;
            }
            else {
                return null;
            }
        }

        #endregion


        #region ** ragel generated code **

        static readonly sbyte[] _JSON_actions = new sbyte[] {
	        0, 1, 0, 1, 1
        };

        static readonly sbyte[] _JSON_key_offsets = new sbyte[] {
	        0, 0, 7, 9, 10, 12, 13, 15, 
	        16, 18, 19
        };

        static readonly char[] _JSON_trans_keys = new char[] {
	        '\u000d', '\u0020', '\u002f', '\u005b', '\u007b', '\u0009', '\u000a', '\u002a', 
	        '\u002f', '\u002a', '\u002a', '\u002f', '\u000a', '\u002a', '\u002f', '\u002a', 
	        '\u002a', '\u002f', '\u000a', '\u000d', '\u0020', '\u002f', '\u0009', '\u000a', 
	        (char) 0
        };

        static readonly sbyte[] _JSON_single_lengths = new sbyte[] {
	        0, 5, 2, 1, 2, 1, 2, 1, 
	        2, 1, 3
        };

        static readonly sbyte[] _JSON_range_lengths = new sbyte[] {
	        0, 1, 0, 0, 0, 0, 0, 0, 
	        0, 0, 1
        };

        static readonly sbyte[] _JSON_index_offsets = new sbyte[] {
	        0, 0, 7, 10, 12, 15, 17, 20, 
	        22, 25, 27
        };

        static readonly sbyte[] _JSON_indicies = new sbyte[] {
	        0, 0, 2, 3, 4, 0, 1, 5, 
	        6, 1, 7, 5, 7, 0, 5, 0, 
	        6, 8, 9, 1, 10, 8, 10, 11, 
	        8, 11, 9, 11, 11, 12, 11, 1, 
	        0
        };

        static readonly sbyte[] _JSON_trans_targs = new sbyte[] {
	        1, 0, 2, 10, 10, 3, 5, 4, 
	        7, 9, 8, 10, 6
        };

        static readonly sbyte[] _JSON_trans_actions = new sbyte[] {
	        0, 0, 0, 3, 1, 0, 0, 0, 
	        0, 0, 0, 0, 0
        };

        const int JSON_start = 1;
        const int JSON_first_final = 10;
        const int JSON_error = 0;

        const int JSON_en_main = 1;

        #endregion


        public static ParserEngineState InitializeState(Parser parser, MutableString source) {
            ParserEngineState json = new ParserEngineState();

            json.parser = parser;
            json.vsource = source;
            json.source = source.ToString();

            json.allow_nan = true;
            json.current_nesting = 0;
            json.max_nesting = DEFAULT_MAX_NESTING;
            json.memo = 0;
            json.len = json.source.Length;

            return json;
        }

        public static Object Parse(ParserEngineState json) {
            int p, pe;
            int cs = EVIL;
            Object result = null;

            #region ** ragel generated code **

            {
                cs = JSON_start;
            }

            #endregion

            String source = json.source;
            p = 0;
            pe = p + json.len;

            #region ** ragel generated code **

            {
                sbyte _klen;
                sbyte _trans;
                sbyte _acts;
                sbyte _nacts;
                sbyte _keys;

                if (p == pe)
                    goto _test_eof;
                if (cs == 0)
                    goto _out;
            _resume:
                _keys = _JSON_key_offsets[cs];
                _trans = (sbyte)_JSON_index_offsets[cs];

                _klen = _JSON_single_lengths[cs];
                if (_klen > 0) {
                    sbyte _lower = _keys;
                    sbyte _mid;
                    sbyte _upper = (sbyte)(_keys + _klen - 1);
                    while (true) {
                        if (_upper < _lower)
                            break;

                        _mid = (sbyte)(_lower + ((_upper - _lower) >> 1));
                        if (source[p] < _JSON_trans_keys[_mid])
                            _upper = (sbyte)(_mid - 1);
                        else if (source[p] > _JSON_trans_keys[_mid])
                            _lower = (sbyte)(_mid + 1);
                        else {
                            _trans += (sbyte)(_mid - _keys);
                            goto _match;
                        }
                    }
                    _keys += (sbyte)_klen;
                    _trans += (sbyte)_klen;
                }

                _klen = _JSON_range_lengths[cs];
                if (_klen > 0) {
                    sbyte _lower = _keys;
                    sbyte _mid;
                    sbyte _upper = (sbyte)(_keys + (_klen << 1) - 2);
                    while (true) {
                        if (_upper < _lower)
                            break;

                        _mid = (sbyte)(_lower + (((_upper - _lower) >> 1) & ~1));
                        if (source[p] < _JSON_trans_keys[_mid])
                            _upper = (sbyte)(_mid - 2);
                        else if (source[p] > _JSON_trans_keys[_mid + 1])
                            _lower = (sbyte)(_mid + 2);
                        else {
                            _trans += (sbyte)((_mid - _keys) >> 1);
                            goto _match;
                        }
                    }
                    _trans += (sbyte)_klen;
                }

            _match:
                _trans = (sbyte)_JSON_indicies[_trans];
                cs = _JSON_trans_targs[_trans];

                if (_JSON_trans_actions[_trans] == 0)
                    goto _again;

                _acts = _JSON_trans_actions[_trans];
                _nacts = _JSON_actions[_acts++];
                while (_nacts-- > 0) {
                    switch (_JSON_actions[_acts++]) {
                        case 0: {
                                int? np;
                                json.current_nesting = 1;
                                np = JSON_parse_object(json, source, ref p, ref pe, ref result);
                                if (!np.HasValue) { p--; { p++; if (true) goto _out; } } else { p = ((np.Value)) - 1; }
                            }
                            break;
                        case 1: {
                                int? np;
                                json.current_nesting = 1;
                                np = JSON_parse_array(json, source, ref p, ref pe, ref result);
                                if (!np.HasValue) { p--; { p++; if (true) goto _out; } } else { p = ((np.Value)) - 1; }
                            }
                            break;
                        default: break;
                    }
                }

            _again:
                if (cs == 0)
                    goto _out;
                if (++p != pe)
                    goto _resume;
            _test_eof: { }
            _out: { }
            }

            #endregion

            if (cs >= JSON_first_final && p == pe) {
                return result;
            }
            else {
                Helpers.ThrowParserException("unexpected token at '{0}'", Helpers.GetMessageForException(source, p, pe));
            }

            return null;
        }
    }
}
