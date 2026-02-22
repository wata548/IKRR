using System;
using ColorMine.ColorSpaces;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

namespace Extension {
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public abstract class RotatingSwitch: MonoBehaviour {

        [SerializeField] protected float _animationDuration = 0.5f;
        [SerializeField] protected Color _beforeColor;
        [SerializeField] protected Color _afterColor;
        public event Action<bool> OnClick = null;
        public event Func<bool> OnAppear = null; 
        protected Image _board;
        protected Button _button;
        protected Tween _animation = null;
        protected bool _isActive = false;

        protected abstract void Setting();
        
        protected void OnEnable() {
            if (OnAppear == null)
                return;
            _isActive = OnAppear();
            Set(_isActive);
        }

        protected void Start() {
            _button = GetComponent<Button>();
            _board = GetComponent<Image>();
            Setting();
            OnEnable();
            OnClick += Animation;
            _button.onClick.AddListener(() => OnClick?.Invoke(_isActive = !_isActive));
        }

        private void Set(bool pActive) {
            _animation?.Kill();
            _board.transform.rotation = Quaternion.Euler(0,0,pActive ? 180 : 0);
            _board.material.SetColor("_AfterColor", pActive ? _afterColor : _beforeColor);
        }
        
        private void Animation(bool pActive) {
            var (start, end) = pActive ? (_beforeColor, _afterColor) : (_afterColor, _beforeColor);
            Set(!pActive);
            var startLch = start.ColorToLch();
            var endLch = end.ColorToLch();
            var process = 0f;
            
            _animation = DOTween.Sequence()
                .Append(_board.transform.DORotate(Vector3.forward * (pActive ? 180f : 0f), _animationDuration))
                .Join(
                    DOTween.To(
                        () => process,
                        t => {
                            process = t;
                            _board.material.SetColor("_AfterColor", ExColor.Lerp(startLch, endLch, process).ToColor());
                        },
                        1,
                        _animationDuration
                    )
                );
        }
    }
}