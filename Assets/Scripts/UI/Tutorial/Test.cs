using Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Tutorial {
    public class Test: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField] private InfoShower _shower;
        protected Info Info() => DataManager.Symbol.GetData(1003).GetInfo();
        
        private bool _onMouse = false;
        public void OnPointerEnter(PointerEventData eventData) {
            var info = Info();
            if (info == null)
                return;
                    
            _onMouse = true;
            _shower.SetInfo(info);
        }
        
        public void OnPointerExit(PointerEventData eventData) {
            if (!_onMouse)
                return;
                    
            _onMouse = false;
            _shower.Hide();
        }
        
        protected void Update() {
            if (_onMouse && !_shower.IsActive) {
                _shower.SetInfo(Info());
            }
        }

    }
}