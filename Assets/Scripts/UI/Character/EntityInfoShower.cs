using Data;
using UnityEngine;

namespace UI.Character {
    public class EntityInfoShower: ShowInfo {

        [SerializeField] private EntityUI _ui;

        protected override Info Info() =>
            _ui.Info();
    }
}