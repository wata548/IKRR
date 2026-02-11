using System;

namespace Data {
    public enum Rarity {
        Normal,
        Rare,
        Epic,
        Unique,
        Legendary,
        Evolution,
        Etc
    }
    
    public static class KoreanEnum {
        public static string ToKorean(this Rarity pTarget) =>
            pTarget switch {
                Rarity.Normal => "노말",
                Rarity.Rare => "레어",
                Rarity.Epic => "에픽",
                Rarity.Unique => "유니크",
                Rarity.Legendary => "레전더리",
                Rarity.Evolution => "진화",
                Rarity.Etc => "특수",
                _ => ""
            };
        public static string ToKorean(this SymbolType pTarget) =>
            pTarget switch {
                SymbolType.Buff => "버프",
                SymbolType.Skill => "스킬",
                _ => ""
            };
    }
}