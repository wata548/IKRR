using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSVData;
using UnityEngine;

namespace Data.Map {
#if UNITY_EDITOR
    public class SpreadSheetStageWidthLoader: IDataLoader<StageWidth> {
                private readonly string _apiKey = "AIzaSyD9TQEfDm8OY3DtfdvZayoZi96QHLl_SA0";
        private readonly string _path = "1BLW__Va3NhcIJzwd6oblRmFck1DOADGoB3fww_RGqpo";
        private readonly string _sheet = "StageWidth";

        public IEnumerable<StageWidth> Load() {
            var targetType = typeof(StageWidth);
            return (List<StageWidth>)CSV.DeserializeToListBySpreadSheet(targetType, _path, _sheet, _apiKey);
        }
    }
#else
    public class CSVStageWidthLoader: IDataLoader<StageWidth> {
        private readonly string _sheet = "StageWidth.csv";
    
        public IEnumerable<StageWidth> Load() {

            var context = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "Datas", _sheet));
            var targetType = typeof(StageWidth);
            return (List<StageWidth>)CSV.DeserializeToList(targetType, CSV.Parse(context));
        }
    }
#endif
}