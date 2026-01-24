using System.Collections.Generic;
using Extension;
using FSM.StateStrategy;
using UnityEngine.Serialization;

namespace FSM {
    public class Fsm: MonoSingleton<Fsm> {
        protected override bool IsNarrowSingleton { get; } = true;

        public int Turn { get; private set; } = 0;
        public State State { get; private set; }
        public State PrevState { get; private set; }
        private IStrategy _strategy;

        private static readonly IReadOnlyDictionary<State, IStrategy> _matchStrategy =
            new Dictionary<State, IStrategy>() {
                { State.SelectStage, new SelectStage() },
                { State.Rolling, new Rolling() },
                    
                { State.EvolveCheck, new EvolveCheckState() },
                { State.BuffCheck, new BuffCheckState() },
                { State.PlayAnimation, new PlayAnimationState() },
                { State.EffectAnimation, new EffectAnimationState() },
                { State.PlayerTurn, new PlayerTurnState() },
                { State.EnemyTurn, new EnemyTurn() },
            };

        public void StartBattle() =>
            Turn = 0;

        public void EndBattle() {
            foreach(var (_, strategy) in _matchStrategy)
                strategy.EndBattle();
        }

        public void NextTurn() =>
            Turn++;
        
        public void Change(State pState) {
            if (State == pState)
                return;

            PrevState = State;
            State = pState;
            _strategy?.OnExit();
            _strategy = _matchStrategy[pState];
            _strategy.OnEnter();
        }

        protected void Start() {
            Change(State.SelectStage);
        }

        protected override void Update() {

            if (State != State.EffectAnimation && EffectAnimationState.AnimationBuffer.Count > 0) {
                Change(State.EffectAnimation);
            }
            
            base.Update();
            _strategy?.Update();
        }
    }
}