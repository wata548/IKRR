using System;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Test {
    public class StatControl: MonoBehaviour {
        [SerializeField] private TargetStatus _stauts;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _up;
        [SerializeField] private Button _down;

        private void Refresh() {
            _text.text = $"{_stauts}: {Status.GetValue(_stauts)}";
        }

        private void Awake() {
            Refresh();
            _up.onClick.AddListener(() => Status.AddValue(_stauts, 1));
            _up.onClick.AddListener(Refresh);
            _down.onClick.AddListener(() => Status.AddValue(_stauts, -1));
            _down.onClick.AddListener(Refresh);
        }
    }
}