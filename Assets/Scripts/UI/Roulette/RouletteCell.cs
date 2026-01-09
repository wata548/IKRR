using System;
using DG.Tweening;
using Extension;
using Roulette;
using UI.Symbol;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace UI.Roulette {
    public class RouletteCell: MonoBehaviour {


        private const float ANIMATION_SCALE = 1.2f; 
        private const float ANIMATION_DURATION = 0.5f; 
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
        
        public void SetIdx(int pColumn, int pRow) {
            Column = pColumn;
            Row = pRow;
        }
        
        public void Use(CellStatus pNextStatus) {
            _icon.transform.DOScale(ANIMATION_SCALE, ANIMATION_DURATION)
                .SetLoops(2, LoopType.Yoyo)
                .OnComplete(() => {
                    SetStatus(pNextStatus);
                });   
        }
        
        public void SetStatus(CellStatus pStatus) {
            _icon.material = MaterialStore.Get( pStatus switch {
                CellStatus.Usable =>  USABLE,
                CellStatus.Unavailable => UNAVAILABLE,
                CellStatus.Used => USED,
            });
        }

        public void SetIcon(int pCode) => 
            _icon.sprite = pCode.GetIcon();

        private void Awake() {
            RectTransform = GetComponent<RectTransform>();
        }
    }
}