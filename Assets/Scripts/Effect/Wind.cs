using Character;

namespace Data {
    public class Wind: EffectBase {
        public override int Code => 3017;
        public override int ShowCount => -1;

        public Wind() { Duration = 1;}
        public override void OnTurnEnd(IEntity pTarget) {}

        public override void OnDeath(IEntity pTarget) {
            var effect = new Burn(new(-5));
            CharactersManager.Player.AddEffect(effect);
        }
    }
}