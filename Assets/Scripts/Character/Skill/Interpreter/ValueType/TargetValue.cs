using System;
using System.Text.RegularExpressions;
using UnityEngine;
using Data;
using Random = UnityEngine.Random;

namespace Character.Skill.Data {
    public readonly struct TargetValue {

        //==================================================||Fields 
        public readonly Positions Targets;

        //==================================================||Constructors 
        public TargetValue(Positions pTargets) => Targets = pTargets;
        public TargetValue(string pValue) =>
            Targets = (Positions)Enum.Parse(typeof(Positions), pValue.Replace('|', ','));
        //==================================================||Methods 
        public static TargetValue Parse(string pValue) => new(pValue);
        public override string ToString() => Targets.ToString();
    }
}