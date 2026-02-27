using System;
using System.Collections.Generic;
using Data;
using Roulette;
using UI.ContainSymbol;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SymbolSelector {
    public class SymbolSelector: MonoBehaviour {
        [SerializeField] private GameObject _pannel;
        [SerializeField] private SymbolShower _prev;
        [SerializeField] private SymbolShower _next;
        [SerializeField] private Button _add;
        [SerializeField] private Button _cancel;
        private Queue<int> _addItems = new();
        private bool _isActive = false;
        private int _addTarget; 
        
        public void Add(int pCode, int pAmount = 1) {
            if (RouletteManager.TryAdd(pCode,  pAmount, out var need))
                return;
            for(int i = 0; i < need; i++)
                _addItems.Enqueue(pCode);
        }

        private void TurnOn(int pCode) {
            _pannel.SetActive(true);
            _isActive = true;
            _prev.Set(DataManager.EMPTY_SYMBOL);
            _next.Set(pCode);
            _addTarget = pCode;
        }

        private void TurnOff(bool pAdd) {
            _pannel.SetActive(false);
            _isActive = false;
            if (!pAdd)
                return;
            RouletteManager.Remove(SelectorShower.SelectedSymbol);
            RouletteManager.TryAdd(_addTarget, 1, out _);
        }

        private void Awake() {
            _add.onClick.AddListener(() => TurnOff(true));
            _cancel.onClick.AddListener(() => TurnOff(false));
        }

        private void Update() {
            if(SelectorShower.SelectedSymbol != _prev.Code)
                _prev.Set(SelectorShower.SelectedSymbol);

            if (_addItems.Count != 0 && !_isActive) {
                var code = _addItems.Dequeue();
                TurnOn(code);
                return;
            }

            _add.interactable = SelectorShower.SelectedSymbol != DataManager.EMPTY_SYMBOL;
        }
    }
}