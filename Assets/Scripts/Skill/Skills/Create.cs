namespace Character.Skill {
    public class Create: Change {
        public Create(string[] pData) : base(pData) {}

        protected override bool IsCreate => true;
    }
}