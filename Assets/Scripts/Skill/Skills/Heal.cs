using System.Linq;
using Character.Skill.Data;
using Data;
using UnityEngine.Scripting;

namespace Character.Skill {
    
    [Preserve]
    public class Heal: SharedSkillBase {

        [SkillParameter]
        public override RangeValue Value { get; protected set; }

        public Heal(RangeValue pValue, TargetValue pPositions):base() =>
            (Value, Target) = (pValue, pPositions);
        
        public Heal(string[] pData): base(pData){}
        protected override void Implement(Positions pCaster) {
            var caster = CharactersManager.GetEntity(pCaster);
            var targets = CharactersManager.GetEntities(pCaster, Target.Value).ToArray();
            
            var idx = targets.Length;
            foreach (var target in targets) {

                target.Heal(Value.Value, pOnComplete: CustomEnd);
                Value.Next();
            }

            void CustomEnd() {
                idx--;
                if (idx == 0)
                    End();
            }
            
        }
    }
}