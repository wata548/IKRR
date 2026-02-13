using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Character.Skill.Data {
    
    public struct RangeValue {
        
       //==================================================||Constants 
        const string PATTERN = @"(?<Min>\d+)\s*[\~\-]\s*(?<Max>\d+)";
        
        //==================================================||Fields 
        public readonly int Min;
        public readonly int Max;
        public int Value;
        
        //==================================================||Constructors 
        public RangeValue(int pValue) {
            (Min, Max) = (pValue, pValue);
            Value = Min;
        }

        public RangeValue(int pMinValue, int pMaxvalue) {
            
            (Min, Max) = (pMinValue, pMaxvalue);
            Value = Random.Range(Min, Max + 1);
        }

        public RangeValue(string pValue) {

            var match = Regex.Match(pValue, PATTERN);
            if (!match.Success) {
                Min = int.Parse(pValue);
                Max = Min;
            }
            else {
                Min = int.Parse(match.Groups["Min"].Value);
                Max = int.Parse(match.Groups["Max"].Value);    
            }
            Value = Random.Range(Min, Max + 1);
        }
        
        //==================================================||Operation 
        public static RangeValue operator +(RangeValue lhs, RangeValue rhs) =>
            new(lhs.Min + rhs.Min, lhs.Max + rhs.Max);
        //==================================================||Methods

        public int Next() {
            return Value = Random.Range(Min, Max + 1);
        }
        public static RangeValue Parse(string value) => new(value);

        public override string ToString() =>
            Mathf.Approximately(Min, Max) ? Min.ToString() : $"{Min} ~ {Max}";
    }
}