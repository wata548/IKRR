using System;
using System.Collections.Generic;
using System.Linq;
using Lang;
using TMPro;
using UnityEngine;

namespace Font {

    [Serializable]
    public struct LanguageFontMatch {
        public Language Language;
        public TMP_FontAsset Font;
    }
    
    [CreateAssetMenu(menuName = "Font/Setting")]
    public class FontSetting: ScriptableObject {
        [SerializeField] 
        private List<LanguageFontMatch> _fonts;

        private Dictionary<Language, TMP_FontAsset> _temp;

        public TMP_FontAsset Get(Language pLang) {
            _temp ??= _fonts.ToDictionary(p => p.Language, p => p.Font);
            return _temp[pLang];
        }
        
        public void Init() {
            foreach (var match in _fonts) {
                TMP_LangText.SetFont(match.Language, match.Font);
            }
        }
    }
}