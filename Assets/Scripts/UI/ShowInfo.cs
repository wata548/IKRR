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
            UIManager.Instance.InfoShower.Set(info);
        }

        public void OnPointerExit(PointerEventData eventData) {
            if (!_onMouse)
                return;
            
            _onMouse = false;
            UIManager.Instance.InfoShower.Hide();
        }

        protected void Update() {
            if (_onMouse && !UIManager.Instance.InfoShower.IsActive) {
                UIManager.Instance.InfoShower.Set(Info());
            }
        }
    }
}