namespace Data {
    public class EnemyData {
        public int SerialNumber { get; protected set; }
        public string Name { get; protected set; }
        public string Desc { get; protected set; }
        public EnemySize Size { get; protected set; }
        public int MaxHp { get; protected set; }
        public int Exp { get; protected set; }
        public string SkillInfo { get; protected set; }

        public Info GetInfo() =>
            new Info(SerialNumber, Name, new() {
                ( "특징", Desc )
            });
    }
}