using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSVData;
using UnityEngine;

namespace Data {
    public class CSVSymbolLoader: IDataLoader<int, SymbolData> {
        
        private readonly string _sheet = "Symbol.csv";
        public IEnumerable<KeyValuePair<int, SymbolData>> Load() {
            var targetType = typeof(CSVSymbolData);
            var path = Path.Combine(Application.streamingAssetsPath, "Datas", _sheet);
            var csv = CSV.Parse(File.ReadAllText(path));
            return ((List<CSVSymbolData>)CSV.DeserializeToList(targetType, csv))
                .Select(symbol => new KeyValuePair<int, SymbolData>(symbol.SerialNumber, symbol));
        }
    }
}