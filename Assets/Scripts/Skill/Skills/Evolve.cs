using Data;
using Roulette;
using UI;

namespace Character.Skill {
    public class Evolve: SkillBase {
        
        [SkillParameter] public int Column { get; private set; }
        [SkillParameter] public int Row { get; private set; }
        [SkillParameter] public int Code { get; private set; }
        [SkillParameter] public bool PlayAnimation { get; private set; } = true;
        
        public Evolve(string[] pData) : base(pData) {}

        protected override void Implement(Positions pCaster) {
            RouletteManager.Change(Column, Row, Code);
            UIManager.Instance.Roulette.Evolve(Column, Row, Code, End, PlayAnimation);
        }
    }
}