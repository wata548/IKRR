using System;
using Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Rest {
    public class Rest: MonoBehaviour {
        
       //==================================================||Fields 
        [SerializeField] private Button _rest;
        [SerializeField] private Button _knowledge;
        [SerializeField] private RectTransform _pannel;
        [SerializeField] private Image _fader;

       //==================================================||Methods 
        public void Show(bool pActive) {

            if (pActive) {
                Fade(() => _pannel.gameObject.SetActive(true));
            }
            else
                _pannel.gameObject.SetActive(false);
        }

        private void Fade(Action pOnComplete) {
            _fader.gameObject.SetActive(true);
            const float FADE_IN = 0.3f;
            const float FADE_INTERVAL = 0.7f;
            const float FADE_OUT = 0.6f;
            
            DOTween.Sequence()
                .Append(_fader.DOFade(1, FADE_IN * Time.timeScale))
                .AppendInterval(FADE_INTERVAL * Time.timeScale)
                .AppendCallback(() => pOnComplete?.Invoke())
                .Append(_fader.DOFade(0, FADE_OUT * Time.timeScale))
                .OnComplete(() => {
                    _fader.gameObject.SetActive(false);
                });
        }

        private void RestFunc() {
            const float HEAL_RATIO = 0.2f;
            
            var player = CharactersManager.Player;
            var amount = Mathf.FloorToInt(player.MaxHp * HEAL_RATIO);
            player.Heal(amount, null);
            Fade(() => {
                UIManager.Instance.Map.ClearStage(true);
                Show(false);
            });
        }

       //==================================================||Unity 
        private void Awake() {
            _rest.onClick.AddListener(RestFunc);
        }
    }
}