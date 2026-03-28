using UI;

namespace FSM.StateStrategy {
    public class RewardState: IStrategy {
        public void Init() {}

        public void OnEnter(State pPrev) {
            UIManager.Instance.Reward.TurnOn();
        }

        public void Update() {
            if (UIManager.Instance.Reward.IsActive)
                return;
            UIManager.Instance.Map.ClearStage(true);
            Fsm.Instance.Change(State.SelectStage);
        }

        public void OnExit() {}

        public void EndBattle() {}
    }
}