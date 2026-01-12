using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Character.Skill;
using Data;
using UI;

namespace Character {
    public class Enemy : IEntity {
        
        //==================================================||Contants 
        private const string PATTERN = @"\s*(?<Skill>.+?)\s*=\s*(?<Appearance>\d+)";
        
        //==================================================||Properties 
        public Positions Position { get; private set; }
        public EnemySize Size { get; private set; } 
        public string Name{ get; private set; }
        public int MaxHp { get; private set; }
        public int Hp { get; private set; }
        public bool IsAlive { get; private set; }
        private List<(int Appearance, ISkill Skill)> _skillAppearance = new();

       //==================================================||Constructors 
        public Enemy(Positions pPosition, EnemyData pData) {
            Position = pPosition;
            Name = pData.Name;
            Size = pData.Size;
                        
            MaxHp = pData.MaxHp;
            Hp = MaxHp;
            IsAlive = true;
                        
            SetSkillSet(pData.SkillInfo);
            CharactersManager.SetEntity(Position, this);
        }
        
       //==================================================||Methods 
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
        
        public void ReceiveDamage(int pAmount, Action pOnComplete) {
            if (!IsAlive) {
                pOnComplete!.Invoke();
                return;
            }
            
            Hp = Math.Max(0, Hp - pAmount);
            if (Hp == 0) {
                IsAlive = false;
                UIManager.Instance.Enemy[Position]
                    .OnDeath(this, pAmount, pOnComplete);
                return;
            }
            
            UIManager.Instance.Enemy[Position]
                .OnReceiveDamage(this, pAmount, pOnComplete);
        }

        public void Heal(int pAmount, Action pOnComplete) {
            if (!IsAlive) {
                pOnComplete!.Invoke();
                return;
            }
            
            Hp = Math.Min(MaxHp, Hp + pAmount);
            
            UIManager.Instance.Enemy[Position]
                .OnHeal(this, pAmount, pOnComplete);
        }

        public void Kill() {
            Hp = 0;
            IsAlive = false;
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
    }
}