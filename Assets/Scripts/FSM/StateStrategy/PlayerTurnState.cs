using Data;
using FSM.StateStrategy;
using UI;

namespace FSM {
    public class PlayerTurnState : IStrategy {
        public void OnEnter(State pPrev) { }

        public void Update() {
            if (Fsm.Instance.CheckBattleEnd())
                return;
            
            if (AnimationStateBase.AnimationBuffer.Count == 0)
                return;
            
            Fsm.Instance.Change(State.PlayAnimation);
        }

        public void OnExit() { }
        public void EndBattle() {}
    }
}