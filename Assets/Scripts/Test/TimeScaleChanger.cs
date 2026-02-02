using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Test {
    public class TimeScaleChanger: MonoBehaviour {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _button;
        [SerializeField] private float _defaultScale = 1;

        private void Change() =>
            Time.timeScale = float.Parse(_inputField.text);
        
        private void Awake() {
            Time.timeScale = _defaultScale;
            _inputField.text = _defaultScale.ToString();
            _button.onClick.AddListener(Change);
        } 
    }
}