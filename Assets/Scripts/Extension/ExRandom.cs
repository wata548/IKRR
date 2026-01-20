using System;

namespace Extension {
    public static class ExRandom {
        public static float Range(this Random pRandom, float pMin, float pMax ) {
            var range = pMax - pMin;
            return ((float)pRandom.NextDouble() * range) + pMin;
        }
    }
}