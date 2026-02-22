using System;
using Extension;
using Extension.Test;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace UI {
    public class DistortionManager: MonoSingleton<DistortionManager> {
        [SerializeField] private Renderer2DData _renderer;
        private bool _preventDistortion = false;
        private FullScreenPassRendererFeature _pass = null;
        protected override bool IsNarrowSingleton => false;

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


        protected override void Awake() {
            base.Awake();
            Init();
        }

        private void OnDestroy() {
            SetDistortion(false);
        }
    }
}