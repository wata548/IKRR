using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Character;
using Character.Skill.Data;
using CSVData.Extensions;
using Newtonsoft.Json;
using UnityEngine;
using Object = System.Object;

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
        public virtual void OnBattleStart(IEntity pTarget){}
        public virtual int OnReceiveDamage(int pAmount, IEntity pTarget, IEntity pOpponent) => pAmount;
        public virtual int OnSendDamage(int pAmount, AttackType pType, IEntity pTarget, IEntity pOpponent) => pAmount;
        public virtual int OnHeal(int pAmount, IEntity pTarget) => pAmount;
        public virtual void OnRouletteStop(IEntity pTarget){}
        public virtual void OnTurnStart(IEntity pTarget){}
        public virtual void OnAdded(IEntity pTarget){}

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

        private static ConstructorInfo Factory(string pContext, out Object[] args) {
            const string PATTERN = @"(?<Skill>\w*)\{(?<Args>.*)\}";
            var match = Regex.Match(pContext, PATTERN);
            var effectType = Type.GetType("Data." + match.Groups["Skill"].Value)!;
            var rawArgs = match.Groups["Args"].Value.Split('|');
            if (rawArgs.Length == 1 && string.IsNullOrWhiteSpace(rawArgs[0]))
                rawArgs = null;
                        
            var constructor = effectType.GetConstructors()
                .First(constructor => constructor.GetParameters().Length == (rawArgs?.Length ?? 0));
            args = rawArgs?.Zip(constructor.GetParameters(),
                (value, type) => CSharpExtension.Parse(type.ParameterType, value)
            ).ToArray() ?? new object[]{};
               
            return constructor;
            
        }

        public static void Factory(string pContext, Positions pCaster, Positions pTarget) {
            var constructor = Factory(pContext, out var args);
            foreach (var target in CharactersManager.GetEntities(pCaster, pTarget)) {
                var effect = constructor!.Invoke(args) as EffectBase;
                target.AddEffect(effect);
            }
        }
        
        public static EffectBase Factory(string pContext) {
            var constructor = Factory(pContext, out var args);
            return constructor!.Invoke(args) as EffectBase;
        }
    }
}