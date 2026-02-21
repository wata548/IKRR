using System.Collections.Generic;
using Data;
using Symbol;

namespace Roulette {

    public struct CellInfo {
        public int Code;
        public CellStatus Status;
    }
    
    public static partial class RouletteManager {
        
        private class RouletteColumn {
            
            //==================================================||Fields 
            private CellInfo[] _column = new CellInfo[MAX_HEIGHT + 1];
            
           //==================================================||Operators 
            public static explicit  operator CellInfo[](RouletteColumn pTarget) =>
                pTarget._column;
            
            //==================================================||Methods 
            public CellStatus GetStatus(int pIdx) => _column[pIdx].Status;
            
            public void SetStatus(int pIdx, CellStatus pStatus) {
                _column[pIdx].Status = pStatus;
            }
            
            private void SetStatus(int pColumn, int pRow, bool pForce) {
                if (!pForce && _column[pRow].Status is CellStatus.Used or CellStatus.ForceUnavailable)
                    return;
                                        
                _column[pRow].Status = SymbolExecutor.IsUsable(pColumn, pRow)
                    ? CellStatus.Usable
                    : CellStatus.Unavailable;
            }
            public void InitStatus() {
                for (int i = 0; i < _column.Length; i++)
                    _column[i].Status = CellStatus.Usable;
            }
            
            public void Clear() {
                for (int i = 0; i < _column.Length; i++) {
                    _column[i].Code = 0;
                    _column[i].Status = CellStatus.Usable;
                }
            }

            public void Set(int pIdx, int pValue) {
                _column[pIdx].Code = pValue;
            }
            
            public void SetAndCheckStatus(int pColumnIdx, int pRow, int pValue, bool pForce = false) {
                _column[pRow].Code = pValue;
                
                if (pValue == DataManager.EMPTY_SYMBOL)
                    return;
                SetStatus(pColumnIdx, pRow, pForce);
            }

            public int this[int pIdx] => _column[pIdx].Code;
            
            public void RefreshStatus(int pColumn) {
                for (int i = 0; i < Height; i++) {
                    SetStatus(pColumn, i, false);
                }

                for (int i = Height; i <= MAX_HEIGHT; i++) {
                    _column[i].Status = CellStatus.Unavailable;
                }
            }
            
            public int Roll(int pIn ) {
                var last = _column[0];
                for (int i = 0; i < Height; i++) {
                    _column[i] = _column[i + 1];
                }

                _column[Height].Code = pIn;
                _column[Height].Status = CellStatus.Usable;
                return last.Code;
            }
        }
    }
}