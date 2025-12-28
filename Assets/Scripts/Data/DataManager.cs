using Symbol;
using UnityEngine;

namespace Data {
    public static class DataManager {
        public static readonly SymbolDataStore SymbolDB = new();

        [RuntimeInitializeOnLoadMethod]
        private static void SetSymbolDB() {
            SymbolDB.LoadData(new SpreadSheetLoader());
        } 
    }
}