using System.Collections.Generic;
using UnityEngine;
using Extension;

namespace Data {
    public class SymbolDB: IDB<int, SymbolData> {

        private IReadOnlyDictionary<int, SymbolData> _symbolBySerialNumber = null;
        public void LoadData(IDataLoader<int, SymbolData> pLoader) {
            _symbolBySerialNumber = pLoader.Load().ToDictionary();
        }

        public SymbolData GetSymbolData(int pNumber) {
            if(_symbolBySerialNumber.TryGetValue(pNumber, out var data))
                return data;

            Debug.LogError($"This {pNumber}Number isn't symbol number. checkAgain");
            return null;
        }
    }
}