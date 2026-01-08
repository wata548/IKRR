using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using DG.Tweening;
using Extension;
using UnityEngine;

namespace UI.Status {
    public class StatusShowerManager: MonoSingleton<StatusShowerManager> {
        protected override bool IsNarrowSingleton { get; } = true;

        [SerializeField] private StatusShower _prefab;
        private readonly Dictionary<TargetStatus, StatusShower> _matchShower = new();

        public void Refresh(TargetStatus pStatus, Action pOnComplete) {
            var sequence = DOTween.Sequence();
            var tweens = pStatus
                .Split()
                .Select(status => _matchShower[status].Refresh())
                .Where(tween => tween != null);

            foreach (var tween in tweens) {
                sequence.Append(tween);
            }
            sequence.OnComplete(() => pOnComplete?.Invoke());
        }
        
        protected override void Awake() {
            base.Awake();
            
            var statuses = ((TargetStatus[])Enum.GetValues(typeof(TargetStatus))).Where(status => status.IsFlag());
            var interval = 1f / (statuses.Count() - 1);

            var vPivot = new Pivot(PivotLocation.Down);
            var rect = transform as RectTransform;
            var pos = Vector2.zero;
            RectTransform prev = null;

            var isFirst = true; 
            foreach (var status in statuses) {
                var shower = Instantiate(_prefab, transform);
                shower.SetStatus(status);
                _matchShower.Add(status, shower);
                                
                var showerRect = shower.transform as RectTransform;
                showerRect.SetLocalPosition(rect, Pivot.Down, pos);
                if (isFirst) {
                    isFirst = false;
                    showerRect.ChangeVirtualPivot(Pivot.Down);
                }
                else
                    showerRect.ChangeVirtualPivot(vPivot);
                pos.y += interval;
                prev = showerRect;
            }
            
            prev.ChangeVirtualPivot(new Pivot(PivotLocation.Middle, PivotLocation.Up));
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(transform.position, (transform as RectTransform)!.sizeDelta);
        }
    }
}