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
        
            
        private List<LanguageChanger> _container = new();

        private void Switch() {
            Active(!_buttonPannel.gameObject.activeSelf);
        }

        public void Active(bool pValue) {
            _buttonPannel.gameObject.SetActive(pValue);
            _languageShower.text = LanguageManager.LangPack.ToString();
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
        }
    }
}