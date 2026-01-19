using Data;
using TMPro;
using UI.Icon;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.ContainSymbol {
    public class SymbolListElement: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField] private Image _shower;
        [SerializeField] private TMP_Text _amountShower;
        public Vector2 Size => _shower.rectTransform.sizeDelta; 
        private bool _isDataSetted;
        private int _data;

        public void Clear() {
            _isDataSetted = false;
            _shower.sprite = null;
            _amountShower.text = "";
        }

        public void Set(int pTarget, int pAmount) {
            _isDataSetted = true;
            _data = pTarget;
            _shower.sprite = pTarget.GetIcon();
            _amountShower.text = $"x {pAmount}";
        }

        public void OnPointerEnter(PointerEventData eventData) {
            if (!_isDataSetted)
                return;
            
            var data = DataManager.Symbol.GetData(_data).GetInfo();
            UIManager.Instance.InfoShower.Set(data);
        }

        public void OnPointerExit(PointerEventData eventData) {
            UIManager.Instance.InfoShower.Hide();
        }
    }
}