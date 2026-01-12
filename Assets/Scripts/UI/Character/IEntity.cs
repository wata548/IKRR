using Data;

namespace Character {
    public interface IEntity {
        public Positions Position { get; }
        public int MaxHp { get; }
        public int Hp { get; }
        public bool IsAlive { get; }

        public void ReceiveDamage(int pAmount);
        public void Heal(int pAmount);
    }
}