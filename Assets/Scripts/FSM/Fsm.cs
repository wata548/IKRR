using System.Collections.Generic;
using Extension;
using FSM.StateStrategy;

namespace FSM {
    public class Fsm: MonoSingleton<Fsm> {
        protected override bool IsNarrowSingleton { get; } = true;

        public State State => _strategy?.State ?? default;
        private IStrategy _strategy;

        private static readonly IReadOnlyDictionary<State, IStrategy> _matchStrategy =
            new Dictionary<State, IStrategy>() {
                { State.SelectStage, new TestStrategy(State.SelectStage) },
                { State.Rolling, new TestStrategy(State.Rolling) },
                { State.PlayAnimation, new TestStrategy(State.PlayAnimation) },
                { State.PlayerTurn, new TestStrategy(State.PlayerTurn) },
                { State.EnemyTurn, new TestStrategy(State.EnemyTurn) },
            };

        public void Change(State pState) {
            if (State == pState)
                return;
            
            _strategy?.OnExit(this);
            _strategy = _matchStrategy[pState];
            _strategy.OnEnter(this);
        }

        protected override void Awake() {
            base.Awake();
            Change(State.SelectStage);
        }

        protected override void Update() {
            base.Update();
            _strategy?.Update(this);
        }
    }
}