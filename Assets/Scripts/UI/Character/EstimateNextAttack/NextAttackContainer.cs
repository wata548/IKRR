using System.Collections.Generic;
using System.Linq;
using Character.Skill;
using Data;
using Extension;
using UnityEngine;

namespace UI.Character {
    public class NextAttackContainer : ShowInfo {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private NextAttackSymbol _symbol;
        [SerializeField] private int _count = 4;
        private List<NextAttackSymbol> _elements = new();
        private ISkill _skill; 
        
        public void Refresh(ISkill pSkill) {
            _skill = pSkill;
            var args = new PlaceArgs<NextAttackSymbol>(
                Vector2.zero,
                1,
                new(1, _count),
                _symbol
            );
            
            if (pSkill is not SkillComposite composite)
                args.Foreach = (symbolShower, idx) =>
                    symbolShower.Set(pSkill.GetType());
            else {
                var textType = typeof(Text);
                var targets = composite
                    .GetElements()
                    .Select(skill => skill.GetType())
                    .Where(skill => skill != textType)
                    .Distinct()
                    .ToList();

                if (targets.Count == 0) {
                    args.Foreach = (symbolShower, idx) =>
                        symbolShower.SetStrange();    
                }
                else {
                    args.Amount = targets.Count;
                    args.Foreach = (symbolShower, idx) =>
                        symbolShower.Set(targets[idx]);    
                }
            }
            _rect.Place(_elements, args);
        }

        protected override Info Info() {
            return new(_skill.GetSkillName(), new() {
                new("설명", _skill.ToString())
            }, null);
        }
    }
}