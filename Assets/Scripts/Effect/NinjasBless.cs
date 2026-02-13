using Character;
using Character.Skill.Data;

namespace Data {
    public class NinjasBless: EffectBase {
        public override int Code => 3004;
        public NinjasBless(RangeValue pDuration) : base(pDuration) {
        }

        public override int OnSendDamage(int pAmount, AttackType pType, IEntity pTarget, IEntity pOpponent) {
            if (pType == AttackType.Shuriken)
                pAmount++;
            return pAmount;
        }
    }
}