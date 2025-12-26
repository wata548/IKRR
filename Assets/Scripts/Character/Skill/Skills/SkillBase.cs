using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Character.Skill.Data;
using Data;
using Extension;
using Extension.Test;
using UnityEngine;

namespace Character.Skill {

    public interface ISkill {
        
        public bool IsEnd { get;}
        public Action OnEnd { get; set; }
        
        //==================================================||Methods 
        public void Execute(Positions pCaster);
    }
    
    public abstract class SkillBase: ISkill {
        
        //==================================================||Properties 
        public bool IsEnd { get; private set; } = false;
        public Action OnEnd { get; set; } = null;

        //==================================================||Constructors 
        protected SkillBase(string[] pData) {
            const BindingFlags flags = 
                BindingFlags.Instance 
                | BindingFlags.Public 
                | BindingFlags.FlattenHierarchy;

            var targetType = GetType();
            var properties = targetType.GetProperties(flags)
                .Where(property => !property.IsDefined(typeof(SkillParameterAttribute)))
                .ToList();

            for (int i = 0; i < pData.Length; i++) {
                var propertyType = properties[i].PropertyType;
                var value = ExParse.ParseToObject(propertyType, pData[i]);
                properties[i].SetValue(this, value);
            }
        }
        
        //==================================================||Methods 
        protected abstract void Implement(Positions pCaster);

        public void Execute(Positions pCaster) {
            Implement(pCaster);
            ExRoutine.StartRoutine(Wait());
            return;
            
            IEnumerator Wait() {
                yield return new WaitUntil(() => IsEnd);
                OnEnd?.Invoke();
            }
        }
    }

    public abstract class PlayerTargetSkillBase : SkillBase {
        
        [SkillParameter]
        public abstract RangeValue Value { get; protected set; }
        protected PlayerTargetSkillBase(string[] pData): base(pData){}
    }
    public abstract class SharedSkillBase: PlayerTargetSkillBase {
        
        [SkillParameter]
        public TargetValue Target { get; protected set; } = default;
        protected SharedSkillBase(string[] pData): base(pData){}
    }
    
    public class SkillComposite: ISkill {

        public bool IsEnd { get; private set; } = true;
        public Action OnEnd { get; set; }
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
        
        public void Execute(Positions pCaster) {

            IsEnd = false;
            ISkill prevSkill = null;
            foreach (var skill in _containner) {
                for (int i = 0; i < RepeatCount; i++) {
                    
                    if (prevSkill != null) {
                        prevSkill.OnEnd = () => skill.Execute(pCaster);
                    }
                    else
                        skill.Execute(pCaster);

                    prevSkill = skill;
                }
            }

            if(prevSkill != null)
                prevSkill.OnEnd = OnEnd;
        }
    }
}