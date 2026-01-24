using FSM.StateStrategy;

namespace FSM {
    public class PlayerTurnState : IStrategy {
        public void OnEnter() { }

        public void Update() {
            if (AnimationStateBase.AnimationBuffer.Count == 0)
                return;
            
            Fsm.Instance.Change(State.PlayAnimation);
        }

        public void OnExit() { }
        public void EndBattle() {}
    }
}