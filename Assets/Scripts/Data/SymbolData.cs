using System;
using CSVData;
using UnityEngine;

namespace Data {
    public class CSVSymbolData : ISymbolData, ICSVDictionaryData  {
        
        public int SerialNumber { get; private set; }
        public string Name { get; private set; }
        public string Condition { get; private set; }
        public string ConditionCode { get; private set; }
        public string Description { get; private set; }
        public SymbolRarity Rarity { get; private set; }
        public string EvolveCondition { get; private set; }
        public string EffectCode { get; private set; }
        public SymbolType Type { get; private set; }
        public TargetStatus Category { get; private set; }
    }
    
    public interface ISymbolData{
        public string Name { get;}

        public string Condition { get;}
        public string ConditionCode { get; }

        public string Description { get; }
        public SymbolRarity Rarity { get; }

        public string EvolveCondition { get; }
        public string EffectCode { get; }

        public SymbolType Type { get; }
        public TargetStatus Category { get; }
    }
}