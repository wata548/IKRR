using Data;
using TMPro;
using UI;
using UI.Icon;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Extension.Effect {
    public class EffectShower: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

        [SerializeField] private Image _shower;
        [SerializeField] private TMP_Text _counter;
        private EffectBase _data;
        
        public void Set(EffectBase pData) {
            _shower.sprite = pData.Code.GetIcon();
            _counter.text = pData.ShowCount.ToString();
            _data = pData;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            UIManager.Instance.InfoShower.Set(_data.GetInfo());
        }

        public void OnPointerExit(PointerEventData eventData) {
            UIManager.Instance.InfoShower.Hide();
        }
    }
}