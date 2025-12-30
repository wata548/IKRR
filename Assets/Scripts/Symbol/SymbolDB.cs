using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Symbol {
    public class SymbolDB: IDB<ISymbolData> {

        private IReadOnlyDictionary<int, ISymbolData> _symbolBySerialNumber = null;
        public void LoadData(IDataLoader<ISymbolData> pLoader) {
            _symbolBySerialNumber = pLoader.Load();
        }

        public ISymbolData GetSymbolData(int pNumber) {
            if(_symbolBySerialNumber.TryGetValue(pNumber, out var data))
                return data;

            Debug.LogError($"This {pNumber}Number isn't symbol number. checkAgain");
            return null;
        }
    }
}