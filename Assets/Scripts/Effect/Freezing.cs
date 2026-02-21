using Character;

namespace Data {
    public class Freezing: EffectBase {
        public override int Code => 3012;
        public override int OnSendDamage(int pAmount, AttackType pType, IEntity pTarget, IEntity pOpponent) {
            return pAmount * 3 / 4;
        }
    }
}