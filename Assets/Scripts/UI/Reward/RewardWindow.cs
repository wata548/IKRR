using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Extension;
using Extension.Test;
using UI.Roulette;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Reward {
    public class RewardWindow: MonoBehaviour {
        [Header("Roulette")]
        [SerializeField] private RectTransform _roulette;
        [SerializeField] private FakeWheel _wheel;
        [SerializeField] private Button _button;
        [SerializeField] private float _buttonTerm = 140;

        [Space, Header("Window")] 
        [SerializeField] private GameObject _pannel;
        [SerializeField] private Button _close;
        [field:SerializeField]public int Width { get; private set; } = 3;
        private readonly List<FakeWheel> _wheels = new();
        private readonly List<Button> _buttons = new();
        private readonly List<int> _rewards = new();
        private int _idx = 0;

        public bool IsActive { get; private set; } = false;

        [TestMethod]
        public void TurnOn() {
            _idx = 0;
            IsActive = true;
            _pannel.SetActive(true);
            SetUp();
            _rewards.Clear();
            var candidate = DataManager.Symbol.Keys.ToList();
            foreach (var wheel in _wheels) {
                var candidates = DataManager.Symbol.Query(
                    new SymbolQueryArgs(DataManager.LevelUp.GetRarity())
                );
                var result = DataManager.ERROR_SYMBOL;
                if(candidates.Count > 0)
                    result = candidates[Random.Range(0, candidates.Count)];
                _rewards.Add(result);
                Debug.Log(result);
                wheel.SetResult(result);
                wheel.Init(-1, 1, candidate, null, null);
                wheel.StartRoll();
            }
        }

        private void TurnOff() {
            foreach (var wheel in _wheels) {
                wheel.Stop();
            }
            _pannel.SetActive(false);
            IsActive = false;
        }

        public void Stop() {
            if (_idx >= Width)
                return;
            _wheels[_idx++].Stop();
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
                var button = Instantiate(_button, _roulette);
                var idx = _buttons.Count;
                button.onClick.AddListener(() => Select(idx));
                _buttons.Add(button);
            }
            
            foreach (var (wheel,button) in _wheels.Zip(_buttons, (wheel, button) => (wheel, button))) {
                wheel.transform.localPosition = pos;
                button.transform.localPosition = pos + Vector3.down * _buttonTerm;
                button.gameObject.SetActive(false);
                pos.x += wheelWidth;
            }
        }

        private void Select(int pIdx) {
            Debug.Log(pIdx);
            UIManager.Instance.Selector.Add(_rewards[pIdx]);
            TurnOff();
        }
        
        private void Update() {
            if (_wheels.Any(wheel => wheel.IsRoll))
                return;
            if(_buttons.Count <= 0 || _buttons[0].gameObject.activeSelf)
                return;
            foreach (var button in _buttons) {
                button.gameObject.SetActive(true);
            }
        }
        
        private void Start() {
            _close.onClick.AddListener(TurnOff);
        }
    }
}