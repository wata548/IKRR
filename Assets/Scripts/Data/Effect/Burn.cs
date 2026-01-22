using System;
using Character;
using Character.Skill.Data;

namespace Data {
    public class Burn: EffectBase {
        
        public Burn(RangeValue pDuration) : base(pDuration) {}

        public override void OnTurnStart(IEntity pTarget) {
            pTarget.ReceiveDamage(Duration);
        }

        public override Info GetInfo() =>
            new Info(
                "그을림",
                new() {("정보", "{0}턴, {1}만큼 데미지를 받습니다.", new object[]{Duration, Duration * 2})}
            );
    }
}