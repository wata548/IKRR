using System;
using System.Collections.Generic;
using System.Linq;
using Character.Skill;
using Data;
using UI;
using UnityEngine;

namespace Character {
    public class Enemy : IEntity {
        
        //==================================================||Properties 
        public Positions Position { get; private set; }
        public int SerialNumber { get; private set; }
        public EnemySize Size { get; private set; } 
        public int MaxHp { get; private set; }
        public int Hp { get; private set; }
        public int Shield { get; private set; }
        public int Exp { get; private set; }
        public int DropMoney { get; private set; }
        public bool IsAlive { get; private set; }
        public int Phase { get; set; }
        public ISkill Skill { get; private set; } 
        public List<EffectBase> Effects { get; private set; } = new();
        private IReadOnlyList<PatternInfo> _patterns;
        private IEnumerator<ISkill> _currentSkill;

        //==================================================||Constructors 
        public Enemy(Positions pPosition, int pCode): this(pPosition, DataManager.Enemy.GetData(pCode)){}
        
        public Enemy(Positions pPosition, EnemyData pData) {
            Phase = 0;
            
            SerialNumber = pData.SerialNumber;
            Position = pPosition;
            Size = pData.Size;
            _patterns = pData.PatternData;
            MoveNextSkill();

            DropMoney = pData.DropMoney.Value;
            Exp = pData.Exp;
            
            MaxHp = pData.MaxHp;
            Hp = MaxHp;
            IsAlive = true;
            
            var effects = pData.InitialEffect
                .Split('\n')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => EffectBase.Factory((s)));
            foreach (var effect in effects) {
                AddEffect(effect);
            }
        }
        
        //==================================================||Methods 

        public void AddShield(int pAmount) {
            Shield += Mathf.Max(0, pAmount);
            UIManager.Instance.Entity.GetEnemyUI(Position).RefreshHpBar(this);
        }

        public void ChangeMaxHp(int pDelta) {
            MaxHp += pDelta;
            Hp = Mathf.Min(MaxHp, Hp);
            UIManager.Instance.Entity.GetEnemyUI(Position).RefreshHpBar(this);
        }
        
        public void OnAttack() {
            UIManager.Instance.Entity.GetEnemyUI(Position).AttackAnimation();
        }
        
        private void OnDeath() {
            foreach (var effect in Effects)
                effect.OnDeath(this);
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
            var receive = pAmount;
                        
            var defence = false;           
            if (Shield > 0) {
                defence = true;
                if (receive > Shield) {
                    receive -= Shield;
                    Shield = 0;
                }
                else {
                    Shield -= receive;
                    receive = 0;
                }
                                
            }
            

            Hp = Math.Max(0, Hp - receive);
            if (Hp == 0) {
                IsAlive = false;
                
                OnDeath();
                CharactersManager.OnDeathEnemy(Position);
                UIManager.Instance.Entity[Position]
                    .OnDeath(this, pAmount, pType, pOnComplete);
                return;
            }
            
            UIManager.Instance.Entity[Position]
                .OnReceiveDamage(this, pAmount, pType, defence, pOnComplete);
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

        public void KillSelf(Action pOnComplete = null) {
            Hp = 0;
            IsAlive = false;
            UIManager.Instance.Entity[Position].Run(pOnComplete);
            CharactersManager.OnDeathEnemy(Position);
        }
        
        public void AddEffect(EffectBase pEffect) {
            var effect = Effects.FirstOrDefault(effect => effect.Code == pEffect.Code);
            if (effect != null) {
                Effects.Remove(effect);
                pEffect += effect;
            }
            Effects.Add(pEffect);
            pEffect.OnAdded(this);
        }

        public void ClearEffect() => Effects.Clear();
        public void RemoveEffect(int pType) =>
            Effects = Effects.Where(effect => effect.Code != pType)
                .ToList();

        public bool HasEffect(int pCode) => Effects.Any(effect => effect.Code == pCode);

        public void MoveNextSkill() {
            if (_currentSkill == null || _currentSkill.Current == null) {
                _currentSkill = _patterns
                    .First(pattern => pattern.Usable(this))
                    .GetSkill()
                    .GetEnumerator();
                _currentSkill.MoveNext();
            }
            Skill = _currentSkill.Current;
            _currentSkill.MoveNext();
        }

        #region ApplyEffect
        
        private void UpdateEffect() =>
            Effects = Effects.Where(effect => effect.Duration > 0).ToList();

        public void OnBattleStart() {
            foreach (var effect in Effects) {
                effect.OnBattleStart(this);
            }
            UpdateEffect();
        }
        
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
            UIManager.Instance.Entity[Position].OnTurnEnd();
        }

        public void OnTurnStart() {
            Shield = 0;
            foreach (var effect in Effects) {
                effect.OnTurnStart(this);
            }
            UpdateEffect();
            UIManager.Instance.Entity[Position].OnTurnStart();
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