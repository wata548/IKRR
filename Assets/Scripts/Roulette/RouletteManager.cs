using System.Collections.Generic;
using System.Linq;
using Extension;
using UnityEngine;

namespace Roulette {
    
    public static class RouletteManager {
        private class RouletteColumn {
            public const int MAX_HEIGHT = 8;
            public static int Height { get; private set; } 
            private int[] _column = new int[MAX_HEIGHT];

            public void Clear() {
                for (int i = 0; i < _column.Length; i++)
                    _column[i] = 0;
            }

            public void Set(int pIdx, int pValue) => _column[pIdx] = pValue;
            public int Get(int pIdx) => _column[pIdx];
            public static void SetHeight(int pHeight) => Height = pHeight;
            
            public int Roll(int pIn) {
                var last = _column[Height - 1];
                for (int i = 0; i < Height - 1; i++) {
                    _column[i + 1] = _column[i];
                }

                _column[0] = pIn;
                return last;
            }
        }
        
        //==================================================||Properties 
        public static int Width { get; private set; } = 5;
        public static int Height { get; private set; } = 3;
        public static int HandSize { get; private set; } = 25;
        public static IEnumerable<int> Hand => _hand.SelectMany(kvp => Enumerable.Repeat(kvp.Key, kvp.Value));
        public static IEnumerable<KeyValuePair<int, int>> HandDictionary => _hand;
        
        //==================================================||Fields 
        private static readonly Dictionary<int, int> _hand = new();
        private static readonly Queue<int> _remainSymbol = new();
        private static readonly List<RouletteColumn> _current = Enumerable.Repeat(new RouletteColumn(), Width).ToList();
        
        //==================================================||Methods 
        public static void Roll(int pColumn) {
            if (pColumn > Width) {
                Debug.LogError($"pRow must be lower than {Width}");
                return;
            }

            var symbol = _remainSymbol.Dequeue();
            var @out = _current[pColumn].Roll(symbol);
            _remainSymbol.Enqueue(@out);
        }
        public static void Initialize(IEnumerable<int> pInitHand) {
            _hand.Clear();
            _hand.Add(0, HandSize - _hand.Count);
            foreach (var symbol in pInitHand) {
                if (!_hand.TryAdd(symbol, 1))
                    _hand[symbol]++;
            }
            ResetRoulette();
        }

        public static void ResetRoulette() {
            _remainSymbol.Clear();
            RouletteColumn.SetHeight(Height);
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
                _current[column].Set(row, column);
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
                Debug.LogError($"Position must be (0 <= pColumn < {Width} && 0 <= pRow < {Height}) but ({pColumn}, {pRow})");
                return;
            }

            _hand[_current[pColumn].Get(pRow)]--;
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
    }
}