using System.Collections.Generic;
using Character.Skill;
using Data;
using UI;
using UnityEngine;

namespace FSM.StateStrategy {
    public class EnemyTurn: IStrategy {
        
        public static float AnimationInterval { get; set; } = 0.4f;
        private Queue<EnemyAnimation> _animationBuffer;
        private float _remainAnimationTerm = 0;
        private ISkill _playingSkill;
        
        public void OnEnter() {
            _animationBuffer = CharactersManager.GetEnemySkills();
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

            UIManager.Instance.Entity.GetEnemyUI(animationData.Caster).AttackAnimation();
            _playingSkill = animationData.Skill;
            _playingSkill.Execute(animationData.Caster);
        }

        public void OnExit() {}
    }
}