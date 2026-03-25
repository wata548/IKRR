using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Extension {
    
    [RequireComponent(typeof(AudioSource))]
    public class Audio: MonoBehaviour {
        
       //==================================================||Fields 
        private AudioSource _source;
        [SerializeField] private AudioClip _clip;
        private float _timeScale;
        
       //==================================================||Properties
       public AudioSource Source => _source;

       //==================================================||Methods 
        public void PlayOneShot() =>
            _source.PlayOneShot(_clip);

        public void Play() {
            _source.clip = _clip;
            _source.Play();
        }

        public void Stop() => 
            _source.Stop();

        public void ChangeClip(AudioClip pClip)
            => _clip = pClip;
        
        public void SetUp() {
            _source = GetComponent<AudioSource>();
        }
       //==================================================||Unity 
       private void Awake() {
           SetUp();
       }
    }
}