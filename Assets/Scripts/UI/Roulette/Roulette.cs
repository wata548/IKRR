using System.Collections.Generic;
using System.Linq;
using Extension;
using Roulette;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Roulette {
    public class Roulette: MonoBehaviour {
        //==================================================||Fields
        [SerializeField] private Wheel _wheelPrefab;
        [SerializeField] private Button _lever;
        private RectTransform _rect;
        private readonly List<Wheel> _wheels = new();
        
        //==================================================||Properties
        public bool IsRoll {
            get {
                foreach (var wheel in _wheels) {
                    if (wheel.IsRoll)
                        return true;
                }
                return false;
            }
        }

       //==================================================||Methods 
        public void Roll() {
            foreach (var wheel in _wheels) {
                wheel.StartRoll();
            }
        }

        private void SetUp() {
            _rect = GetComponent<RectTransform>();

            var interval = 1f / RouletteManager.Width;
            var wheelSize = _rect.sizeDelta;
            wheelSize.x *= interval;

            var pos = Vector2.zero;
            for (int i = 0; i < RouletteManager.Width; i++, pos.x += interval) {
                var wheel = Instantiate(_wheelPrefab, transform);
                _wheels.Add(wheel);
                
                wheel.RectTransform.sizeDelta = wheelSize;
                wheel.RectTransform.SetLocalPosition(Pivot.Down, pos);
                wheel.RectTransform.ChangeVirtualPivot(Pivot.Down);

                wheel.Init(i, RouletteManager.Height, RouletteManager.GetColumn(i));
            }
        }

        private void Stop() {
            if (!IsRoll)
                return;
            _wheels.First(wheel => wheel.IsRoll).StopRoll();
        }
        
       //==================================================||Unity 
       private void Start() {
           //TODO: This code is just test code
           RouletteManager.Init(Enumerable.Repeat(1001, 12));

           _lever.onClick.AddListener(Stop);
           SetUp();

           //TODO: This code is just test code
           Roll();
       }
    }
}