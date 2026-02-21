using System.Collections.Generic;
using Character;
using Character.Skill.Data;

namespace Data {
    public class Support: EffectBase {
        public override int Code => 3010;
        public int Amount { get; private set; }

        public Support(int pAmount) =>
            Amount = pAmount;

        public override Dictionary<string, object> Infos => new() {
            { "Amount", Amount }
        };

        public override void OnTurnEnd(IEntity pTarget) {}

        public override void OnTurnStart(IEntity pTarget) {
            PlayerData.GetMoney(Amount);
        }
        public override void OnBattleStart(IEntity pTarget) {
            
        }
    }
}