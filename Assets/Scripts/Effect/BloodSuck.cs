using Character;
using UnityEngine;

namespace Data {
    public class BloodSuck: EffectBase {
        public override int Code => 3014;
        public override int ShowCount => -1;

        public BloodSuck() => Duration = 1;


        public override void OnTurnEnd(IEntity pTarget) { }
        public override int OnSendDamage(int pAmount, AttackType pType, IEntity pTarget, IEntity pOpponent) {
            pTarget.Heal(Mathf.CeilToInt(pAmount / 3f));
            return base.OnSendDamage(pAmount, pType, pTarget, pOpponent);
        }
        
    }
}