using System;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ContainSymbol {
    public class SelectorShower: SymbolAmountShower {

        [SerializeField] private Button _button;
        public static int SelectedSymbol { get; private set; }
        public const string MATERIAL = "TargetOutLine";
        private bool _isSelected = false;  
        
        public override void Set(int pTarget, int pAmount) {
            base.Set(pTarget, pAmount);
            SelectedSymbol = DataManager.EMPTY_SYMBOL;
            _shower.material = null;
        }

        private void Awake() {
            _button.onClick.AddListener(() => SelectedSymbol = _data);
        }

        private void FixedUpdate() {

            if (_isSelected != (SelectedSymbol != _data))
                return;
            
            _isSelected = !_isSelected;
            _shower.material = _isSelected ? MaterialStore.Get(MATERIAL) : null;
        }
    }
}