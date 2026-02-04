using System;
using System.Collections;
using Character;
using UnityEngine;
using Data;
using DG.Tweening;
using UI.Icon;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Character {
    public class EnemyUI: EntityUI {
        //==================================================||Fields
        [SerializeField] private HpBar _hpBar;
        [SerializeField] private Image _shower;
        [SerializeField] private Button _button;
        private Tween _idleAnimation;
        private Tween _attackAnimation;
        private Vector3? _origin = null;
        
        //==================================================||Methods 
        public void SetMaterial(string pMaterialName) {
            _shower.material = MaterialStore.Get(pMaterialName);
        }
        
        public void SetData(Enemy pData) {
            gameObject.SetActive(true);
            
            RefreshEffectBox(true);
            _hpBar.Set(pData.MaxHp, pData.MaxHp);
            _shower.sprite = pData.SerialNumber.GetIcon();
            transform.localScale = (float)pData.Size / 100f * Vector3.one;
        }

        public virtual void AttackAnimation() {
            _attackAnimation?.Kill();
            _attackAnimation = DOTween.Sequence()
                .Append(DOTween.Sequence()
                    .Append(_shower.transform.DOLocalRotate(Vector3.forward * -20, 0.3f))
                    .Append(_shower.transform.DOLocalRotate(Vector3.zero, 0.12f).SetEase(Ease.OutBack))
                );
        } 
        
        private void IdleAnimation() {
            const float VERTICAL_MOVEMENT = 0.015f; 
            const float STRETCH_RATIO = 1.03f; 
            const float ANIMATION_SPEED = 0.8f; 
            
            var posY = _shower.rectTransform.sizeDelta.y * _shower.transform.localScale.y * VERTICAL_MOVEMENT
                       + _shower.rectTransform.localPosition.y;
            
            _idleAnimation?.Kill();
            _origin ??= _shower.transform.localPosition;
            _shower.transform.localPosition = (Vector3)_origin;
            _idleAnimation = DOTween.Sequence()
                .Append(_shower.transform.DOLocalMoveY(posY, ANIMATION_SPEED))
                .Join(_shower.transform.DOScaleY(STRETCH_RATIO, ANIMATION_SPEED))
                .SetEase(Ease.OutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }
        
        public override void OnReceiveDamage(IEntity pEntity, int pAmount, AttackType pType, Action pOnComplete) {
            _hpBar.Damage(pEntity.MaxHp, pEntity.Hp, pAmount)
                .OnComplete(() => pOnComplete?.Invoke());
        }

        public override void OnDeath(IEntity pEntity, int pAmount, Action pOnComplete) {
            pOnComplete += () => gameObject.SetActive(false);
            _hpBar.Damage(pEntity.MaxHp, pEntity.Hp, pAmount)
                .OnComplete(() => StartCoroutine(Death()));
            IEnumerator Death() {
                const float DEATH_ANIMATION = 1f;
                var mat = _shower.material;
                var deathMat = MaterialStore.Get("Death");
                _shower.material = deathMat;
                
                var time = 0f;
                while (time < DEATH_ANIMATION) {
                    time += Time.deltaTime;
                    deathMat.SetFloat("_CurTime", time/DEATH_ANIMATION); yield return null; }

                _shower.material = mat;
                pOnComplete?.Invoke();
            }
        }

        public override void OnHeal(IEntity pEntity, int pAmount, Action pOnComplete) {
            _hpBar.Damage(pEntity.MaxHp, pEntity.Hp, pAmount)
                .OnComplete(() => pOnComplete?.Invoke());
        }

        private void OnClick() {
            CharactersManager.TargetEnemy = _position;
        }
        
        //==================================================||Unity 
        
        protected void OnBecameVisible() {
            IdleAnimation();
        }

        protected virtual void Awake() {
            _button.onClick.AddListener(OnClick);
            gameObject.SetActive(false);
        }

        protected override Info Info() {
            var code = (CharactersManager.GetEntity(_position) as Enemy)!.SerialNumber;
            return DataManager.Enemy.GetData(code).GetInfo();
        }
    }
}