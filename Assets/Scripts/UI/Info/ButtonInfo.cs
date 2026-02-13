using Data;
using UnityEngine;

namespace UI {
    public class ButtonInfo: ShowInfo {
        [SerializeField] private InfoSO _info;

        protected override Info Info() => _info;
    }
}