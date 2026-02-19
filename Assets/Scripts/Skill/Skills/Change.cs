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
        protected virtual bool IsCreate => false;
        
        public Change(string[] pData) : base(pData) {}

        protected override void Implement(Positions pCaster) {
            if (!RouletteManager.Change(Column, Row, Code, IsCreate)) {
                End();
                return;
            }
            UIManager.Instance.Roulette.Evolve(Column, Row, Code, End, PlayAnimation);
        }
    }
}