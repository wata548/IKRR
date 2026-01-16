using Lang;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Test {
    public class LanguageChanger: MonoBehaviour {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _language;
        private Language _lang;

        public void Set(Language pLang) {
            _language.text = pLang.ToString();
            _lang = pLang;
        }
        private void Awake() {
            _button.onClick.AddListener(() => LanguageManager.LangPack = _lang);
        }
    }
}