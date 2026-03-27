using System.Collections.Generic;
using Character;
using Character.Skill.Data;
using UI;

namespace Data {
    public class Double: EffectBase {
        public override int Code => 3019;
        public override int ShowCount => _amount;
        private int _amount;

        public Double(RangeValue pDuration, RangeValue pAmount): base(pDuration) {
            _amount = pAmount.Value + 1;
        }


        protected override EffectBase AddOperation(EffectBase rhs) {
            if (rhs is not Double other) return this;
            _amount += other._amount - 1;
            return this;
        }

        public override int OnSendDamage(int pAmount, AttackType pType, IEntity pTarget, IEntity pOpponent) {
            return pAmount * _amount;
        }

        public override Dictionary<string, object> Infos => new() {
            { "Duration", Duration },
            { "Amount", _amount }
        };
    }
}