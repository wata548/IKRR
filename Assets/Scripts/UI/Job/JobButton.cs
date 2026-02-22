using UI.Icon;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Job {
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Button))]
    public class JobButton: MonoBehaviour {
        public static int SelectedJob { get; private set; } = 0;

        private int _code;
        private Image _shower;
        private Button _button;

        public void Set(int pCode) {
            if (SelectedJob == 0)
                SelectedJob = pCode;
            _code = pCode;

            _shower ??= GetComponent<Image>();
            _shower.sprite = _code.GetIcon();
            if (_button is not  null)
                return;
            
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => SelectedJob = pCode);
        }
        
    }
}