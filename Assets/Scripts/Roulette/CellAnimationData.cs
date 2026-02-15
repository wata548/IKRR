using Character.Skill;

namespace Roulette {
    public enum AnimationType {
        Use, Evolve, Buff, 
    }
    
    public struct CellAnimationData {

        public readonly AnimationType Type;
        public readonly int Column;
        public readonly int Row;

        
        
        public CellAnimationData(AnimationType pType, int pColumn, int pRow) =>
            (Type, Column, Row) = (pType, pColumn, pRow);
    } 
}