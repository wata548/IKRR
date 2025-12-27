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
                .Where(property => property.IsDefined(typeof(SkillParameterAttribute)))
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
}