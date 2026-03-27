using System.Collections.Generic;
using Character;
using Character.Skill.Data;
using UnityEngine;

namespace Data {
    public class Limit: EffectBase {
        public override int Code => 3021;
        public override int ShowCount => _amount;
        private int _amount;

        public Limit(RangeValue pAmount) {
            _amount = pAmount.Value;
            Duration = 1;
        }

        public override void OnTurnEnd(IEntity pTarget) {}
        public override int OnReceiveDamage(int pAmount, IEntity pTarget, IEntity pOpponent) =>
            Mathf.Min(pAmount, _amount);

        public override Dictionary<string, object> Infos => new() {
            { "Amount", _amount }
        };
    }
}