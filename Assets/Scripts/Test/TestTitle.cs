using Extension.Scene;
using UnityEngine;

namespace Test {
    public class TestTitle: MonoBehaviour {
        public void StartGame() {
            SceneManager.LoadScene(Scene.Main);
        }
    }
}