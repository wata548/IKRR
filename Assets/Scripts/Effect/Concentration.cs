using System.Collections.Generic;
using Character;
using Character.Skill.Data;
using UI;

namespace Data {
    public class Concentration: EffectBase {
        public Concentration(RangeValue pDuration, RangeValue pAmount) : base(pDuration) {
            _amount = pAmount.Value;
        }
        public override int Code => 3006;
        private int _amount;

        public override int ShowCount => _amount;

        public override Dictionary<string, object> Infos => new() {
            { "Amount", _amount }
        };

        protected override EffectBase AddOperation(EffectBase rhs) {
            if (rhs is Concentration concentration)
                _amount += concentration._amount;
            return this;
        }

        public override void OnTurnStart(IEntity pTarget) {
            Status.AddValue(TargetStatus.Wisdom, _amount);
            UIManager.Instance.Status.Refresh(TargetStatus.Wisdom, null);
            Duration = 0;
            Update();
        }
    }
}