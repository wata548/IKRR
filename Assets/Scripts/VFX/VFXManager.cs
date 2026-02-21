using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

namespace Data {
    public static class VFXManager {

        private static IReadOnlyDictionary<string, VFXPool> _vfxs;
        private static Transform _folder;
        
        [RuntimeInitializeOnLoadMethod]
        private static void SetUp() {
            _vfxs = Resources.LoadAll<VisualEffectAsset>("VFX")
                .ToDictionary(vfx => vfx.name, vfx => new VFXPool(vfx));
            _folder = new GameObject("VFX").transform;
        }

        public static VisualEffect Get(string pName) {

            _vfxs.TryGetValue(pName, out var pool);
            return pool?.Get(_folder);
        }

        public static void ApplySize(this VisualEffect pVFX, EnemySize pSize) {
            pVFX.SetFloat("Scale", (int)pSize / 100f);
        }
    }

    public class VFXPool {

        private readonly VisualEffectAsset _prefab;
        private readonly List<VisualEffect> _pool = new();

        public VFXPool(VisualEffectAsset pVFX) => _prefab = pVFX;

        public VisualEffect Get(Transform pParent) {
            var candidate = _pool.FirstOrDefault(vfx => vfx.aliveParticleCount == 0);
            if (candidate != null) 
                return candidate;
            
            var target = new GameObject();
            target.transform.parent = pParent;
            var vfx = target.AddComponent<VisualEffect>();
            vfx.visualEffectAsset = _prefab;
            _pool.Add(vfx);
            return vfx;
        }
    }
}