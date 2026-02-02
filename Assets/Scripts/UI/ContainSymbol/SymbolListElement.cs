using Data;
using TMPro;
using UI.Icon;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.ContainSymbol {
    public class SymbolListElement: ShowInfo {
        [SerializeField] private Image _shower;
        [SerializeField] private TMP_Text _amountShower;
        public Vector2 Size => _shower.rectTransform.sizeDelta; 
        private bool _isDataSet;
        private int _data;

        public void Clear() {
            _isDataSet = false;
            _shower.sprite = null;
            _amountShower.text = "";
        }

        public void Set(int pTarget, int pAmount) {
            _isDataSet = true;
            _data = pTarget;
            _shower.sprite = pTarget.GetIcon();
            _amountShower.text = $"x {pAmount}";
        }

        protected override Info Info() =>
            _isDataSet ? DataManager.Symbol.GetData(_data).GetInfo() : null;
    }
}