using System.Collections.Generic;
using Character.Skill;
using Data;
using FSM.StateStrategy;
using Roulette;
using UI;
using UnityEngine;

namespace FSM {
    
    public class PlayAnimationState : AnimationStateBase {
        public override State NextState => State.PlayerTurn;
        protected override void Enter() {}
        protected override void OnSkillUse() {
            CharactersManager.Player.OnSkillUse();
        }
    }
    
    public class EvolveCheckState: AnimationStateBase {
        
        public override State NextState => State.BuffCheck;
        
        protected override void Enter() {
            var temp = RouletteManager.GetEvolveTargets();
            while (temp.Count > 0) {
                var data = temp.Dequeue();
                if(data.Skill != null)
                    AnimationBuffer.Enqueue(data);
            }
        }
    }
    
    public class BuffCheckState: AnimationStateBase {
        public override State NextState => State.PlayerTurn;
            
        protected override void Enter() {
            
            
            var temp = RouletteManager.GetUsableBuffs();
            while (temp.Count > 0) {
                var data = temp.Dequeue();
                if(data.Skill != null)
                    AnimationBuffer.Enqueue(data);
            }
            CharactersManager.Player.OnRouletteStop();
        }

        public override void OnExit() { }
    }
    
    public abstract class AnimationStateBase: IStrategy {
        
        //==================================================||Fields 
        public static readonly Queue<CellAnimationData> AnimationBuffer = new();
        private ISkill _playingSkill; 
        private float _remainAnimationTerm = 0;
        private int _lastTurn = -1;
        
        //==================================================||Properties 
        public abstract State NextState { get; }
        public static float AnimationInterval { get; set; } = 0.4f;
        
        
        //==================================================||Methods 
        public void EndBattle() => _lastTurn = -1;
        public void OnEnter() {
            if (Fsm.Instance.Turn == _lastTurn)
                return;
            
            _lastTurn = Fsm.Instance.Turn;
            _remainAnimationTerm = 0;
            Enter();
        }

        protected abstract void Enter();

        protected virtual void OnSkillUse() {}
        
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
            OnSkillUse();           
            _playingSkill.Execute(Positions.Player);
        }

        public virtual void OnExit() { }
    }
}