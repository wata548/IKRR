using Data;
using Lang;
using Roulette;

namespace Character.Skill {
    public class SetSymbolStatus: SkillBase {

        [SkillParameter] public int Column { get; private set; }
        [SkillParameter] public int Row { get; private set; }
        [SkillParameter] public CellStatus Status { get; private set; } = CellStatus.ForceUnavailable;

        public SetSymbolStatus(string[] args): base(args){}
        
        protected override void Implement(Positions pCaster) {
            RouletteManager.SetStatus(Column, Row, Status);
            End();
        }
    }
}