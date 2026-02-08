using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSVData;
using UnityEngine;

namespace Data {
#if UNITY_EDITOR
    public class SSGroupLoader<TData, TValue>: IDataLoader<int, List<TValue>> {
        private readonly string _apiKey = "AIzaSyD9TQEfDm8OY3DtfdvZayoZi96QHLl_SA0";
        private readonly string _path = "1BLW__Va3NhcIJzwd6oblRmFck1DOADGoB3fww_RGqpo";
        private readonly string _sheet;
        private readonly Func<TData, int> _group;
        private readonly Func<TData, TValue> _postProcessing;
         
        public SSGroupLoader(string pSheet, Func<TData, int> pGroup, Func<TData, TValue> pPostProcessing = null) {
            _sheet = pSheet;
            _group = pGroup;
            _postProcessing = pPostProcessing;
        }
        
        public IEnumerable<KeyValuePair<int, List<TValue>>> Load() {
            var targetType = typeof(TData);
            var data = (List<TData>)CSV.DeserializeToListBySpreadSheet(targetType, _path, _sheet, _apiKey);
            return data
                .GroupBy(info => _group(info))
                .Select(kvp => 
                    new KeyValuePair<int, List<TValue>>(
                        kvp.Key, 
                        kvp.Select(kvp2 => {
                            if (kvp2 is TValue value)
                                return value;
                            return _postProcessing(kvp2);
                        }).ToList()
                    )
                );
        }
    }
#endif
    public class CSVGroupLoader<TData, TValue>: IDataLoader<int, List<TValue>> {
        private readonly string _sheet;
        private readonly Func<TData, int> _group;
        private readonly Func<TData, TValue> _postProcessing;
             
        public CSVGroupLoader(string pSheet, Func<TData, int> pGroup, Func<TData, TValue> pPostProcessing = null) {
            _sheet = pSheet;
            _group = pGroup;
            _postProcessing = pPostProcessing;
        }
            
        public IEnumerable<KeyValuePair<int, List<TValue>>> Load() {
            var targetType = typeof(TData);
            var path = Path.Combine(Application.streamingAssetsPath, "Datas", _sheet + ".csv");
            var csv = CSV.Parse(File.ReadAllText(path));
            var data = (List<TData>)CSV.DeserializeToList(targetType, csv);
            return data
                .GroupBy(info => _group(info))
                .Select(kvp => 
                    new KeyValuePair<int, List<TValue>>(
                        kvp.Key, 
                        kvp.Select(kvp2 => _postProcessing(kvp2))
                            .ToList()
                    )
                );
        }
    }
}