using DG.Tweening;
using Extension.Test;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Character {
    public class HpBar: MonoBehaviour {
        
        private const float ANIMATION_SPEED = 0.3f;
        [SerializeField] private Image _fill;
        [SerializeField] private TMP_Text _current;
        [SerializeField] private TMP_Text _max;
        [SerializeField] private TMP_Text _damageShower;
        private Tween _animation;
        
        //==================================================||Methods 
        [TestMethod(pRuntimeOnly:true)]
        public Tween SetWithAnimation(int pMax, int pCurrent) {
            
            _animation?.Kill();
            var result = (float)pCurrent / pMax;
            _animation = DOTween.Sequence()
                .Append(DOTween.To(
                        () => _fill.fillAmount,
                        x => _fill.fillAmount = x,
                        result,
                        ANIMATION_SPEED)
                    .SetEase(Ease.OutCubic)
                ).Join(_current.DOCounter(
                        int.Parse(_current.text),
                        pCurrent,
                        ANIMATION_SPEED)
                    .SetEase(Ease.OutSine)
                );
            
            _max.text = pMax.ToString();
            return _animation;
        }
        
        public Tween Heal(int pMax, int pCurrent, int pAmount) {
            return SetWithAnimation(pMax, pCurrent);
        }
        public Tween Damage(int pMax, int pCurrent, int pAmount) {
            return SetWithAnimation(pMax, pCurrent);
        }

        public void Set(int pMax, int pCurrent) {
            _animation?.Kill();
            _max.text = pMax.ToString();
            _current.text = pCurrent.ToString();
            _fill.fillAmount = (float)pCurrent / pMax;
        } 
    }
}