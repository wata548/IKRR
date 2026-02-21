using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Data {
    public class JobData {
        public int SerialNumber { get; protected set; }
        public string Name { get; protected set; }
        public string Desc { get; protected set; }
        public int MaxHp { get; protected set; }
        public int Money { get; protected set; }
        public string InitialEffects { get; protected set; }
        public TargetStatus TargetStatus { get; protected set; }
        public SymbolCategory MainCategory { get; protected set; }
        public string LevelUpString { get; protected set; }
        public string StartItemString { get; protected set; }
        
        [JsonIgnore]
        public IEnumerable<int> StartItem { get; protected set; }
        [JsonIgnore]
        public LevelUpReward LevelUpReward { get; protected set; }

        public IEnumerable<EffectBase> GetInitialEffects() =>
            InitialEffects.Split('\n').Select(EffectBase.Factory);
        
        public void Init() {
            StartItem = StartItemString.Split('\n').SelectMany(row => {
                var temp = row.Split('*');
                var target = int.Parse(temp[0]);
                var cnt = int.Parse(temp[1]);
                return Enumerable.Repeat(target, cnt);
            });
            LevelUpReward = new(LevelUpString);
        } 
    }

    public class LevelUpReward {
        public readonly int DefaultItem;
        public readonly int MaxLevelUpRewardLevel;
        public readonly int MaxLevelUpRewardTerm;
        public readonly int MaxLevelUpReward;
        private readonly IReadOnlyList<(int Level, int Reward)> _levelUpRewards;

        public int GetReward(int pLevel) {
            if (pLevel >= MaxLevelUpRewardLevel) {
                return (pLevel - MaxLevelUpRewardLevel) % MaxLevelUpRewardTerm == 0
                    ? MaxLevelUpReward
                    : DefaultItem;
            }

            //binary search
            var start = 0;
            var end = _levelUpRewards.Count - 1;
            while (start <= end) {
                var middle = (start + end) / 2;
                if (pLevel > _levelUpRewards[middle].Level)
                    start = middle + 1;
                else if (pLevel < _levelUpRewards[middle].Level)
                    end = middle - 1;
                else
                    return _levelUpRewards[middle].Reward;
            }
            return DefaultItem;
        }
        
        public LevelUpReward(string pString) {
            var rows = pString.Split('\n');
            DefaultItem = int.Parse(rows[0]);
            var maxLevelData = rows[1].Split('-');
            MaxLevelUpRewardLevel = int.Parse(maxLevelData[0]);
            MaxLevelUpRewardTerm = int.Parse(maxLevelData[1]);
            MaxLevelUpReward = int.Parse(maxLevelData[2]);
            _levelUpRewards = rows[2..].Select(row => {
                var temp = row.Split(':').Select(element => int.Parse(element)).ToArray();
                return (temp[0], temp[1]);
            }).ToList();
        }
    } 
}