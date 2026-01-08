using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI {
    public static class MaterialStore {
        private static Dictionary<string, Material> _matchMaterials = new();

        [RuntimeInitializeOnLoadMethod]
        private static void Init() {
            if (_matchMaterials.Count != 0) {
                return;
            }

            _matchMaterials = Resources.LoadAll<Material>("Material")
                .Where(mat => mat.name.StartsWith("M_"))
                .ToDictionary(mat => mat.name[2..], material => material);
        }

        public static Material Get(string pMaterial) {
            return _matchMaterials.GetValueOrDefault(pMaterial);
        }
    }
}