using System.Collections.Generic;
using System.Linq;
using CSVData;

namespace Data.Map {
    public class SpreadSheetStageWidthLoader: IDataLoader<StageWidth> {
                private readonly string _apiKey = "AIzaSyD9TQEfDm8OY3DtfdvZayoZi96QHLl_SA0";
        private readonly string _path = "1BLW__Va3NhcIJzwd6oblRmFck1DOADGoB3fww_RGqpo";
        private readonly string _sheet = "StageWidth";

        public IEnumerable<StageWidth> Load() {
            var targetType = typeof(StageWidth);
            return ((List<StageWidth>)CSV.DeserializeToListBySpreadSheet(targetType, _path, _sheet, _apiKey));
        }
    }
}