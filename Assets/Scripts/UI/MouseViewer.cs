using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI {
    public class MouseViewer: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        
        //==================================================||Fields 
        [SerializeField] private bool _applyAnimation = true;
        [SerializeField] private Image _targetShower;
        
        [Space, Header("Values")]
        [SerializeField] private float _rotationMaximumH = 30f; 
        [SerializeField] private float _rotationMaximumV = 30f; 
                    
        private RectTransform _rect;
        private Vector3 _initialScale;
        private bool _isMouseOn = false;
        private Tween _animation;
        private Vector3 _originScale;

        //==================================================||Unity 
        private void Start() {
            _initialScale = transform.localScale;
            _rect = (transform as RectTransform)!;
        }
        
        private void Update() {
            
            if (!_isMouseOn) {
                return;
            }
                        
            var mouse = Input.mousePosition;
            mouse.z = transform.position.z + Math.Abs(Camera.main!.transform.position.z);
            mouse = Camera.main!.ScreenToWorldPoint(mouse);
                    
            var localPos = (mouse - transform.position) / (_rect.sizeDelta * _rect.lossyScale * 0.5f);
            var rotation = Quaternion.Euler(_rotationMaximumV * localPos.y, -_rotationMaximumH * localPos.x, 0);
            _targetShower.transform.rotation = rotation;
        }
        
        public void OnPointerEnter(PointerEventData eventData) {
            _isMouseOn = true;
            var targetScale = _initialScale * 1.25f;
            
            if(_applyAnimation)
                _animation = _targetShower.transform.DOScale(targetScale, 0.2f);
        }
        
        public void OnPointerExit(PointerEventData eventData) {
            _isMouseOn = false;
            _targetShower.transform.rotation = Quaternion.identity;

            if (_applyAnimation) {
                _animation?.Kill();
                _targetShower.transform.localScale = _initialScale;
            }
        }

        public void OnDisable() {
            _isMouseOn = false;
            _targetShower.transform.rotation = Quaternion.identity;
            _animation?.Kill();
            _targetShower.transform.localScale = _initialScale;
        }
    }
}