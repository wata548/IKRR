using CSVData;

namespace Data {
    public class CSVEnemyData: IEnemyData, ICSVDictionaryData {
        
        public int SerialNumber { get; private set; }
        public string Name { get; private set; }
        public EenemySize Size { get; private set; }
        public int MaxHp { get; private set; }
        public string SkillInfo { get; private set; }
    }
    
    public interface IEnemyData {
        string Name { get; }
        EenemySize Size { get; }
        int MaxHp { get; }
        string SkillInfo { get; }
    }
}