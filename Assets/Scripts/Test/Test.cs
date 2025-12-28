using System;
using System.Linq;
using System.Reflection;
using Character.Entity;
using Character.Skill;
using Data;
using Extension;
using Extension.Test;
using UnityEngine;

namespace Test {
    public static class Test {
        [TestMethod]
        public static void Flag() {
            var target = Positions.AllEnemy;
            var result = string.Join(',', target.Split().Select(target => target.ToString()));
            Debug.Log(result);
        }

        [TestMethod]
        public static void SkillParameter(string pTargetSkillName) {
            const BindingFlags flags = 
                BindingFlags.Instance 
                | BindingFlags.Public 
                | BindingFlags.FlattenHierarchy;
            
            var targetType = Type.GetType($"Character.Skill.{pTargetSkillName}");
            var properties = targetType.GetProperties(flags)
                .Where(property => property.IsDefined(typeof(SkillParameterAttribute)))
                .Select(property => $"{property.Name}: {property.PropertyType}");
            Debug.Log(string.Join("\n", properties));
        }

        [TestMethod]
        public static void SetSkill(string pSkillSet) {
            var obj = new GameObject("Enemy1");
            var enemy = obj.AddComponent<Enemy1>();
            enemy.SetSkillSet(pSkillSet);
        }
    }
}