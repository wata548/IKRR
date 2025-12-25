using System;
using CSVData;
using UnityEngine;

namespace Data {
    [Serializable]
    public class SymbolData : ICSVDictionaryData {
        [field: SerializeField] public int SerialNumber { get; private set; }
        [field: SerializeField] public string Name { get; private set; }

        [field: SerializeField] public string Condition { get; private set; }
        [field: SerializeField] public string ConditionCode { get; private set; }

        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public SymbolRarity Rarity { get; private set; }

        [field: SerializeField] public string EvolveCondition { get; private set; }
        [field: SerializeField] public string EffectCode { get; private set; }

        [field: SerializeField] public SymbolType Type { get; private set; }
        [field: SerializeField] public TargetStatus Category { get; private set; }
    }
}