using System.Collections.Generic;
using System.Linq;
using Extension;
using UI.Character;
using UnityEngine;

namespace Data {
    public class DamageEffectPool: MonoSingleton<DamageEffectPool> {
        protected override bool IsNarrowSingleton => true;
        
        [SerializeField] private DamageEffect _effect;
        [SerializeField] private Canvas _canvas;
        private List<DamageEffect> _pool = new();
        private Transform _folder;

        public DamageEffect GetEffect() {
            var candi = _pool.FirstOrDefault(effect => !effect.IsPlaying);
            if (candi is not null)
                return candi;

            var effect = Instantiate(_effect, _folder);
            _pool.Add(effect);
            return effect;
        }

        protected override void Awake() {
            base.Awake();
            _folder = new GameObject("Damages").transform;
            _folder.transform.parent = _canvas.transform;
            _folder.localPosition = Vector3.zero;
            _folder.localScale = Vector3.one;
        }
    }
}