using System;
using System.Collections.Generic;
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

        public void Init() {
            foreach (var match in _fonts) {
                TMP_LangText.SetFont(match.Language, match.Font);
            }
        }
    }
}