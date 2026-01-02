using CSVData;

namespace Data {
    public class CSVEnemyData: EnemyData, ICSVDictionaryData {
        
        public int SerialNumber { get; private set; }
        
    }
}