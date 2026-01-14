using Data;
using UI;
using UnityEngine.Scripting;

namespace Character.Skill {
    [Preserve]
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