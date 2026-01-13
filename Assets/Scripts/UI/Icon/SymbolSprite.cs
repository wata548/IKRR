using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UI.Icon {
    public static class SymbolSprite {

        private static IReadOnlyDictionary<int, Sprite> _icons;
        private const string PATH = "IKRR_Icons/{0}";
        private static readonly string[] TARGET_PATHS = { "Enemy", "Symbol" };
        
        [RuntimeInitializeOnLoadMethod]
        private static void Init() {
            const string PATTERN = @"^(-?\d+)";
            _icons = TARGET_PATHS.SelectMany(path => Resources.LoadAll<Sprite>(string.Format(PATH, path)))
                .ToDictionary(
                    sprite => int.Parse(Regex.Match(sprite.name, PATTERN).Value), 
                    sprite => sprite
                );
        }

        public static Sprite ToIcon(this int pCode) {
            if (_icons.TryGetValue(pCode, out var result))
                return result;
            return _icons[-1];
        } 
    }
}