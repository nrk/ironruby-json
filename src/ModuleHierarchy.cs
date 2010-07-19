using System;
using IronRuby.Runtime;

namespace IronRuby.JsonExt {
    /* TODO: we can not extend the JSON module defined in the ruby code due to 
     *       IronRuby bug #23827. This hack is horrible, but it is the fastest 
     *       way to to circumvent that bug. See http://is.gd/li5d for reference.
     */

    [RubyModule("JSON__")]
    public static partial class JSON {
        [RubyModule]
        public static partial class Ext { }
    }
}
