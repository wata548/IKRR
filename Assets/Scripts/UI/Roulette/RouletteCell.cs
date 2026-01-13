using System;
using System.Collections;
using DG.Tweening;
using Extension;
using Roulette;
using UI.Icon;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace UI.Roulette {
    public class RouletteCell: MonoBehaviour {
        
        public const string CHANGE = "ChangeSymbol";
        public const string USABLE = "NULL";
        public const string USED = "GrayScale";
        public const string UNAVAILABLE = "Unavailable";
        
        [SerializeField] private Image _icon;
        [SerializeField] private Button _useButton;
        public RectTransform RectTransform { get; private set; }
        public int Row { get; private set; }
        public int Column { get; private set; }

        public void AddOnClickListener(Action pAction) {
            _useButton.onClick.AddListener(() => pAction?.Invoke());
        }

        public void Evolve(int pNewCode, Action pOnComplete, bool pPlayAnimation) {

            const float ANIMATION_DURATION = 0.5f;
            
            var changeMat = MaterialStore.Get(CHANGE);
            _icon.material = changeMat;
            var sprite = pNewCode.ToIcon();
            _icon.material.SetTexture("_After", sprite.texture);
            
            if(pPlayAnimation) PlayAnimation();
            StartCoroutine(ChangeAnimation());

            IEnumerator ChangeAnimation() {
                var time = 0f; 
                while (time < ANIMATION_DURATION) {
                    time += Time.deltaTime;
                    changeMat.SetFloat("_CurTime", time / ANIMATION_DURATION);
                    yield return null;
                }

                _icon.sprite = sprite;
                _icon.material = null;
                SetStatus(RouletteManager.GetStatus(Column, Row));
                pOnComplete?.Invoke();
            }
        }
        
        public void SetIdx(int pColumn, int pRow) {
            Column = pColumn;
            Row = pRow;
        }

        private Tween PlayAnimation() {
            const float ANIMATION_SCALE = 1.2f; 
            const float ANIMATION_DURATION = 0.45f;
            return _icon.transform.DOScale(ANIMATION_SCALE, ANIMATION_DURATION)
                .SetLoops(2, LoopType.Yoyo);
        }
        
        public void PlayAnimation(CellStatus pStatus) {
            PlayAnimation().OnComplete(() => SetStatus(pStatus));
        }
        
        public void SetStatus(CellStatus pStatus) {
            if (_icon.material.name == $"M_{CHANGE}")
                return;
            
            _icon.material = MaterialStore.Get( pStatus switch {
                CellStatus.Usable =>  USABLE,
                CellStatus.Unavailable => UNAVAILABLE,
                CellStatus.Used => USED,
                _ => throw new ArgumentException()
            });
        }

        public void SetIcon(int pCode) => 
            _icon.sprite = pCode.ToIcon();

        private void Awake() {
            RectTransform = GetComponent<RectTransform>();
        }
    }
}