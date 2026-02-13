using Character;
using Character.Skill;
using Character.Skill.Data;
using FSM;

namespace Data {
    public class Needle: EffectBase {
        public override int Code => 3003;
        public Needle(RangeValue pDuration) : base(pDuration) {
        }
        public override int OnReceiveDamage(int pAmount, IEntity pTarget, IEntity pOpponent) {
            var attack = new Attack(new(3), new(pOpponent.Position), AttackType.Needle);
            EffectAnimationState.Add(new(pTarget.Position, attack));
            return pAmount;
        }
    }
}