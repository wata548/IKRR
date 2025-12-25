using Data;

namespace Symbol {
    public interface ISymbolDB {
        public SymbolData GetSymbolData(int pNumber);
    }
}