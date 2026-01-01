using Character.Skill;

namespace Symbol {
    public interface ISymbolExecutor {

        void Update();
        bool IsUsable(int pColumn, int pRow);
        int Evolution(int pColumn, int pRow);
        ISkill Execute(int pColumn, int pRow);
    }
}