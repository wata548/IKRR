using Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI {
    public abstract class ShowInfo: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        protected abstract Info Info();
        
        private bool _onMouse = false;
        public void OnPointerEnter(PointerEventData eventData) {
            var info = Info();
            if (info == null)
                return;
            
            _onMouse = true;
            InfoShower.Instance.SetInfo(info);
        }

        public void OnPointerExit(PointerEventData eventData) {
            if (!_onMouse)
                return;
            
            _onMouse = false;
            InfoShower.Instance.Hide();
        }

        public void OnDisable() {
            OnPointerExit(null);
        }
        
        protected void Update() {
            if (_onMouse && !InfoShower.Instance.IsActive) {
                InfoShower.Instance.SetInfo(Info());
            }
        }
    }
}