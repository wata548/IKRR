using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Extension {
    public static class ExTween {

        public static Tween DOBreathing(this Transform pTransform, float pCycle, float pEndValue, Ease pEase = Ease.OutSine) =>
            pTransform.DOScale(pTransform.localScale * pEndValue, pCycle)
                .SetEase(pEase)
                .SetLoops(-1, LoopType.Yoyo);
        public static Tween ButtonHighlight(this Button pButton) {
            const float DEGREE = 20;
            pButton.transform.rotation = Quaternion.Euler(0, 0, DEGREE);
            return pButton.transform.DORotate(new Vector3(0, 0, -DEGREE), 0.5f * Time.timeScale)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}