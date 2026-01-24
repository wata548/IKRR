using UI;

namespace FSM.StateStrategy {
    public class SelectStage: IStrategy {
        public void OnEnter() {
            UIManager.Instance.Map.SetActive(true);
        }

        public void Update() {
        }

        public void OnExit() {
            UIManager.Instance.Map.SetActive(false);
        }

        public void EndBattle() {}
    }
}