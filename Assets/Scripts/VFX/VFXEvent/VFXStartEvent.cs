using UnityEngine;
using UnityEngine.VFX;

namespace Extension {
    [RequireComponent(typeof(VisualEffect))]
    public abstract class VFXStartEvent: MonoBehaviour {
        public abstract void OnPlay();
    }
}