using UnityEngine;

namespace FSM.StateStrategy {
    public class TestStrategy: IStrategy {
        public State State { get; }
        public TestStrategy(State pState) => State = pState;
        
        public void OnEnter(Fsm pMachine) {
            Debug.Log($"Enter {State} state");
        }

        public void Update(Fsm pMachine) {
        }

        public void OnExit(Fsm pMachine) {
            Debug.Log($"Exit {State} state");
        }
    }
}