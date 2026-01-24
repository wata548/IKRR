using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Data;
using Extension;
using Extension.Test;
using UnityEngine;

namespace Character.Skill {

    public abstract class SkillBase: ISkill {
        
        //==================================================||Properties 
        public bool IsEnd { get; protected set; } = false;
        public Action OnEnd { get; set; } = null;

        //==================================================||Constructors 
        protected SkillBase(){}
        protected SkillBase(string[] pData) {
            const BindingFlags flags = 
                BindingFlags.Instance 
                | BindingFlags.Public 
                | BindingFlags.FlattenHierarchy;

            var targetType = GetType();
            var properties = targetType.GetProperties(flags)
                .Where(property => property.IsDefined(typeof(SkillParameterAttribute)))
                .OrderBy(property => property.GetCustomAttribute<SkillParameterAttribute>()!.Priority)
                .ToList();

            for (int i = 0; i < pData.Length; i++) {
                var propertyType = properties[i].PropertyType;
                var value = ExParse.ParseToObject(propertyType, pData[i].Trim());
                properties[i].SetValue(this, value);
            }
        }
        
        //==================================================||Methods 
        protected abstract void Implement(Positions pCaster);

        public void Execute(Positions pCaster) {
            IsEnd = false;
            Implement(pCaster);
            ExRoutine.StartRoutine(Wait());
            return;
            
            IEnumerator Wait() {
                yield return new WaitUntil(() => IsEnd);
                OnEnd?.Invoke();
            }
        }

        protected void End() => IsEnd = true;
    }
}