using System.IO;
using Data;
using Extension;
using Extension.Scene;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Title {
    public class TitleButtons: MonoBehaviour {
        [SerializeField] private Button _start;
        [SerializeField] private Button _continue;
        [SerializeField] private Button _setting;
        [SerializeField] private Button _quit;
        [SerializeField] private GameObject _jobPannel;
        
        private void StartGame() {
            _jobPannel.SetActive(true);
            Tutorial.Tutorial.Instance.Set("Job");
            return;
        }

        private void Continue() {
            var path = Path.Combine(Application.persistentDataPath, "Save.json");
            SaveSystem.Load(path);
            SceneManager.LoadScene(Scene.Main);
        }
        
        private void Awake() {
            var savePath = Path.Combine(Application.persistentDataPath, "Save.json");
            if (!File.Exists(savePath)) {
                File.Create(savePath);
                _continue.interactable = false;
            }
            else if (string.IsNullOrWhiteSpace(File.ReadAllText(savePath))) {
                _continue.interactable = false;
            }
                
            _continue.onClick.AddListener(Continue);
            _start.onClick.AddListener(StartGame);
            _quit.onClick.AddListener(Application.Quit);
        }

        private void Start() {
            BGMManager.Instance.Change("Lobby");
        }
    }
}