namespace Symbol {
    public static class SymbolExecuteManager {
        public static readonly ISymbolExecutor Executor = new LuaSymbolExecutor();
    }
}