using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Character.Skill;
using Data;
using UI;
using UI.Character;
using UnityEngine;

namespace Character {
    public class Enemy : IEntity {
        
        //==================================================||Contants 
        private const string PATTERN = @"\s*(?<Skill>.+?)\s*=\s*(?<Appearance>\d+)";
        
        //==================================================||Properties 
        public Positions Position { get; private set; }
        public int SerialNumber { get; private set; }
        public EnemySize Size { get; private set; } 
        public int MaxHp { get; private set; }
        public int Hp { get; private set; }
        public int Exp { get; private set; }
        public int DropMoney { get; private set; }
        public bool IsAlive { get; private set; }
        private List<(int Appearance, ISkill Skill)> _skillAppearance = new();
        public List<EffectBase> Effects { get; private set; } = new();

        //==================================================||Constructors 
        public Enemy(Positions pPosition, int pCode): this(pPosition, DataManager.Enemy.GetData(pCode)){}
        
       
        public Enemy(Positions pPosition, EnemyData pData) {
            SerialNumber = pData.SerialNumber;
            Position = pPosition;
            Size = pData.Size;

            DropMoney = pData.DropMoney.Value;
            Exp = pData.Exp;
            
            MaxHp = pData.MaxHp;
            Hp = MaxHp;
            IsAlive = true;
                        
            SetSkillSet(pData.SkillInfo);
            
            
        }
        
        //==================================================||Methods 

        public void ChangeMaxHp(int pDelta) {
            MaxHp += pDelta;
            Hp = Mathf.Min(MaxHp, Hp);
            UIManager.Instance.Entity.GetEnemyUI(Position).OnMaxHpChange(this, pDelta);
        }
        
        public void OnAttack() {
            UIManager.Instance.Entity.GetEnemyUI(Position).AttackAnimation();
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

        private void OnDeath() {
            PlayerData.GetExp(Exp);
            PlayerData.GetMoney(DropMoney);
        }

        public void ReceiveDamage(int pAmount, IEntity pOpponent, bool pApplyEffect, AttackType pType, Action pOnComplete) {
            if (!IsAlive) {
                pOnComplete!.Invoke();
                return;
            }

            if(pApplyEffect)
                pAmount = Effects.Aggregate(pAmount, (current, effect) => effect.OnReceiveDamage(current, this, pOpponent));

            Hp = Math.Max(0, Hp - pAmount);
            if (Hp == 0) {
                IsAlive = false;
                
                OnDeath();
                CharactersManager.OnDeathEnemy(Position);
                UIManager.Instance.Entity[Position]
                    .OnDeath(this, pAmount, pOnComplete);
                return;
            }
            
            UIManager.Instance.Entity[Position]
                .OnReceiveDamage(this, pAmount, pType, pOnComplete);
        }

        public void Heal(int pAmount, Action pOnComplete) {
            if (!IsAlive) {
                pOnComplete!.Invoke();
                return;
            }

            pAmount = Effects.Aggregate(pAmount, (amount, effect) => effect.OnHeal(pAmount, this));
            Hp = Math.Min(MaxHp, Hp + pAmount);
            
            UIManager.Instance.Entity[Position]
                .OnHeal(this, pAmount, pOnComplete);
        }

        public void KillSelf() {
            Hp = 0;
            IsAlive = false;
        }
        
        public ISkill GetSkill() {
            
            var point = UnityEngine.Random.Range(1, _skillAppearance[^1].Appearance + 1);
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

        public void AddEffect(EffectBase pEffect) {
            var effect = Effects.FirstOrDefault(effect => effect.Code == pEffect.Code);
            if (effect != null) {
                Effects.Remove(effect);
                pEffect += effect;
            }
            Effects.Add(pEffect);
        }

        #region ApplyEffect
        
        private void UpdateEffect() =>
            Effects = Effects.Where(effect => effect.Duration > 0).ToList();

        public int AttackDamageCalc(int pAmount, AttackType pType, IEntity pTarget) {
            
            var value = Effects.Aggregate(pAmount, (amount, effect) => effect.OnSendDamage(amount, pType, this, pTarget));
            UpdateEffect();
            return value;
        }

        public void OnTurnEnd() {
            foreach (var effect in Effects) {
                effect.OnTurnEnd(this);
            }
            Effects = Effects.Where(effect => effect.Duration > 0).ToList();
            UpdateEffect();
        }

        public void OnTurnStart() {
            foreach (var effect in Effects) {
                effect.OnTurnStart(this);
            }
            UpdateEffect();
        }
        
        public void OnSkillUse() {
            foreach (var effect in Effects) {
                effect.OnSkillUse(this);
            }
            UpdateEffect();
        }
        public void OnRouletteStop() {
            foreach (var effect in Effects) {
                effect.OnRouletteStop(this);
            }
            UpdateEffect();
        }
        #endregion
    }
}