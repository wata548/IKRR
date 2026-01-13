using System.Collections.Generic;
using Character.Skill;
using Data;
using FSM.StateStrategy;
using Roulette;
using UI;
using UnityEngine;

namespace FSM {

    public class PlayerTurnState : IStrategy {
        public void OnEnter() { }

        public void Update() {
            if (AnimationStateBase.AnimationBuffer.Count == 0)
                return;
            
            Fsm.Instance.Change(State.PlayAnimation);
        }

        public void OnExit() { }
    }

    public class PlayAnimationState : AnimationStateBase {
        public override State NextState => State.PlayerTurn;
        protected override void Enter() {}
    }
    
    public class EvolveCheckState: AnimationStateBase {
        
        public override State NextState => State.BuffCheck;
        
        protected override void Enter() {
            var temp = RouletteManager.Evolve();
            while (temp.Count > 0) {
                AnimationBuffer.Enqueue(temp.Dequeue());
            }
        }
    }
    
    public class BuffCheckState: AnimationStateBase {
        public override State NextState => State.PlayerTurn;
            
        protected override void Enter() {
            var temp = RouletteManager.UsableBuff();
            while (temp.Count > 0) {
                AnimationBuffer.Enqueue(temp.Dequeue());
            }
        }
    }
    
    public abstract class AnimationStateBase: IStrategy {
        
        //==================================================||Fields 
        public static readonly Queue<CellAnimationData> AnimationBuffer = new();
        private ISkill _playingSkill; 
        private float _remainAnimationTerm = 0;
        
        //==================================================||Properties 
        public abstract State NextState { get; }
        public static float AnimationInterval { get; set; } = 0.4f;
        
        
        //==================================================||Methods 
        public void OnEnter() {
            _remainAnimationTerm = 0;
            Enter();
        }

        protected abstract void Enter();

        public void Update() {
            
            if (_playingSkill is { IsEnd: false })
                return;
            
            if (AnimationBuffer.Count == 0) {
                Fsm.Instance.Change(NextState);
                return;
            }
            if (_remainAnimationTerm > 0) {
                _remainAnimationTerm -= Time.deltaTime;
                return;
            }
            
            
            _remainAnimationTerm = AnimationInterval;
            var animationData = AnimationBuffer.Dequeue();
            
            _playingSkill = animationData.Skill;
            var roulette = UIManager.Instance.Roulette;
            if (animationData.Type == AnimationType.Use) {
                var (column, row, status) = 
                    (animationData.Column, animationData.Row, animationData.Status);
                            
                RouletteManager.SetStatus(column, row, status);
                roulette.UseAnimation(column, row, status);
            }
            
            _playingSkill.OnEnd += roulette.Refresh;
            _playingSkill.Execute(Positions.Player);
        }

        public void OnExit() { }
    }
}