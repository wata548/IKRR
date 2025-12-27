using System;
using Data;

namespace Character.Skill.Data {
    public readonly struct StatValue {
        
        //==================================================||Fields 
        public readonly TargetStatus Value;

        //==================================================||Constructors 
        public StatValue(TargetStatus pStatus) => Value = pStatus;
        public StatValue(string pValue) => 
            Value = (TargetStatus)Enum.Parse(typeof(TargetStatus), pValue.Replace('|', ','));
        
        //==================================================||Methods
        public static StatValue Parse(string value) => new(value);
        public override string ToString() => Value.ToString();
    }
}