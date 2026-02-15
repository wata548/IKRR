using System;

namespace Data {
    [Flags]
    public enum SymbolCategory {
        None      = 0b00000,
        Paper     = 0b00001,
        Sword     = 0b00010,
        Magic     = 0b00100,
        Body      = 0b01000,
        Shuriken  = 0b10000,
    }
}