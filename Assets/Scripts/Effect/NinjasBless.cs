using System.Collections.Generic;
using Character;
using Character.Skill.Data;
using UI;

namespace Data {
    public class NinjasBless: EffectBase {
        public override int Code => 3004;
        private int _amount = 0;
        public NinjasBless(RangeValue pAmount) {
            _amount = pAmount.Value;
            Duration = 1;
        }

        public override int ShowCount => _amount;

        public override void OnTurnEnd(IEntity pTarget) {}
        public override void OnBattleStart(IEntity pTarget) {
            Duration = 0;
            Update();
        }

        protected override EffectBase AddOperation(EffectBase rhs) {
            if (rhs is NinjasBless ninja)
                _amount += ninja._amount;
            return this;
        }

        public override int OnSendDamage(int pAmount, AttackType pType, IEntity pTarget, IEntity pOpponent) {
            if (pType == AttackType.Shuriken)
                pAmount += _amount;
            return pAmount;
        }

        public override Dictionary<string, object> Infos => new() {
            { "Amount", _amount }
        };
    }
}