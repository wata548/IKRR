using Symbol;
using UnityEngine;

namespace Data {
    public static class DataManager {

        public const int ERROR_SYMBOL = -1;
        
        public static readonly IDB<int, SymbolData> SymbolDB = new SymbolDB();
        public static readonly IDB<int, EnemyData> EnemyDB = new EnemyDB();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void SetSymbolDB() {
            SymbolDB.LoadData(new SpreadSheetSymbolLoader());
            EnemyDB.LoadData(new SpreadSheetEnemyLoader());
        } 
    }
}