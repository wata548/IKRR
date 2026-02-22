using Data;
using Lang;
using UI.Icon;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Job {
    public class InGameJobShower: MonoBehaviour {
        [SerializeField] private Image _shower;
        [SerializeField] private TMP_LangText _name;

        private void Start() {
            _shower.sprite = PlayerData.Job.SerialNumber.GetIcon();
            _name.text = PlayerData.Job.Name;
        }
    }
}