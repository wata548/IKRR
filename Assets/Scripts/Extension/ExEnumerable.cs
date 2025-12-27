using System;
using System.Collections.Generic;

namespace Extension {
    public static class ExEnumerable {
        public static List<T> Shuffle<T>(this List<T> pContent) {

            var random = new Random();
            for (int i = 0; i < pContent.Count - 1; i++) {
                //i ~ pContent.Count - 1
                var targetIdx = i + random.Next() % (pContent.Count - i);
                (pContent[i], pContent[targetIdx]) = (pContent[targetIdx], pContent[i]);
            }

            return pContent;
        }
    }
}