using System;
using FSM;
using TMPro;
using UnityEngine;

namespace Test {
    public class ShowFsm: MonoBehaviour {
        [SerializeField] private TMP_Text _shower;

        private void Update() {
            _shower.text = $"State: {Fsm.Instance.State}";
        }
    }
}