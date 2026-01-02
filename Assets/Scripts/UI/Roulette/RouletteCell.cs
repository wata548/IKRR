using System;
using Roulette;
using UI.Symbol;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Roulette {
    public class RouletteCell: MonoBehaviour {

        [SerializeField] private Image _icon;
        public RectTransform RectTransform { get; private set; }
        public RectTransform Parent { get; private set; }

        
        public void SetStatus(CellStatus pStatus) {

            return;
            //TODO: Apply material
            _icon.material = pStatus switch {
                _ => throw new NotImplementedException()
            };
        }

        public void SetIcon(int pCode) => 
            _icon.sprite = pCode.GetIcon();

        private void Awake() {
            RectTransform = GetComponent<RectTransform>();
            Parent = transform.parent!.GetComponent<RectTransform>();
        }
    }
}