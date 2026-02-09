using System.Collections.Generic;

namespace Data {
       
    public class SymbolData{
        public int SerialNumber { get; protected set; } 
        public string Name { get; protected set; }
        public string Condition { get; protected set; }
        public string ConditionCode { get; protected set; }
        public string Description { get; protected set; }
        public Rarity Rarity { get; protected set; }
        public string EvolveDescription { get; protected set; }
        public string EvolveCondition { get; protected set; }
        public string EffectCode { get; protected set; }
        public SymbolType Type { get; protected set; }
        public TargetStatus StatCategory { get; protected set; }
        public SymbolCategory Category { get; protected set; }

        public Info GetInfo() {

            var descs = new List<InfoDetail>();
            if(!string.IsNullOrWhiteSpace(Condition))
                descs.Add(new("조건", Condition));

            descs.Add(new("정보", Description));
            
            if (!string.IsNullOrWhiteSpace(EvolveDescription))
                descs.Add(new(
                    "진화",
                    UseInfo.Get(SerialNumber) 
                        ? EvolveDescription 
                        : "???"
                    )
                );
            
            return new Info(Name, descs, null, Rarity);
        } 
    }
}