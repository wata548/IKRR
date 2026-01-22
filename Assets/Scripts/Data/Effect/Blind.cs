using Character;
using Character.Skill.Data;
using UnityEngine;

namespace Data {
    public class Blind: EffectBase {
        public Blind(RangeValue pDuration) : base(pDuration) {}
        public override int OnSendDamage(int pAmount, IEntity pTarget) {
            var value = Random.Range(0f, 1f);
            return pAmount * (value > 0.3f ? 1 : 0);
        }

        public override Info GetInfo() =>
            new Info(
                "실명",
                new() {("정보", "{0}턴, 30%의 확률로 공격이 빗나갑니다.", new object[]{Duration})}
            );
    }
}