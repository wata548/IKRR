using System.Collections.Generic;
using Data;
using DG.Tweening;
using FSM;
using Extension;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class TurnShower: MonoBehaviour {

        [SerializeField] private Image _banner;
        [SerializeField] private TMP_Text _showerText;

        private static readonly IReadOnlyDictionary<State, string> _matchTurn = new Dictionary<State, string> {
            { State.EnemyTurn, "Enemy's Turn" },
            { State.Rolling, "Player's Turn" },
        };
        
        public Tween StartAnimation(State state) {

            if (!CharactersManager.IsFighting)
                return null;
            
            if (!_matchTurn.TryGetValue(state, out var value))
                return null;

            var timeScale = Time.timeScale;
            _showerText.text = value;
            var startPos = _showerText.rectTransform.GetLocalPositionX(PivotLocation.Down, 1f);
            var secondPos = _showerText.rectTransform.GetLocalPositionX(PivotLocation.Down, 0.5f);
            var endPos = _showerText.rectTransform.GetLocalPositionX(PivotLocation.Down, 0f);

            return DOTween.Sequence()
                .AppendCallback(() => _showerText.transform.localPosition = startPos)
                .Append(_banner.DOFade(1, 0.3f * timeScale))
                .Append(_showerText.rectTransform.DOLocalMove(secondPos, 0.5f * timeScale)
                    .SetEase(Ease.OutBack)
                )
                .AppendInterval(0.3f)
                .Append(_showerText.rectTransform.DOLocalMove(endPos, 0.3f * timeScale))
                .Append(_banner.DOFade(0, 0.4f * timeScale));
        }
    }
}