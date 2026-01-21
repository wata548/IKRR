using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extension;
using Roulette;
using UnityEngine;

namespace UI.Roulette {
    public class FakeWheel: Wheel {

        private List<int> _queue;
        private int _queueIdx = 0;
        private bool _isLastOneAppear = false;
        private int _lastOne;
        private bool _needLastOneGenerate = false;
        private int _height;
        private int _remainCnt;

        public void SetLastOne(int pCode) => _lastOne = pCode;
        
        public override void Init(int pIdx, int pHeight, IEnumerable<CellInfo> pData, Action<RouletteCell> pOnClick = null, Action pOnStop = null) {
            _queue = pData.Select(data => data.Code).ToList();
            _height = pHeight;
            base.Init(pIdx, pHeight, pData, pOnClick, pOnStop);
        }

        public override void StartRoll() {
            const float ANIMATION_DURATION = 3;

            _remainCnt = _height;
            _queueIdx = 0;
            _isLastOneAppear = false;
            _needLastOneGenerate = false;
            base.StartRoll();
            StartCoroutine(Wait());

            IEnumerator Wait() {
                yield return new WaitForSeconds(ANIMATION_DURATION * Time.timeScale);
                _needLastOneGenerate = true;
            } 
        }

        protected override void ShowNewCell() {
            var interval = 1f / (_cells.Count - 1);
            var temp = _cells[0];
            
            _cells.RemoveAt(0);
            var pos = _cells[^1].RectTransform.GetLocalPosition(RectTransform, Pivot.Down).y + interval;
            temp.RectTransform.SetLocalPositionY(RectTransform, PivotLocation.Down, pos);
            _cells.Add(temp);

            _queueIdx++;
            if (_queueIdx >= _queue.Count)
                _queueIdx = 0;

            var code = _queue[_queueIdx];
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