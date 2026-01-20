using System;

namespace Data {
    [Flags]
    public enum SymbolCategory {
        None  = 0b000,
        Paper = 0b001,
        Sword = 0b010,
        Magic = 0b100,
    }
}