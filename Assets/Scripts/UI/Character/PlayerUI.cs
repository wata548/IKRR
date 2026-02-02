using System;
using Character;
using Data;
using DG.Tweening;
using Extension.Scene;
using TMPro;
using UnityEngine;

namespace UI.Character {
    public class PlayerUI: EntityUI {

       //==================================================||Fields 
        [SerializeField] private HpBar _hpBar;
        [SerializeField] private SlideBar _exp;
        [SerializeField] private TMP_Text _moneyShower;
        private int _lastUpdate = -1;
        private int _money = 0;
        
       //==================================================||Methods 
        public override void OnReceiveDamage(IEntity pEntity, int pAmount, AttackType pType, Action pOnComplete) {
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

        private void ExpAndMoneyUpdate() {
            const float ANIMATION_SPEED = 0.3f;
            if (_lastUpdate == PlayerData.LastUpdate)
                return;
            _lastUpdate = PlayerData.LastUpdate;
            _exp.SetWithAnimation(PlayerData.NeedExp, PlayerData.CurExp);
            _moneyShower.DOCounter(_money, PlayerData.Money, ANIMATION_SPEED);
            _money = PlayerData.Money;
        }
        
       //==================================================||Unity 
       protected override Info Info() => null;

       protected override void Update() {
            base.Update();
            ExpAndMoneyUpdate();
        }
    }
}