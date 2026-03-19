using Data;
using UI.Icon;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class SymbolShower: ShowInfo {

        [SerializeField] private Image _shower;
        private int _code;
        public int Code => _code;
        
        public void Set(int pCode) {
            _code = pCode;
            _shower.sprite = _code.GetIcon();
        }

        protected override Info Info() =>
            DataManager.Symbol.GetData(_code).GetInfo();
    }
}