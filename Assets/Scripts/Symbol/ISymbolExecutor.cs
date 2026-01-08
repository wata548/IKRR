using Character.Skill;

namespace Symbol {
    public interface ISymbolExecutor {

        void Update();
        bool IsUsable(int pColumn, int pRow);
        int Evolution(int pColumn, int pRow);
        ISkill GetSkill(int pColumn, int pRow);
    }
}