using System.Collections.Generic;
using CSVData;
using Data;

namespace Symbol {
    public class SpreadSheetLoader: ISymbolDataLoader {

        private readonly string _apiKey;
        private readonly string _path;
        private readonly string _sheet;

        public SpreadSheetLoader(string api, string path, string sheet) =>
            (_apiKey, _path, _sheet) = (api, path, sheet);
        
        public List<SymbolData> Load() {
            var targetType = typeof(SymbolData);
            return (List<SymbolData>)CSV.DeserializeToListBySpreadSheet(targetType, _path, _sheet, _apiKey);
        }
    }
}