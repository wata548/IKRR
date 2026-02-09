using System.IO;
using Data;
using Extension.Scene;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Title {
    public class TitleButtons: MonoBehaviour {
        [SerializeField] private Button _start;
        [SerializeField] private Button _continue;
        [SerializeField] private Button _setting;
        [SerializeField] private Button _quit;

        private void StartGame() {
            SaveFile.Start();
            SceneManager.LoadScene(Scene.Main);
        }

        private void Continue() {
            var path = Path.Combine(Application.streamingAssetsPath, "Save.json");
            SaveFile.Load(path);
            SceneManager.LoadScene(Scene.Main);
        }
        
        private void Awake() {
            var savePath = Path.Combine(Application.streamingAssetsPath, "Save.json");
            if (string.IsNullOrWhiteSpace(File.ReadAllText(savePath))) {
                _continue.interactable = false;
            }
                
            _continue.onClick.AddListener(Continue);
            _start.onClick.AddListener(StartGame);
            _quit.onClick.AddListener(Application.Quit);
        } 
    }
}