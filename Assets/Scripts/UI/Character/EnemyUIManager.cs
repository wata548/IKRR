using System;
using System.Linq;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace UI.Character {
    public class EnemyUIManager: MonoBehaviour {
        [Serializable]
        public class MatchPositionEnemyUI {
            public Positions Position;
            public EnemyUI UI;
        }

        [SerializeField] private List<MatchPositionEnemyUI> _matchList;
        private Dictionary<Positions, EnemyUI> _uis;

        public EnemyUI this[Positions pPosition] => _uis[pPosition];
        public EnemyUI Get(Positions pPosition) => this[pPosition]; 
        
        private void Awake() {
            _uis = _matchList.ToDictionary(match => match.Position, match => match.UI);
        }
    }
}