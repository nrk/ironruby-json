using System;
using IronRuby.Builtins;
using IronRuby.Runtime;

namespace IronRuby.Libraries.Json.Builtins {
    public static partial class MutableStringOps {
        [RubyMethod("json_create", RubyMethodAttributes.PublicSingleton)]
        public static MutableString JsonCreate(ConversionStorage<IntegerValue>/*!*/ integerConversion, 
            RubyClass/*!*/ self, Hash/*!*/ creatable) {

            RubyArray raw = (RubyArray)creatable[MutableString.CreateAscii("raw")];
            MutableStringStream stream = new MutableStringStream();
            for (int i = 0; i < raw.Count; i++) {
                stream.WriteByte(unchecked((byte)Protocols.CastToUInt32Unchecked(integerConversion, raw[i])));
            }
            return stream.String;
        }
    }
}
