using Data;
using TMPro;
using UI.Icon;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ContainSymbol {
    public class SymbolAmountShower: ShowInfo {
        [SerializeField] protected Image _shower;
        [SerializeField] private TMP_Text _amountShower;
        private bool _isDataSet;
        protected int _data;

        public void Clear() {
            _isDataSet = false;
            _shower.sprite = null;
            _amountShower.text = "";
        }

        public virtual void Set(int pTarget, int pAmount) {
            _isDataSet = true;
            _data = pTarget;
            _shower.sprite = pTarget.GetIcon();
            _amountShower.text = $"x {pAmount}";
        }

        protected override Info Info() =>
            _isDataSet ? DataManager.Symbol.GetData(_data).GetInfo() : null;
    }
}