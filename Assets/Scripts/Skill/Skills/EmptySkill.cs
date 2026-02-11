using Character.Skill;
using Data;

namespace Skill.Skills {
    public class EmptySkill: SkillBase {
        protected override void Implement(Positions pCaster) {
            End();
        }
    }
}