using System;
using Extension;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BackGround {
    public class Cloud: MonoBehaviour {
        [SerializeField] private float _speed;
        [SerializeField] private Image _cloudPrefab;
        private Image _cloud1;
        private Image _cloud2;
        private RectTransform _rect;

        private void Move(Image pTarget) {
            var pos = pTarget.transform.localPosition;
            pos.x -= _speed * Time.deltaTime / Time.timeScale;
            pTarget.transform.localPosition = pos;
        }
        
        private void Awake() {
            _rect = transform as RectTransform;
            _cloud1 = Instantiate(_cloudPrefab, transform);
            _cloud2 = Instantiate(_cloudPrefab, transform);
            _cloud2.rectTransform.SetLocalPositionX(_rect, PivotLocation.Up, 0.5f);
        }

        private void Update() {
            Move(_cloud1);
            Move(_cloud2);
            if (_cloud1.transform.localPosition.x < -_rect.sizeDelta.x) {
                var pos = _cloud2.transform.localPosition;
                pos.x += _rect.sizeDelta.x;
                _cloud1.transform.localPosition = pos;
                (_cloud1, _cloud2) = (_cloud2, _cloud1);
            }
        }
     
        
    }
}