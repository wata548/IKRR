using Character.Skill;

namespace Roulette {
    public enum AnimationType {
        Use, Evolve, Enemy
    }
    
    public struct CellAnimationData {

        public readonly AnimationType Type;
        public readonly int Column;
        public readonly int Row;
        public ISkill Skill;
        public readonly CellStatus Status;

        
        
        public CellAnimationData(AnimationType pType, int pColumn, int pRow, ISkill pSkill, CellStatus pStatus = CellStatus.Usable) =>
            (Type, Column, Row, Skill, Status) = (pType, pColumn, pRow, pSkill, pStatus);
    } 
}