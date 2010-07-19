using System;
using IronRuby.Runtime;

namespace IronRuby.JsonExt {
    [RubyModule("JSON")]
    internal sealed class JsonModule { 
    }

    [RubyModule("Ext", DefineIn = typeof(JsonModule))]
    internal sealed class ExtModule { 
    }

    [RubyModule("Generator", DefineIn = typeof(ExtModule))]
    internal sealed class GeneratorModule { 
    }
}
