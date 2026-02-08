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
        private static IDB<int, List<EventData>> _event;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void SetSymbolDB() {
            
            Symbol = new SymbolDB();
            Enemy = new DictionaryBaseDB<EnemyData>();
            Effect = new DictionaryBaseDB<EffectData>();
            _enemyAppearance = new DictionaryBaseDB<List<List<int>>>();
            _event = new DictionaryBaseDB<List<EventData>>();
                
#if UNITY_EDITOR
            LevelUp = new LevelUpAppearanceDB(new SSLevelUpAppearanceLoader());
            
            Symbol.LoadData(new SpreadSheetLoader<SymbolData>("Symbol"));
            Enemy.LoadData(new SpreadSheetLoader<EnemyData>("Enemy"));
            Effect.LoadData(new SpreadSheetLoader<EffectData>("Effect"));
            _enemyAppearance.LoadData(new SSGroupLoader<EnemyAppearanceInfo, List<int>>(
                "StageEnemy",
                info => info.Chapter,
                info => info.GetEnemies())
            );
            _event.LoadData(new SSGroupLoader<EventData, EventData>(
                "Event",
                data => data.Chapter
            ));
#else
            LevelUp = new LevelUpAppearanceDB(new CSVLevelUpAppearanceLoader());
            
            Symbol.LoadData(new CSVLoader<SymbolData>("Symbol"));
            Enemy.LoadData(new CSVLoader<EnemyData>("Enemy"));
            Effect.LoadData(new CSVLoader<EffectData>("Effect"));
            _enemyAppearance.LoadData(new CSVGroupLoader<EnemyAppearanceInfo, List<int>>(
                "StageEnemy",
                info => info.Chapter,
                info => info.GetEnemies())
            );
            _event.LoadData(new CSVGroupLoader<EventData, EventData>(
                "Event",
                data => data.Chapter
            ));
#endif
        }

        public static List<int> GetStageEnemy(int pChapter) {
            var candidates = _enemyAppearance.GetData(pChapter);
            var idx = Random.Range(0, candidates.Count);
            return candidates[idx];
        }

        public static EventData GetEvent(int pChapter) {
            var candidates = _event.GetData(pChapter);
            var idx = Random.Range(0, candidates.Count);
            return candidates[idx];
        }
    }
}