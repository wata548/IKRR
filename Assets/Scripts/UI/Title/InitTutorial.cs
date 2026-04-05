using Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Title {
    [RequireComponent(typeof(Button))]
    public class InitTutorial:MonoBehaviour {
        private void Awake() {
            var button = GetComponent <Button>();
            button.onClick.AddListener(() => Tutorial.Tutorial.Instance.InitTutorial());
            UseInfo.Clear();
        }
    }
}