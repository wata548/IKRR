using Data;
using TMPro;
using UI;
using UI.Icon;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Extension.Effect {
    
    public class EffectShower: ShowInfo  {

        [SerializeField] private Image _shower;
        [SerializeField] private TMP_Text _counter;
        private EffectBase _data;
        
        public void Set(EffectBase pData) {
            _shower.sprite = pData.Code.GetIcon();
            var cnt = pData.ShowCount;
            _counter.text = cnt < 0 ? "" : cnt.ToString();
            _data = pData;
        }

        protected override Info Info() =>
            _data.GetInfo();
    }
}