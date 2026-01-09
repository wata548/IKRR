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
        
        private readonly Queue<(int, int)> _animationBuffer = new();
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
                wheel.Init(i, RouletteManager.Height, RouletteManager.GetColumn(i), OnClick);
            }
            _volum.transform.SetAsLastSibling();

            return;

            void OnClick(RouletteCell cell) {
                if (IsRoll) {
                    Debug.Log("Roulette is rolling");
                    return;
                }
                
                if (Fsm.Instance.State != State.PlayerTurn)
                    return;
                _animationBuffer.Enqueue((cell.Column, cell.Row));
            }
        }

        private void Stop() {
            if (!IsRoll || Fsm.Instance.State != State.Rolling)
                return;
            
            _wheels.First(wheel => wheel.IsRoll).StopRoll();
            if (!IsRoll) {
                
                Fsm.Instance.Change(State.PlayAnimation);
                _animation?.Kill();
                transform.position = _origin;
                Refresh();
                
                var temp = RouletteManager.UsableBuff();
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
        
        private void PlayBuffSymbol() {

            if (_remainAnimationTerm > 0 && _playingSkill is {IsEnd: true}) {
                _remainAnimationTerm -= Time.deltaTime;
            }
            
            if (Fsm.Instance.State is not (State.PlayerTurn or State.PlayAnimation))
                return;
            
            if (_animationBuffer.Count == 0) {
                Fsm.Instance.Change(State.PlayerTurn);
                return;
            }
            
            Fsm.Instance.Change(State.PlayAnimation);
            
            if (_playingSkill is { IsEnd: false })
                return;

            _remainAnimationTerm = AnimationInterval;
            var (x, y) = _animationBuffer.Dequeue();
            if (!RouletteManager.Use(x, y, out var status, out var skill)) {
                return;
            }

            _wheels[x].Use(y, status);
            _playingSkill = skill;
            skill.OnEnd = Refresh;
            skill.Execute(Positions.Player);
        }
        
        //==================================================||Unity 
        private void Start() {
            //TODO: This code is just test code
            var list = new List<int>();
            list.AddRange(Enumerable.Repeat(1001, 10));
            list.AddRange(Enumerable.Repeat(1003, 9));
            list.AddRange(Enumerable.Repeat(1004, 3));
            list.AddRange(Enumerable.Repeat(1005, 3));
            RouletteManager.Init(list);

            _lever.onClick.AddListener(Stop);
            SetUp();

            //TODO: This code is just test code
            Roll();
        }

        private void Update() {
            PlayBuffSymbol();           
        }
    }
}