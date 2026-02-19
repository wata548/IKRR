using System.Collections.Generic;
using Roulette;
using UnityEngine;

namespace UI.SymbolSelector {
    public class SymbolSelector: MonoBehaviour {
        private Queue<int> _addItems = new();
        
        public void Add(int pCode, int pAmount = 1) {
            if (RouletteManager.TryAdd(pCode,  pAmount, out var need))
                return;
            for(int i = 0; i < need; i++)
                _addItems.Enqueue(pCode);
        }
    }
}