using System.Collections.Generic;
using Character.Skill;
using Data;
using FSM.StateStrategy;
using Roulette;
using UI;
using UnityEngine;

namespace FSM {
    public class EffectAnimationState: IStrategy {
        
        public static float AnimationInterval { get; set; } = 0.4f;
        public static readonly Queue<EntityAnimation> AnimationBuffer = new();
        private float _remainAnimationTerm = 0;
        private ISkill _playingSkill;
        private State _prev;

        public static void Add(EntityAnimation pAnimation) => AnimationBuffer.Enqueue(pAnimation);
        
        public void EndBattle() {}

        public void OnEnter(State pPrev) {
            if (pPrev != State.Reward)
                _prev = pPrev;
        }

        public void Update() {
            if (_playingSkill is { IsEnd: false })
                return;

            if (UIManager.Instance.LevelUp.NeedUpdate) {
                Fsm.Instance.Change(State.Reward);
                return;
            }
            
            if (AnimationBuffer.Count == 0) {
                Fsm.Instance.Change(_prev, true);
                return;
            }
            if (_remainAnimationTerm > 0) {
                _remainAnimationTerm -= Time.deltaTime;
                return;
            }
            
            _remainAnimationTerm = AnimationInterval;
            var animationData = AnimationBuffer.Dequeue();

            CharactersManager.GetEntity(animationData.Caster).OnSkillUse();
            _playingSkill = animationData.Skill;
            _playingSkill.Execute(animationData.Caster);
        }

        public void OnExit() {}
    }
}