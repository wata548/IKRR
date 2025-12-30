#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using CSVData;
using Data;

namespace Symbol {
    
    public class SpreadSheetSymbolLoader: IDataLoader<ISymbolData> {

        private readonly string _apiKey = "AIzaSyD9TQEfDm8OY3DtfdvZayoZi96QHLl_SA0";
        private readonly string _path = "1BLW__Va3NhcIJzwd6oblRmFck1DOADGoB3fww_RGqpo";
        private readonly string _sheet = "Symbol";

        public SpreadSheetSymbolLoader() {}
        
        public SpreadSheetSymbolLoader(string api, string path, string sheet) =>
            (_apiKey, _path, _sheet) = (api, path, sheet);
        
        public Dictionary<int, ISymbolData> Load() {
            var targetType = typeof(CSVSymbolData);
            return ((List<CSVSymbolData>)CSV.DeserializeToListBySpreadSheet(targetType, _path, _sheet, _apiKey))
                .ToDictionary(symbol => symbol.SerialNumber, symbol => symbol as ISymbolData);
        }
    }
}
#endif