using System;

namespace Character.Skill {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class SkillParameterAttribute : Attribute {
        
        public readonly int Priority = 0;
        
        public SkillParameterAttribute(){}

        public SkillParameterAttribute(int pPriority) =>
            Priority = pPriority;
    }
}