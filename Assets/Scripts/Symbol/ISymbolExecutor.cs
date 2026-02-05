using Character.Skill;
using UnityEditor.Timeline.Actions;

namespace Symbol {
    public interface ISymbolExecutor {

        void Update();
        void Invoke(string pContext);
        bool IsUsable(int pColumn, int pRow);
        ISkill Evolution(int pColumn, int pRow);
        ISkill GetSkill(int pColumn, int pRow);
    }
}