using System;
using System.Collections.Generic;
using System.Linq;
using Data;

namespace Character.Skill {
    public class SkillComposite: ISkill {

        public bool IsEnd { get; private set; } = true;
        public Action OnEnd { get; set; }
        public int RepeatCount { get; private set; }
        private List<ISkill> _containner = new();

        public void SetRepeatCount(int pRepeatCount) =>
            RepeatCount = pRepeatCount;

        public void AddSkill(ISkill pTarget) =>
            _containner.Add(pTarget);
        
        public void AddSkills(IEnumerable<ISkill> pTargets) =>
            _containner.AddRange(pTargets);

        public SkillComposite(int pRepeatCount = 1) {
            RepeatCount = pRepeatCount;
        }

        public SkillComposite(params ISkill[] pContent) =>
            (RepeatCount, _containner) = (1, pContent.ToList());
        public SkillComposite(int pRepeatAmount, params ISkill[] pContent) {
            RepeatCount = pRepeatAmount;
            _containner = pContent.ToList();
        }
        
        public void Execute(Positions pCaster) {

            IsEnd = false;
            ISkill prevSkill = null;
            foreach (var skill in _containner) {
                for (int i = 0; i < RepeatCount; i++) {
                    
                    if (prevSkill != null) {
                        prevSkill.OnEnd = () => skill.Execute(pCaster);
                    }
                    else
                        skill.Execute(pCaster);

                    prevSkill = skill;
                }
            }

            if (prevSkill != null) {
                prevSkill.OnEnd = OnEnd;
                prevSkill.OnEnd += () => IsEnd = true;
            }
            else {
                IsEnd = true;
            }
        }
    }
}