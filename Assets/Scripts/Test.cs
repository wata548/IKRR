using System.Linq;
using Data;
using Extension;
using Extension.Test;
using UnityEngine;

namespace DefaultNamespace {
    public static class Test {
        [TestMethod]
        public static void Flag() {
            var target = Targets.AllEnemy;
            var result = string.Join(',', target.Split().Select(target => target.ToString()));
            Debug.Log(result);
        }
    }
}