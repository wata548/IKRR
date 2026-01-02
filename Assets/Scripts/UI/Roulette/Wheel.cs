using System;
using System.Collections.Generic;
using Extension;
using Extension.Test;
using Roulette;
using UnityEngine;

namespace UI.Roulette {
    public class Wheel: MonoBehaviour {

        [SerializeField] private RouletteCell _cellPrefab;
        [SerializeField] private float _power;
        private readonly List<RouletteCell> _cells = new();
        private bool _isRoll = true;
        
        [TestMethod(pRuntimeOnly:true)]
        private void Init(int pAmount) {
            if (pAmount == 0)
                return;
            
            while (_cells.Count != pAmount + 1) {
                if (_cells.Count > pAmount + 1) {
                    var temp = _cells[0];
                    Destroy(temp.gameObject);
                    _cells.RemoveAt(0);
                }
                else {
                    var newCell = Instantiate(_cellPrefab, transform);
                    _cells.Add(newCell);
                }    
            }

            var interval = 1f / pAmount;
            var pos = new Vector2(0.5f, interval / 2f);
            var scale = GetComponent<RectTransform>().sizeDelta;
            var cellScale = Math.Min(scale.y / pAmount, scale.x);
            
            for (int i = 0; i <= pAmount; i++, pos.y += interval) {
                _cells[i].RectTransform.SetLocalPosition(Pivot.Down, pos);
                _cells[i].RectTransform.sizeDelta = Vector2.one * cellScale;
            }
        }

        private void ShowNewCell() {
            var interval = 1f / (_cells.Count - 1);
            var temp = _cells[0];
            _cells.RemoveAt(0);

            var pos = _cells[^1].RectTransform.GetLocalPosition(_cells[^1].Parent, Pivot.Down).y + interval;
            temp.RectTransform.SetLocalPositionY(temp.Parent, PivotLocation.Down, pos);
            _cells.Add(temp);
            //RouletteManager.Roll(0);
        }
        
        private void Move() {

            if (_cells.Count == 0)
                return;
            
            var speed = -_power * Time.deltaTime;
            foreach (var cell in _cells) {
                cell.RectTransform.AddPositionY(speed);
            }

            var nextPoint = -0.5f / (_cells.Count - 1);
            while (_cells[0].RectTransform.GetLocalPosition(_cells[0].Parent, Pivot.Down).y < nextPoint) {
                ShowNewCell();
            }
        }

        [TestMethod]
        private void Stop() {
            _isRoll = false;
            
            var pos = _cells[0].RectTransform.GetLocalPosition(_cells[0].Parent, Pivot.Down).y;
            if (pos < 0) {
                ShowNewCell();
                pos = _cells[0].RectTransform.GetLocalPosition(_cells[0].Parent, Pivot.Down).y;
            }

            var delta = 0.5f / (_cells.Count - 1) - pos;
            foreach (var cell in _cells) {
                cell.RectTransform.AddLocalPosition(cell.Parent, new(0, delta));
            }
        }

        private void Update() {
            if(_isRoll)
                Move();
        }
    }
}