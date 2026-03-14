using Data;

namespace Character.Skill {
    public class NextPhase: SkillBase {
        protected override void Implement(Positions pCaster) {
            var enemy = (CharactersManager.GetEntity(pCaster) as Enemy)!;
            enemy.Phase++;
            End();
        }
    }
}