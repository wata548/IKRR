using System.Collections.Generic;
using System.Linq;
using Data;
using Extension;
using Symbol;
using UnityEngine;

namespace Roulette {
    
    public static partial class RouletteManager {
        
        //==================================================||Properties 
        public const int MAX_HEIGHT = 8;
        public static int Width { get; private set; } = 5;
        public static int Height { get; private set; } = 3;
        public static int HandSize { get; private set; } = 25;
        public static IEnumerable<int> Hand => _hand.SelectMany(kvp => Enumerable.Repeat(kvp.Key, kvp.Value));
        public static IEnumerable<KeyValuePair<int, int>> HandDictionary => _hand;
        
        //==================================================||Fields 
        private static readonly Dictionary<int, int> _hand = new();
        private static readonly Queue<int> _remainSymbol = new();
        private static List<RouletteColumn> _current = new();
        
        //==================================================||Methods 
        
        public static void Init(IEnumerable<int> pInitHand) {

            if (_current.Count == 0) {
                for (int i = 0; i < Width; i++) {
                    _current.Add(new());
                }
            }
            
            _hand.Clear();
            _hand.Add(0, HandSize - pInitHand.Count());
            foreach (var symbol in pInitHand) {
                if (!_hand.TryAdd(symbol, 1))
                    _hand[symbol]++;
            }
            ResetRoulette();
        }

        /// <summary>
        /// output is inserted symbolCode
        /// </summary>
        /// <returns></returns>
        public static int Roll(int pColumn) {
            if (pColumn > Width) {
                Debug.LogError($"pRow must be lower than {Width}");
                return DataManager.ERROR_SYMBOL;
            }

            var symbol = _remainSymbol.Dequeue();
            var @out = _current[pColumn].Roll(symbol);
            _remainSymbol.Enqueue(@out);
            return symbol;
        }
        
        public static void ClearStatus() {
            foreach (var column in _current) {
                column.ClearStatus();
            }
        }
        
        public static void ResetRoulette() {
            _remainSymbol.Clear();
            foreach (var column in  _current)
                column.Clear();
            
            var idx = -1;
            foreach (var symbol in Hand.ToList().Shuffle()) {
                idx++;
                if (idx > Width * Height) {
                    _remainSymbol.Enqueue(symbol);
                    continue;
                }
            
                var column = idx % Width;
                var row = idx / Width;
                _current[column].Set(row, symbol);
            }
        }
        
        public static void Remove(int pSymbol, int pAmount = 1) {

            if (!_hand.TryGetValue(pSymbol, out var amount)) {
                Debug.LogError($"{pSymbol} didn't exist");
                return;
            }
            if (amount < pAmount) {
                Debug.LogError($"{pSymbol} is lack ({amount}/{pAmount})");
                return;
            }

            _hand[pSymbol] -= pAmount;
            _hand[0] += pAmount;
            ResetRoulette();
        }

        public static void RemoveByPos(int pColumn, int pRow) {
            if (pColumn < 0 || pColumn >= Width || pRow < 0 || pRow >= Height) {
                Debug.LogWarning($"Position must be (0 <= pColumn < {Width} && 0 <= pRow < {Height}) but ({pColumn}, {pRow})");
                return;
            }

            _hand[_current[pColumn][pRow]]--;
            _hand[0]++;
            _current[pColumn].Set(pRow, 0);
        }
        
        public static bool TryAdd(int pSymbol, int pAmount = 1) {
            if (_hand[0] < pAmount)
                return false;
            
            _hand[0] -= pAmount;
            _hand[pSymbol] += pAmount;
            return true;
        }

        public static CellStatus GetStatus(int pColumn, int pRow) =>
            _current[pColumn].GetStatus(pRow);

        public static void Refresh() {
            int idx = 0;
            foreach (var column in _current) {
                column.RefreshStatus(idx++);
            }
        }

        public static IEnumerable<CellInfo> GetColumn(int pColumn) =>
            (CellInfo[])_current[pColumn];
        
        public static int Get(int pColumn, int pRow) {
            if (pColumn < 0 || pColumn >= Width || pRow < 0 || pRow >= Height) {
                Debug.LogWarning($"Position must be (0 <= pColumn < {Width} && 0 <= pRow < {Height}) but ({pColumn}, {pRow})");
                return DataManager.ERROR_SYMBOL;
            }           
            return _current[pColumn][pRow];
        }

        public static bool Use(int pColumn, int pRow, out CellStatus status) {
            status = GetStatus(pColumn, pRow);
            if (status != CellStatus.Usable)
                return false;
            
            _current[pColumn].UseSkill(pRow);
            status = GetStatus(pColumn, pRow);
            return true;
        }

        public static bool Change(int pColumn, int pRow, int pNewItem) {

            var target = _current[pColumn][pRow];
            if (target == pNewItem)
                return false;
            
            _hand[target]--;
            if(!_hand.TryAdd(pNewItem, 1))
                _hand[pNewItem]++;
            _current[pColumn].Set(pRow, pNewItem);
            return true;
        }

        public static List<EvolveInfo> Evolve() {
            var isChanged = false;
            var changed = new List<EvolveInfo>();
            for (int row = 0; row < Height; row++) {
                for (int column = 0; column < Width; column++) {
                    var newCode = SymbolExecutor.Evolution(column, row);

                    if (Change(column, row, newCode)) {
                        isChanged = true;
                        changed.Add(new(column, row, newCode));
                    }
                }
            }

            if (isChanged)
                Refresh();
            return changed;
        }
        
        public static List<(int Column, int Row)> UsableBuff() {

            var result = new List<(int, int)>();
            for (int row = 0; row < Height; row++) {
                for (int column = 0; column < Width; column++) {
                    
                    if(GetStatus(column, row) != CellStatus.Usable)
                        continue;
                    var targetType = DataManager.SymbolDB.GetData(Get(column, row)).Type;
                    if(targetType != SymbolType.Buff)
                        continue;
                    
                    result.Add((column, row));
                }
            }

            return result;
        }
    }
}