using Character.Skill.Data;

namespace Character.Skill {
    public abstract class SharedSkillBase: PlayerTargetSkillBase {
        
        [SkillParameter]
        public TargetValue Target { get; protected set; } = new();
        protected SharedSkillBase(string[] pData): base(pData){}
        protected SharedSkillBase(): base(){}
    }
}