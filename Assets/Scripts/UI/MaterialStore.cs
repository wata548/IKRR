using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Extension.StaticUpdate;
using UnityEngine;

namespace UI {
    public static class MaterialStore {
        private static Dictionary<string, Material> _matchMaterials = new();
        private static float _lastTimeScale = 1; 
        
        [RuntimeInitializeOnLoadMethod]
        private static void Init() {
            if (_matchMaterials.Count != 0) {
                return;
            }

            _matchMaterials = Resources.LoadAll<Material>("Material")
                .Where(mat => mat.name.StartsWith("M_"))
                .ToDictionary(mat => mat.name[2..], material => material);
        }

        [StaticUpdate]
        public static void Update() {
            if (Mathf.Approximately(_lastTimeScale, Time.timeScale))
                return;

            var cur = Time.timeScale;
            foreach (var (_, mat) in _matchMaterials) {
                mat.SetFloat("_TimeScale", cur);
            }
            _lastTimeScale = cur;
        }
        
        public static Material Get(string pMaterial) {
            return _matchMaterials.GetValueOrDefault(pMaterial);
        }
    }
}