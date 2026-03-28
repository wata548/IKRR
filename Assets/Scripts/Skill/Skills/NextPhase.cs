using Data;

namespace Character.Skill {
    public class NextPhase: SkillBase {
        public NextPhase(string[] pData) : base(pData){}
        protected override void Implement(Positions pCaster) {
            var enemy = (CharactersManager.GetEntity(pCaster) as Enemy)!;
            enemy.Phase++;
            End();
        }
    }
}