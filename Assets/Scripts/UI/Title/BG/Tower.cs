using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Title.BG {
    public class Tower: MonoBehaviour {
        [SerializeField] private Image _tower1;
        [SerializeField] private Image _tower2;
        [SerializeField] private float _speed = 50;

        private void Move(Image pImage) {
            const float SCREEN_HEIGHT = 1080f;
            pImage.transform.localPosition -= Vector3.up * (_speed * Time.deltaTime / Time.timeScale);
            
            var size = pImage.rectTransform.sizeDelta.y;
            if (pImage.transform.localPosition.y <= -size - SCREEN_HEIGHT / 2f) {
                pImage.transform.localPosition += Vector3.up * size * 2;
            }
        }
        
        private void Update() {
            Move(_tower1);
            Move(_tower2);
            
        }
    }
}