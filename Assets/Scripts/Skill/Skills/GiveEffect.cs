using Character.Skill.Data;
using Data;
using Lang;

namespace Character.Skill {
    public class GiveEffect: SkillBase {
        
        [SkillParameter]
        public TargetValue Target { get; protected set; }
        [SkillParameter]
        public string Effect { get; protected set; }

        public GiveEffect(string[] pArgs) : base(pArgs) {}

        protected override void Implement(Positions pCaster) {
            EffectBase.Factory(Effect, pCaster, Target.Value);
            IsEnd = true;
        }

        public override string ToString() {
            string FORMAT = "{0}에게 상태 이상({1})";
            using (var effect = EffectBase.Factory(Effect)) {
                var data = DataManager.Effect.GetData(effect.Code);
                return string.Format(FORMAT.ApplyLang(), Target.Value.ToRuntimeLanguage(), data.Name.ApplyLang());
            }
        }
    }
}