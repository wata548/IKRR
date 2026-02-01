using System.Collections.Generic;
using Character;
using Character.Skill.Data;
using UnityEngine;

namespace Data {
    public abstract class EffectBase {
        public int Duration { get; protected set; }
        public int LastUpdateFrame { get; private set; } = -1;
        public abstract int Code { get; }
        public virtual Dictionary<string, object> Infos => new() {
            {"Duration", Duration}
        };

        public virtual int ShowCount => Duration;

        protected void Update() => LastUpdateFrame = Time.frameCount;
        public virtual int OnReceiveDamage(int pAmount, IEntity pTarget, IEntity pOpponent) => pAmount;
        public virtual int OnSendDamage(int pAmount, IEntity pTarget, IEntity pOpponent) => pAmount;
        public virtual int OnHeal(int pAmount, IEntity pTarget) => pAmount;
        public virtual void OnRouletteStop(IEntity pTarget){}
        public virtual void OnTurnStart(IEntity pTarget){}

        public virtual void OnTurnEnd(IEntity pTarget) {
            Duration--;
            Update();
        }
        
        public virtual void OnSkillUse(IEntity pTarget){}
        public virtual void OnKill(IEntity pTarget){}

        public Info GetInfo() {
            var data = DataManager.Effect.GetData(Code);
            return new Info(data.Name, new List<(string, string, Dictionary<string, object>)> {("정보", data.Desc, Infos)});
        }
        
        protected EffectBase(RangeValue pDuration) =>
            Duration = pDuration.Value;

        protected virtual EffectBase AddOperation(EffectBase rhs) {
            Duration += rhs.Duration;
            Update();
            return this;
        }
        
        public static EffectBase operator+(EffectBase lhs, EffectBase rhs) {
            return lhs.AddOperation(rhs);
        }
    }
}