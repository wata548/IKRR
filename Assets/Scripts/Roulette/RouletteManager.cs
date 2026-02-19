using System.Collections.Generic;
using System.Linq;
using Character.Skill;
using Data;
using Extension;
using Symbol;
using TMPro;
using UnityEngine;

namespace Roulette {
    
    public static partial class RouletteManager {
        
        //==================================================||Constants
        private const int DEFAULT_WIDTH = 5; 
        private const int DEFAULT_HEIGHT = 3;
        private const int DEFAULT_HAND_SIZE = 25;
       
        //==================================================||Properties 
        public const int MAX_HEIGHT = 8;
        public static int Width { get; private set; } = DEFAULT_WIDTH;
        public static int Height { get; private set; } = DEFAULT_HEIGHT;
        public static int HandSize { get; private set; } = DEFAULT_HAND_SIZE;
        public static IEnumerable<int> Hand => _hand.SelectMany(kvp => Enumerable.Repeat(kvp.Key, kvp.Value));
        public static IEnumerable<int> HandKind => _hand.Select(kvp => kvp.Key);
        public static IEnumerable<KeyValuePair<int, int>> HandDictionary => _hand;
        public static int LastUpdateFrame { get; private set; }= 0; 
        public static int UseSymbolCnt { get; private set; } = 0;
        
        //==================================================||Fields 
        private static readonly Dictionary<int, int> _hand = new();
        private static readonly Queue<int> _remainSymbol = new();
        private static List<RouletteColumn> _current = new();
        //==================================================||Methods 

        public static void AddHandSize(int pAmount, int pTarget = DataManager.EMPTY_SYMBOL) {
            HandSize += pAmount;
            if(!_hand.TryAdd(pTarget, pAmount))
                _hand[pTarget] += pAmount;
            foreach(var element in Enumerable.Repeat(pTarget, pAmount))
                _remainSymbol.Enqueue(element);
            
            LastUpdateFrame = Time.frameCount;
        }

        public static void OnTurnStart() {
            UseSymbolCnt = 0;
        }

        public static void OnSkillSymbolUse() =>
            UseSymbolCnt++;
        
        public static void Init(IEnumerable<int> pInitHand) {
            Init(DEFAULT_WIDTH, DEFAULT_HEIGHT, DEFAULT_HAND_SIZE, pInitHand);
        }
        
        public static void Init(int pWidth, int pHeight, int pHandSize, IEnumerable<int> pInitHand) {

            Width = pWidth;
            Height = pHeight;
            HandSize = pHandSize;
            
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
                column.InitStatus();
            }
        }
        
        private static void ResetRoulette() {
            _remainSymbol.Clear();
            foreach (var column in  _current)
                column.Clear();
            
            var idx = -1;
            foreach (var symbol in Hand.ToList().Shuffle()) {
                idx++;
                if (idx >= Width * (Height + 1)) {
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
            LastUpdateFrame = Time.frameCount;
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
            LastUpdateFrame = Time.frameCount;
        }
        
        public static bool TryAdd(int pSymbol, int pAmount, out int notAdded) {
            if (_hand[0] < pAmount) {
                if (_hand[0] == 0) {
                    notAdded = pAmount;
                    return false;
                }

                var temp = _hand[0];
                if(!_hand.TryAdd(pSymbol, temp))
                    _hand[pSymbol] += temp;
                notAdded = pAmount - temp;
                return false;
            }
            
            _hand[0] -= pAmount;
            if(!_hand.TryAdd(pSymbol, pAmount))
                _hand[pSymbol] += pAmount;
            ResetRoulette();
            LastUpdateFrame = Time.frameCount;
            notAdded = 0;
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

        public static void SetStatus(int pColumn, int pRow, CellStatus pStatus) =>
            _current[pColumn].SetStatus(pRow, pStatus);
        
        public static bool Use(int pColumn, int pRow, out CellStatus pStatus, out ISkill pSkill) {
            pStatus = GetStatus(pColumn, pRow);
            pSkill = null;
            if (pStatus != CellStatus.Usable)
                return false;

            pSkill = SymbolExecutor.GetSkill(pColumn, pRow);
            
            pStatus = CellStatus.Used;
            
            return true;
        }

        public static bool Change(int pColumn, int pRow, int pNewItem, bool pCreate) {

            if (pColumn < 0 || pColumn >= Width || pRow < 0 || pRow >= Height)
                return false;
            
            var target = _current[pColumn][pRow];
            if (target == pNewItem)
                return false;
            
            _hand[target]--;
            if(!_hand.TryAdd(pNewItem, 1))
                _hand[pNewItem]++;
            _current[pColumn].SetAndCheckStatus(pColumn, pRow, pNewItem, pCreate);
            LastUpdateFrame = Time.frameCount;
            return true;
        }

        public static Queue<CellAnimationData> GetEvolveTargets() {
            var changed = new Queue<CellAnimationData>();
            for (int column = 0; column < Width; column++) {
                for (int row = Height - 1; row >= 0; row--) {
                    var code = Get(column, row);
                    if(string.IsNullOrWhiteSpace(DataManager.Symbol.GetData(code).EvolveCondition))
                        continue;
                    changed.Enqueue(new(AnimationType.Evolve, column, row));
                }
            }
            return changed;
        }
        
        public static Queue<CellAnimationData> GetUsableBuffs() {

            var result = new Queue<CellAnimationData>();
            for (int column = 0; column < Width; column++) {
                for (int row = Height - 1; row >= 0; row--) {
                    
                    if(GetStatus(column, row) != CellStatus.Usable)
                        continue;
                    var targetType = DataManager.Symbol.GetData(Get(column, row)).Type;
                    if(targetType != SymbolType.Buff)
                        continue;
                    
                    result.Enqueue(new(AnimationType.Buff, column, row));
                }
            }

            return result;
        }
    }
}