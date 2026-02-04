using UI;

namespace FSM.StateStrategy {
    public class RewardState: IStrategy {

        private State _prevState; 
        
        public void OnEnter(State pPrev) {
            _prevState = pPrev;
            UIManager.Instance.LevelUp.TurnOn();
        }

        public void Update() {

            if (!UIManager.Instance.LevelUp.IsActive)
                Fsm.Instance.Change(_prevState);
        }

        public void OnExit() {}

        public void EndBattle() { }
    }
}