using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using Data;

namespace Symbol {
    public class SymbolDataStore: ISymbolDB {

        private ReadOnlyDictionary<int, SymbolData> _symbolBySerialNumber = null;
        public void LoadData(ISymbolDataLoader pLoader) {
            var temp = pLoader.Load()
                .ToDictionary(symbol => symbol.SerialNumber, symbol => symbol);
            _symbolBySerialNumber = new(temp);
        }

        public SymbolData GetSymbolData(int pNumber) {
            if(_symbolBySerialNumber.TryGetValue(pNumber, out var data))
                return data;

            Debug.LogError($"This {pNumber}Number isn't symbol number. checkAgain");
            return null;
        }
    }
}