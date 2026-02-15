using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Extension.Scene;
using UI;
using UnityEngine;

namespace Character {
    public class JsonPlayerData {
        public int MaxHp { get; set; }
        public int Hp { get; set; }
        public bool IsAlive { get; set; }
        public List<EffectBase> Effects { get; set; } = new();
        
        public JsonPlayerData(){}
        public JsonPlayerData(Player pPlayer) {
            Hp = pPlayer.Hp;
            MaxHp = pPlayer.MaxHp;
            IsAlive = pPlayer.IsAlive;
            Effects = pPlayer.Effects;
        }
    }
    
    public class Player: IEntity {
        
        //==================================================||Properties 
        public int MaxHp { get; private set; }
        public int Hp { get; private set; }
        public bool IsAlive { get; private set; }
        public List<EffectBase> Effects { get; private set; } = new();

        public Positions Position => Positions.Player;
        //==================================================||Constructor
        public Player(){}

        public Player(JsonPlayerData pData) {
            MaxHp = pData.MaxHp;
            Hp = pData.Hp;
            Effects = pData.Effects;
            IsAlive = pData.IsAlive;
        }
        
        public Player(int pMaxHp) {
            MaxHp = pMaxHp;
            Hp = MaxHp;
            IsAlive = true;
        }
        
        //==================================================||Methods 
        public void AddEffect(EffectBase pEffect) {
            var effect = Effects.FirstOrDefault(effect => effect.Code == pEffect.Code);
            if (effect != null) {
                Effects.Remove(effect);
                pEffect += effect;
            }
            Effects.Add(pEffect);
        }

        public void OnAttack() {
            
        }
        
        public void ChangeMaxHp(int pDelta) {
            MaxHp += pDelta;
            Hp = Mathf.Min(MaxHp, Hp);
            UIManager.Instance.Entity.GetEnemyUI(Position).OnMaxHpChange(this, pDelta);
        }
        
        public void ReceiveDamage(int pAmount,IEntity pOpponent, bool pApplyEffect, AttackType pType, Action pOnComplete) {
            if (!IsAlive) {
                pOnComplete!.Invoke();
                return;
            }
            
            pAmount = Effects.Aggregate(pAmount, (current, effect) => effect.OnReceiveDamage(current, this, pOpponent));
            
            Hp = Math.Max(0, Hp - pAmount);
            if (Hp == 0) {
                IsAlive = false;
             
                UIManager.Instance.Entity[Position].
                    OnDeath(this, pAmount, pOnComplete);
                
                return;
            }
             
            UIManager.Instance.Entity[Position].
                OnReceiveDamage(this, pAmount, pType, pOnComplete);
        }

        public void Heal(int pAmount, Action pOnComplete) {
            if (!IsAlive) {
                pOnComplete!.Invoke();
                return;
            }
            
            Hp = Math.Min(MaxHp, Hp + pAmount);
             
            UIManager.Instance.Entity[Positions.Player].
                OnHeal(this, pAmount, pOnComplete);
        }

        public void KillSelf() {
            SceneManager.LoadScene(Scene.GameOver);
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
        }
        
        public void OnKill(IEntity pDead) {
            foreach (var effect in Effects) {
                effect.OnKill(pDead);
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