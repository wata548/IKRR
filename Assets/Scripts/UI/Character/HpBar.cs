using Data;
using DG.Tweening;
using Extension.Test;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Character {
    public class HpBar: SlideBar {
        
        [SerializeField] private TMP_Text _current;
        [SerializeField] private TMP_Text _max;
        
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
            PlayEffect(pAmount, Color.green);
            return SetWithAnimation(pMax, pCurrent);
        }
        public Tween Damage(int pMax, int pCurrent, int pAmount) {
            PlayEffect(pAmount, Color.red);
            return SetWithAnimation(pMax, pCurrent);
        }

        private void PlayEffect(int pAmount, Color pColor) {
            var effect = DamageEffectPool.Instance.GetEffect();
            var size = new Vector2(_fill.rectTransform.rect.width, _fill.rectTransform.rect.height);
            size.Scale(_fill.transform.lossyScale);
            var randomX = Random.Range(0, size.x) - size.x/2f;
            var randomY = Random.Range(0, size.y) - size.y/2f;
            
            
            effect.transform.position = transform.position + new Vector3(randomX, randomY) * 0.5f;
            effect.Play(pAmount, pColor);
        }

        public override void Set(int pMax, int pCurrent) {
            base.Set(pMax, pCurrent);
            _max.text = pMax.ToString();
            _current.text = pCurrent.ToString();
        } 
    }
}