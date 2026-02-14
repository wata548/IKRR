using System.Collections.Generic;
using Character;
using Character.Skill.Data;
using Roulette;

namespace Data {
    public class Bounty: EffectBase {

        public override int Code => 3007;
        public Bounty() {
            Duration = 2;
        }

        private int _cnt = 0; 
        
        public override int ShowCount => _cnt == 0 
            ? RouletteManager.UseSymbolCnt * 2 
            : _cnt;
        
        public override Dictionary<string, object> Infos => new() {
            { "Amount", ShowCount}
        };

        public override void OnSkillUse(IEntity pTarget) {
            Update();
        }

        public override void OnTurnEnd(IEntity pTarget) {
            _cnt = ShowCount;
        }

        public override void OnTurnStart(IEntity pTarget) {
            PlayerData.GetMoney(_cnt);
            Duration = 0;
        }
    }
}