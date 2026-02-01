using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

namespace Extension.Effect {
    public class EffectBox: MonoBehaviour {
        [SerializeField] private Vector2Int _tableSize;
        [SerializeField] private Vector2 _padding;

        [SerializeField] private EffectShower _prefab;
        private List<EffectShower> _elements = new();
        private RectTransform _rect;
        
        public void Refresh(List<EffectBase> pEffect) {
            _rect.Place(_elements, new(_padding, pEffect.Count,_tableSize, _prefab, null));
            foreach (var (shower, effect) in _elements.Zip(pEffect, (shower, effect) => (shower, effect))) {
                shower.Set(effect);
            }
        }

        private void Awake() {
            _rect = transform as RectTransform;
        }
    }
}