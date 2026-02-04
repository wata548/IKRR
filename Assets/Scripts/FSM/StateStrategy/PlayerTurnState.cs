using Data;
using FSM.StateStrategy;

namespace FSM {
    public class PlayerTurnState : IStrategy {
        public void OnEnter(State pPrev) { }

        public void Update() {
            if (!CharactersManager.IsFighting) {
                Fsm.Instance.Change(State.SelectStage);
                return;
            }
            
            if (AnimationStateBase.AnimationBuffer.Count == 0)
                return;
            
            Fsm.Instance.Change(State.PlayAnimation);
        }

        public void OnExit() { }
        public void EndBattle() {}
    }
}