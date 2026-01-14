using Character.Skill.Data;
using Data;
using UnityEngine.Scripting;

namespace Character.Skill {
    [Preserve]  
    public class Heal: SharedSkillBase {

        [SkillParameter]
        public override RangeValue Value { get; protected set; }
        
        public Heal(string[] pData): base(pData){}
        protected override void Implement(Positions pCaster) {
            throw new System.NotImplementedException();
        }
    }
}