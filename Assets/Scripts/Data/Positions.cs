using System;
using System.Collections.Generic;
using Extension;
using Lang;
using Roulette;
using UnityEngine.VFX;
using XLua;

namespace Data {
    [Flags, Serializable]
    public enum Positions {
        None    = 0b0000000,
        Player  = 0b0000001,
        Caster  = 0b0000010,
        
        Middle  = 0b0000100,
        Left    = 0b0001000, 
        Right   = 0b0010000,
        AllEnemy= 0b0011100,
        
        NotMe   = 0b0100000,
        Target  = 0b1000000,
    }

    [Flags]
    public enum TargetStatus {
        None        = 0b000,
        Strength    = 0b001,
        Dexterity   = 0b010,
        Wisdom      = 0b100,
        All         = 0b111,
    }
    
    
    public static class ExKorean {
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

        public static string ToRuntimeLanguage(this TargetStatus pTarget) {
            var list = new List<string>();
            foreach (var flag in pTarget.Split()) {
                list.Add((flag switch {
                    TargetStatus.Strength => "힘",
                    TargetStatus.Dexterity => "민첩",
                    TargetStatus.Wisdom => "지혜"
                }).ApplyLang());    
            }

            return string.Join(", ", list);
        }
    }
}