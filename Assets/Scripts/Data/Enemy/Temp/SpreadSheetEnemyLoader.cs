#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using CSVData;

namespace Data {

    public class SpreadSheetEnemyLoader: IDataLoader<int, EnemyData> {
        private readonly string _apiKey = "AIzaSyD9TQEfDm8OY3DtfdvZayoZi96QHLl_SA0";
        private readonly string _path = "1BLW__Va3NhcIJzwd6oblRmFck1DOADGoB3fww_RGqpo";
        private readonly string _sheet = "Enemy";

        public SpreadSheetEnemyLoader() {}
        
        public SpreadSheetEnemyLoader(string api, string path, string sheet) =>
            (_apiKey, _path, _sheet) = (api, path, sheet);
        
        public IEnumerable<KeyValuePair<int, EnemyData>> Load() {
            var targetType = typeof(EnemyData);
            return (Dictionary<int, EnemyData>)CSV.DeserializeToDictionaryBySpreadSheet(targetType, _path, _sheet, _apiKey, out _);
        }
    }
}
#endif