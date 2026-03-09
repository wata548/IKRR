using Character.Skill.Data;
using Data;
using Extension;
using Lang;

namespace Character.Skill {
    public class Shield: SkillBase {
        
        [SkillParameter] public RangeValue Amount { get; private set; }
        [SkillParameter] public TargetValue Target { get; private set; }

        public Shield(string[] pData) : base(pData) {}

        protected override void Implement(Positions pCaster) {
            foreach (var entity in CharactersManager.GetEntities(pCaster, Target.Value)) {
                entity.AddShield(Amount.Value);
                Amount.Next();
            }
            End();
        }

        public override string ToString() {
            return "방어 획득".ApplyLang();
        }
    }

    public class ClearShield : SkillBase {
        
        [SkillParameter] public TargetValue Target { get; private set; }

        public ClearShield(string[] pData) : base(pData) {}
        
        protected override void Implement(Positions pCaster) {
            foreach (var entity in CharactersManager.GetEntities(pCaster, Target.Value)) {
                entity.AddShield(-entity.Shield); 
            }
            End();
        }
    }
}