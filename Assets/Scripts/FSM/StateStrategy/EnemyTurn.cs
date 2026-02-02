using System.Collections.Generic;
using Character.Skill;
using Data;
using Extension;
using UI;
using UnityEngine;

namespace FSM.StateStrategy {
    public class EnemyTurn: IStrategy {
        
        public static float AnimationInterval { get; set; } = 0.4f;
        private Queue<EntityAnimation> _animationBuffer;
        private float _remainAnimationTerm = 0;
        private ISkill _playingSkill;
        private int _lastTurn = -1;

        public void EndBattle() => _lastTurn = -1;
        
        public void OnEnter() {
            if (Fsm.Instance.Turn == _lastTurn)
                return;

            _lastTurn = Fsm.Instance.Turn;
            _animationBuffer = CharactersManager.GetEnemySkills();
            CharactersManager.OnTurnEnd(true);
            CharactersManager.OnTurnStart(false);
        }

        public void Update() {
            if (_playingSkill is { IsEnd: false })
                return;
            
            if (_animationBuffer.Count == 0) {
                Fsm.Instance.Change(State.Rolling);
                return;
            }
            if (_remainAnimationTerm > 0) {
                _remainAnimationTerm -= Time.deltaTime;
                return;
            }
            
            _remainAnimationTerm = AnimationInterval;
            var animationData = _animationBuffer.Dequeue();

            CharactersManager.GetEntity(animationData.Caster).OnSkillUse();
            UIManager.Instance.Entity.GetEnemyUI(animationData.Caster).AttackAnimation();
            _playingSkill = animationData.Skill;
            _playingSkill.Execute(animationData.Caster);
        }

        public void OnExit() { }
    }
}