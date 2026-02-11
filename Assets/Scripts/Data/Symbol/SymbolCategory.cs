using System;

namespace Data {
    [Flags]
    public enum SymbolCategory {
        None  = 0b0000,
        Paper = 0b0001,
        Sword = 0b0010,
        Magic = 0b0100,
        Body  = 0b1000,
    }
}