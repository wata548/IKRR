using System;
using DG.Tweening;
using Extension;
using Extension.Test;
using Lang;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.SkillShower {
    public class SkillShower: MonoBehaviour {

       //==================================================||Fields 
        [SerializeField] private TMP_LangText _shower;
        [SerializeField] private Image _border;

        [SerializeField] private float _appearPositionRatio = 0.15f;
        [SerializeField] private float _stopPositionRatio = 0.35f;
        private Tween _animation = null;

       //==================================================||Methods 
        public void Show(string pContext, Action pOnComplete) {
            _shower.text = pContext;
            AppearAnimation(pOnComplete);
        }

        [TestMethod(pAllowFoldOut: false, pRuntimeOnly: true)]
        private Tween AppearAnimation(Action pOnComplete) {
            const float AlphaAfter = 0.85f;
            const float IdleDuration = 1f;
            const float MoveTime = 0.5f;
            const float FadeInTime = 0.4f;
            const float FadeOutTime = 0.4f;
            
            _animation?.Kill();
            _border.rectTransform.SetLocalScaleX(1f);

            var height = (_border.transform.parent as RectTransform)!.sizeDelta.y;
            _border.rectTransform.localPosition = new(0, height * _appearPositionRatio);
            var position = height * _stopPositionRatio;

            _animation = DOTween.Sequence()
                .Append(_border.rectTransform
                    .DOLocalMoveY(position, MoveTime)
                    .SetEase(Ease.OutBack)
                )
                .Join(_border
                    .DOFade(AlphaAfter, FadeInTime)
                    .OnComplete(() => pOnComplete?.Invoke())
                )
                .Join(_shower.Tmp
                    .DOFade(AlphaAfter, FadeInTime)
                )
                .AppendInterval(IdleDuration)
                .Append(HideAnimation(FadeOutTime));

            return _animation;
        }

        private Tween HideAnimation(float pFadeDuration) =>
            DOTween.Sequence()
                .Append(_border.DOFade(0, pFadeDuration))
                .Join(_shower.Tmp.DOFade(0, pFadeDuration));

       //==================================================||Unity 
        private void Start() {
            _animation = HideAnimation(0f);
        }
    }
}