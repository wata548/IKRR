namespace Symbol {
    public interface ISymbolExecutor {

        void Update();
        bool IsUsable(int pColumn, int pRow);
        void Evolution(int pColumn, int pRow);
        void Execute(int pColumn, int pRow);
    }
}