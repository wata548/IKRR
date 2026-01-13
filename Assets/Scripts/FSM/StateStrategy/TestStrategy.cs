using UnityEngine;

namespace FSM.StateStrategy {
    public class TestStrategy: IStrategy {
        public State State { get; }
        public TestStrategy(State pState) => State = pState;
        
        public void OnEnter() {
            Debug.Log($"Enter {State} state");
        }

        public void Update() {
        }

        public void OnExit() {
            Debug.Log($"Exit {State} state");
        }
    }
}