using System.Collections.Generic;
using Character;
using Character.Skill.Data;

namespace Data {
    public abstract class EffectBase {
        public int Duration { get; protected set; }
        public abstract int Code { get; }
        public virtual object[] Infos => new object[] { Duration };
        
        public virtual int OnReceiveDamage(int pAmount, IEntity pTarget) => pAmount;
        public virtual int OnSendDamage(int pAmount, IEntity pTarget) => pAmount;
        public virtual int OnHeal(int pAmount, IEntity pTarget) => pAmount;
        public virtual void OnRouletteStop(IEntity pTarget){}
        public virtual void OnTurnStart(IEntity pTarget){}

        public virtual void OnTurnEnd(IEntity pTarget) =>
            Duration--;
        
        public virtual void OnRefresh(IEntity pTarget){}
        public virtual void OnSkillUse(IEntity pTarget){}

        public Info GetInfo() {
            var data = DataManager.Effect.GetData(Code);
            return new Info(data.Name, new List<(string, string, object[])>{("정보", data.Desc, Infos)});
        }
        
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