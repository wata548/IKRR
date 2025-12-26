using System;

namespace Character.Skill {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class SkillParameterAttribute: Attribute {}
}