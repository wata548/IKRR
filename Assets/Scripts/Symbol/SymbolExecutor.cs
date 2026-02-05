using Character.Skill;
using Extension.StaticUpdate;

namespace Symbol {
    public static class SymbolExecutor {
        private static readonly ISymbolExecutor _executor = new LuaSymbolExecutor();
        
        //==================================================|| 
        
        [StaticUpdate]
        private static void Update() {
            _executor.Update();
        }

        public static void Invoke(string pContext) => _executor.Invoke(pContext);
        public static bool IsUsable(int pColumn, int pRow) => _executor.IsUsable(pColumn, pRow);
        public static ISkill Evolution(int pColumn, int pRow) => _executor.Evolution(pColumn, pRow);
        public static ISkill GetSkill(int pColumn, int pRow) => _executor.GetSkill(pColumn, pRow);
    }
}