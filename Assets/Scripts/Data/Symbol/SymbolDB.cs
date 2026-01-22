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
    
    public class SymbolDB: DictionaryBaseDB<SymbolData>, IQueryDB<int, SymbolData, SymbolQueryArgs> {

        private IReadOnlyDictionary<int, SymbolData> _symbolBySerialNumber = null;

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
    }
}