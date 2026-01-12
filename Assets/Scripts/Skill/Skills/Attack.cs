using Character.Skill.Data;
using Data;

namespace Character.Skill {
    
    public class Attack: SharedSkillBase {

        [SkillParameter]
        public override RangeValue Value { get; protected set; }
        
        public Attack(string[] pData): base(pData){}
        protected override void Implement(Positions pCaster) {
            var targets = CharactersManager.GetEntity(pCaster, Target.Value);
            var idx = targets.Length;
            foreach (var target in targets) {
                target.ReceiveDamage(Value.Value, CustomEnd);
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