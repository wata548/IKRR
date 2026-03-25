using System.Collections.Generic;
using System.Linq;
using Extension;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

namespace Data {
    public static class VFXManager {

        private static IReadOnlyDictionary<string, VFXPool> _vfxs;
        private static Transform _folder;
        
        public static void SetUp() {
            _vfxs = Resources.LoadAll<VisualEffect>("VFX")
                .ToDictionary(vfx => vfx.name, vfx => new VFXPool(vfx));
            _folder = new GameObject("VFX").transform;
        }

        public static VisualEffect Get(string pName) {

            _vfxs.TryGetValue(pName, out var pool);
            return pool?.Get(_folder);
        }

        public static void PlayWithEvent(this VisualEffect pVFX) {
            var startEvent = pVFX.GetComponent<VFXStartEvent>();
            if(startEvent)
                startEvent.OnPlay();
        }

        public static void ApplySize(this VisualEffect pVFX, EnemySize pSize) {
            pVFX.SetFloat("Scale", (int)pSize / 100f);
        }
    }

    public class VFXPool {

        private readonly VisualEffect _prefab;
        private readonly List<VisualEffect> _pool = new();

        public VFXPool(VisualEffect pVFX) => _prefab = pVFX;

        public VisualEffect Get(Transform pParent) {
            var candidate = _pool.FirstOrDefault(vfx => vfx.aliveParticleCount == 0);
            if (candidate != null) {
                candidate.Reinit();
                return candidate;
            }

            var vfx = Object.Instantiate(_prefab);
            _pool.Add(vfx);
            return vfx;
        }
    }
}