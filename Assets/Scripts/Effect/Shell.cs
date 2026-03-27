using System.Collections.Generic;
using Character;
using Character.Skill.Data;

namespace Data {
    public class Shell: EffectBase {
        public override int Code => 3020;
        public override int ShowCount => _amount;
        private int _amount;

        public Shell(RangeValue pAmount) {
            _amount = pAmount.Value;
            Duration = 1;
        }

        public override void OnTurnEnd(IEntity pTarget) {}
        public override void OnTurnStart(IEntity pTarget) {
            pTarget.AddShield(_amount);
        }

        protected override EffectBase AddOperation(EffectBase rhs) {
            if (rhs is Shell shell)
                _amount += shell._amount;
            return this;
        }

        public override Dictionary<string, object> Infos => new() {
            { "Amount", _amount }
        };
    }
}