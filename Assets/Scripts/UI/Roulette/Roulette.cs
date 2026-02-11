using System;
using System.Collections.Generic;
using System.Linq;
using Character.Skill;
using Data;
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
        [SerializeField] private Image _volum;
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
            
            UIManager.Instance.Status.Clear();
            ClearStatus();
            _origin = transform.position;   
            _animation = transform.DOShakePosition(1, ANIMATION_POWER, fadeOut:false).SetLoops(-1);
            
            _wheels[0].Focus(true);
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
                wheel.RectTransform.SetLocalPosition(new Pivot(PivotLocation.Down), pos);
                wheel.RectTransform.ChangeVirtualPivot(new Pivot(PivotLocation.Down));
                wheel.Init(i, RouletteManager.Height, RouletteManager.GetColumn(i), OnCellClick);
            }
            _volum.transform.SetAsLastSibling();

            return;

            void OnCellClick(RouletteCell cell) {
                if (IsRoll) {
                    Debug.Log("Roulette is rolling");
                    return;
                }
                if (Fsm.Instance.State != State.PlayerTurn)
                    return;
                if (!RouletteManager.Use(cell.Column, cell.Row, out var status, out var skill))
                    return;
                if (skill == null)
                    return;
                
                AnimationStateBase.AnimationBuffer
                    .Enqueue(new(AnimationType.Use, cell.Column, cell.Row, skill, status));
            }
        }

        private void Stop() {
            if (!IsRoll || Fsm.Instance.State != State.Rolling)
                return;
            
            var wheel = _wheels.First(wheel => wheel.IsRoll);
            wheel.StopRoll();
            wheel.Focus(false);
            
            if (!IsRoll) {
                
                _animation?.Kill();
                transform.position = _origin;
                Refresh();
                
                Fsm.Instance.Change(State.EvolveCheck);
            }
            else {
                wheel = _wheels.First(wheel => wheel.IsRoll);
                wheel.Focus(true);
            }
        }

        private void ClearStatus() {
            RouletteManager.ClearStatus();
            SymbolStatusApply();
        } 
        
        public void Refresh() {
            RouletteManager.Refresh();
            SymbolStatusApply();
        }
        
        private void SymbolStatusApply() {
            foreach (var wheel in _wheels) {
                wheel.Refresh();
            }
        }

        public void Evolve(int pColumn, int pRow, int pNewCode, Action pOnComplete, bool pPlayAnimation) {
            _wheels[pColumn].Evolve(pRow, pNewCode, pOnComplete, pPlayAnimation);
        }

        public void UseAnimation(int pColumn, int pRow, CellStatus pStatus) {
            _wheels[pColumn].Use(pRow, pStatus);
        }

        //==================================================||Unity 
        private void Start() {
            _lever.onClick.AddListener(Stop);
            SetUp();
        }
    }
}