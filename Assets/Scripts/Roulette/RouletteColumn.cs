using System.Collections.Generic;
using Symbol;

namespace Roulette {

    public struct CellInfo {
        public int Code;
        public CellStatus Status;
    }
    
    public static partial class RouletteManager {
        
        private class RouletteColumn {
            
            //==================================================||Fields 
            public const int MAX_HEIGHT = 8;
            private CellInfo[] _column = new CellInfo[MAX_HEIGHT + 1];
            
            //==================================================||Methods 
            public static explicit  operator CellInfo[](RouletteColumn pTarget) =>
                pTarget._column;
            public void ClearStatus() {
                for (int i = 0; i < _column.Length; i++)
                    _column[i].Status = CellStatus.Usable;
            }
            
            public void Clear() {
                for (int i = 0; i < _column.Length; i++) {
                    _column[i].Code = 0;
                    _column[i].Status = CellStatus.Usable;
                }
            }

            public void Set(int pIdx, int pValue) => _column[pIdx].Code = pValue;
            public int this[int pIdx] => _column[pIdx].Code;
            public CellStatus GetStatus(int pIdx) => _column[pIdx].Status;

            public void UseSkill(int pIdx) {
                _column[pIdx].Status = CellStatus.Used;
            } 
            
            public void RefreshStatus(int pColumnIdx) {
                for (int i = 0; i < Height; i++) {
                    if(_column[i].Status == CellStatus.Used)
                        continue;
                    _column[i].Status = SymbolExecutor.IsUsable(pColumnIdx, i)
                        ? CellStatus.Usable
                        : CellStatus.Impossible;
                }
            }
            
            public int Roll(int pIn ) {
                var last = _column[Height];
                for (int i = 0; i < Height; i++) {
                    _column[i + 1] = _column[i];
                }

                _column[0].Code = pIn;
                _column[0].Status = CellStatus.Usable;
                return last.Code;
            }
        }
    }
}