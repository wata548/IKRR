using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Extension;

namespace Data {
    public record SymbolQueryArgs(
        Rarity Rarity = Rarity.Etc,
        SymbolCategory Category = SymbolCategory.None,
        TargetStatus Status = TargetStatus.None,
        SymbolType Type = SymbolType.None
    );
    
    public class SymbolDB: DictionaryBaseDB<int, SymbolData>, IQueryDB<int, SymbolData, SymbolQueryArgs> {

        public List<int> Query(SymbolQueryArgs pArgs) =>
            _matchToSerialNumber
                .Select(kvp => kvp.Value)
                .Where(symbol =>
                        symbol.Rarity != Rarity.Etc
                        && (pArgs.Rarity == Rarity.Etc || symbol.Rarity == pArgs.Rarity)
                        && (pArgs.Type == SymbolType.None || symbol.Type == pArgs.Type)
                        && (pArgs.Category == SymbolCategory.None || symbol.Category.HasFlag(pArgs.Category))
                        && (pArgs.Status == TargetStatus.None || symbol.StatCategory.HasFlag(pArgs.Status))
                ).Select(symbol => symbol.SerialNumber)
                .ToList();

        public List<int> SubQuery(List<int> pTarget, SymbolQueryArgs pArgs) =>
            pTarget
                .Select(code => _matchToSerialNumber[code])
                .Where(symbol =>
                    (pArgs.Rarity == Rarity.Etc || symbol.Rarity == pArgs.Rarity)
                    && (pArgs.Type == SymbolType.None || symbol.Type == pArgs.Type)
                    && (pArgs.Category == SymbolCategory.None || symbol.Category.HasFlag(pArgs.Category))
                    && (pArgs.Status == TargetStatus.None || symbol.StatCategory.HasFlag(pArgs.Status))
                ).Select(symbol => symbol.SerialNumber)
                .ToList();
    }
}