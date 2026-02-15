using System.Collections;
using Character.Skill;
using Data;
using Extension;

namespace Skill.Skills {
    public class EmptySkill: SkillBase {
        protected override void Implement(Positions pCaster) {
            ExRoutine.Wait(0.2f, End);
        }
    }
}