using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Extension;
using Extension.Test;
using UI.Roulette;
using UnityEngine;

namespace UI.Reward {
    public class RewardWindow: MonoBehaviour {
        [SerializeField] private FakeWheel _wheel;
        [SerializeField] private RectTransform _roulette;
        [field:SerializeField]public int Width { get; private set; } = 3;
        private List<FakeWheel> _wheels = new();
        public bool IsStop => !_wheels.Any(wheel => wheel.IsRoll);

        public void TurnOn() {
            var candidate = DataManager.Symbol.Keys.ToList();
            foreach (var wheel in _wheels) {
                var candidates = DataManager.Symbol.Query(new(DataManager.LevelUp.GetRarity()));
                var result = candidates[Random.Range(0, candidates.Count)];
                Debug.Log(result);
                wheel.SetResult(result);
                wheel.Init(-1, 1, candidate, null, null);
                wheel.StartRoll();
            }
        }

        public void Stop() {
            if (IsStop)
                return;
            var target = _wheels.First(wheel => wheel.IsRoll);
            target.Stop();
        } 
        
        private void SetUp() {
            var wheelWidth = (_wheel.transform as RectTransform)!.sizeDelta.x;
            var size = _roulette.sizeDelta;
            size.x = wheelWidth * Width;
            _roulette.sizeDelta = size;
            var pos = new Vector3((wheelWidth - size.x) * 0.5f, 0);
            
            while (_wheels.Count < Width) {
                var wheel = Instantiate(_wheel, _roulette);
                _wheels.Add(wheel);
            }
            
            foreach (var wheel in _wheels) {
                wheel.transform.localPosition = pos;
                pos.x += wheelWidth;
            }
        }
        
        private void Start() {
            SetUp();
            TurnOn();
        }
    }
}