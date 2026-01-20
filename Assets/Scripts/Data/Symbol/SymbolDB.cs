using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Extension;

namespace Data {
    public interface ISymbolDB : IDB<int, SymbolData> {
        public int GetRandom(SymbolRarity pRarity);
    }
    
    public class SymbolDB: ISymbolDB {

        private IReadOnlyDictionary<int, SymbolData> _symbolBySerialNumber = null;
        private IReadOnlyDictionary<SymbolRarity, List<int>> _rarityDictionary = null;
        
        public void LoadData(IDataLoader<int, SymbolData> pLoader) {
            _symbolBySerialNumber = pLoader.Load().ToDictionary();
        }

        public SymbolData GetData(int pNumber) {
            if(_symbolBySerialNumber.TryGetValue(pNumber, out var data))
                return data;

            Debug.LogError($"This {pNumber}Number isn't symbol number. checkAgain");
            return null;
        }

        public int GetRandom(SymbolRarity pRarity) {
            _rarityDictionary ??= _symbolBySerialNumber
                .GroupBy(kvp => kvp.Value.Rarity)
                .Select(group => 
                    new KeyValuePair<SymbolRarity, List<int>>(
                        group.Key, 
                        group.Select(kvp => kvp.Value.SerialNumber)
                             .ToList()
                    )
                ).ToDictionary();

            var target = _rarityDictionary[pRarity];
            var idx = Random.Range(0, target.Count);
            return target[idx];
        }
    }
}