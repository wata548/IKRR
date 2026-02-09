using Extension.Scene;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class RuntimeSettingButton: MonoBehaviour {
        
        [Header("ShowSetting")]
        [SerializeField] private Button _settingTrigger;
        [SerializeField] private GameObject _settingPannel;
        
        [Space, Header("OnSetting")]
        [SerializeField] private Button _quit;
        [SerializeField] private Button _title;

        private void ToTitle() => SceneManager.LoadScene(Scene.Title);
        private void Quit() => Application.Quit();

        private void ShowSwitch() {
            _settingPannel.SetActive(!_settingPannel.activeSelf);
        }
        
        private void Awake() {
            _settingTrigger.onClick.AddListener(ShowSwitch);
            _quit.onClick.AddListener(Quit);
            _title.onClick.AddListener(ToTitle);
        }
    }
}