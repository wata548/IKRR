using CSVData;

namespace Data {
    public class CSVSymbolData : SymbolData, ICSVDictionaryData  {
        public int SerialNumber { get; private set; }
    }
 
}