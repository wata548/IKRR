using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Extension;

namespace Data {
    public record SymbolQueryArgs(
        SymbolRarity Rarity = SymbolRarity.Etc,
        SymbolCategory Category = SymbolCategory.None,
        TargetStatus Status = TargetStatus.None,
        SymbolType Type = SymbolType.None
    );
    
    public interface ISymbolDB : IDB<int, SymbolData> {
        public int GetRandom(SymbolRarity pRarity);
        
        public List<int> Query(SymbolQueryArgs pArgs);
        public List<int> SubQuery(List<int> pTarget, SymbolQueryArgs pArgs);
    }
    
    public class SymbolDB: ISymbolDB {

        private IReadOnlyDictionary<int, SymbolData> _symbolBySerialNumber = null;
        private IReadOnlyDictionary<SymbolRarity, List<int>> _rarityDictionary = null;
        
        public void LoadData(IDataLoader<int, SymbolData> pLoader) {
            _symbolBySerialNumber = pLoader.Load().ToDictionary();
            _rarityDictionary = _symbolBySerialNumber
                .GroupBy(kvp => kvp.Value.Rarity)
                .Select(group => 
                    new KeyValuePair<SymbolRarity, List<int>>(
                        group.Key, 
                        group.Select(kvp => kvp.Value.SerialNumber)
                            .ToList()
                    )
                ).ToDictionary();
        }
        
        public SymbolData GetData(int pNumber) {
            if(_symbolBySerialNumber.TryGetValue(pNumber, out var data))
                return data;

            Debug.LogError($"This {pNumber}Number isn't symbol number. check again.");
            return null;
        }

        public List<int> Query(SymbolQueryArgs pArgs) =>
            _symbolBySerialNumber
                .Select(kvp => kvp.Value)
                .Where(symbol =>
                        symbol.Rarity != SymbolRarity.Etc
                        && (pArgs.Rarity == SymbolRarity.Etc || symbol.Rarity == pArgs.Rarity)
                        && (pArgs.Type == SymbolType.None || symbol.Type == pArgs.Type)
                        && (pArgs.Category == SymbolCategory.None || symbol.Category.HasFlag(pArgs.Category))
                        && (pArgs.Status == TargetStatus.None || symbol.StatCategory.HasFlag(pArgs.Status))
                ).Select(symbol => symbol.SerialNumber)
                .ToList();

        public List<int> SubQuery(List<int> pTarget, SymbolQueryArgs pArgs) =>
            pTarget
                .Select(code => _symbolBySerialNumber[code])
                .Where(symbol =>
                    (pArgs.Rarity == SymbolRarity.Etc || symbol.Rarity == pArgs.Rarity)
                    && (pArgs.Type == SymbolType.None || symbol.Type == pArgs.Type)
                    && (pArgs.Category == SymbolCategory.None || symbol.Category.HasFlag(pArgs.Category))
                    && (pArgs.Status == TargetStatus.None || symbol.StatCategory.HasFlag(pArgs.Status))
                ).Select(symbol => symbol.SerialNumber)
                .ToList();

        public int GetRandom(SymbolRarity pRarity) {
            
            var target = _rarityDictionary[pRarity];
            var idx = Random.Range(0, target.Count);
            return target[idx];
        }
    }
}