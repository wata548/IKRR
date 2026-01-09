using System;
using System.Collections.Generic;
using Data;
using Extension;
using UnityEngine;

namespace UI.SkillShower {

    [Serializable]
    public struct ShowerPositionPair {
        public Positions Positions;
        public SkillShower Shower;
    } 
    
    public class SkillShowerManager: MonoBehaviour {
       //==================================================||Fields 
        [SerializeField] private List<ShowerPositionPair> _kvps;
        private Dictionary<Positions, SkillShower> _showerMatches = new();
        
        //==================================================||Methods
        public void Show(Positions pPosition, string pContext, Action pOnComplete) {
            if (!pPosition.IsFlag()) {
                throw new NotImplementedException();
            }
            _showerMatches[pPosition].Show(pContext, pOnComplete);
        }
        
        //==================================================||Unity 
        private void Awake() {
            foreach (var kvp in _kvps) {
                if (!kvp.Positions.IsFlag())
                    return;
                
                _showerMatches.Add(kvp.Positions, kvp.Shower);
            }
        }
    }
}