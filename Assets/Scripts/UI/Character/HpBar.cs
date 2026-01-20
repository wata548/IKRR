using DG.Tweening;
using Extension.Test;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Character {
    public class HpBar: SlideBar {
        
        [SerializeField] private TMP_Text _current;
        [SerializeField] private TMP_Text _max;
        [SerializeField] private TMP_Text _damageShower;
        
        //==================================================||Methods 
        [TestMethod(pRuntimeOnly:true)]
        public override Tween SetWithAnimation(int pMax, int pCurrent) {
            
            _animation?.Kill();
            _animation = DOTween.Sequence()
                .Append(base.SetWithAnimation(pMax, pCurrent))
                .Join(_current.DOCounter(
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

        public override void Set(int pMax, int pCurrent) {
            _max.text = pMax.ToString();
            _current.text = pCurrent.ToString();
        } 
    }
}