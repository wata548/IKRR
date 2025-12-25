using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Character.Skill.Data;
using Data;
using Extension.Test;

namespace Character.Skill {
    public enum SkillType {
        Attack, 
        Defence,
        Heal,
        Decay,
        Poison,
        Needle,
        Wound,
        Confuse,
        
        Steal,
        Spark,
        Fade,
        Limit,
        AddSymbol,
        RemoveSymbol,
    }

    public interface ISkill {
        public string Name { get;}
        public void Execute(Targets pCaster, int pRepeatCount = 1, int pRepeatIdx = 1);
    }
    
    public abstract class SkillBase: ISkill {
        public abstract string Name { get; protected set; }
        public abstract void Execute(Targets pCaster, int pRepeatCount = 1, int pRepeatIdx = 1);

        [IgnoreSkillParameter]
        public abstract SkillType SkillType { get; protected set; }
        public RangeValue ContinuesTime { get; protected set; } = new(1);

        public void TurnUpdate(){}

        protected SkillBase(string[] pData) {
            const BindingFlags flags = 
                BindingFlags.Instance 
                | BindingFlags.Public 
                | BindingFlags.FlattenHierarchy;

            var targetType = GetType();
            var properties = targetType.GetProperties(flags)
                .Where(property => !property.IsDefined(typeof(IgnoreSkillParameterAttribute)))
                .ToList();

            for (int i = 0; i < pData.Length; i++) {
                var propertyType = properties[i].PropertyType;
                var value = ExParse.ParseToObject(propertyType, pData[i]);
                properties[i].SetValue(this, value);
            }
        }
    }

    public abstract class PlayerTargetSkillBase : SkillBase {
        
        public RangeValue Value { get; protected set; } = default;
        protected PlayerTargetSkillBase(string[] pData): base(pData){}
    }
    public abstract class SharedSkillBase: PlayerTargetSkillBase {
        public TargetValue Target { get; protected set; } = default;
        protected SharedSkillBase(string[] pData): base(pData){}
    }
    
    public class SkillComposite: ISkill {
        public string Name {
            get => _containner[0].Name;
        }

        public int RepeatCount { get; private set; }
        private List<ISkill> _containner = new();

        public void SetRepeatCount(int pRepeatCount) =>
            RepeatCount = pRepeatCount;

        public void AddSkill(ISkill pTarget) =>
            _containner.Add(pTarget);
        
        public void AddSkills(IEnumerable<ISkill> pTargets) =>
            _containner.AddRange(pTargets);

        public SkillComposite(int pRepeatCount = 1) {
            RepeatCount = pRepeatCount;
        }

        public SkillComposite(params ISkill[] pContent) =>
            (RepeatCount, _containner) = (1, pContent.ToList());
        public SkillComposite(int pRepeatAmount, params ISkill[] pContent) {
            RepeatCount = pRepeatAmount;
            _containner = pContent.ToList();
        }
        
        public void Execute(Targets pCaster, int pRepeatCount = 1, int pRepeatIdx = 1) {

            foreach (var skill in _containner) {
                for (int i = 0; i < RepeatCount; i++) {
                    skill.Execute(pCaster, RepeatCount, i);
                }
            }
        }
    }
}