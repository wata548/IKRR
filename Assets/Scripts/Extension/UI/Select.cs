using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Extension {
    public class Select: MonoBehaviour {

        private static readonly Dictionary<string, (int Frame, int Idx)> _curSelection = new();
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;
        private string _category;
        private int _idx;
        private int _lastUpdate = -1;

        public static int GetSelection(string pCategory) => _curSelection[pCategory].Idx;
        
        public void Set(string pCategory, int pIdx) {
            _category = pCategory;
            _idx = pIdx;
            _curSelection.TryAdd(_category, (Time.frameCount, 0));
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => _curSelection[_category] = (Time.frameCount, _idx));
        }
        
        public void Set(Sprite pSprite, string pCategory, int pIdx) {

            _image.sprite = pSprite;
            Set(pCategory, pIdx);
        }

        private void Update() {

            if (_lastUpdate == _curSelection[_category].Frame)
                return;
            
            _lastUpdate = _curSelection[_category].Frame;
            _button.image.color = _idx != _curSelection[_category].Idx
                ? Color.gray
                : Color.white;
        }
    }
}