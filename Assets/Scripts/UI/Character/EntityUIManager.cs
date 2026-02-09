using System;
using System.Linq;
using System.Collections.Generic;
using Character;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Character {
    public class EntityUIManager: MonoBehaviour {
        
        [Serializable]
        public class MatchPositionEnemyUI {
            public Positions Position;
            public EntityUI UI;
        }

        //==================================================||Fields 
        [SerializeField] private List<MatchPositionEnemyUI> _matchList;
        private Dictionary<Positions, EntityUI> _uis;
        private Positions _target = Positions.None;

        //==================================================||Properties 
        public EntityUI this[Positions pPosition] => _uis[pPosition];
        public PlayerUI Player => _uis[Positions.Player] as PlayerUI;

       //==================================================||Methods 
        public EnemyUI GetEnemyUI(Positions pPosition) {
            if (pPosition == Positions.Player)
                throw new ArgumentException("Player isn't enemy");
            return (this[pPosition] as EnemyUI)!;
        } 

        
        public void SetTarget(Positions pTarget) {
            const string OUTLINE = "OutLine";
            const string TARGET_OUTLINE = "TargetOutLine"; 
            
            if (pTarget == _target)
                return;
            
            if(_target != Positions.None)
                GetEnemyUI(_target).SetMaterial(OUTLINE);
            GetEnemyUI(pTarget).SetMaterial(TARGET_OUTLINE);
            _target = pTarget;
        }
        
       //==================================================||Unity 
        private void Awake() {
            _uis = _matchList.ToDictionary(match => match.Position, match => match.UI);
        }
    }
}