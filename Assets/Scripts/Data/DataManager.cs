using System.Collections.Generic;
using Data.EnemyAppearance;
using UnityEngine;

namespace Data {
    public static class DataManager {

        public const int ERROR_SYMBOL = -1;
        public const int EMPTY_SYMBOL = 0;
        
        public static readonly IDB<int, SymbolData> Symbol = new SymbolDB();
        public static readonly IDB<int, EnemyData> Enemy = new EnemyDB();
        private static readonly IDB<int, List<List<int>>> _enemyAppearance = new EnemyAppearanceDB();
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void SetSymbolDB() {
#if UNITY_EDITOR
            Symbol.LoadData(new SpreadSheetSymbolLoader());
            Enemy.LoadData(new SpreadSheetEnemyLoader());
            _enemyAppearance.LoadData(new SpreadSheetEnemyAppearanceLoader());
#else
            SymbolDB.LoadData(new CSVSymbolLoader());
            EnemyDB.LoadData(new CSVEnemyLoader());
            EnemyAppearance.LoadData(new CSVEnemyAppearanceLoader());
#endif
        }

        public static List<int> GetStageEnemy(int chapter) {
            var candidates = _enemyAppearance.GetData(chapter);
            var idx = Random.Range(0, candidates.Count);
            return candidates[idx];
        }
    }
}