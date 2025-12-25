using System.Collections.Generic;
using Data;

namespace Symbol {
    public interface ISymbolDataLoader {
        public List<SymbolData> Load();
    }
}