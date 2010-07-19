using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using IronRuby.Builtins;
using IronRuby.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;

namespace IronRuby.JsonExt {
    public static partial class JSON {
        public static partial class Ext {

            #region JSON::Ext::Generator::State

            [RubyModule("Generator")]
            public class Generator {
                [RubyClass("State", Extends = typeof(GeneratorState))]
                public class GeneratorStateOps {

                    #region constructors and initializers

                    [RubyMethod("new", RubyMethodAttributes.PublicSingleton)]
                    public static GeneratorState/*!*/ CreateGeneratorState(RubyClass/*!*/ self, [Optional]Hash configuration) {
                        GeneratorState state = new GeneratorState();
                        if (configuration != null) {
                            Reinitialize(self.Context, state, configuration);
                        }
                        return state;
                    }

                    [RubyMethod("initialize", RubyMethodAttributes.PrivateInstance)]
                    public static GeneratorState Reinitialize(RubyContext context, GeneratorState self, Hash configuration) {
                        if (configuration != null) {
                            GeneratorState.Configure(context, self, configuration);
                        }

                        return self;
                    }

                    #endregion

                    #region instance methods

                    [RubyMethod("configure")]
                    public static GeneratorState Reconfigure(RubyContext context, GeneratorState/*!*/ self, Hash/*!*/ configuration) {
                        GeneratorState.Configure(context, self, configuration);
                        return self;
                    }

                    [RubyMethod("to_h")]
                    public static Hash ToHash(RubyContext/*!*/ context, GeneratorState/*!*/ self) {
                        // TODO: vOpts.respondsTo("to_hash")
                        Hash configurationHash = new Hash(context);

                        configurationHash.Add(Helpers.GetGeneratorStateKey(context, "indent"), self.Indent);
                        configurationHash.Add(Helpers.GetGeneratorStateKey(context, "space"), self.Space);
                        configurationHash.Add(Helpers.GetGeneratorStateKey(context, "space_before"), self.SpaceBefore);
                        configurationHash.Add(Helpers.GetGeneratorStateKey(context, "object_nl"), self.ObjectNl);
                        configurationHash.Add(Helpers.GetGeneratorStateKey(context, "array_nl"), self.ArrayNl);
                        configurationHash.Add(Helpers.GetGeneratorStateKey(context, "check_circular"), self.CheckCircular);
                        configurationHash.Add(Helpers.GetGeneratorStateKey(context, "allow_nan"), self.AllowNaN);
                        configurationHash.Add(Helpers.GetGeneratorStateKey(context, "max_nesting"), self.MaxNesting);

                        return configurationHash;
                    }

                    [RubyMethod("remember")]
                    public static bool ARemember(RubyContext context, GeneratorState self, Object obj) {
                        self.Remember(context, obj);
                        return true;
                    }

                    [RubyMethod("forget")]
                    public static Object AForget(RubyContext context, GeneratorState self, Object obj) {
                        return self.Forget(context, obj) ? true : null as Object;
                    }

                    [RubyMethod("seen?")]
                    public static Object ASeen(RubyContext context, GeneratorState self, Object obj) {
                        return self.Seen(context, obj) ? true : null as Object;
                    }

                    [RubyMethod("from_state", RubyMethodAttributes.PublicSingleton)]
                    public static GeneratorState FromState(RubyContext/*!*/ context, RubyModule/*!*/ self, Object/*!*/ source) {
                        if (source is GeneratorState) {
                            return source as GeneratorState;
                        }

                        if (source is Hash) {
                            return Reinitialize(context, new GeneratorState(), source as Hash);
                        }

                        return Reinitialize(context, new GeneratorState(), null);
                    }

                    #endregion

                    #region public accessors

                    [RubyMethod("indent")]
                    public static MutableString GetIndent(RubyContext context, GeneratorState self) {
                        return self.Indent;
                    }

                    [RubyMethod("indent=")]
                    public static void SetIndent(RubyContext context, GeneratorState self, MutableString value) {
                        self.Indent = value;
                    }

                    [RubyMethod("space")]
                    public static MutableString GetSpace(RubyContext context, GeneratorState self) {
                        return self.Space;
                    }

                    [RubyMethod("space=")]
                    public static void SetSpace(RubyContext context, GeneratorState self, MutableString value) {
                        self.Space = value;
                    }

                    [RubyMethod("space_before")]
                    public static MutableString GetSpaceBefore(RubyContext context, GeneratorState self) {
                        return self.SpaceBefore;
                    }

                    [RubyMethod("space_before=")]
                    public static void SetSpaceBefore(RubyContext context, GeneratorState self, MutableString value) {
                        self.SpaceBefore = value;
                    }

                    [RubyMethod("object_nl")]
                    public static MutableString GetObjectNl(RubyContext context, GeneratorState self) {
                        return self.ObjectNl;
                    }

                    [RubyMethod("object_nl=")]
                    public static void SetObjectNl(RubyContext context, GeneratorState self, MutableString value) {
                        self.ObjectNl = value;
                    }

                    [RubyMethod("array_nl")]
                    public static MutableString GetArrayNl(RubyContext context, GeneratorState self) {
                        return self.ObjectNl;
                    }

                    [RubyMethod("array_nl=")]
                    public static void SetArrayNl(RubyContext context, GeneratorState self, MutableString value) {
                        self.ArrayNl = value;
                    }

                    [RubyMethod("check_circular?")]
                    public static Boolean GetCheckCircular(RubyContext context, GeneratorState self) {
                        return self.CheckCircular;
                    }

                    [RubyMethod("max_nesting")]
                    public static Int32 GetMaxNesting(RubyContext context, GeneratorState self) {
                        return self.MaxNesting;
                    }

                    [RubyMethod("max_nesting=")]
                    public static void SetMaxNesting(RubyContext context, GeneratorState self, Int32 value) {
                        self.MaxNesting = value;
                    }

                    #endregion

                }
            }

            #endregion

        }
    }
}
