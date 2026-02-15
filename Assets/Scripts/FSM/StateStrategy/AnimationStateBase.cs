using System.Collections.Generic;
using Character.Skill;
using Data;
using FSM.StateStrategy;
using Roulette;
using Skill.Skills;
using Symbol;
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
        public static float AnimationInterval { get; set; } = 0.05f;
        
        
        //==================================================||Methods 
        public void EndBattle() => _lastTurn = -1;
        public void OnEnter(State pPrev) {
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

            if (UIManager.Instance.LevelUp.NeedUpdate) {
                Fsm.Instance.Change(State.Reward);
                return;
            }
            
            if (AnimationBuffer.Count == 0) {
                Fsm.Instance.Change(NextState, true);
                return;
            }
            if (_remainAnimationTerm > 0) {
                _remainAnimationTerm -= Time.deltaTime;
                return;
            }
            
            var animationData = AnimationBuffer.Dequeue();
            var nextStatus = default(CellStatus);
            if (animationData.Type == AnimationType.Evolve) {
                while (true) {
                    _playingSkill = SymbolExecutor.Evolution(animationData.Column, animationData.Row);
                    if (_playingSkill != null)
                        break;
                    if (AnimationBuffer.Count == 0)
                        return;
                    animationData = AnimationBuffer.Dequeue();
                }
            }
            else {
                while (!RouletteManager.Use(animationData.Column, animationData.Row, out nextStatus, out _playingSkill)) {
                    if (AnimationBuffer.Count == 0)
                        return;
                    animationData = AnimationBuffer.Dequeue();
                }
            }
            
            var roulette = UIManager.Instance.Roulette;
            if (_playingSkill == null) {
                if (animationData.Type is AnimationType.Buff)
                    _playingSkill = new EmptySkill();
                else
                    return;
            }
            
            if (animationData.Type is (AnimationType.Use or AnimationType.Buff)) {
                var (column, row, status) = 
                    (animationData.Column, animationData.Row, nextStatus);
                            
                if(animationData.Type == AnimationType.Use)
                    RouletteManager.OnSkillSymbolUse();
                RouletteManager.SetStatus(column, row, status);
                roulette.UseAnimation(column, row, status);
            }
            
            _remainAnimationTerm = AnimationInterval;
            _playingSkill.OnEnd += roulette.Refresh;
            OnSkillUse();           
            _playingSkill.Execute(Positions.Player);
        }

        public virtual void OnExit() { }
    }
}