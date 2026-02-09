using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Title {
    [RequireComponent(typeof(Slider))]
    public abstract class SettingSlider: MonoBehaviour {
        [SerializeField] protected TMP_Text _shower;
        [SerializeField] protected string _format;

        protected abstract void Show(float pValue);
        protected abstract void OnAwake(Slider pSlider);
        
        private void Awake() {
            var slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener(Show);
            OnAwake(slider);
        }
    }
}