using Extension;
using UI.SkillShower;
using UI.Status;
using UnityEngine;

namespace UI {
    public class UIManager: MonoSingleton<UIManager> {
        protected override bool IsNarrowSingleton { get; } = true;
        [field: SerializeField] public StatusShowerManager Status { get; private set; }
        [field: SerializeField] public SkillShowerManager SkillShower { get; private set; }
        [field: SerializeField] public Roulette.Roulette Roulette { get; private set; }
    }
}