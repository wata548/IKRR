using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSVData;
using UnityEngine;

namespace Data.Effect {
#if UNITY_EDITOR
    public class SpreadSheetLoader<T>: IDataLoader<int, T> {
        private const string API_KEY = "AIzaSyD9TQEfDm8OY3DtfdvZayoZi96QHLl_SA0";
        private const string PATH = "1BLW__Va3NhcIJzwd6oblRmFck1DOADGoB3fww_RGqpo";
        private readonly string _sheet;

        public SpreadSheetLoader(string pSheet) => _sheet = pSheet; 
        
        public IEnumerable<KeyValuePair<int, T>> Load() {
            var targetType = typeof(T);
            var data = (Dictionary<int, T>)CSV.DeserializeToDictionaryBySpreadSheet(targetType, PATH, _sheet, API_KEY, out _);
            return data;
        }
    }
#endif
    public class CSVLoader<T>: IDataLoader<int, T> {
        private readonly string _sheet;

        public CSVLoader(string pSheet) => _sheet = pSheet;
        
        public IEnumerable<KeyValuePair<int, T>> Load() {
            var targetType = typeof(T);
            var path = Path.Combine(Application.streamingAssetsPath, "Datas", _sheet+".csv");
            var csv = CSV.Parse(File.ReadAllText(path));
            return (Dictionary<int, T>)CSV.DeserializeToDictionary(targetType, csv, "SerialNumber");
        }
    }
}