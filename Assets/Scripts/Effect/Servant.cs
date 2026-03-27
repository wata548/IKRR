using System.Collections.Generic;
using Character;
using Character.Skill.Data;

namespace Data {
    public class Servant: EffectBase {
        public override int Code => 3018;
        private Positions _pos;

        public Servant(Positions pPos) {
            _pos = pPos;
            Duration = 1;
        }

        public override int ShowCount => -1;
        public override void OnTurnEnd(IEntity pTarget) {}

        public override void OnTurnStart(IEntity pTarget) {
            if (!CharactersManager.GetEntity(_pos).IsAlive)
                pTarget.KillSelf();
        }

        public override Dictionary<string, object> Infos => new() {
            {
                "Pos", _pos.ToRuntimeLanguage()
            }
        };
    }
}