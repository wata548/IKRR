using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Extension.StaticUpdate {
    public class StaticUpdateHelper: MonoBehaviour {
        private IEnumerable<MethodInfo> _targetMethods;
        
        [RuntimeInitializeOnLoadMethod]
        private static void Init() {
            var helperObj = new GameObject("StaticUpdateHelper");
            helperObj.AddComponent<StaticUpdateHelper>();
            DontDestroyOnLoad(helperObj);
        }

        private void Awake() {

            var flag = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            _targetMethods = Assembly.GetExecutingAssembly()
                .GetTypes()
                .SelectMany(type =>
                    type.GetMethods(flag).Where(method => method.IsDefined(typeof(StaticUpdateAttribute)))
                );
        }

        private void Update() {
            foreach (var method in _targetMethods) {
                method.Invoke(method.DeclaringType, new object[]{ });
            }
        }
    }
}