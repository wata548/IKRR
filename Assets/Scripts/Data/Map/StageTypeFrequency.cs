using System;
using System.Collections.Generic;
using System.Linq;
using Extension;

namespace Data.Map {
    public static class StageTypeFrequency {

        //==================================================||Fields
        private static Dictionary<Stage, int> _frequency;
        private static List<StageType> _prefix;

        //==================================================||Methods
        public static void LoadData(IEnumerable<KeyValuePair<Stage, int>> pSetting) {
            _frequency = pSetting.ToDictionary();
            FixPrefix();
        }

        private static void FixPrefix() {
            _prefix = _frequency.Select(kvp => new StageType(kvp.Key, kvp.Value)).ToList();
            for (int i = 1; i < _prefix.Count; i++) {
                _prefix[i].Frequency += _prefix[i - 1].Frequency;
            }
        }

        public static void AddPercent(Stage pStage, int pAmount) {
            _frequency[pStage] += pAmount;
            FixPrefix();
        }

        public static void SubPercent(Stage pStage, int pAmount) {
            _frequency[pStage] -= pAmount;
            FixPrefix();
        }

        public static void MulPercent(Stage pStage, int pAmount) {
            _frequency[pStage] *= pAmount;
            FixPrefix();
        }
        public static void DivPercent(Stage pStage, int pAmount) {
            _frequency[pStage] /= pAmount;
            FixPrefix();
        }

        public static Stage Random(Random pRandom) => Random(pRandom, new Stage[] { });
        public static Stage Random(Random pRandom, params Stage[] pExclude) {

            var random = 1 + pRandom.Next() % _prefix[^1].Frequency;

            int start = 0;
            int end = _prefix.Count - 1;
            while (start < end) {
                var middle = (start + end) / 2;
                var compare = _prefix[middle].Frequency.CompareTo(random);
                    
                if (compare == 0) {
                    start = middle;
                    break;
                }
                if (compare > 0) {
                    end = middle;
                }
                else {
                    start = middle + 1;
                }
            }

            return _prefix[start].Type;
        }
    }
}