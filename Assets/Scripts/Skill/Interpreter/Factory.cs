using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Character.Skill {
    public static partial class SkillInterpreter {
        //except composite skill, just consider single skill
        private static SkillBase Generate(string pInput) {
            
            var skillType = GetSkillType(pInput, out var parameters);
            var temp = Activator.CreateInstance(skillType, new[] { parameters });
            
            return temp as SkillBase;
        }
        
        private static Type GetSkillType(string pCommandLine, out string[] pParameters) {
            const string pattern = @"(?<Command>.*)\((?<Params>.*)\)";
            var match = Regex.Match(pCommandLine, pattern);
            pParameters = match.Groups["Params"].Value
                .Split(',')
                .Select(param => param.Trim())
                .ToArray();
            return Type.GetType($"Character.Skill.{match.Groups["Command"].Value}");
        }
    }
}