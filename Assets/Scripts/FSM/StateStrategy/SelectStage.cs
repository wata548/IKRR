using UI;

namespace FSM.StateStrategy {
    public class SelectStage: IStrategy {
        public void OnEnter(State pPrev) {
            UIManager.Instance.Map.SetActive(true);
            Fsm.Instance.EndBattle();
        }

        public void Update() {
        }

        public void OnExit() {}

        public void EndBattle() {}
    }
}