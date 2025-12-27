using Character.Skill.Data;

namespace Character.Skill {
    public abstract class PlayerTargetSkillBase : SkillBase {
        
        [SkillParameter]
        public abstract RangeValue Value { get; protected set; }
        protected PlayerTargetSkillBase(string[] pData): base(pData){}
    }
}