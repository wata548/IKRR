using System.Collections.Generic;
using Data;
using DG.Tweening;
using Extension;
using FSM.StateStrategy;
using Roulette;
using UI;
using UnityEngine.Serialization;

namespace FSM {
    public class Fsm: MonoSingleton<Fsm> {
        protected override bool IsNarrowSingleton { get; } = true;
        public int Turn { get; private set; } = 0;
        public State State { get; private set; }
        private IStrategy _strategy;

        private static readonly IReadOnlyDictionary<State, IStrategy> _matchStrategy =
            new Dictionary<State, IStrategy>() {
                { State.SelectStage, new SelectStage() },
                { State.Rolling, new Rolling() },
                { State.Reward, new RewardState() },
                    
                { State.EvolveCheck, new EvolveCheckState() },
                { State.BuffCheck, new BuffCheckState() },
                { State.PlayAnimation, new PlayAnimationState() },
                { State.EffectAnimation, new EffectAnimationState() },
                { State.PlayerTurn, new PlayerTurnState() },
                { State.EnemyTurn, new EnemyTurn() },
            };

        public void StartBattle() {
            Turn = 0;
            foreach (var character in CharactersManager.GetEntities()) {
                character.OnBattleStart();
            }
        }

        public void EndBattle() {
            foreach(var (_, strategy) in _matchStrategy)
                strategy.EndBattle();
        }

        public void NextTurn() {
            Turn++;
            RouletteManager.OnTurnStart();
        }
        
        public void Change(State pState, bool pSkipAnimation = false) {
            if (State == pState)
                return;

            var prev = State;
            State = pState;
            _strategy?.OnExit();
            if (pSkipAnimation) {
                EnterEvent();       
                return;
            }

            var animation = UIManager.Instance.TurnShower.StartAnimation(State);
            if (animation == null) {
                EnterEvent();
                return;
            }

            _strategy = null;
            animation.OnComplete(EnterEvent);

            void EnterEvent() {
                _strategy = _matchStrategy[pState];
                _strategy.OnEnter(prev);
            }
        }

        protected void Start() {
            Change(State.SelectStage);
        }

        protected override void Update() {

            if (State is not (State.Reward or State.EffectAnimation) && EffectAnimationState.AnimationBuffer.Count > 0) {
                Change(State.EffectAnimation);
            }
            
            base.Update();
            _strategy?.Update();
        }
    }
}