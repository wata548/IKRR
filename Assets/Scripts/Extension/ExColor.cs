using ColorMine.ColorSpaces;
using UnityEngine;

namespace Extension {
    public static class ExColor {
        public static Lab Lerp(Lab lhs, Lab rhs, float t) {
            var l = lhs.L * (1 - t) + t * rhs.L;
            var a = lhs.A * (1 - t) + t * rhs.A;
            var b = lhs.B * (1 - t) + t * rhs.B;
            return new Lab { L = l, A = a, B = b };
        }

        public static Color ToColor(this Lab pColor) {
            var rgb = pColor.To<Rgb>();
            return new Color((float)rgb.R / 255f, (float)rgb.G / 255f, (float)rgb.B / 255f);
        } 
                
        public static Lab ToLab (this Color pColor) =>
            new Rgb() { R = pColor.r * 255, G = pColor.g * 255, B = pColor.b * 255 }.To<Lab>();
    }
}