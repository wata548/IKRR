using Lang;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class InfoShowerOption: MonoBehaviour {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_LangText _option;

        public void Set(string pContext) =>
            _option.text = pContext;
        
        public void SetActive(bool pActive) {
            _image.color = pActive ? Color.white : Color.clear;
        }
    }
}