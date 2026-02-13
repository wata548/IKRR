using System.Collections.Generic;
using System.Linq;
using Character;
using Character.Skill;
using Character.Skill.Data;
using FSM;

namespace Data {
    public class Reflect: EffectBase {

       //==================================================||Properties 
        public override int Code => 3005;
        public int Limit { get; private set; }

       //==================================================||Constructors 
        public Reflect(RangeValue pDuration, RangeValue pLimit) : base(pDuration) {
            Limit = pLimit.Value;
        }

       //==================================================||Methods 
        protected override EffectBase AddOperation(EffectBase rhs) {
            if (rhs is Reflect needle)
                Limit += needle.Limit;
            
            return base.AddOperation(rhs);
        }

        public override int OnReceiveDamage(int pAmount, IEntity pTarget, IEntity pOpponent) {
            Limit -= pAmount;
            if (Limit < 0) {
                pAmount += Limit;
                Limit = 0;
            }

            var attack = new Attack(new(pAmount), new(pOpponent.Position), AttackType.Needle);
            EffectAnimationState.Add(new(pTarget.Position, attack));
            Update();
            return pAmount;
        }

        public override Dictionary<string, object> Infos => new() {
            {"Duration", Duration}, 
            {"Amount", Limit}
        };
    }
}