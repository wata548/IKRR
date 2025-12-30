using Symbol;
using UnityEngine;

namespace Data {
    public static class DataManager {

        public const int ERROR_SYMBOL = 9999;
        
        public static readonly IDB<ISymbolData> SymbolDB = new SymbolDB();
        public static readonly IDB<IEnemyData> EnemyDB = new EnemyDB();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void SetSymbolDB() {
            SymbolDB.LoadData(new SpreadSheetSymbolLoader());
            EnemyDB.LoadData(new SpreadSheetEnemyLoader());
        } 
    }
}