using Data;
using UI.Icon;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    [RequireComponent(typeof(Image))]  
    public class SymbolShower: ShowInfo {

        private Image _shower;
        private int _code;
        
        public void Set(int pCode) {
            _shower ??= GetComponent<Image>();
            _code = pCode;
            _shower.sprite = _code.GetIcon();
        }

        protected override Info Info() =>
            DataManager.Symbol.GetData(_code).GetInfo();
    }
}