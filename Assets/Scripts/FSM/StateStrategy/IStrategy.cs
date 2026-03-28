namespace FSM.StateStrategy {
    public interface IStrategy {
        void Init();
        void OnEnter(State pPrev);
        void Update();
        void OnExit();
        void EndBattle();
    }
}