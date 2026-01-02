namespace Roulette {
    public struct EvolveInfo {
        public readonly int Row;
        public readonly int Column;
        public readonly int Code;

        public EvolveInfo(int pRow, int pColumn, int pCode) =>
            (Row, Column, Code) = (pRow, pColumn, pCode);
    }
}