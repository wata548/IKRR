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
        
        private readonly Queue<CellAnimationData> _animationBuffer = new();
        private ISkill _playingSkill = null;
        private float _remainAnimationTerm = 0;
        
        //==================================================||Properties

        [field: SerializeField] public float AnimationInterval { get; set; } = 0.1f;
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
            
            UIManager.Instance.Status.Clear();
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
                
                _animationBuffer.Enqueue(new(AnimationType.Use, cell.Column, cell.Row, skill, status));
            }
        }

        private void Stop() {
            if (!IsRoll || Fsm.Instance.State != State.Rolling)
                return;
            
            _wheels.First(wheel => wheel.IsRoll).StopRoll();
            if (!IsRoll) {
                
                _animation?.Kill();
                transform.position = _origin;
                Refresh();
                
                Fsm.Instance.Change(State.EvolveCheck);
                var temp = RouletteManager.Evolve();
                while (temp.Count > 0) {
                    _animationBuffer.Enqueue(temp.Dequeue());
                }
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

        public void Evolve(int pColumn, int pRow, int pNewCode, Action pOnComplete, bool pPlayAnimation) {
            _wheels[pColumn].Evolve(pRow, pNewCode, pOnComplete, pPlayAnimation);
        }
        
        private void PlayAnimation() {
            if (_playingSkill is { IsEnd: false })
                return;
            
            if (_remainAnimationTerm > 0) {
                _remainAnimationTerm -= Time.deltaTime;
            }
            
            if (Fsm.Instance.State is not (State.PlayerTurn or State.PlayAnimation or State.EvolveCheck))
                return;
            
            if (_animationBuffer.Count == 0) {

                switch (Fsm.Instance.State) {
                    case State.PlayerTurn:
                        return;
                    case State.PlayAnimation:
                        Fsm.Instance.Change(State.PlayerTurn);
                        return;
                    case State.EvolveCheck:
                        var temp = RouletteManager.UsableBuff();
                        while (temp.Count > 0) {
                            _animationBuffer.Enqueue(temp.Dequeue());
                        }
                        Fsm.Instance.Change(State.PlayAnimation);
                        break;
                }
            }
            if(Fsm.Instance.State is State.PlayerTurn)
                Fsm.Instance.Change(State.PlayAnimation);
            
            _remainAnimationTerm = AnimationInterval;
            var animationData = _animationBuffer.Dequeue();
            

            _playingSkill = animationData.Skill;
            if (animationData.Type == AnimationType.Use) {
                var (column, row, status) = 
                    (animationData.Column, animationData.Row, animationData.Status);
                
                RouletteManager.SetStatus(column, row, status);
                _wheels[column].Use(row);
                _playingSkill.OnEnd += () => _wheels[column].SetStatus(row, status);
            }

            _playingSkill.OnEnd += Refresh;
            _playingSkill.Execute(Positions.Player);
        }
        
        //==================================================||Unity 
        private void Start() {
            _lever.onClick.AddListener(Stop);
            SetUp();
        }

        private void Update() {
            PlayAnimation();           
        }
    }
}