using System;
using Extension.Test;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

namespace UI {
    public class OXPannel: MonoBehaviour {
        [SerializeField] private Image _pannel;
        [SerializeField] private VisualEffect _effect;
        public bool IsActive => _effect.aliveParticleCount != 0;
        
        [TestMethod]
        public void Show(bool pOX) {
            var color = _pannel.color;
            _pannel.gameObject.SetActive(true);
            _effect.SetBool("Condition", pOX);
            _effect.Reinit();
            _effect.Play();
        }

        private void Update() {
            if (IsActive) {
                return;
            }
            if (!_pannel.gameObject.activeSelf)
                return;
            _pannel.gameObject.SetActive(false);
        }
    }
}