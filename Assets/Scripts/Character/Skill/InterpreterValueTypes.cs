using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Data;
using Random = UnityEngine.Random;

namespace Character.Skill.Data {
    
    public struct RangeValue {
        
        //==================================================||Fields 
        public readonly float Min;
        public readonly float Max;
        public int Value;
        
        //==================================================||Constructors 
        public RangeValue(int pValue = 1) {
            (Min, Max) = (pValue, pValue);
            Value = (int)Min;
        }

        public RangeValue(int pMinValue, int pMaxvalue) {
            
            (Min, Max) = (pMinValue, pMaxvalue);
            Value = (int)Random.Range(Min, Max);
        }

        public RangeValue(string pValue) {

            const string pattern = @"^\s*(?:(\d*\.\d+)|(\d+))(?:\s*[\~\-]\s*(?:(\d*\.\d+)|(\d+)))?";
            var match = Regex.Match(pValue, pattern).Groups
                .Select(v => v.Value)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Skip(1)
                .ToList();

            Debug.Log(pValue);
            Min = float.Parse(match[0], NumberStyles.AllowDecimalPoint);
            Max = float.Parse(match[^1], NumberStyles.AllowDecimalPoint);
            Value = (int)Random.Range(Min, Max);
        }

        //==================================================||Methods

        public int Next() {
            return Value = (int)Random.Range(Min, Max);
        }
        public static RangeValue Parse(string value) => new(value);

        public override string ToString() =>
            Mathf.Approximately(Min, Max) ? Min.ToString() : $"{Min} ~ {Max}";
    }

    public readonly struct TargetValue {

        //==================================================||Fields 
        public readonly Targets Targets;
        private static Dictionary<char, Targets> targetDict = null;

        //==================================================||Constructors 
        public TargetValue(Targets pTargets) => Targets = pTargets;
        public TargetValue(string pValue) {
            const string pattern = @"^(?i)([lrmnptca])(?:\s*\|\s*([lrm]))*";

            if (!Regex.IsMatch(pValue, pattern)) {
                Targets = default;
                return;
            }
            
            targetDict ??= ((Targets[])Enum.GetValues(typeof(Targets)))
                .Skip(1)
                .ToDictionary(v => char.ToLower(v.ToString()[0]), v => v);

            var data = pValue.Split('|')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => targetDict[char.ToLower(s[0])]);
            
            Targets = default;
            foreach (var target in data) {
                Targets |= target;
            }
            
        }
        
        //==================================================||Methods 
        public static TargetValue Parse(string pValue) => new(pValue);
        public override string ToString() => Targets.ToString();
    }

    public readonly struct StatValue {
        
        //==================================================||Fields 
        public readonly TargetStatus Status;
        private static Dictionary<char, TargetStatus> targetDict = null;

        //==================================================||Constructors 
        public StatValue(TargetStatus pStatus) => Status = pStatus;
        public StatValue(string pValue) {
            const string pattern = @"(?i)(?:(str|dex|wis)(?:\s*\|\s*(str|dex|wis))*)|(all)";
            
            targetDict ??= ((TargetStatus[])Enum.GetValues(typeof(TargetStatus)))
                .ToDictionary(v => char.ToLower(v.ToString()[0]), v => v);
            
            var data = pValue.Split('|')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => targetDict[char.ToLower(s[0])]);
            
            Status = TargetStatus.None;
            foreach(var target in data)
                Status |= target;
        }
        
        //==================================================||Methods
        public static StatValue Parse(string value) => new(value);
        public override string ToString() => Status.ToString();
    }
}