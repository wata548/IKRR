using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSVData;
using UnityEngine;

namespace Data {
    public class CSVEnemyLoader: IDataLoader<int, EnemyData> {
        private readonly string _sheet = "Enemy.csv";
        public IEnumerable<KeyValuePair<int, EnemyData>> Load() {
            var targetType = typeof(CSVEnemyData);
            var path = Path.Combine(Application.streamingAssetsPath, "Datas", _sheet);
            var csv = CSV.Parse(File.ReadAllText(path));
            return ((List<CSVEnemyData>)CSV.DeserializeToList(targetType, csv))
                .Select(enemy => new KeyValuePair<int, EnemyData>(enemy.SerialNumber, enemy));
        }
    }
}