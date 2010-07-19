#pragma warning disable 169 // mcs: unused private method
[assembly: IronRuby.Runtime.RubyLibraryAttribute(typeof(IronRuby.JsonExt.JsonExtLibraryInitializer))]

namespace IronRuby.JsonExt {
    using System;
    using Microsoft.Scripting.Utils;
    
    public sealed class JsonExtLibraryInitializer : IronRuby.Builtins.LibraryInitializer {
        protected override void LoadModules() {
            IronRuby.Builtins.RubyClass classRef0 = GetClass(typeof(System.SystemException));
            
            
            ExtendClass(typeof(IronRuby.Builtins.FalseClass), 0x00000000, null, LoadIronRuby__Builtins__FalseClass_Instance, null, null, IronRuby.Builtins.RubyModule.EmptyArray);
            ExtendClass(typeof(IronRuby.Builtins.Hash), 0x00000000, null, LoadIronRuby__Builtins__Hash_Instance, null, null, IronRuby.Builtins.RubyModule.EmptyArray);
            ExtendClass(typeof(IronRuby.Builtins.MutableString), 0x00000000, null, LoadIronRuby__Builtins__MutableString_Instance, LoadIronRuby__Builtins__MutableString_Class, null, IronRuby.Builtins.RubyModule.EmptyArray);
            ExtendClass(typeof(IronRuby.Builtins.RubyArray), 0x00000000, null, LoadIronRuby__Builtins__RubyArray_Instance, null, null, IronRuby.Builtins.RubyModule.EmptyArray);
            ExtendClass(typeof(IronRuby.Builtins.TrueClass), 0x00000000, null, LoadIronRuby__Builtins__TrueClass_Instance, null, null, IronRuby.Builtins.RubyModule.EmptyArray);
            IronRuby.Builtins.RubyModule def3 = DefineGlobalModule("JSON__", typeof(IronRuby.JsonExt.JSON), 0x00000008, null, null, null, IronRuby.Builtins.RubyModule.EmptyArray);
            IronRuby.Builtins.RubyModule def6 = DefineModule("JSON__::Ext", typeof(IronRuby.JsonExt.JSON.Ext), 0x00000008, null, null, null, IronRuby.Builtins.RubyModule.EmptyArray);
            IronRuby.Builtins.RubyModule def1 = DefineModule("JSON__::Ext::Generator", typeof(IronRuby.JsonExt.JSON.Ext.Generator), 0x00000008, null, null, null, IronRuby.Builtins.RubyModule.EmptyArray);
            IronRuby.Builtins.RubyClass def4 = DefineClass("JSON__::JSONError", typeof(IronRuby.JsonExt.JSON.BaseException), 0x00000008, classRef0, null, null, null, IronRuby.Builtins.RubyModule.EmptyArray);
            ExtendClass(typeof(Microsoft.Scripting.Runtime.DynamicNull), 0x00000000, null, LoadMicrosoft__Scripting__Runtime__DynamicNull_Instance, null, null, IronRuby.Builtins.RubyModule.EmptyArray);
            ExtendModule(typeof(System.Collections.IDictionary), 0x00000000, LoadSystem__Collections__IDictionary_Instance, null, null, IronRuby.Builtins.RubyModule.EmptyArray);
            ExtendModule(typeof(System.Collections.IList), 0x00000000, LoadSystem__Collections__IList_Instance, null, null, IronRuby.Builtins.RubyModule.EmptyArray);
            ExtendClass(typeof(System.Double), 0x00000000, null, LoadSystem__Double_Instance, null, null, IronRuby.Builtins.RubyModule.EmptyArray);
            ExtendClass(typeof(System.Int32), 0x00000000, null, LoadSystem__Int32_Instance, null, null, IronRuby.Builtins.RubyModule.EmptyArray);
            ExtendClass(typeof(System.Object), 0x00000000, null, LoadSystem__Object_Instance, null, null, IronRuby.Builtins.RubyModule.EmptyArray);
            ExtendClass(typeof(System.String), 0x00000000, null, LoadSystem__String_Instance, null, null, IronRuby.Builtins.RubyModule.EmptyArray);
            IronRuby.Builtins.RubyClass def2 = DefineClass("JSON__::Ext::Generator::State", typeof(IronRuby.JsonExt.GeneratorState), 0x00000000, Context.ObjectClass, LoadJSON____Ext__Generator__State_Instance, LoadJSON____Ext__Generator__State_Class, null, IronRuby.Builtins.RubyModule.EmptyArray);
            IronRuby.Builtins.RubyClass def10 = DefineClass("JSON__::Ext::Parser", typeof(IronRuby.JsonExt.Parser), 0x00000000, Context.ObjectClass, LoadJSON____Ext__Parser_Instance, null, null, IronRuby.Builtins.RubyModule.EmptyArray, 
                new Func<IronRuby.Runtime.RespondToStorage, IronRuby.Runtime.RubyScope, IronRuby.Builtins.RubyClass, IronRuby.Builtins.MutableString, IronRuby.Builtins.Hash, IronRuby.JsonExt.Parser>(IronRuby.JsonExt.JSON.Ext.ParserOps.CreateParser)
            );
            IronRuby.Builtins.RubyClass def7 = DefineClass("JSON__::GeneratorError", typeof(IronRuby.JsonExt.JSON.GenerateException), 0x00000008, def4, null, null, null, IronRuby.Builtins.RubyModule.EmptyArray);
            IronRuby.Builtins.RubyClass def9 = DefineClass("JSON__::ParserError", typeof(IronRuby.JsonExt.JSON.ParserException), 0x00000008, def4, null, null, null, IronRuby.Builtins.RubyModule.EmptyArray);
            IronRuby.Builtins.RubyClass def5 = DefineClass("JSON__::CircularDatastructure", typeof(IronRuby.JsonExt.JSON.CircularDatastructureException), 0x00000008, def7, null, null, null, IronRuby.Builtins.RubyModule.EmptyArray);
            IronRuby.Builtins.RubyClass def8 = DefineClass("JSON__::NestingError", typeof(IronRuby.JsonExt.JSON.NestingException), 0x00000008, def9, null, null, null, IronRuby.Builtins.RubyModule.EmptyArray);
            SetConstant(def3, "Ext", def6);
            SetConstant(def6, "Generator", def1);
            SetConstant(def3, "JSONError", def4);
            SetConstant(def1, "State", def2);
            SetConstant(def6, "Parser", def10);
            SetConstant(def3, "GeneratorError", def7);
            SetConstant(def3, "ParserError", def9);
            SetConstant(def3, "CircularDatastructure", def5);
            SetConstant(def3, "NestingError", def8);
        }
        
        private static void LoadIronRuby__Builtins__FalseClass_Instance(IronRuby.Builtins.RubyModule/*!*/ module) {
            DefineLibraryMethod(module, "to_json", 0x11, 
                0x00000000U, 
                new Func<System.Boolean, IronRuby.JsonExt.GeneratorState, System.Int32, IronRuby.Builtins.MutableString>(IronRuby.JsonExt.FalseOps.ToJson)
            );
            
        }
        
        private static void LoadIronRuby__Builtins__Hash_Instance(IronRuby.Builtins.RubyModule/*!*/ module) {
            DefineLibraryMethod(module, "to_json", 0x11, 
                0x00000000U, 
                new Func<IronRuby.Runtime.ConversionStorage<IronRuby.Builtins.MutableString>, IronRuby.Runtime.RubyScope, IronRuby.Builtins.Hash, IronRuby.JsonExt.GeneratorState, System.Int32, IronRuby.Builtins.MutableString>(IronRuby.JsonExt.HashOps.ToJson)
            );
            
        }
        
        private static void LoadIronRuby__Builtins__MutableString_Instance(IronRuby.Builtins.RubyModule/*!*/ module) {
            DefineLibraryMethod(module, "to_json", 0x11, 
                0x00000000U, 
                new Func<IronRuby.Builtins.MutableString, IronRuby.JsonExt.GeneratorState, System.Int32, IronRuby.Builtins.MutableString>(IronRuby.JsonExt.MutableStringOps.ToJson)
            );
            
            DefineLibraryMethod(module, "to_json_raw", 0x11, 
                0x00000000U, 
                new Func<IronRuby.Runtime.RubyScope, IronRuby.Builtins.MutableString, IronRuby.Builtins.MutableString>(IronRuby.JsonExt.MutableStringOps.ToJsonRaw)
            );
            
            DefineLibraryMethod(module, "to_json_raw_object", 0x11, 
                0x00000000U, 
                new Func<IronRuby.Runtime.RubyScope, IronRuby.Builtins.MutableString, IronRuby.Builtins.Hash>(IronRuby.JsonExt.MutableStringOps.ToJsonRawObject)
            );
            
        }
        
        private static void LoadIronRuby__Builtins__MutableString_Class(IronRuby.Builtins.RubyModule/*!*/ module) {
            DefineLibraryMethod(module, "json_create", 0x21, 
                0x00000000U, 
                new Func<IronRuby.Runtime.ConversionStorage<IronRuby.Runtime.IntegerValue>, IronRuby.Builtins.RubyClass, IronRuby.Builtins.Hash, IronRuby.Builtins.MutableString>(IronRuby.JsonExt.MutableStringOps.JsonCreate)
            );
            
        }
        
        private static void LoadIronRuby__Builtins__RubyArray_Instance(IronRuby.Builtins.RubyModule/*!*/ module) {
            DefineLibraryMethod(module, "to_json", 0x11, 
                0x00000000U, 
                new Func<IronRuby.Runtime.RubyScope, IronRuby.Builtins.RubyArray, IronRuby.JsonExt.GeneratorState, System.Int32, IronRuby.Builtins.MutableString>(IronRuby.JsonExt.ArrayOps.ToJson)
            );
            
        }
        
        private static void LoadIronRuby__Builtins__TrueClass_Instance(IronRuby.Builtins.RubyModule/*!*/ module) {
            DefineLibraryMethod(module, "to_json", 0x11, 
                0x00000000U, 
                new Func<System.Boolean, IronRuby.JsonExt.GeneratorState, System.Int32, IronRuby.Builtins.MutableString>(IronRuby.JsonExt.TrueOps.ToJson)
            );
            
        }
        
        private static void LoadJSON____Ext__Generator__State_Instance(IronRuby.Builtins.RubyModule/*!*/ module) {
            DefineLibraryMethod(module, "array_nl", 0x11, 
                0x00000000U, 
                new Func<IronRuby.Runtime.RubyContext, IronRuby.JsonExt.GeneratorState, IronRuby.Builtins.MutableString>(IronRuby.JsonExt.JSON.Ext.Generator.GeneratorStateOps.GetArrayNl)
            );
            
            DefineLibraryMethod(module, "array_nl=", 0x11, 
                0x00000000U, 
                new Action<IronRuby.Runtime.RubyContext, IronRuby.JsonExt.GeneratorState, IronRuby.Builtins.MutableString>(IronRuby.JsonExt.JSON.Ext.Generator.GeneratorStateOps.SetArrayNl)
            );
            
            DefineLibraryMethod(module, "check_circular?", 0x11, 
                0x00000000U, 
                new Func<IronRuby.Runtime.RubyContext, IronRuby.JsonExt.GeneratorState, System.Boolean>(IronRuby.JsonExt.JSON.Ext.Generator.GeneratorStateOps.GetCheckCircular)
            );
            
            DefineLibraryMethod(module, "configure", 0x11, 
                0x00000000U, 
                new Func<IronRuby.Runtime.RubyContext, IronRuby.JsonExt.GeneratorState, IronRuby.Builtins.Hash, IronRuby.JsonExt.GeneratorState>(IronRuby.JsonExt.JSON.Ext.Generator.GeneratorStateOps.Reconfigure)
            );
            
            DefineLibraryMethod(module, "forget", 0x11, 
                0x00000000U, 
                new Func<IronRuby.Runtime.RubyContext, IronRuby.JsonExt.GeneratorState, System.Object, System.Object>(IronRuby.JsonExt.JSON.Ext.Generator.GeneratorStateOps.AForget)
            );
            
            DefineLibraryMethod(module, "indent", 0x11, 
                0x00000000U, 
                new Func<IronRuby.Runtime.RubyContext, IronRuby.JsonExt.GeneratorState, IronRuby.Builtins.MutableString>(IronRuby.JsonExt.JSON.Ext.Generator.GeneratorStateOps.GetIndent)
            );
            
            DefineLibraryMethod(module, "indent=", 0x11, 
                0x00000000U, 
                new Action<IronRuby.Runtime.RubyContext, IronRuby.JsonExt.GeneratorState, IronRuby.Builtins.MutableString>(IronRuby.JsonExt.JSON.Ext.Generator.GeneratorStateOps.SetIndent)
            );
            
            DefineLibraryMethod(module, "initialize", 0x12, 
                0x00000000U, 
                new Func<IronRuby.Runtime.RubyContext, IronRuby.JsonExt.GeneratorState, IronRuby.Builtins.Hash, IronRuby.JsonExt.GeneratorState>(IronRuby.JsonExt.JSON.Ext.Generator.GeneratorStateOps.Reinitialize)
            );
            
            DefineLibraryMethod(module, "max_nesting", 0x11, 
                0x00000000U, 
                new Func<IronRuby.Runtime.RubyContext, IronRuby.JsonExt.GeneratorState, System.Int32>(IronRuby.JsonExt.JSON.Ext.Generator.GeneratorStateOps.GetMaxNesting)
            );
            
            DefineLibraryMethod(module, "max_nesting=", 0x11, 
                0x00000000U, 
                new Action<IronRuby.Runtime.RubyContext, IronRuby.JsonExt.GeneratorState, System.Int32>(IronRuby.JsonExt.JSON.Ext.Generator.GeneratorStateOps.SetMaxNesting)
            );
            
            DefineLibraryMethod(module, "object_nl", 0x11, 
                0x00000000U, 
                new Func<IronRuby.Runtime.RubyContext, IronRuby.JsonExt.GeneratorState, IronRuby.Builtins.MutableString>(IronRuby.JsonExt.JSON.Ext.Generator.GeneratorStateOps.GetObjectNl)
            );
            
            DefineLibraryMethod(module, "object_nl=", 0x11, 
                0x00000000U, 
                new Action<IronRuby.Runtime.RubyContext, IronRuby.JsonExt.GeneratorState, IronRuby.Builtins.MutableString>(IronRuby.JsonExt.JSON.Ext.Generator.GeneratorStateOps.SetObjectNl)
            );
            
            DefineLibraryMethod(module, "remember", 0x11, 
                0x00000000U, 
                new Func<IronRuby.Runtime.RubyContext, IronRuby.JsonExt.GeneratorState, System.Object, System.Boolean>(IronRuby.JsonExt.JSON.Ext.Generator.GeneratorStateOps.ARemember)
            );
            
            DefineLibraryMethod(module, "seen?", 0x11, 
                0x00000000U, 
                new Func<IronRuby.Runtime.RubyContext, IronRuby.JsonExt.GeneratorState, System.Object, System.Object>(IronRuby.JsonExt.JSON.Ext.Generator.GeneratorStateOps.ASeen)
            );
            
            DefineLibraryMethod(module, "space", 0x11, 
                0x00000000U, 
                new Func<IronRuby.Runtime.RubyContext, IronRuby.JsonExt.GeneratorState, IronRuby.Builtins.MutableString>(IronRuby.JsonExt.JSON.Ext.Generator.GeneratorStateOps.GetSpace)
            );
            
            DefineLibraryMethod(module, "space_before", 0x11, 
                0x00000000U, 
                new Func<IronRuby.Runtime.RubyContext, IronRuby.JsonExt.GeneratorState, IronRuby.Builtins.MutableString>(IronRuby.JsonExt.JSON.Ext.Generator.GeneratorStateOps.GetSpaceBefore)
            );
            
            DefineLibraryMethod(module, "space_before=", 0x11, 
                0x00000000U, 
                new Action<IronRuby.Runtime.RubyContext, IronRuby.JsonExt.GeneratorState, IronRuby.Builtins.MutableString>(IronRuby.JsonExt.JSON.Ext.Generator.GeneratorStateOps.SetSpaceBefore)
            );
            
            DefineLibraryMethod(module, "space=", 0x11, 
                0x00000000U, 
                new Action<IronRuby.Runtime.RubyContext, IronRuby.JsonExt.GeneratorState, IronRuby.Builtins.MutableString>(IronRuby.JsonExt.JSON.Ext.Generator.GeneratorStateOps.SetSpace)
            );
            
            DefineLibraryMethod(module, "to_h", 0x11, 
                0x00000000U, 
                new Func<IronRuby.Runtime.RubyContext, IronRuby.JsonExt.GeneratorState, IronRuby.Builtins.Hash>(IronRuby.JsonExt.JSON.Ext.Generator.GeneratorStateOps.ToHash)
            );
            
        }
        
        private static void LoadJSON____Ext__Generator__State_Class(IronRuby.Builtins.RubyModule/*!*/ module) {
            DefineLibraryMethod(module, "from_state", 0x21, 
                0x00000000U, 
                new Func<IronRuby.Runtime.RubyContext, IronRuby.Builtins.RubyModule, System.Object, IronRuby.JsonExt.GeneratorState>(IronRuby.JsonExt.JSON.Ext.Generator.GeneratorStateOps.FromState)
            );
            
            DefineLibraryMethod(module, "new", 0x21, 
                0x00000000U, 
                new Func<IronRuby.Builtins.RubyClass, IronRuby.Builtins.Hash, IronRuby.JsonExt.GeneratorState>(IronRuby.JsonExt.JSON.Ext.Generator.GeneratorStateOps.CreateGeneratorState)
            );
            
        }
        
        private static void LoadJSON____Ext__Parser_Instance(IronRuby.Builtins.RubyModule/*!*/ module) {
            DefineLibraryMethod(module, "parse", 0x11, 
                0x00000000U, 
                new Func<IronRuby.Runtime.RespondToStorage, IronRuby.Runtime.UnaryOpStorage, IronRuby.Runtime.BinaryOpStorage, IronRuby.Runtime.RubyScope, IronRuby.JsonExt.Parser, System.Object>(IronRuby.JsonExt.JSON.Ext.ParserOps.Parse)
            );
            
        }
        
        private static void LoadMicrosoft__Scripting__Runtime__DynamicNull_Instance(IronRuby.Builtins.RubyModule/*!*/ module) {
            DefineLibraryMethod(module, "to_json", 0x11, 
                0x00000000U, 
                new Func<Microsoft.Scripting.Runtime.DynamicNull, IronRuby.JsonExt.GeneratorState, System.Int32, IronRuby.Builtins.MutableString>(IronRuby.JsonExt.DynamicNullOps.ToJson)
            );
            
        }
        
        private static void LoadSystem__Collections__IDictionary_Instance(IronRuby.Builtins.RubyModule/*!*/ module) {
            DefineLibraryMethod(module, "to_json", 0x11, 
                0x00000000U, 
                new Func<IronRuby.Runtime.ConversionStorage<IronRuby.Builtins.MutableString>, IronRuby.Runtime.RubyScope, System.Collections.IDictionary, IronRuby.JsonExt.GeneratorState, System.Int32, IronRuby.Builtins.MutableString>(IronRuby.JsonExt.DictionaryOps.ToJson)
            );
            
        }
        
        private static void LoadSystem__Collections__IList_Instance(IronRuby.Builtins.RubyModule/*!*/ module) {
            DefineLibraryMethod(module, "to_json", 0x11, 
                0x00000000U, 
                new Func<IronRuby.Runtime.RubyScope, System.Collections.IList, IronRuby.JsonExt.GeneratorState, System.Int32, IronRuby.Builtins.MutableString>(IronRuby.JsonExt.IListOps.ToJson)
            );
            
        }
        
        private static void LoadSystem__Double_Instance(IronRuby.Builtins.RubyModule/*!*/ module) {
            DefineLibraryMethod(module, "to_json", 0x11, 
                0x00000000U, 
                new Func<System.Double, IronRuby.JsonExt.GeneratorState, System.Int32, IronRuby.Builtins.MutableString>(IronRuby.JsonExt.FloatOps.ToJson)
            );
            
        }
        
        private static void LoadSystem__Int32_Instance(IronRuby.Builtins.RubyModule/*!*/ module) {
            DefineLibraryMethod(module, "to_json", 0x11, 
                0x00000000U, 
                new Func<System.Int32, IronRuby.JsonExt.GeneratorState, System.Int32, IronRuby.Builtins.MutableString>(IronRuby.JsonExt.FixnumOps.ToJson)
            );
            
        }
        
        private static void LoadSystem__Object_Instance(IronRuby.Builtins.RubyModule/*!*/ module) {
            DefineLibraryMethod(module, "to_json", 0x11, 
                0x00000000U, 
                new Func<IronRuby.Runtime.ConversionStorage<IronRuby.Builtins.MutableString>, System.Object, IronRuby.JsonExt.GeneratorState, System.Int32, IronRuby.Builtins.MutableString>(IronRuby.JsonExt.ObjectOps.ToJson)
            );
            
        }
        
        private static void LoadSystem__String_Instance(IronRuby.Builtins.RubyModule/*!*/ module) {
            DefineLibraryMethod(module, "to_json", 0x11, 
                0x00000000U, 
                new Func<System.String, IronRuby.JsonExt.GeneratorState, System.Int32, IronRuby.Builtins.MutableString>(IronRuby.JsonExt.ClrStringOps.ToJson)
            );
            
        }
        
    }
}

