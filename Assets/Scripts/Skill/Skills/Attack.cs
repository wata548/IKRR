using Character.Skill.Data;
using Data;
using UnityEngine;
using UnityEngine.Scripting;

namespace Character.Skill {
    
    [Preserve]
    public class Attack: SharedSkillBase {

        [SkillParameter]
        public override RangeValue Value { get; protected set; }

        [SkillParameter(100)]
        public AttackType Type { get; protected set; } = AttackType.Default;


        public Attack(RangeValue pValue, TargetValue pPositions, AttackType pType):base() =>
            (Value, Target, Type) = (pValue, pPositions, pType);
        
        public Attack(string[] pData): base(pData){}
        protected override void Implement(Positions pCaster) {
            var caster = CharactersManager.GetEntity(pCaster);
            var targets = CharactersManager.GetEntity(pCaster, Target.Value);
            
            var idx = targets.Length;
            foreach (var target in targets) {

                var amount = caster.AttackDamageCalc(Value.Value, target);
                target.ReceiveDamage(amount, target, pType: Type, pOnComplete: CustomEnd);
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