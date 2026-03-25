using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI.Title {
    public class SoundSlider: SettingSlider {
        public static float Sound { get; private set; } = 1f;
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private string _tag;


        protected override void OnAwake(Slider pSlider) {
            pSlider.value = Sound;
        }
        protected override void OnValueChanged(float pValue) {
            Sound = pValue;
            var value = Mathf.FloorToInt(pValue * 100f);
            _shower.text = string.Format(_format, value);
            _mixer.SetFloat(_tag, 30 * Mathf.Log10(value / 100f));
        }
    }
}