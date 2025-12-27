using System;
using Data;

namespace Character.Skill.Data {
    public readonly struct TargetValue {

        //==================================================||Fields 
        public readonly Positions Value;

        //==================================================||Constructors 
        public TargetValue(Positions pTargets) => Value = pTargets;
        public TargetValue(string pValue) =>
            Value = (Positions)Enum.Parse(typeof(Positions), pValue.Replace('|', ','));
        //==================================================||Methods 
        public static TargetValue Parse(string pValue) => new(pValue);
        public override string ToString() => Value.ToString();
    }
}