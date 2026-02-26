using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extension;
using Roulette;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI.Roulette {
    public class FakeWheel: Wheel {

        private List<int> _queue;
        private bool _isLastOneAppear = false;
        private int _lastOne;
        private bool _needLastOneGenerate = false;
        private int _height;
        private int _remainCnt;

        public void SetResult(int pCode) => _lastOne = pCode;
        
        public override void Init(int pIdx, int pHeight, List<int> pData, Action<RouletteCell> pOnClick = null, Action pOnStop = null) {
            _queue = pData;
            _height = pHeight;
            base.Init(pIdx, pHeight, pData, pOnClick, pOnStop);
        }

        public override void StartRoll() {
            _remainCnt = _height;
            _isLastOneAppear = false;
            _needLastOneGenerate = false;
            base.StartRoll();
        }

        public void Stop() {
            _needLastOneGenerate = true;
        }

        protected override void ShowNewCell() {
            var interval = 1f / (_cells.Count - 1);
            var temp = _cells[0];
            
            _cells.RemoveAt(0);
            var pos = _cells[^1].RectTransform.GetLocalPosition(RectTransform, Pivot.Down).y + interval;
            temp.RectTransform.SetLocalPositionY(RectTransform, PivotLocation.Down, pos);
            _cells.Add(temp);

            var idx = Random.Range(0, _queue.Count);
            var code = _queue[idx];
            if (_isLastOneAppear) {
                _remainCnt--;
            }
            if (_needLastOneGenerate) {
                _isLastOneAppear = true;
                _needLastOneGenerate = false;
                code = _lastOne;    
            }
            temp.SetIcon(code);
            if (_remainCnt == 0) {
                StopRoll();
            }
        }
    }
}