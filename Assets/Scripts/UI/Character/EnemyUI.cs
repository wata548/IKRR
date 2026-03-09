using System;
using System.Collections;
using Character;
using UnityEngine;
using Data;
using DG.Tweening;
using UI.Icon;
using UnityEngine.UI;

namespace UI.Character {
    public class EnemyUI: EntityUI {
        //==================================================||Fields
        [SerializeField] private HpBar _hpBar;
        [SerializeField] private Image _shower;
        [SerializeField] private Button _button;
        [SerializeField] private NextAttackContainer _nextAttack;
        private Tween _idleAnimation;
        private Tween _attackAnimation;
        private Vector3? _origin = null;
        private Tween _shake = null;

        private EnemySize _size;
        //==================================================||Methods 
        private void RefreshNextAttackData() {
            var skill = (CharactersManager.GetEntity(_position) as Enemy)!.Skill;
            _nextAttack.Refresh(skill);
        } 
        
        public void SetMaterial(string pMaterialName) {
            _shower.material = MaterialStore.Get(pMaterialName);
        }
        
        public void SetData(Enemy pData) {
            gameObject.SetActive(true);
            _size = pData.Size;
            
            RefreshEffectBox(true);
            _hpBar.Set(pData.MaxHp, pData.MaxHp);
            _shower.sprite = pData.SerialNumber.GetIcon();
            transform.localScale = (float)pData.Size / 100f * Vector3.one;

            RefreshNextAttackData();
        }
        
        public override void RefreshHpBar(IEntity pEntity) {
            _hpBar.SetWithAnimation(pEntity.MaxHp, pEntity.Hp, pEntity.Shield);
        }

        public override void OnTurnStart() {
            var shield = CharactersManager.GetEntity(_position).Shield;
            _hpBar.SetShield(shield);
        }
        
        public override void OnTurnEnd() =>
            RefreshNextAttackData();

        public virtual void AttackAnimation() {
            _attackAnimation?.Kill();
            _attackAnimation = DOTween.Sequence()
                .Append(DOTween.Sequence()
                    .Append(_shower.transform.DOLocalRotate(Vector3.forward * -20, 0.3f))
                    .Append(_shower.transform.DOLocalRotate(Vector3.zero, 0.12f).SetEase(Ease.OutBack))
                );
        } 
        
        /*private void IdleAnimation() {
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
        }*/

        private void PlayVfx(AttackType pType) {
            
            _shake?.Kill();
            var localPos = transform.localPosition;
            _shake = _shower.transform.DOShakePosition(0.4f, new Vector3(10f, 3f)).OnKill(() => transform.localPosition = localPos);
            
            var vfx = VFXManager.Get(pType.ToString());
            var defaultVfx = VFXManager.Get("Damage");
            
            var pos = transform.position;
            var height = (transform as RectTransform)!.rect.height * transform.lossyScale.y / 2f;
            pos.y += height;
            if (vfx is not null) {
                vfx.transform.position = pos;
                //vfx.ApplySize(_size);
                vfx.Play();
            }
            defaultVfx.transform.position = pos;
            defaultVfx.Play();
        }
        
        public override void OnReceiveDamage(IEntity pEntity, int pAmount, AttackType pType, bool pDefence, Action pOnComplete) {
            
            PlayVfx(pType);   
            _hpBar.Damage(pEntity.MaxHp, pEntity.Hp, pAmount, pEntity.Shield, pDefence)
                .OnComplete(() => pOnComplete?.Invoke());
        }

        public override void OnDeath(IEntity pEntity, int pAmount, AttackType pType, Action pOnComplete) {
            PlayVfx(pType);   
            _hpBar.Damage(pEntity.MaxHp, pEntity.Hp, pAmount, 0, false)
                .OnComplete(() => StartCoroutine(Death()));
            IEnumerator Death() {
                pOnComplete?.Invoke(); 
                const float DEATH_ANIMATION = 1f;
                var mat = _shower.material;
                var deathMat = MaterialStore.Get("Death");
                _shower.material = deathMat;
                
                var time = 0f;
                while (time < DEATH_ANIMATION) {
                    time += Time.deltaTime;
                    deathMat.SetFloat("_CurTime", time/DEATH_ANIMATION); yield return null; }

                _shower.material = mat;
                gameObject.SetActive(false);
            }
        }

        public override void Run(Action pOnComplete) {
            StartCoroutine(Run());
            IEnumerator Run() {
                const float DURATION = 0.8f;
                
                var time = 0f;
                var color = _shower.color;
                while (time < DURATION) {
                    time += Time.deltaTime;
                    color.a = 1 - time / DURATION;
                    _shower.color = color;
                    yield return null; 
                }

                pOnComplete?.Invoke();
                gameObject.SetActive(false);
                color.a = 1;
                _shower.color = color;
            }   
        }

        public override void OnHeal(IEntity pEntity, int pAmount, Action pOnComplete) {
            _hpBar.Heal(pEntity.MaxHp, pEntity.Hp, pAmount, pEntity.Shield)
                .OnComplete(() => pOnComplete?.Invoke());
        }

        private void OnClick() {
            CharactersManager.TargetEnemy = _position;
        }
        public override Info Info() {
            var code = (CharactersManager.GetEntity(_position) as Enemy)!.SerialNumber;
            return DataManager.Enemy.GetData(code).GetInfo();
        }
        //==================================================||Unity 
        protected virtual void Awake() {
            _button.onClick.AddListener(OnClick);
            gameObject.SetActive(false);
        }
    }
}