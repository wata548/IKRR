namespace Data {
       
    public class SymbolData{
        public int SerialNumber { get; protected set; } 
        public string Name { get; protected set; }
        public string Condition { get; protected set; }
        public string ConditionCode { get; protected set; }
        public string Description { get; protected set; }
        public SymbolRarity Rarity { get; protected set; }
        public string EvolveDescription { get; protected set; }
        public string EvolveCondition { get; protected set; }
        public string EffectCode { get; protected set; }
        public SymbolType Type { get; protected set; }
        public TargetStatus StatCategory { get; protected set; }
        public SymbolCategory Category { get; protected set; }

        public Info GetInfo() {
            return new Info(
                SerialNumber,
                Name,
                new() {
                    ("사용 조건:", Condition),
                    ("설명:", Description),
                    ("진화 정보:", UseInfo.Get(SerialNumber) ? EvolveDescription : "???")
                }
            );
        } 
    }
}