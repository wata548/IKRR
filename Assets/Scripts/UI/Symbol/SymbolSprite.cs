using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UI.Symbol {
    public static class SymbolSprite {

        private static IReadOnlyDictionary<int, Sprite> _symbolIcon;
        
        [RuntimeInitializeOnLoadMethod]
        private static void Init() {
            const string PATTERN = @"^\d+";
            //_symbolIcon = Resources.LoadAll<Sprite>("Symbols")
                //.ToDictionary(sprite => int.Parse(Regex.Match(sprite.name, PATTERN).Value), sprite => sprite);
        }

        public static Sprite GetIcon(this int pCode) {
            if (_symbolIcon.TryGetValue(pCode, out var result))
                return result;
            return _symbolIcon[-1];
        } 
    }
}