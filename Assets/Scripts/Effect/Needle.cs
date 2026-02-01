using System.Collections.Generic;
using System.Linq;
using Character;
using Character.Skill;
using Character.Skill.Data;
using FSM;

namespace Data {
    public class Needle: EffectBase {

       //==================================================||Properties 
        public override int Code => 3003;
        public int Limit { get; private set; }

       //==================================================||Constructors 
        public Needle(RangeValue pDuration, RangeValue pLimit) : base(pDuration) {
            Limit = pLimit.Value;
        }

       //==================================================||Methods 
        protected override EffectBase AddOperation(EffectBase rhs) {
            if (rhs is Needle needle)
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
            pOpponent.ReceiveDamage(pAmount, pOpponent, false, AttackType.Needle);
            Update();
            return pAmount;
        }

        public override Dictionary<string, object> Infos => new() {
            {"Duration", Duration}, 
            {"Amount", Limit}
        };
    }
}