#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using CSVData;
using Data.Map;

namespace Data {

    public class SpreadSheetStageFrequencyLoader: IDataLoader<Stage, int> {
        private readonly string _apiKey = "AIzaSyD9TQEfDm8OY3DtfdvZayoZi96QHLl_SA0";
        private readonly string _path = "1BLW__Va3NhcIJzwd6oblRmFck1DOADGoB3fww_RGqpo";
        private readonly string _sheet = "StageType";
        
        public IEnumerable<KeyValuePair<Stage, int>> Load() {
            var targetType = typeof(StageType);
            return ((List<StageType>)CSV.DeserializeToListBySpreadSheet(targetType, _path, _sheet, _apiKey))
                .Select(data => new KeyValuePair<Stage, int>(data.Type, data.Frequency));
        }
    }
}
#endif