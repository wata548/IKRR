namespace Data {
       
    public class SymbolData{
        public string Name { get; protected set; }
        public string Condition { get; protected set; }
        public string ConditionCode { get; protected set; }
        public string Description { get; protected set; }
        public SymbolRarity Rarity { get; protected set; }
        public string EvolveCondition { get; protected set; }
        public string EffectCode { get; protected set; }
        public SymbolType Type { get; protected set; }
        public TargetStatus Category { get; protected set; }
    }
}