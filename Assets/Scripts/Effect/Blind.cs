using Character;
using Character.Skill.Data;
using UnityEngine;

namespace Data {
    public class Blind: EffectBase {
        public Blind(RangeValue pDuration) : base(pDuration) {}
        public override int Code => 3002;

        public override int OnSendDamage(int pAmount, IEntity pTarget, IEntity pOpponent) {
            var value = Random.Range(0f, 1f);
            Update();
            return pAmount * (value > 0.3f ? 1 : 0);
        }
    }
}