using Data;
using UI;

namespace FSM.StateStrategy {
    public class Rolling: IStrategy {
        
        public void OnEnter(State pPrev) {

            if (!CharactersManager.IsFighting) {
                Fsm.Instance.Change(State.SelectStage);
                UIManager.Instance.Map.ClearStage(true);
                return;
            }
                
            
            if (UIManager.Instance.Roulette.IsRoll)
                return;

            Fsm.Instance.NextTurn();
            UIManager.Instance.Roulette.Roll();
            CharactersManager.OnTurnEnd(false);
            CharactersManager.OnTurnStart(true);
        }

        public void Update() {
        }

        public void OnExit() {
        }

        public void EndBattle() { }
    }
}