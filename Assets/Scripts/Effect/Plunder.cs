using Character;
using UnityEngine;

namespace Data {
    public class Plunder:EffectBase {
        public override int Code => 3015;
        public override int ShowCount => _amount;
        private int _amount = 0;

        public Plunder() => Duration = 1;
        public override void OnTurnEnd(IEntity pTarget) {}
        public override int OnSendDamage(int pAmount, AttackType pType, IEntity pTarget, IEntity pOpponent) {
            var range = Random.Range(5, 11);
            if (PlayerData.Money > 0) {
                _amount += Mathf.Min(PlayerData.Money, range);
                PlayerData.GetMoney(-range);
            }

            Update();
            return base.OnSendDamage(pAmount, pType, pTarget, pOpponent);
        }
        public override void OnDeath(IEntity pTarget) {
            PlayerData.GetMoney(_amount);
        }
    }
}