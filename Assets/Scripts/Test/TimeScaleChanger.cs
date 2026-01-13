using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Test {
    public class TimeScaleChanger: MonoBehaviour {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _button;

        private void Change() =>
            Time.timeScale = float.Parse(_inputField.text);
        
        private void Awake() {
            _button.onClick.AddListener(Change);
        } 
    }
}