using Character;
using Character.Skill.Data;

namespace Data {
    public class Eat: EffectBase {
        public override int Code => 3008;
        public Eat(RangeValue pDuration) : base(pDuration){}

        public override void OnKill(IEntity pDead) {
            CharactersManager.Player.Heal(5, null);
        }
    }
}