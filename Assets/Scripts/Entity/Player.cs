using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Extension.Scene;
using UI;

namespace Character {
    public class Player: IEntity {
        
        //==================================================||Properties 
        public int MaxHp { get; private set; }
        public int Hp { get; private set; }
        public bool IsAlive { get; private set; }
        public List<EffectBase> Effects { get; private set; } = new();

        public Positions Position => Positions.Player;
        //==================================================||Constructor
        public Player(int pMaxHp) {
            Effects.Add(new Burn(new(100)));
            MaxHp = pMaxHp;
            Hp = MaxHp;
            IsAlive = true;
        }
        
        //==================================================||Methods 
        #region ApplyEffect
        public int AttackDamageCalc(int pAmount, IEntity pTarget) =>
            Effects.Aggregate(pAmount, (amount, effect) => effect.OnSendDamage(amount, this, pTarget));
        
        public void OnTurnEnd() {
            foreach (var effect in Effects) {
                effect.OnTurnEnd(this);
            }
            Effects = Effects.Where(effect => effect.Duration > 0).ToList();
        
        }
        
        public void OnTurnStart() {
            foreach (var effect in Effects) {
                effect.OnTurnStart(this);
            }
        }
                
        public void OnSkillUse() {
            foreach (var effect in Effects) {
                effect.OnSkillUse(this);
            }
        }
        
        public void OnKill(IEntity pTarget) {
            foreach (var effect in Effects) {
                effect.OnKill(pTarget);
            }
        }
                
        public void OnRouletteStop() {
            foreach (var effect in Effects) {
                effect.OnRouletteStop(this);
            }
        }

        
        #endregion

        public void OnAttack() {
            
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

    }
}