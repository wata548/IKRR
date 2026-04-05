using Data;
using Roulette;
using UI;
using UnityEngine.Scripting;

namespace Character.Skill {
    [Preserve]
    public class Evolve: Change {
        public Evolve(string[] pData) : base(pData) {}
        protected override void End() {
            UI.Tutorial.Tutorial.Instance.Set("Evolve");
            base.End();
        }

        protected override void Implement(Positions pCaster) {
            UseInfo.Evolve(RouletteManager.Get(Column, Row));
            base.Implement(pCaster);
        }
    }
}