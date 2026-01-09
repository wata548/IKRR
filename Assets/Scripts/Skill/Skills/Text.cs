using Data;
using UI;

namespace Character.Skill {
    public class Text: SkillBase {
        
        [SkillParameter]
        public string Context { get; private set; } 
        
        
        public Text(string[] pData) : base(pData) {}
        protected override void Implement(Positions pCaster) {
            UIManager.Instance.SkillShower.Show(pCaster, Context, null);  
            End();
        }
    }
}