using System.Collections.Generic;
using Data.Effect;
using Data.EnemyAppearance;
using UnityEngine;

namespace Data {
    public static class DataManager {

        public const int ERROR_SYMBOL = -1;
        public const int EMPTY_SYMBOL = 0;
        
        public static IQueryDB<int, SymbolData, SymbolQueryArgs> Symbol { get; private set; }
        public static IDB<int, EnemyData> Enemy { get; private set; }
        public static ILevelUpAppearanceDB LevelUp { get; private set; } = null;
        public static IDB<int, EffectData> Effect { get; private set; }
        private static IDB<int, List<List<int>>> _enemyAppearance;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void SetSymbolDB() {
            
            
#if UNITY_EDITOR
            Symbol = new SymbolDB();
            Enemy = new DictionaryBaseDB<EnemyData>();
            Effect = new DictionaryBaseDB<EffectData>();
            _enemyAppearance = new EnemyAppearanceDB();
            
            LevelUp = new LevelUpAppearanceDB(new SSLevelUpAppearanceLoader());
            
            Symbol.LoadData(new SpreadSheetLoader<SymbolData>("Symbol"));
            Enemy.LoadData(new SpreadSheetLoader<EnemyData>("Enemy"));
            Effect.LoadData(new SpreadSheetLoader<EffectData>("Effect"));
            _enemyAppearance.LoadData(new SpreadSheetEnemyAppearanceLoader());
            
#else
            Symbol = new SymbolDB();
            Enemy = new DictionaryBaseDB<EnemyData>();
            Effect = new DictionaryBaseDB<EffectData>();
            _enemyAppearance = new EnemyAppearanceDB();
            
            LevelUp = new LevelUpAppearanceDB(new CSVLevelUpAppearanceLoader());
            
            Symbol.LoadData(new CSVLoader<SymbolData>("Symbol"));
            Enemy.LoadData(new CSVLoader<EnemyData>("Enemy"));
            Effect.LoadData(new CSVLoader<EffectData>("Effect"));
            _enemyAppearance.LoadData(new CSVEnemyAppearanceLoader());
#endif
        }

        public static List<int> GetStageEnemy(int chapter) {
            var candidates = _enemyAppearance.GetData(chapter);
            var idx = Random.Range(0, candidates.Count);
            return candidates[idx];
        }
    }
}