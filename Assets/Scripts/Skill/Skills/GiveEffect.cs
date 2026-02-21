using System;
using System.Linq;
using System.Text.RegularExpressions;
using Character.Skill.Data;
using CSVData.Extensions;
using Data;

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

    }
}