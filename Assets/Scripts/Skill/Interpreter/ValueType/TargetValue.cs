using System;
using Data;
using Lang;

namespace Character.Skill.Data {
    public static class ExPositions {
        public static string ToRuntimeLanguage(this Positions pTarget) =>
            (pTarget switch {
                Positions.Player => "플레이어",
                Positions.Left => "가장 왼쪽 적",
                Positions.Right => "가장 오른쪽 적",
                Positions.Middle => "중앙 적",
                Positions.AllEnemy => "모든 적",
                Positions.Caster => "본인",
                Positions.Target => "타겟",
            }).ApplyLang();
    }
    
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