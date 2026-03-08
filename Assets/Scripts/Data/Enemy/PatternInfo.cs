using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Character;
using Character.Skill;
using LanguageEmbed;
using UnityEngine;

namespace Data {
    public class PatternInfo {
        
        //==================================================||Fields 
        private const string PATTERN = @"\s*(?<Skill>.+?)\s*=\s*(?<Appearance>\d+)";
        private static readonly ILanguageEmbed _executor = new LuaEmbed();
        private static string _format = null;
        
        public readonly string Condition;
        public readonly IReadOnlyList<(int Appearance, IEnumerable<ISkill> Sequence)> Skill;

        //==================================================||Constructors
        public PatternInfo(string pSkillSet) {
            var idx = pSkillSet.IndexOf('\n');
            Condition = pSkillSet[..idx];
            if (string.IsNullOrWhiteSpace(Condition))
                Condition = "return true;";
            var skillSet = pSkillSet[(idx + 1)..];
            var matches = Regex.Matches(skillSet, PATTERN);

            var data = new List<(int, IEnumerable<ISkill>)>();
            var prefixSum = 0;
            foreach (Match match in matches) {
                var appearance = int.Parse(match.Groups["Appearance"].Value);
                appearance = prefixSum += appearance;

                var sequence = match.Groups["Skill"].Value
                    .Split("->")
                    .Select(SkillInterpreter.Interpret);
                Debug.Log(sequence.Count());
                data.Add((appearance, sequence));
            }

            Skill = data;
        } 
        
        //==================================================||Methods 
        public bool Usable(Enemy pEnemy) {
            _format ??= File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "PatternFunc.txt"));
            var code = string.Format(_format, "Func", Condition);
            return LanguageExecutor.Executor.Invoke<bool>(code, "Func", new[]{pEnemy});
        }
        
        public IEnumerable<ISkill> GetSkill() {
            var point = UnityEngine.Random.Range(1, Skill[^1].Appearance + 1);
            var start = 0;
            var end = Skill.Count - 1;
            while (start < end) {
                var middle = (start + end) / 2;
                var compare = point.CompareTo(Skill[middle].Appearance);
                
                if (compare == 0) {
                    start = middle;
                    break;
                }
                if (compare > 0)
                    start = middle + 1;
                else
                    end = middle;
            }

            var skill = Skill[start].Sequence;
            return skill;
        }

    }
}