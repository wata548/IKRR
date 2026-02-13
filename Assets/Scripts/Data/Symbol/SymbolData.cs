using System.Collections.Generic;
using Lang;

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

        public bool ContainCategory(SymbolCategory pCategory) =>
            (Category | pCategory) != SymbolCategory.None;
        
        public Info GetInfo() {

            var details = new List<InfoDetail>();
            var desc = Rarity.ToKorean().ApplyLang() + 
                       ", " + Type.ToKorean().ApplyLang() + 
                       '\n'+ Description.ApplyLang();
            details.Add(new("정보", desc));
            if(!string.IsNullOrWhiteSpace(Condition))
                details.Add(new("조건", Condition));
            if (!string.IsNullOrWhiteSpace(EvolveDescription))
                details.Add(new(
                    "진화",
                    UseInfo.Get(SerialNumber) 
                        ? EvolveDescription 
                        : "???"
                    )
                );
            
            return new Info(Name, details, null, Rarity);
        } 
    }
}