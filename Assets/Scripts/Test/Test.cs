using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Character.Skill;
using Data;
using Data.Event;
using Extension;
using Extension.Test;
using Newtonsoft.Json;
using UnityEngine;

namespace Test {
    public static class Test {

        [TestMethod]
        public static void GetJobs() {
            Debug.Log(string.Join(',', DataManager.Job.Keys));
        }
        
        [TestMethod]
        public static void Paths() {
            Debug.Log($"Data: {Application.dataPath}"); 
            Debug.Log($"Persistent: {Application.persistentDataPath}"); 
        }
        
        [TestMethod]
        public static void SaveTest() {
            new SaveSystem().Save();
        }
        [TestMethod]
        public static void Parse(Positions pPos) {
            var target = CharactersManager.GetEntity(pPos);
            var setting = new JsonSerializerSettings();
            setting.TypeNameHandling = TypeNameHandling.All;
            var json = JsonConvert.SerializeObject(target, Formatting.None, setting);
            Debug.Log(json);
        }
        
        [TestMethod]
        public static void Match(string pContext) {
            const string PATTERN = @"@(?<Label>\d+):(?<Context>[^@]*)";
            var matches = Regex.Split(pContext, PATTERN);
        }
        
        [TestMethod]
        public static void EventScript(string pContext) {
            var a = new SingleScript(pContext);
            Debug.Log(a);
        }
        
        [TestMethod]
        public static void EventButton(string pContext) {
            var a = new Data.Event.Button(pContext);
            Debug.Log($"{a.Option}: {a.FuncContext}");
        } 
        
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