using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using DG.Tweening;
using Roulette;
using UI.Character;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Rest {
    public class Rest: MonoBehaviour {
        
        const float HEAL_RATIO = 0.2f;
       //==================================================||Fields 
        [SerializeField] private Button _rest;
        [SerializeField] private Button _knowledge;
        [SerializeField] private RectTransform _pannel;
        [SerializeField] private Image _fader;
        [SerializeField] private HpBar _bar;
        [SerializeField] private Image _estimate;
        private IEnumerable<int> _knowledgeCandidates;
        
       //==================================================||Methods 
        public void Show(bool pActive) {

            if (pActive) {
                var player = CharactersManager.Player;
                _bar.Set(player.MaxHp, player.Hp);
                var estimate = Mathf.Min(player.MaxHp * HEAL_RATIO + player.Hp, player.MaxHp);
                _bar.Set(player.MaxHp, player.Hp);
                _estimate.fillAmount = estimate / player.MaxHp;
                Fade(() => _pannel.gameObject.SetActive(true), null);
            }
            else
                _pannel.gameObject.SetActive(false);
        }

        private void EvolveCondition() {
            _knowledgeCandidates = RouletteManager.HandKind
                .Where(code => {
                    var data = DataManager.Symbol.GetData(code);
                    if (string.IsNullOrWhiteSpace(data.EvolveCondition))
                        return false;
                   return !UseInfo.Get(code); 
                });
            _knowledge.interactable = _knowledgeCandidates.Any();
        }

        private void Fade(Action pOnAppear, Action pOnComplete) {
            _fader.gameObject.SetActive(true);
            const float FADE_IN = 0.3f;
            const float FADE_INTERVAL = 0.7f;
            const float FADE_OUT = 0.6f;
            
            DOTween.Sequence()
                .Append(_fader.DOFade(1, FADE_IN * Time.timeScale))
                .AppendInterval(FADE_INTERVAL * Time.timeScale)
                .AppendCallback(() => pOnAppear?.Invoke())
                .Append(_fader.DOFade(0, FADE_OUT * Time.timeScale))
                .OnComplete(() => {
                    _fader.gameObject.SetActive(false);
                    pOnComplete?.Invoke();
                });
        }

        private void RestFunc() {
            
            var player = CharactersManager.Player;
            var amount = Mathf.FloorToInt(player.MaxHp * HEAL_RATIO);
            player.Heal(amount, null);
            Fade(() => Show(false),
                () => UIManager.Instance.Map.ClearStage(true)
            );
        }

       //==================================================||Unity 
        private void Awake() {
            _rest.onClick.AddListener(RestFunc);
        }
    }
}