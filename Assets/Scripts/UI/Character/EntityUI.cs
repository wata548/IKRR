using System;
using System.Linq;
using Character;
using UnityEngine;
using Data;
using Extension.Effect;

namespace UI.Character {
    public abstract class EntityUI: MonoBehaviour {

        [SerializeField] private EffectBox _effectBox;
        [SerializeField] protected Positions _position;
        private int _lastEffectUpdate = 0;
        
        public abstract void OnReceiveDamage(IEntity pEntity, int pAmount, AttackType pType, Action pOnComplete);
        public abstract void OnDeath(IEntity pEntity, int pAmount, Action pOnComplete);
        public abstract void OnHeal(IEntity pEntity, int pAmount, Action pOnComplete);

        private void RefreshEffectBox(IEntity pEntity) {
            var effects = pEntity.Effects;
            if (effects == null || effects.Count == 0)
                return;
            var targetFrame = effects.Max(effect => effect.LastUpdateFrame);
            if (targetFrame == _lastEffectUpdate)
                return;
            
            _lastEffectUpdate = targetFrame;
            _effectBox.Refresh(effects);
        }

        protected virtual void Update() {
            var entity = CharactersManager.GetEntity(_position);
            if (entity is {IsAlive: false})
                return;
            RefreshEffectBox(entity);
        }
    }
}