using System;

namespace Data {
    [Flags, Serializable]
    public enum Positions {
        None    = 0b0000000,
        Player  = 0b0000001,
        Caster  = 0b0000010,
        
        Left    = 0b0000100, 
        Middle  = 0b0001000,
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
}