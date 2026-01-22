using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSVData;
using UnityEngine;

namespace Data.EnemyAppearance {
#if UNITY_EDITOR
    public class SpreadSheetEnemyAppearanceLoader: IDataLoader<int, List<List<int>>> {
        private readonly string _apiKey = "AIzaSyD9TQEfDm8OY3DtfdvZayoZi96QHLl_SA0";
        private readonly string _path = "1BLW__Va3NhcIJzwd6oblRmFck1DOADGoB3fww_RGqpo";
        private readonly string _sheet = "StageEnemy";
        
        public IEnumerable<KeyValuePair<int, List<List<int>>>> Load() {
            var targetType = typeof(EnemyAppearanceInfo);
            var data = (List<EnemyAppearanceInfo>)CSV.DeserializeToListBySpreadSheet(targetType, _path, _sheet, _apiKey);
            return data
                .GroupBy(info => info.Chapter)
                .Select(kvp => 
                    new KeyValuePair<int, List<List<int>>>(
                        kvp.Key, 
                        kvp.Select(kvp2 => kvp2.GetEnemies())
                            .ToList()
                    )
                );
        }
    }
#endif
    public class CSVEnemyAppearanceLoader: IDataLoader<int, List<List<int>>> {
        private readonly string _sheet = "StageEnemy.csv";
        public IEnumerable<KeyValuePair<int, List<List<int>>>> Load() {
            var targetType = typeof(EnemyAppearanceInfo);
            var path = Path.Combine(Application.streamingAssetsPath, "Datas", _sheet);
            var csv = CSV.Parse(File.ReadAllText(path));
            var data = (List<EnemyAppearanceInfo>)CSV.DeserializeToList(targetType, csv);
            return data
                .GroupBy(info => info.Chapter)
                .Select(kvp => 
                    new KeyValuePair<int, List<List<int>>>(
                        kvp.Key, 
                        kvp.Select(kvp2 => kvp2.GetEnemies())
                            .ToList()
                    )
                );
        }
    }
}