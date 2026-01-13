using System;
using Character;
using DG.Tweening;
using Extension.Scene;
using UnityEngine;

namespace UI.Character {
    public class PlayerUI: EntityUI {

        [SerializeField] private HpBar _hpBar;
        
        public override void OnReceiveDamage(IEntity pEntity, int pAmount, Action pOnComplete) {
            _hpBar.Damage(pEntity.MaxHp, pEntity.Hp, pAmount)
                .OnComplete(() => pOnComplete?.Invoke());
        }

        public override void OnDeath(IEntity pEntity, int pAmount, Action pOnComplete) {
            SceneManager.LoadScene(Scene.GameOver);
        }

        public override void OnHeal(IEntity pEntity, int pAmount, Action pOnComplete) {
            _hpBar.Heal(pEntity.MaxHp, pEntity.Hp, pAmount)
                .OnComplete(() => pOnComplete?.Invoke());
        }
    }
}