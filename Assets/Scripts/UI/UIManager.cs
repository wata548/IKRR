using Extension;
using UI.Character;
using UI.Event;
using UI.LevelUpReward;
using UI.SkillShower;
using UI.Status;
using UnityEngine;

namespace UI {
    public class UIManager: MonoSingleton<UIManager> {
        
        protected override bool IsNarrowSingleton { get; } = true;
        [field: SerializeField] public Map.Map Map { get; private set; }
        [field: SerializeField] public Rest.Rest Rest { get; private set; }
        [field: SerializeField] public StatusShowerManager Status { get; private set; }
        [field: SerializeField] public SkillShowerManager SkillShower { get; private set; }
        [field: SerializeField] public Roulette.Roulette Roulette { get; private set; }
        [field: SerializeField] public EntityUIManager Entity { get; private set; }
        [field: SerializeField] public InfoShower InfoShower { get; private set; }
        [field: SerializeField] public LevelUpRewardWindow LevelUp { get; private set; }
        [field: SerializeField] public TurnShower TurnShower { get; private set; }
        [field: SerializeField] public EventShower Event { get; private set; }
    }
}