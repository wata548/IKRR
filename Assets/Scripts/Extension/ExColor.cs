using ColorMine.ColorSpaces;
using UnityEngine;

namespace Extension {
    public static class ExColor {
        public static Lch Lerp(Lch a, Lch b, float t) {
            var l = a.L * (1 - t) + t * b.L;
            var c = a.C * (1 - t) + t * b.C;
            var h = a.H * (1 - t) + t * b.H;
            return new Lch { L = l, C = c, H = h };
        }

        public static Color ToColor(this Lch pColor) {
            var rgb = pColor.To<Rgb>();
            return new Color((float)rgb.R / 255f, (float)rgb.G / 255f, (float)rgb.B / 255f);
        } 
                
        public static Lch ToLch (this Color pColor) =>
            new Rgb() { R = pColor.r * 255, G = pColor.g * 255, B = pColor.b * 255 }.To<Lch>();
    }
}