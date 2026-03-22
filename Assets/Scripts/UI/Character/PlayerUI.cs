using System;
using System.Collections.Generic;
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
        [SerializeField] private InfoSO _info;
        private int _lastUpdate = -1;
        private int _money = 0;
        private Dictionary<string, object> _infoParams = new();
        
       //==================================================||Methods 
       public void Refresh(IEntity pEntity) {
           _hpBar.Set(pEntity.MaxHp, pEntity.Hp);
       }
       
       public override void RefreshHpBar(IEntity pEntity) {
           _hpBar.SetWithAnimation(pEntity.MaxHp, pEntity.Hp, pEntity.Shield);
       }

       public override void OnTurnStart() {
           var shield = CharactersManager.GetEntity(_position).Shield;
            _hpBar.SetShield(shield);
       }

       public override void OnTurnEnd() { }

       public override void OnReceiveDamage(IEntity pEntity, int pAmount, AttackType pType, bool pDefence, Action pOnComplete) {
            _hpBar.Damage(pEntity.MaxHp, pEntity.Hp, pAmount, pEntity.Shield, pDefence)
                .OnComplete(() => pOnComplete?.Invoke());
        }

        public override void OnDeath(IEntity pEntity, int pAmount, AttackType pType, Action pOnComplete) {
            _hpBar.Damage(pEntity.MaxHp, pEntity.Hp, pAmount, pEntity.Shield, false)
                .OnComplete(() => UIManager.Instance.GameOver.Show());
        }

        public override void Run(Action pOnComplete) { }

        public override void OnHeal(IEntity pEntity, int pAmount, Action pOnComplete) {
            _hpBar.Heal(pEntity.MaxHp, pEntity.Hp, pAmount, pEntity.Shield)
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
       public override Info Info() {
           var info = _info.GetInfo();

           _infoParams["Level"] = PlayerData.Level;
           _infoParams["MaxExp"] = PlayerData.NeedExp;
           _infoParams["CurExp"] = PlayerData.CurExp;

           info.Params ??= _infoParams;
           return info;
       }

       protected override void Update() {
            base.Update();
            ExpAndMoneyUpdate();
       }

       private void OnDisable() {
           foreach (var effect in CharactersManager.GetEntity(_position).Effects)
               effect.OnDisable();
       }
    }
}