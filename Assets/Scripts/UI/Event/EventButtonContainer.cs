using System.Collections.Generic;
using Data.Event;
using Extension;
using UnityEngine;
using UnityEngine.InputSystem.Composites;

namespace UI.Event {
    public class EventButtonContainer: MonoBehaviour {
        [SerializeField] private EventButton _prefab;
        [SerializeField] private Vector2Int _tableSize;
        private List<EventButton> _buttons = new();
        private RectTransform _rect;

        public void SetData(SingleScript pScript) {
            var buttons = pScript.Buttons;
            _rect.Place(_buttons, new(
                Vector2.zero,
                buttons.Length,
                _tableSize,
                _prefab,
                null
            ));
            var idx = -1;
            foreach (var button in _buttons) {
                idx ++;
                button.SetData(buttons[idx]);
            }
        }

        public void Clear() {
            foreach (var button in _buttons) {
                Destroy(button.gameObject);
            }
            _buttons.Clear();
        }

        private void Awake() {
            _rect = transform as RectTransform;
        }
    }
}