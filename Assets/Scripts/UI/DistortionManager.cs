using System;
using Extension.Test;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace UI {
    public class DistortionManager: MonoBehaviour {
        [SerializeField] private Renderer2DData _renderer;
        private bool _preventDistortion = false;
        private FullScreenPassRendererFeature _pass = null;

        public bool PreventDistortion {
            get => _preventDistortion;
            set {
                _preventDistortion = value;
                _pass.passMaterial.SetInt("_NoRotate", value ? 1 : 0);
            }
        }

        private void Init() {
            if (_pass != null) 
                return;
            
            _renderer.TryGetRendererFeature(out _pass);
            _pass.passMaterial.SetInt("_NoRotate", 0);
        }
 
        [TestMethod]
        public void SetDistortion(bool pActive) {
            _pass.SetActive(pActive);
        }

        private void Awake() {
            Init();
        }

        private void OnDestroy() {
            SetDistortion(false);
        }
    }
}