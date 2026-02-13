using System;
using System.Linq;
using Character;
using UnityEngine;
using Data;
using Extension.Effect;

namespace UI.Character {
    public abstract class EntityUI: ShowInfo {

        [SerializeField] private EffectBox _effectBox;
        [SerializeField] protected Positions _position;
        private int _lastEffectUpdate = 0;
        private int _lastEffectCnt = 0;
        
        public abstract void OnReceiveDamage(IEntity pEntity, int pAmount, AttackType pType, Action pOnComplete);
        public abstract void OnDeath(IEntity pEntity, int pAmount, Action pOnComplete);
        public abstract void OnHeal(IEntity pEntity, int pAmount, Action pOnComplete);

        protected void RefreshEffectBox(bool pForce = false) {
            var entity = CharactersManager.GetEntity(_position);
            if (entity is {IsAlive: false})
                return;
            
            var effects = entity.Effects;
            if (effects == null)
                return;
            if (effects.Count == 0) {
                _effectBox.Clear();
                return;
            }

            pForce |= _lastEffectCnt != effects.Count;
            _lastEffectCnt = effects.Count;
            var targetFrame = effects.Max(effect => effect.LastUpdateFrame);
            if (!pForce && targetFrame == _lastEffectUpdate)
                return;
            
            _lastEffectUpdate = targetFrame;
            _effectBox.Refresh(effects);
        }

        protected virtual void Update() {
            base.Update();
            
            RefreshEffectBox();
        }
    }
}