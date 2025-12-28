#if UNITY_EDITOR
using System.Collections.Generic;
using CSVData;
using Data;

namespace Symbol {
    
    public class SpreadSheetLoader: ISymbolDataLoader {

        private readonly string _apiKey = "AIzaSyD9TQEfDm8OY3DtfdvZayoZi96QHLl_SA0";
        private readonly string _path = "1BLW__Va3NhcIJzwd6oblRmFck1DOADGoB3fww_RGqpo";
        private readonly string _sheet = "Symbol";

        public SpreadSheetLoader() {}
        
        public SpreadSheetLoader(string api, string path, string sheet) =>
            (_apiKey, _path, _sheet) = (api, path, sheet);
        
        public List<SymbolData> Load() {
            var targetType = typeof(SymbolData);
            return (List<SymbolData>)CSV.DeserializeToListBySpreadSheet(targetType, _path, _sheet, _apiKey);
        }
    }
}
#endif