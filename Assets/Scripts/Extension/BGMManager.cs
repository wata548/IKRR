using System.Collections.Generic;
using Extension.Test;
using UnityEngine;

namespace Extension {
    
    [RequireComponent(typeof(Audio))]
    public class BGMManager: MonoSingleton<BGMManager> {
        protected override bool IsNarrowSingleton => false;
        [SerializeField] private List<SerializableKVP<string, AudioClip>> _kvps;
        private Audio _audio;
        private Dictionary<string, AudioClip> _matches;
        private string _bgm;
        
        public void Change(string pTitle) {
            if (pTitle == _bgm)
                return;
            
            _audio.Source.Stop();
            _audio.ChangeClip(_matches.GetValueOrDefault(pTitle));
            _audio.Play();
            _bgm = pTitle;
        }

        protected void Start() {
            _audio = GetComponent<Audio>();
            _matches = _kvps.ToDictionary();
            _kvps.Clear();

            _audio.SetUp();
            _audio.Source.loop = true;
        }
    }
}