using System;
using Character.Skill.Data;
using Data;

namespace Character.Skill {
    public interface ISkill {
        
        public string ShowCount { get; }
        public bool IsEnd { get;}
        public Action OnEnd { get; set; }
        
        //==================================================||Methods 
        public void Execute(Positions pCaster);
        public string GetSkillName();
    }
}