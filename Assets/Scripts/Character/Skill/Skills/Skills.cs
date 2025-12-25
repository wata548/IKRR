using Character.Skill.Data;
using Data;

namespace Character.Skill {
    
    public class AttackSkill: SharedSkillBase {

        public override SkillType SkillType { get; protected set; } = SkillType.Attack;
        public override string Name { get; protected set; }
        public RangeValue Damage { get; protected set; }
        
        public AttackSkill(params string[] pData): base(pData){}
        public override void Execute(Targets pCaster, int pRepeatCount = 1, int pRepeatIdx = 1) {
        }
    }
    
    public class HealSkill: SharedSkillBase {

        public override SkillType SkillType { get; protected set; } = SkillType.Attack;
        public override string Name { get; protected set; }
        public RangeValue Damage { get; protected set; }
        
        public HealSkill(params string[] pData): base(pData){}
        public override void Execute(Targets pCaster, int pRepeatCount = 1, int pRepeatIdx = 1) {

        }
    }
}