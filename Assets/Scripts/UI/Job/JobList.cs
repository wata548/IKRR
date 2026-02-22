using System.Collections.Generic;
using System.Linq;
using Data;
using Extension;
using UnityEngine;

namespace UI.Job {
    public class JobList: MonoBehaviour {
        [SerializeField] private JobButton _prefab;
        private List<JobButton> _buttonPool = new();
        
        private void Start() {
            var element = DataManager.Job.Keys.ToList();
            var rect = transform as RectTransform;
            var args = new PlaceArgs<JobButton>(
                Vector2.zero,
                element.Count,
                new(element.Count, 1),
                _prefab,
                null,
                (button, idx) => button.Set(element[idx])
            );
            
            rect.Place(_buttonPool, args);

        }
    }
}