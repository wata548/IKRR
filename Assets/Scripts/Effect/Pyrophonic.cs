using Character;
using UnityEngine;

namespace Data {
    public class Pyrophonic:EffectBase {
        public override int Code => 3016;
        public override int ShowCount => -1;
        
        public Pyrophonic() { Duration = 1;}
        public override void OnTurnEnd(IEntity pTarget) {}

        public override int OnReceiveDamage(int pAmount, IEntity pTarget, IEntity pOpponent) {
            var r = Random.Range(0, 2);
            if (r == 0) {
                CharactersManager.Player.AddEffect(new Burn(new(1)));
            }
            return pAmount;
        }
    }
}