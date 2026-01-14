using Data;
using Roulette;
using UnityEngine.Scripting;

namespace Character.Skill {
    [Preserve]
    public class Evolve: Change {
        public Evolve(string[] pData) : base(pData) {}
        protected override void Implement(Positions pCaster) {
            UseInfo.Evolve(RouletteManager.Get(Column, Row));
            base.Implement(pCaster);
        }
    }
}