using UnityEngine;

namespace Extension.VFXEvent {
    
    [RequireComponent(typeof(Audio))]
    public class PlayVFXSound: VFXStartEvent {

        private Audio _audio;
        
        public override void OnPlay() {
            _audio.PlayOneShot();
        }

        private void Awake() {
            _audio = GetComponent<Audio>();
        }
    }
}