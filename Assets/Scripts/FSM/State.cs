namespace FSM {
    public enum State {
        Error,
        SelectStage,
        Rolling,
        EvolveCheck,
        BuffCheck,
        PlayAnimation,
        EffectAnimation,
        PlayerTurn,
        EnemyTurn,
    }
}