using UnityEngine;
using UnityEngine.UI;

namespace UI.Title {
    public class SoundSlider: SettingSlider {
        public static float Sound { get; private set; } = 1f;


        protected override void OnAwake(Slider pSlider) {
            pSlider.value = Sound;
        }
        protected override void Show(float pValue) {
            Sound = pValue;
            _shower.text = string.Format(_format, Mathf.FloorToInt(pValue * 100f));
        }
    }
}