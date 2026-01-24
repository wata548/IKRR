namespace FSM.StateStrategy {
    public interface IStrategy {
        void OnEnter();
        void Update();
        void OnExit();
        void EndBattle();
    }
}