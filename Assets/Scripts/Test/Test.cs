using System;
using System.Linq;
using System.Reflection;
using Character.Skill;
using Data;
using Extension;
using Extension.Test;
using TMPro;
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
        public static void SymbolIcon(TargetStatus pTarget) {
            Debug.Log(string.Join(',', Resources.LoadAll($"Status").Select(sprite => sprite.name)));
            Debug.Log(Resources.Load<Sprite>($"Status/{pTarget}"));
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
        private static void ZipTest() {
            var a = new[] { 1, 2, 3, 4, 5 };
            var b = new[] { "a", "b", "c", "d" };
            var c = a.Zip(b, (n, m) => (n, m));
            foreach (var element in c) {
                Debug.Log($"{element.n} - {element.m}");
            }
        }
    }
}