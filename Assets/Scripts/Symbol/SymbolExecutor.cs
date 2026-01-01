using Character.Skill;
using Extension;

namespace Symbol {
    public class SymbolExecutor: MonoSingleton<SymbolExecutor> {
        private readonly ISymbolExecutor _executor = new LuaSymbolExecutor();
        protected override bool IsNarrowSingleton { get; set; } = false;
        
        //==================================================|| 
        private new void Update() {
            base.Update();
            _executor.Update();
        }
        
        public bool IsUsable(int pColumn, int pRow) => _executor.IsUsable(pColumn, pRow);
        public int Evolution(int pColumn, int pRow) => _executor.Evolution(pColumn, pRow);
        public ISkill Execute(int pColumn, int pRow) => _executor.Execute(pColumn, pRow);
    }
}