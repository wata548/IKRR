using System;
using Data;

namespace Character.Skill {
    public interface ISkill {
        
        public bool IsEnd { get;}
        public Action OnEnd { get; set; }
        
        //==================================================||Methods 
        public void Execute(Positions pCaster);
    }
}