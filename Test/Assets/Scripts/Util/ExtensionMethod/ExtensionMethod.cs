using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Test.Util.ExtensionMethod {
    public static class MyListExtensionMethod {
        public static TSource SecondLast<TSource>(this List<TSource> _list) {
            if(_list.Count < 2) {
                return default(TSource);
            }
            return _list[_list.Count - 2];
        }
    }
}