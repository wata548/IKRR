using Data;
using UI;

namespace FSM.StateStrategy {
    public class Rolling: IStrategy {
        
        public void OnEnter() {
            if (UIManager.Instance.Roulette.IsRoll)
                return;

            Fsm.Instance.NextTurn();
            CharactersManager.OnTurnEnd(false);
            CharactersManager.OnTurnStart(true);
            UIManager.Instance.Roulette.Roll();
        }

        public void Update() {
        }

        public void OnExit() {
        }

        public void EndBattle() { }
    }
}