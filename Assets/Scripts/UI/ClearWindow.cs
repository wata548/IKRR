using Extension.Scene;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class ClearWindow: MonoBehaviour {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _title;
        [SerializeField] private Button _exit;

        public void Show() {
            _panel.SetActive(true);
        }

        private void Awake() {
            _title.onClick.AddListener(() => SceneManager.LoadScene(Scene.Title));
            _exit.onClick.AddListener(Application.Quit);
        }
    }
}