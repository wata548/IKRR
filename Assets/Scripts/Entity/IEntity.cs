using System;
using System.Collections.Generic;
using Data;
using Data.Effect;

namespace Character {
    
    public interface IEntity {
        public List<EffectBase> Effects { get; }
        public Positions Position { get; }
        public int MaxHp { get; }
        public int Hp { get; }
        public int Shield { get; }
        public bool IsAlive { get; }

        public void AddShield(int pAmount);
        public void ChangeMaxHp(int pDelta);
        public void AddEffect(EffectBase pEffect);
        public bool HasEffect(int pCode);
        public int AttackDamageCalc(int pAmount, AttackType pType, IEntity pTarget);
        public void OnAttack();
        public void OnSkillUse();
        public void ReceiveDamage(int pAmount, IEntity pOpponent, bool pApplyEffect = true, AttackType pType = AttackType.Swing, Action pOnComplete = null);
        public void OnTurnEnd();
        public void OnTurnStart();
        public void OnRouletteStop();
        public void OnBattleStart();
        
        public void Heal(int pAmount, Action pOnComplete = null);
        public void KillSelf();
    }
}