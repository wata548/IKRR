namespace FSM.StateStrategy {
    public interface IStrategy {
        State State { get; }
        void OnEnter(Fsm pMachine);
        void Update(Fsm pMachine);
        void OnExit(Fsm pMachine);
        
    }
}