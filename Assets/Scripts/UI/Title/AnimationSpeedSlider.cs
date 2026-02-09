using UnityEngine;
using UnityEngine.UI;

namespace UI.Title {
    public class AnimationSpeedSlider: SettingSlider {
        protected override void Show(float pValue) {
            var temp = (int)pValue;
            Time.timeScale = temp;
            _shower.text = string.Format(_format, temp);
        }

        protected override void OnAwake(Slider pSlider) {
            pSlider.value = Time.timeScale;
        }
    }
}