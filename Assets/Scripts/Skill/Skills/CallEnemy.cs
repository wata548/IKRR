using Character.Skill.Data;
using Data;

namespace Character.Skill {
    public class CallEnemy: SkillBase {
        
        [SkillParameter]
        public int Enemy { get; private set; }
        [SkillParameter]
        public TargetValue Pos { get; private set; }
        
        public CallEnemy(string[] pData) : base(pData) {}
        protected override void Implement(Positions pCaster) {
            foreach (var entity in CharactersManager.GetEntities(pCaster, Pos.Value)) {
                if(!entity.IsAlive)
                    CharactersManager.SetEnemy(Enemy, Pos.Value);
            }
            End();
        }

        public override string ToString() => "동료를 불러냅니다.";
    }
}