using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Character.Skill.Data;
using CSVData.Extensions;
using Data;
using Extension;

namespace Character.Skill {
    public class GiveEffect: SkillBase {
        
        [SkillParameter]
        public TargetValue Target { get; protected set; }
        [SkillParameter]
        public string Effect { get; protected set; }

        private const string PATTERN = @"(?<Skill>\w*)\{(?<Args>.*)\}";

        public GiveEffect(string[] pArgs) : base(pArgs) {}

        protected override void Implement(Positions pCaster) {

            var match = Regex.Match(Effect, PATTERN);
            var effectType = Type.GetType("Data." + match.Groups["Skill"].Value)!;
            var rawArgs = match.Groups["Args"].Value.Split();
            var constructor = effectType.GetConstructors()
                .First(constructor => constructor.GetParameters().Length == rawArgs.Length);
            var args = rawArgs.Zip(constructor.GetParameters(),
                (value, type) => CSharpExtension.Parse(type.ParameterType, value)
            ).ToArray();
            
            foreach (var target in CharactersManager.GetEntities(pCaster, Target.Value)) {
                var effect = constructor!.Invoke(args) as EffectBase;
                target.AddEffect(effect);
            }
            
            
            IsEnd = true;
        }

    }
}