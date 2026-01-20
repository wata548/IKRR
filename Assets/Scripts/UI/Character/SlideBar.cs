using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Character {
    public class SlideBar: MonoBehaviour {
        
        [SerializeField] private Image _fill;
        protected Tween _animation;
        protected const float ANIMATION_SPEED = 0.3f;
        
        public virtual void Set(int pMax, int pCurrent) {
            _animation?.Kill();
            _fill.fillAmount = (float)pCurrent / pMax;
        }

        public virtual Tween SetWithAnimation(int pMax, int pCurrent) {
                    
            _animation?.Kill();
            var result = (float)pCurrent / pMax;
            _animation = DOTween.Sequence()
                .Append(DOTween.To(
                        () => _fill.fillAmount,
                        x => _fill.fillAmount = x,
                        result,
                        ANIMATION_SPEED)
                    .SetEase(Ease.OutCubic)
                );
                    
            return _animation;
        }
    }
}