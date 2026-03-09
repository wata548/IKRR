using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Character {
    public class NextAttackSymbol: MonoBehaviour {
        [SerializeField] private Image _shower;

        public void SetStrange() {
            _shower.sprite = Resources.Load<Sprite>($"SkillSymbol/Strange");
        }
        
        public void Set(Type pType) {
            _shower.sprite = Resources.Load<Sprite>($"SkillSymbol/{pType.Name}");
        }
    }
}