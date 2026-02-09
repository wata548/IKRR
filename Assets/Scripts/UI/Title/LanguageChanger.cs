using Lang;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Title {
    public class LanguageChanger: MonoBehaviour {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _language;
        private Language _lang;
        private LanaguageButtonManager _manager;

        public void Set(LanaguageButtonManager pManager, Language pLang) {
            _manager = pManager;
            _language.text = pLang.ToString();
            _lang = pLang;
        }
        
        private void Awake() {
            _button.onClick.AddListener(() => LanguageManager.LangPack = _lang);
            _button.onClick.AddListener(() => _manager.Active(false));
            
        }
    }
}