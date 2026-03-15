using UnityEngine;
using UnityEngine.UI;

namespace UI.Title {
    public class AnimationSpeedSlider: SettingSlider {
        protected override void Show(float pValue) {
            if (Mathf.Approximately(Time.timeScale, 0.3f))
                return;
            var temp = (int)pValue;
            Time.timeScale = temp;
            _shower.text = string.Format(_format, temp);
        }

        protected override void OnAwake(Slider pSlider) {
            pSlider.value = Time.timeScale;
        }
    }
}