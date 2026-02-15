using System;
using System.Collections.Generic;
using System.Linq;
using Extension;
using Lang;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Title {
    public class LanaguageButtonManager: MonoBehaviour {
        [SerializeField] private LanguageChanger _changer;
        [SerializeField] private RectTransform _buttonPannel;
        [SerializeField] private Vector2Int _tableSize;
        [SerializeField] private Vector2 _padding;
        [SerializeField] private Button _button; 
        [SerializeField] private TMP_Text _languageShower;
        private float _lastClick = 0;
        private const float OTHER_PLACE_CLICK_CHECK_TIME = 0.3f;
            
        private List<LanguageChanger> _container = new();

        private void Switch() {
            Active(!_buttonPannel.gameObject.activeSelf);
            _lastClick = 0;
        }

        public void Active(bool pValue) {
            _buttonPannel.gameObject.SetActive(pValue);
            _languageShower.text = LanguageManager.LangPack.ToString();
            _lastClick = 0;
        }
        
        private void Start() {
            _button.onClick.AddListener(Switch);
            
            var targetLanguages = LanguageManager.Table.AllowLanguages().ToArray();
            _buttonPannel.Place(_container, new(
                _padding,
                targetLanguages.Length,
                _tableSize,
                _changer,
                (button, idx) => button.Set(this, targetLanguages[idx++]
                ))
            );
            _languageShower.text = LanguageManager.LangPack.ToString();
        }

        private void Update() {
            if (!_buttonPannel.gameObject.activeSelf)
                return;

            if (_lastClick != 0 && _lastClick + OTHER_PLACE_CLICK_CHECK_TIME < Time.time) {
                _lastClick = 0;
                _buttonPannel.gameObject.SetActive(false);
            }
            
            if (Input.GetKeyDown(KeyCode.Mouse0))
                _lastClick = Time.time;
        }
    }
}