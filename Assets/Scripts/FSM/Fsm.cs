using System.Collections.Generic;
using Extension;
using FSM.StateStrategy;
using UnityEngine.Serialization;

namespace FSM {
    public class Fsm: MonoSingleton<Fsm> {
        protected override bool IsNarrowSingleton { get; } = true;

        public State State { get; private set; }
        private IStrategy _strategy;

        private static readonly IReadOnlyDictionary<State, IStrategy> _matchStrategy =
            new Dictionary<State, IStrategy>() {
                { State.SelectStage, new SelectStage() },
                { State.Rolling, new Rolling() },
                    
                { State.EvolveCheck, new EvolveCheckState() },
                { State.BuffCheck, new BuffCheckState() },
                { State.PlayAnimation, new PlayAnimationState() },
                { State.PlayerTurn, new PlayerTurnState() },
                { State.EnemyTurn, new EnemyTurn() },
            };

        public void Change(State pState) {
            if (State == pState)
                return;

            State = pState;
            _strategy?.OnExit();
            _strategy = _matchStrategy[pState];
            _strategy.OnEnter();
        }

        protected void Start() {
            Change(State.SelectStage);
        }

        protected override void Update() {
            base.Update();
            _strategy?.Update();
        }
    }
}