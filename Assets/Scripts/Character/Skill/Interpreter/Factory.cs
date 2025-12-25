using System;
using System.Text.RegularExpressions;

namespace Character.Skill {
    public static partial class SkillInterpreter {
        public static SkillBase Generate(string pInput) {
            
            var skillType = GetSkillType(pInput, out var parameters);
            var targetType = Type.GetType($"Character.Skill.{skillType}Skill");
            var temp = Activator.CreateInstance(targetType, new[] { parameters });
            
            return temp as SkillBase;
        }
        
        private static SkillType GetSkillType(string pCommandLine, out string[] pParameters) {
            const string pattern = @"(?<Command>.*)\((?<Params>.*)\)";
            var match = Regex.Match(pCommandLine, pattern);
            pParameters = match.Groups["Params"].Value.Split(',');
        
            return Enum.Parse<SkillType>(match.Groups["Command"].Value);
        }
    }
}