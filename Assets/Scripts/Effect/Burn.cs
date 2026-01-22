using System;
using Character;
using Character.Skill.Data;

namespace Data {
    public class Burn: EffectBase {

        public override int Code => 3001;
        public Burn(RangeValue pDuration) : base(pDuration) {}

        public override void OnTurnStart(IEntity pTarget) {
            pTarget.ReceiveDamage(Duration);
        }

        public override object[] Infos =>
            new object[] { Duration, Duration * 2 };
    }
}