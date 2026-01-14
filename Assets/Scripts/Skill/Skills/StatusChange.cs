using System;
using Character.Skill.Data;
using Data;
using UI;
using UI.Status;
using UnityEngine.Scripting;

namespace Character.Skill {
    [Preserve]
    public class StatusChange: SkillBase {
        
        [SkillParameter]
        public StatValue TargetStat { get; private set; }
        [SkillParameter]
        public char Symbol { get; private set; }
        [SkillParameter]
        public int Rhs { get; private set; }
        
        public StatusChange(string[] pData) : base(pData) {}

        protected override void Implement(Positions pCaster) {
            Action<TargetStatus, int> func = Symbol switch {
                '+' => Status.AddValue,
                '-' => Status.SubValue,
                '*' => Status.MulValue,
                '/' => Status.DivValue,
                '=' => Status.SetValue,
                _ => throw new ArgumentException($"{Symbol} isn't operator")
            };
            func.Invoke(TargetStat.Value, Rhs);
            UIManager.Instance.Status.Refresh(TargetStat.Value, End);                      
        }
    }
}