using System;
using System.Collections.Generic;
using Character;
using Character.Skill;
using Character.Skill.Data;
using FSM;

namespace Data {
    public class Burn: EffectBase {

        public override int Code => 3001;
        public Burn(RangeValue pDuration) : base(pDuration) {}

        public override void OnTurnStart(IEntity pTarget) {
            var attack = new Attack(new(Duration * 2), new(pTarget.Position), AttackType.Burn);
            EffectAnimationState.Add(new(pTarget.Position, attack));
        }

        public override Dictionary<string, object> Infos =>
            new() {
                {"Duration", Duration},
                {"Amount", Duration * 2}
            };
    }
}