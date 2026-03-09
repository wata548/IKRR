using Data;
using UnityEngine;

namespace Character.Skill {
    public class Run: SkillBase {
        public Run(string[] pArgs) : base(pArgs) {}
        
        protected override void Implement(Positions pCaster) {
            CharactersManager.GetEntity(pCaster).KillSelf(End);
        }

        public override string ToString() {
            return "전투에서 사라진다.";
        }
    }
}