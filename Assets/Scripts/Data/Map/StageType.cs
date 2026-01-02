namespace Data.Map {
    public class StageType {
        public Stage Type { get; private set; }
        public int Frequency { get; set; }


        public StageType(){}
        public StageType(Stage pType, int pFrequency) {
            Type = pType;
            Frequency = pFrequency;
        }
    }
}