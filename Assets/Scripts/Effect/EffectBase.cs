using System.Collections.Generic;
using Character;
using Character.Skill.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace Data {
    public abstract class EffectBase {
        public int Duration { get; protected set; }
        public abstract int Code { get; }
        
        [JsonIgnore]
        public int LastUpdateFrame { get; private set; } = -1;
        
        [JsonIgnore]
        public virtual Dictionary<string, object> Infos => new() {
            {"Duration", Duration}
        };

        [JsonIgnore]
        public virtual int ShowCount => Duration;

        protected void Update() => LastUpdateFrame = Time.frameCount;
        public virtual int OnReceiveDamage(int pAmount, IEntity pTarget, IEntity pOpponent) => pAmount;
        public virtual int OnSendDamage(int pAmount, AttackType pType, IEntity pTarget, IEntity pOpponent) => pAmount;
        public virtual int OnHeal(int pAmount, IEntity pTarget) => pAmount;
        public virtual void OnRouletteStop(IEntity pTarget){}
        public virtual void OnTurnStart(IEntity pTarget){}

        public virtual void OnTurnEnd(IEntity pTarget) {
            Duration--;
            Update();
        }
        public virtual void OnSkillUse(IEntity pTarget){}
        public virtual void OnKill(IEntity pDead){}

        public Info GetInfo() {
            var data = DataManager.Effect.GetData(Code);
            return new Info(data.Name, new(){new("정보", data.Desc)}, Infos);
        }

        protected EffectBase(){}
        
        protected EffectBase(RangeValue pDuration) {
            Duration = pDuration.Value;
            pDuration.Next();
            Update();
        }

        protected virtual EffectBase AddOperation(EffectBase rhs) {
            Duration += rhs.Duration;
            return this;
        }
        
        public static EffectBase operator+(EffectBase lhs, EffectBase rhs) {
            var temp = lhs.AddOperation(rhs);
            temp.Update();
            return temp;
        }
    }
}