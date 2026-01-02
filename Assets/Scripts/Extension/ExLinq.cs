using System.Collections.Generic;
using System.Linq;

namespace Extension {
    public static class ExLinq {
        public static Dictionary<TK, TV> ToDictionary<TK, TV>(this IEnumerable<KeyValuePair<TK, TV>> pList) =>
            pList.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
}