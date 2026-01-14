using Data;
using Roulette;
using UI;
using UnityEngine.Scripting;

namespace Character.Skill {
    [Preserve]
    public class Change: SkillBase {
        
        [SkillParameter] public int Column { get; protected set; }
        [SkillParameter] public int Row { get; protected set; }
        [SkillParameter] public int Code { get; protected set; }
        [SkillParameter] public bool PlayAnimation { get; protected set; } = true;
        
        public Change(string[] pData) : base(pData) {}

        protected override void Implement(Positions pCaster) {
            RouletteManager.Change(Column, Row, Code);
            UIManager.Instance.Roulette.Evolve(Column, Row, Code, End, PlayAnimation);
        }
    }
}