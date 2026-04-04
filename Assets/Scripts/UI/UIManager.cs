using Extension;
using UI.Character;
using UI.Event;
using UI.LevelUpReward;
using UI.Reward;
using UI.Shop;
using UI.SkillShower;
using UI.Status;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI {
    public class UIManager: MonoSingleton<UIManager> {
        
        protected override bool IsNarrowSingleton { get; } = true;
        [field: SerializeField] public Map.Map Map { get; private set; }
        [field: SerializeField] public Rest.Rest Rest { get; private set; }
        [field: SerializeField] public StatusShowerManager Status { get; private set; }
        [field: SerializeField] public SkillShowerManager SkillShower { get; private set; }
        [field: SerializeField] public Roulette.Roulette Roulette { get; private set; }
        [field: SerializeField] public EntityUIManager Entity { get; private set; }
        [field: SerializeField] public LevelUpRewardWindow LevelUp { get; private set; }
        [field: SerializeField] public TurnShower TurnShower { get; private set; }
        [field: SerializeField] public EventShower Event { get; private set; }
        [field: SerializeField] public SymbolSelector.SymbolSelector Selector { get; private set; }
        [field: SerializeField] public RewardWindow Reward { get; private set; }
        [field: SerializeField] public OXPannel OXEffect { get; private set; }
        [field: SerializeField] public ShopManager Shop { get; private set; }
        [field: SerializeField] public GameOver.GameOver GameOver  { get; private set; }
        [field: SerializeField] public ClearWindow ClearWindow  { get; private set; }
    }
}