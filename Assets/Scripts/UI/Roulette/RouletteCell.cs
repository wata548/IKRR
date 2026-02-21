using System;
using System.Collections;
using Data;
using DG.Tweening;
using Roulette;
using UI.Icon;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace UI.Roulette {
    public class RouletteCell: ShowInfo {
        
        public const string CHANGE = "ChangeSymbol";
        public const string USABLE = "NULL";
        public const string USED = "GrayScale";
        public const string UNAVAILABLE = "Unavailable";
        
        [SerializeField] private Image _icon;
        [SerializeField] private Button _useButton;
        public RectTransform RectTransform { get; private set; }
        public int Row { get; private set; }
        public int Column { get; private set; }
        public bool IsRoll { get; set; } = false;
        private int _code = 0;

        public void AddOnClickListener(Action pAction) {
            _useButton.onClick.AddListener(() => pAction?.Invoke());
        }

        public void Evolve(int pNewCode, Action pOnComplete, bool pPlayAnimation) {

            const float ANIMATION_DURATION = 0.5f;
            
            var changeMat = MaterialStore.Get(CHANGE);
            _icon.material = changeMat;
            _code = pNewCode;
            var sprite = pNewCode.GetIcon();
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
                if(pNewCode != DataManager.EMPTY_SYMBOL)
                    SetStatus(RouletteManager.GetStatus(Column, Row), true);
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
                .SetLoops(2, LoopType.Yoyo)
                .OnComplete(() => _icon.transform.localScale = Vector3.one);
        }
        
        public void PlayAnimation(CellStatus pStatus) {
            PlayAnimation().OnComplete(() => SetStatus(pStatus));
        }

        public void Empty() {
            _icon.material = null;
        }
        
        public void SetStatus(CellStatus pStatus, bool pForce = false) {
            if (!pForce && _icon.material.name == $"M_{CHANGE}")
                return;
            
            _icon.material = MaterialStore.Get( pStatus switch {
                CellStatus.Usable =>  USABLE,
                CellStatus.Unavailable => UNAVAILABLE,
                CellStatus.Used => USED,
                CellStatus.ForceUnavailable => UNAVAILABLE,
                _ => throw new ArgumentException()
            });
        }

        public void SetIcon(int pCode) {
            _code = pCode;
            _icon.sprite = pCode.GetIcon();
        }

        private void Awake() {
            RectTransform = GetComponent<RectTransform>();
        }

        protected override Info Info() =>
            !IsRoll ? DataManager.Symbol.GetData(_code).GetInfo() : null;
    }
}