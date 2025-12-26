using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Extension {

    
    public static class ExRoutine {
        
        private class RoutinePlayer : MonoBehaviour {}
       //==================================================||Fields 
        private static RoutinePlayer _routinePlayer = null;

       //==================================================||Methods 
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize() {
            var newObject = new GameObject("RoutinePlayer");
            _routinePlayer = newObject.AddComponent<RoutinePlayer>();
            Object.DontDestroyOnLoad(newObject);
        }
        
        public static void StartRoutine(IEnumerator pContent) {
            _routinePlayer.StartCoroutine(pContent);
        } 
    }
}