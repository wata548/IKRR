using System;
using System.Linq;
using Extension;
using Lang;
using UnityEngine;

namespace Test {
    public class LanaguageButtonManager: MonoBehaviour {
        [SerializeField] private LanguageChanger _changer;

        private void Start() {
            var targetLanguages = LanguageManager.Table.AllowLanguages();
            var interval = 1f / (targetLanguages.Count() + 1);
            var rect = transform as RectTransform;
            var pivot = new Pivot(PivotLocation.Middle, PivotLocation.Up);
            var pos = Vector2.zero;
            
            foreach (var language in targetLanguages) {
                
                pos.y -= interval;
                var obj = Instantiate(_changer, transform);
                (obj.transform as RectTransform)!.SetLocalPosition(pivot, pos);
                obj.Set(language);
            }
        }
    }
}