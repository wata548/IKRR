using Character;
using UnityEngine;

namespace Data {
    public class Pyrophonic:EffectBase {
        public override int Code => 3016;
        public override int ShowCount => -1;

        public Pyrophonic() { Duration = 1;}
        public override void OnTurnEnd(IEntity pTarget) {}
        public override int OnReceiveDamage(int pAmount, IEntity pTarget, IEntity pOpponent) {
            var value = Random.Range(0, 1f);
            if (value >= 0.5f)
                pOpponent.AddEffect(new Burn(new(1)));
            return pAmount;
        }
    }
}