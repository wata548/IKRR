using Character;
using Character.Skill.Data;

namespace Data {
    public abstract class EffectBase {
        public int Duration { get; protected set; }
        
        public virtual int OnReceiveDamage(int pAmount, IEntity pTarget) => pAmount;
        public virtual int OnSendDamage(int pAmount, IEntity pTarget) => pAmount;
        public virtual int OnHeal(int pAmount, IEntity pTarget) => pAmount;
        public virtual void OnRouletteStop(IEntity pTarget){}
        public virtual void OnTurnStart(IEntity pTarget){}

        public virtual void OnTurnEnd(IEntity pTarget) =>
            Duration--;
        
        public virtual void OnRefresh(IEntity pTarget){}
        public virtual void OnSkillUse(IEntity pTarget){}

        public abstract Info GetInfo();
        
        protected EffectBase(RangeValue pDuration) =>
            Duration = pDuration.Value;

        protected virtual EffectBase AddOperation(EffectBase rhs) {
            Duration += rhs.Duration;
            return this;
        }
        
        public static EffectBase operator+(EffectBase lhs, EffectBase rhs) {
            return lhs.AddOperation(rhs);
        }
    }
}