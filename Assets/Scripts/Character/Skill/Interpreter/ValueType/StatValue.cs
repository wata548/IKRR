using System;
using Data;

namespace Character.Skill.Data {
    public readonly struct StatValue {
        
        //==================================================||Fields 
        public readonly TargetStatus Status;

        //==================================================||Constructors 
        public StatValue(TargetStatus pStatus) => Status = pStatus;
        public StatValue(string pValue) => 
            Status = (TargetStatus)Enum.Parse(typeof(TargetStatus), pValue.Replace('|', ','));
        
        //==================================================||Methods
        public static StatValue Parse(string value) => new(value);
        public override string ToString() => Status.ToString();
    }
}