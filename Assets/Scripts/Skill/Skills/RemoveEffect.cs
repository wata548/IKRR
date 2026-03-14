using Character.Skill.Data;
using Lang;
using Data;

namespace Character.Skill {
    public class RemoveEffect: SkillBase {
        
        [SkillParameter]
        public TargetValue Target { get; private set; }
        [SkillParameter]
        public int Effect { get; private set; }
        
        protected override void Implement(Positions pCaster) {
            foreach (var entity in CharactersManager.GetEntities(pCaster, Target.Value)) {
                entity.RemoveEffect(Effect);
            }
            End();
        }

        public override string ToString() =>
            string.Format("{0}의 {1}효과를 제거한다.".ApplyLang(),
                Target.Value.ToRuntimeLanguage(),
                DataManager.Effect.GetData(Effect).Name.ApplyLang()
            );
    }

    public class ClearEffect : SkillBase {
         [SkillParameter]
         public TargetValue Target { get; private set; }
         
        protected override void Implement(Positions pCaster) {
            foreach (var entity in CharactersManager.GetEntities(pCaster, Target.Value)) {
                entity.ClearEffect();
            }
            End();
        }
        
        public override string ToString() =>
            string.Format("{0}의 효과를 전부 제거한다.".ApplyLang(),
                Target.Value.ToRuntimeLanguage()
            );
    }
}