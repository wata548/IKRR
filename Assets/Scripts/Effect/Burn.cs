using System;
using Character;
using Character.Skill;
using Character.Skill.Data;
using FSM;

namespace Data {
    public class Burn: EffectBase {

        public override int Code => 3001;
        public Burn(RangeValue pDuration) : base(pDuration) {}

        public override void OnTurnStart(IEntity pTarget) {
            var attack = new Attack(new(Duration), new(pTarget.Position), AttackType.Burn);
            EffectAnimationState.Add(new(pTarget.Position, attack));
        }

        public override object[] Infos =>
            new object[] { Duration, Duration * 2 };
    }
}