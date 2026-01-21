using System.Collections.Generic;
using System.IO;
using CSVData;
using UnityEngine;

namespace Data {
#if UNITY_EDITOR
    public class SSLevelUpAppearanceLoader: IDataLoader<LevelUpAppearance> {
        private readonly string _apiKey = "AIzaSyD9TQEfDm8OY3DtfdvZayoZi96QHLl_SA0";
        private readonly string _path = "1BLW__Va3NhcIJzwd6oblRmFck1DOADGoB3fww_RGqpo";
        private readonly string _sheet = "LevelUpAppearance";

        public IEnumerable<LevelUpAppearance> Load() {
            var targetType = typeof(LevelUpAppearance);
            return (List<LevelUpAppearance>)CSV.DeserializeToListBySpreadSheet(targetType, _path, _sheet, _apiKey);
        }
    }
#endif
    
    public class CSVLevelUpAppearanceLoader: IDataLoader<LevelUpAppearance> {
        private readonly string _sheet = "LevelUpAppearance.csv";
        public IEnumerable<LevelUpAppearance> Load() {
            var targetType = typeof(LevelUpAppearance);
            var path = Path.Combine(Application.streamingAssetsPath, "Datas", _sheet);
            var csv = CSV.Parse(File.ReadAllText(path));
            return ((List<LevelUpAppearance>)CSV.DeserializeToList(targetType, csv));
        }
    }
}