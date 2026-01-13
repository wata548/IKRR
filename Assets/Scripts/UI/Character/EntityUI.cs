using System;
using Character;
using UnityEngine;

namespace UI.Character {
    public abstract class EntityUI: MonoBehaviour {
        public abstract void OnReceiveDamage(IEntity pEntity, int pAmount, Action pOnComplete);
        public abstract void OnDeath(IEntity pEntity, int pAmount, Action pOnComplete);
        public abstract void OnHeal(IEntity pEntity, int pAmount, Action pOnComplete);
    }
}