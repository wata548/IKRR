namespace FSM {
    public enum State {
        Error,
        SelectStage,
        Reward,
        
        Rolling,
        EvolveCheck,
        BuffCheck,
        PlayAnimation,
        EffectAnimation,
        PlayerTurn,
        EnemyTurn,
    }
}