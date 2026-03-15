using UnityEngine;
using UnityEngine.UI;

namespace UI.Tutorial {
    public class Tutorial: MonoBehaviour {
        [SerializeField] private Image _focus;

        public void Focus(Vector2 pPos, Vector2 pSize) {
            _focus.rectTransform.position = pPos;
            _focus.rectTransform.localScale = pSize * 0.01f;
            _focus.gameObject.SetActive(true);
        }
    }
}