using System.Collections.Generic;
using System.Linq;
using Extension;
using Roulette;
using UnityEngine;

namespace UI.ContainSymbol {
    public class SymbolList: MonoBehaviour {
        [SerializeField] private SymbolListElement _symbolPrefab;
        [SerializeField] private int _columnCnt = 2;
        [SerializeField] private int _rowCnt = 15;
        [SerializeField] private float _padding;
        private readonly List<SymbolListElement> _elements = new();
        private int _lastUpdate = -1;

        private void UpdateState() {
            if (RouletteManager.LastUpdateFrame == _lastUpdate)
                return;
            _lastUpdate = RouletteManager.LastUpdateFrame;

            var rect = (transform as RectTransform)!;
            var startPosition = _symbolPrefab.Size.y / (2f * rect.sizeDelta.y);
            var intervalRow =  (1 - startPosition) / (_rowCnt - 1);
            var intervalColumn =  1f / _columnCnt;

            var initPos = new Vector2(_padding / 2f, -startPosition);

            var dict = RouletteManager.HandDictionary.Where(kvp => kvp.Value > 0);
            var cnt = dict.Count();
            var pivot = new Pivot(PivotLocation.Down, PivotLocation.Up);
            var localPivot = new Pivot(PivotLocation.Down, PivotLocation.Middle);
            while (_elements.Count != cnt) {
                if (_elements.Count < cnt) {
                    var pos = initPos;
                    pos.y -= (_elements.Count / _columnCnt) * intervalRow;
                    pos.x += _elements.Count % _columnCnt * intervalColumn;

                    var symbol = Instantiate(_symbolPrefab, transform);
                    var symbolRect = (symbol.transform as RectTransform)!;
                    symbolRect.SetLocalPosition(rect, pivot, pos);
                    symbolRect.ChangeVirtualPivot(localPivot);
                    _elements.Add(symbol);
                }else{
                    Destroy(_elements[^1].gameObject);
                    _elements.RemoveAt(_elements.Count - 1);
                }
            }

            foreach (var (element, code, amount) in _elements.Zip(dict, (element, kvp) => (element, kvp.Key, kvp.Value))) {
                element.Set(code, amount);
            }
        }
        
        
        private void Update() {
            UpdateState();
        }

    }
}