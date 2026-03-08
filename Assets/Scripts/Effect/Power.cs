using System.Collections.Generic;
using Character;
using Character.Skill.Data;

namespace Data {
    public class Power : EffectBase {
        //==================================================||Fields 
        private int _amount;
        
        //==================================================||Properties 
        public override int Code => 3013;
        public override int ShowCount => _amount;

        public override Dictionary<string, object> Infos => new()  {
            {"Amount", _amount}
        };

        //==================================================||Constructors 
        public Power(RangeValue pAmount) {
            _amount = pAmount.Value;
            Duration = 1;
        }
        
        //==================================================||Methods 
        public override void OnBattleStart(IEntity pTarget) {
            Duration = 0;
        }

        public override void OnTurnEnd(IEntity pTarget) { }

        public override int OnSendDamage(int pAmount, AttackType pType, IEntity pTarget, IEntity pOpponent) {
            return pAmount + _amount;
        }

        protected override EffectBase AddOperation(EffectBase rhs) {
            if (rhs is Power power)
                _amount += power._amount;
            return this;
        }
    }
}