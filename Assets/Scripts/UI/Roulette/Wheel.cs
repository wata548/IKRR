using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using DG.Tweening;
using Extension;
using Roulette;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI.Roulette {
    [RequireComponent(typeof(Image))]
    public class Wheel: MonoBehaviour {

       //==================================================||Fields 
        [SerializeField] protected RouletteCell _cellPrefab;
        [SerializeField] protected float _power;
        [SerializeField] protected float _powerDelta = 300;
        private Image _board; 
        protected readonly List<RouletteCell> _cells = new();
        public bool IsRoll { get; private set; } = false;
        private int _idx;
        private Action _onStop;
        
       //==================================================||Properties
       public RectTransform RectTransform { get; private set; }
       
       //==================================================||Methods 
        public virtual void Init(int pIdx, int pHeight, IEnumerable<CellInfo> pData, Action<RouletteCell> pOnClick = null, Action pOnStop = null) {
            
            if (pHeight == 0)
                return;

            _onStop = pOnStop;
            _idx = pIdx;
            while (_cells.Count != pHeight + 1) {
                if (_cells.Count > pHeight + 1) {
                    var temp = _cells[0];
                    Destroy(temp.gameObject);
                    _cells.RemoveAt(0);
                }
                else {
                    var newCell = Instantiate(_cellPrefab, transform);
                    _cells.Add(newCell);
                }    
            }

            var interval = 1f / pHeight;
            var pos = new Vector2(0.5f, interval / 2f);
            var scale = GetComponent<RectTransform>().sizeDelta;
            var cellScale = Math.Min(scale.y / pHeight, scale.x);

            foreach (var (cell, info) in _cells.Zip(pData, (cell, info) => (cell, info))) {
                
                cell.AddOnClickListener(() => pOnClick?.Invoke(cell));
                cell.RectTransform.SetLocalPosition(Pivot.Down, pos);
                cell.RectTransform.sizeDelta = Vector2.one * cellScale;
                
                cell.SetIcon(info.Code);
                cell.SetStatus(info.Status);
                pos.y += interval;
            }
        }

        public void Refresh() {
            for (int row = 0; row < RouletteManager.Height; row++) {
                if (RouletteManager.Get(_idx, row) == DataManager.EMPTY_SYMBOL) {
                    _cells[row].Empty();
                    continue;
                }
                var status = RouletteManager.GetStatus(_idx, row);
                _cells[row].SetStatus(status);
            }
        }

        public void Refresh(int pIdx) {
            if (RouletteManager.Get(_idx, pIdx) == DataManager.EMPTY_SYMBOL)
                return;
            var status = RouletteManager.GetStatus(_idx, pIdx);
            _cells[pIdx].SetStatus(status);
        }
        
        protected virtual void ShowNewCell() {
            var interval = 1f / (_cells.Count - 1);
            var temp = _cells[0];
            
            _cells.RemoveAt(0);
            var pos = _cells[^1].RectTransform.GetLocalPosition(RectTransform, Pivot.Down).y + interval;
            temp.RectTransform.SetLocalPositionY(RectTransform, PivotLocation.Down, pos);
            _cells.Add(temp);
            
            var code = RouletteManager.Roll(_idx);
            temp.SetIcon(code);
        }

        public void Evolve(int pIdx, int pNewCode, Action pOnComplete, bool pPlayAnimation) {
            _cells[pIdx].Evolve(pNewCode, pOnComplete, pPlayAnimation);
        }
        
        private void Move() {

            if (Time.timeScale == 0)
                return;
            
            if (_cells.Count == 0)
                return;
            
            var speed = -_power * Time.deltaTime / Time.timeScale;
            foreach (var cell in _cells) {
                var pos = cell.RectTransform.localPosition;
                pos.y += speed;
                cell.RectTransform.localPosition = pos;
                
                //add symbols 3d style
                //var localPos = cell.RectTransform.GetLocalPosition(RectTransform, Pivot.Middle).y;
                //cell.transform.rotation = Quaternion.Euler(new(localPos * 35 * 2, 0, 0));
            }

            var nextPoint = -0.5f / (_cells.Count - 1);
            while (_cells[0].RectTransform.GetLocalPosition(RectTransform, Pivot.Down).y < nextPoint) {
                ShowNewCell();
            }
        }

        public virtual void StartRoll() {
            IsRoll = true;
            foreach (var cell in _cells) {
                cell.IsRoll = true;
            }
        }
        
        public virtual void StopRoll() {
            IsRoll = false;
            
            var pos = _cells[0].RectTransform.GetLocalPosition(RectTransform, Pivot.Down).y;
            if (pos < 0) {
                ShowNewCell();
                pos = _cells[0].RectTransform.GetLocalPosition(RectTransform, Pivot.Down).y;
            }

            var delta = 0.5f / (_cells.Count - 1) - pos;
            var rowIdx = -1;
            foreach (var cell in _cells) {
                rowIdx++;
                
                cell.RectTransform.DOLocalMoveY(cell.RectTransform.localPosition.y + RectTransform.sizeDelta.y * delta, 0.8f * Time.timeScale)
                    .SetEase(Ease.OutElastic)
                    .OnComplete(() => {
                        _onStop?.Invoke();
                        foreach (var cell in _cells) {
                            cell.IsRoll = false;
                        }
                    });
                cell.SetIdx(_idx, rowIdx);
            }
        }

        public void Use(int pIdx, CellStatus pStatus) => 
            _cells[pIdx].PlayAnimation(pStatus);

        public void Focus(bool pActive) {
            if (!pActive) {
                _board.material = null;
                return;
            }

            var mat = MaterialStore.Get("RatioOutline");
            mat.SetVector("_SizeDelta", _board.rectTransform.sizeDelta);
            _board.material = mat;
        }
        
       //==================================================||Unity 
        private void Update() {
            if (!IsRoll)
                return;
            
            Move();
        }

        private void Awake() {
            RectTransform = GetComponent<RectTransform>();
            _power += Random.Range(0, _powerDelta);
            _board = GetComponent<Image>();
        }
        
    }
}