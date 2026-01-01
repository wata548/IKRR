using Symbol;

namespace Roulette {
    public static partial class RouletteManager {
        private class RouletteColumn {
            public const int MAX_HEIGHT = 8;
            public static int Height { get; private set; } 
            private (int, CellStatus)[] _column = new (int, CellStatus)[MAX_HEIGHT];

            public void ClearStatus() {
                for (int i = 0; i < _column.Length; i++)
                    _column[i].Item2 = CellStatus.Usable;
            }
            
            public void Clear() {
                for (int i = 0; i < _column.Length; i++)
                    _column[i] = (0, CellStatus.Usable);
            }

            public void Set(int pIdx, int pValue) => _column[pIdx].Item1 = pValue;
            public int this[int pIdx] => _column[pIdx].Item1;
            public CellStatus GetStatus(int pIdx) => _column[pIdx].Item2;

            public void UseSkill(int pIdx) {
                _column[pIdx].Item2 = CellStatus.Used;
            } 
            
            public void RefreshStatus(int pColumnIdx) {
                for (int i = 0; i < Height; i++) {
                    if(_column[i].Item2 == CellStatus.Used)
                        continue;
                    _column[i].Item2 = SymbolExecutor.IsUsable(pColumnIdx, i)
                        ? CellStatus.Usable
                        : CellStatus.Impossible;
                }
            }
            
            public static void SetHeight(int pHeight) => Height = pHeight;
            
            public int Roll(int pIn) {
                var last = _column[Height - 1];
                for (int i = 0; i < Height - 1; i++) {
                    _column[i + 1] = _column[i];
                }

                _column[0] = (pIn, CellStatus.Usable);
                return last.Item1;
            }
        }
    }
}