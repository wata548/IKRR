using Character.Skill.Data;
using Data;

namespace Character.Skill {
    public class CallEnemy: SkillBase {
        
        [SkillParameter]
        public int Enemy { get; private set; }
        [SkillParameter]
        public TargetValue Pos { get; private set; }
        
        protected override void Implement(Positions pCaster) {
            if (CharactersManager.Exist(Pos.Value)) {
                End();
                return;
            }
            CharactersManager.SetEnemy(Enemy, Pos.Value);
            End();
        }
    }
}