using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Data {
    public interface ILevelUpAppearanceDB {
        public Rarity GetRarity();
    }

    public class LevelUpAppearanceDB : ILevelUpAppearanceDB {

        private List<LevelUpAppearance> _datas;
        
        public LevelUpAppearanceDB(IDataLoader<LevelUpAppearance> pLoader) {
            _datas = pLoader.Load().ToList();
        }
        
        public Rarity GetRarity() {
            var target = Random.Range(0f, 1f);
            return _datas.First(data => data.Appearance >= target).Rarity;
        }
    }
}