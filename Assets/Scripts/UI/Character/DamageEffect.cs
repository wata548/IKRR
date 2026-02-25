using DG.Tweening;
using Extension;
using Extension.Test;
using TMPro;
using UnityEngine;

namespace UI.Character {
    public class DamageEffect: MonoBehaviour {
        [SerializeField] private TMP_Text _shower;
        [SerializeField] private float _gravity;
        [SerializeField] private Vector3 _force;
        [SerializeField] private float _highLightScale = 1.5f;
        [SerializeField] private float _animationDuration = 0.5f;
        [SerializeField] private float _stayDuration = 0.5f;
        [SerializeField] private Ease _ease;
        public bool IsPlaying => _animation?.IsPlaying() ?? false;
        private Vector3 _remainForce = Vector3.zero;
        private Tween _animation;
        
        [TestMethod(pRuntimeOnly:true)]
        public void Play(float pValue, Color pHighlightColor) {
            _shower.text = pValue.ToString();
            var fontSize = _shower.fontSize;
            _shower.fontSize *= _highLightScale;
            _remainForce = _force;

            var progress = 0f;
            var color = _shower.color;
            var startLch = pHighlightColor.ToLab();
            var endLch = _shower.color.ToLab();
            _shower.color = pHighlightColor;
            
            _animation = DOTween.Sequence()
                .Append(_shower.DOFontSize(fontSize, _animationDuration * Time.timeScale))
                .Join(DOTween.To(
                    () => progress,
                    t => {
                        progress = t;
                        _shower.color = ExColor.Lerp(startLch, endLch, progress).ToColor();
                    },
                    1,
                    _animationDuration * Time.timeScale)
                ).Append(_shower.DOFade(0, _stayDuration * Time.timeScale))
                .SetEase(_ease);
        }
        
        private void Update() {
            if (!IsPlaying)
                return;
            _remainForce.y -= Time.deltaTime * _gravity;
            _shower.transform.localPosition += _remainForce * (Time.deltaTime / Time.timeScale);
        }  
    }
}