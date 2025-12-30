using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Character.Skill;
using UnityEngine;
using Data;

namespace Character.Entity {
    public abstract class EnemyBase: MonoBehaviour, IEntity {

        private const string PATTERN = @"\s*(?<Skill>.+?)\s*=\s*(?<Appearance>\d+)";
        
        //==================================================||Fields
        private List<(int Appearance, ISkill Skill)> _skillAppearance = new();  
        
        //==================================================||Properties 
        public Positions Position { get; private set; }
        public EenemySize Size{ get; private set; }
        public string Name{ get; private set; }
        public int MaxHp { get; private set; }
        public int Hp { get; private set; }
        public bool IsAlive { get; private set; }
        
        //==================================================||Methods 
        public void SetUp(IEnemyData pData, Positions pPosition) {

            Name = pData.Name;
            Size = pData.Size;
            
            MaxHp = pData.MaxHp;
            Hp = MaxHp;
            IsAlive = true;
            
            Position = pPosition;
            SetSkillSet(pData.SkillInfo);
        }
       
        public void ReceiveDamage(int pAmount) {
            
            Hp = Math.Max(0, Hp - pAmount);
            OnReceiveDamage(pAmount);
            if (Hp == 0) {
                IsAlive = false;
                OnDeath();
            }
        }

        public void Heal(int pAmount) {
            Hp = Math.Min(MaxHp, Hp + pAmount);
            OnHeal(pAmount);
        }

        private void SetSkillSet(string pSkillSet) {
            
            _skillAppearance.Clear();
            var matches = Regex.Matches(pSkillSet, PATTERN);
                        
            foreach (Match match in matches) {
                var appearance = int.Parse(match.Groups["Appearance"].Value);
                //prefix
                if (_skillAppearance.Count > 0)
                    appearance += _skillAppearance[^1].Appearance;
                            
                var skill = SkillInterpreter.Interpret(match.Groups["Skill"].Value);
                _skillAppearance.Add((appearance, skill));
            }
        }
        
        public ISkill GetSkill() {
            
            var point = UnityEngine.Random.Range(1, _skillAppearance[^1].Appearance);
            var start = 0;
            var end = _skillAppearance.Count - 1;
            while (start < end) {
                var middle = (start + end) / 2;
                var compare = point.CompareTo(_skillAppearance[middle].Appearance);
                
                if (compare == 0) {
                    start = middle;
                    break;
                }
                if (compare > 0)
                    start = middle + 1;
                else
                    end = middle;
            }

            var skill = _skillAppearance[start].Skill;
            return skill;
        }
        
        protected virtual void OnReceiveDamage(int pAmount) =>
            Debug.Log($"{Name}({gameObject.name}) receive damage {pAmount}");

        protected virtual void OnDeath() =>
            Debug.Log($"{Name}({gameObject.name}) is death");

        protected virtual void OnHeal(int pAmount) =>
            Debug.Log($"{Name}({gameObject.name}) heal {pAmount}");
    }
}