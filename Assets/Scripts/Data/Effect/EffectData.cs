using CSVData;

namespace Data.Effect {
    public class EffectData: ICSVDictionaryData {
        public int SerialNumber { get; private set; }
        public string Name { get; private set; }
        public string Desc { get; private set; }
    }
}