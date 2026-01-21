using System.Collections.Generic;
using System.Linq;
using Extension;
using Roulette;
using UnityEngine;

namespace UI.ContainSymbol {
    public class SymbolList: MonoBehaviour {
        [SerializeField] private SymbolListElement _symbolPrefab;
        [SerializeField] private Vector2Int _tableSize = new(3, 5);
        [SerializeField] private Vector2 _padding = new(0.1f, 0);
        private readonly List<SymbolListElement> _elements = new();
        private int _lastUpdate = -1;

        private void UpdateState() {
            if (RouletteManager.LastUpdateFrame == _lastUpdate)
                return;
            _lastUpdate = RouletteManager.LastUpdateFrame;
            
            var dict = RouletteManager.HandDictionary.Where(kvp => kvp.Value > 0);
            var cnt = dict.Count();
            var args = new PlaceArgs<SymbolListElement>(
                _padding,
                cnt,
                _tableSize,
                _symbolPrefab,
                null
            );
            
            (transform as RectTransform)!.Place(_elements, args);
            foreach (var (element, code, amount) in _elements.Zip(dict, (element, kvp) => (element, kvp.Key, kvp.Value))) {
                element.Set(code, amount);
            }
        }
        
        
        private void Update() {
            UpdateState();
        }

    }
}