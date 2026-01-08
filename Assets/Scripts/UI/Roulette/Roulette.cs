using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Extension;
using FSM;
using Roulette;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Roulette {
    public class Roulette: MonoBehaviour {
        private static readonly Vector3 ANIMATION_POWER = new(0, 4);
        
        //==================================================||Fields
        [SerializeField] private Wheel _wheelPrefab;
        [SerializeField] private Button _lever;
        private RectTransform _rect;
        private readonly List<Wheel> _wheels = new();
        private Tween _animation = null;
        private Vector3 _origin;
        
        //==================================================||Properties
        public bool IsRoll {
            get {
                foreach (var wheel in _wheels) {
                    if (wheel.IsRoll)
                        return true;
                }
                return false;
            }
        }

        //==================================================||Methods 
        public void Roll() {
            if (IsRoll)
                return;
            
            Fsm.Instance.Change(State.Rolling);
            ClearStatus();
            _origin = transform.position;   
            _animation = transform.DOShakePosition(1, ANIMATION_POWER, fadeOut:false).SetLoops(-1);
            
            foreach (var wheel in _wheels) {
                wheel.StartRoll();
            }
        }

        private void SetUp() {
            _rect = GetComponent<RectTransform>();

            var interval = 1f / RouletteManager.Width;
            var wheelSize = _rect.sizeDelta;
            wheelSize.x *= interval;

            var pos = Vector2.zero;
            for (int i = 0; i < RouletteManager.Width; i++, pos.x += interval) {
                var wheel = Instantiate(_wheelPrefab, transform);
                _wheels.Add(wheel);
                
                wheel.RectTransform.sizeDelta = wheelSize;
                wheel.RectTransform.SetLocalPosition(Pivot.Down, pos);
                wheel.RectTransform.ChangeVirtualPivot(Pivot.Down);
                wheel.Init(i, RouletteManager.Height, RouletteManager.GetColumn(i), OnClick);
            }

            return;

            void OnClick(RouletteCell cell) {
                if (IsRoll) {
                    Debug.Log("Roulette is rolling");
                    return;
                }
                var result = Use(cell.Column, cell.Row);
                Debug.Log($"({cell.Column}, {cell.Row}): {result}");
            }
        }

        private void Stop() {
            if (!IsRoll || Fsm.Instance.State != State.Rolling)
                return;
            
            _wheels.First(wheel => wheel.IsRoll).StopRoll();
            if (!IsRoll) {
                
                Fsm.Instance.Change(State.PlayerTurn);
                _animation?.Kill();
                transform.position = _origin;
                Refresh();
            }
        }

        private void ClearStatus() {
            RouletteManager.ClearStatus();
            SymbolStatusApply();
        } 
        private void Refresh() {
            RouletteManager.Refresh();
            SymbolStatusApply();
        }
        private void SymbolStatusApply() {
            foreach (var wheel in _wheels) {
                wheel.Refresh();
            }
        }
        
        private bool Use(int pColumn, int pRow) {
            if (Fsm.Instance.State != State.PlayerTurn)
                return false;
            if (!RouletteManager.Use(pColumn, pRow, out var status))
                return false;
            _wheels[pColumn].Use(pRow, status);
            return true;
        }
        
        
        //==================================================||Unity 
        private void Start() {
            //TODO: This code is just test code
            RouletteManager.Init(Enumerable.Repeat(1001, 12));

            _lever.onClick.AddListener(Stop);
            SetUp();

            //TODO: This code is just test code
            Roll();
        }
    }
}