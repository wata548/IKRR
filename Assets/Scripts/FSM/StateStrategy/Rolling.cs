using UI;

namespace FSM.StateStrategy {
    public class Rolling: IStrategy {
        public void OnEnter() {
            UIManager.Instance.Roulette.Roll();
        }

        public void Update() {
        }

        public void OnExit() {
        }
    }
}