using System.Collections.Generic;
using Roulette;
using UnityEngine;

namespace UI.SymbolSelector {
    public class SymbolSelector: MonoBehaviour {
        private Queue<int> _addItems = new();
        
        public void Add(int pCode) {
            if (RouletteManager.TryAdd(pCode))
                return;
            _addItems.Enqueue(pCode);
            Debug.Log("sdf");
        }
    }
}