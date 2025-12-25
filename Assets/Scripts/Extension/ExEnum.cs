using System;
using System.Collections.Generic;
using System.Linq;

namespace Extension {
    public static class ExEnum {

        public static bool IsFlag<T>(this T pValue) where T: Enum {
            var value = Convert.ToInt32(pValue);
            return value != 0 && ((value - 1) & value) == 0;
        }
        public static IEnumerable<T> Split<T>(this T pValue) where T: Enum =>
            ((T[])Enum.GetValues(typeof(T)))
                .Where(flag => flag.IsFlag() && pValue.HasFlag(flag));
    }
}