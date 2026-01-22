using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Extension;
using Roulette;
using UI.Roulette;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI.LevelUpReward {
    public class LevelUpRewardWindow: MonoBehaviour {

       //==================================================||Fields 
       [SerializeField] private GameObject _pannel;
       
        [Header("Option Select")]
        [SerializeField] private RectTransform _statusSelectZone;
        [SerializeField] private GameObject _optionSection;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Select _select;

        [Space, Header("Apply")] 
        [SerializeField] private FakeWheel _fakeWheel;
        [SerializeField] private Button _add;
        [SerializeField] private Button _cancel;
        
        private readonly List<Select> _statusSelect = new();
        private const string STATUS = "STATUS";
        private int _rouletteResult;
        private bool _isActive = false;
        private int _level = 1;
        
        //==================================================||Methods 
        public void TurnOn() {
            _isActive = true;
            _pannel.SetActive(true);
            _optionSection.SetActive(true);
            var cnt = ((TargetStatus[])Enum.GetValues(typeof(TargetStatus)))
                .Count(status => status.IsFlag());
            _statusSelectZone.Place(_statusSelect, new(Vector2.zero, cnt, new(3, 0), _select, OnStatusGenerate));
        }

        private void OnStatusGenerate(Select pObj, int pIdx) {
            var targetStatus = (TargetStatus)(1 << pIdx);
            var sprite = Resources.Load<Sprite>($"Status/{targetStatus}");
            pObj.Set(sprite, STATUS, pIdx);
        }

        private void ConfirmOption() {
            _optionSection.SetActive(false);

            var rarity = DataManager.LevelUp.GetRarity();
            var targetStatus = (TargetStatus)(1 << Select.GetSelection(STATUS));
            var fakeCandidate = DataManager.Symbol.Query(new(
                Status: targetStatus
            ));
            var candidate = DataManager.Symbol.SubQuery(fakeCandidate, new(
                Rarity: rarity
            ));
            _rouletteResult = candidate[Random.Range(0, candidate.Count)];
            Debug.Log($"{_rouletteResult}({targetStatus}) - {rarity}(candidate cnt: {fakeCandidate.Count})");
            RollFakeWheel(_rouletteResult, fakeCandidate);
        }

        private void RollFakeWheel(int pResult, List<int> pRawCandidate) {
            
            var candidate = pRawCandidate
                .Shuffle()
                .Select(code => new CellInfo {
                    Code = code,
                    Status = CellStatus.Usable
                });
            _fakeWheel.gameObject.SetActive(true);
                        _fakeWheel.SetLastOne(pResult);
            _fakeWheel.Init(-1, 1, candidate, null, () => SetActiveApplyButtons(true));
            _fakeWheel.StartRoll();
        }

        private void SetActiveApplyButtons(bool pActive) {
            
            _add.gameObject.SetActive(pActive);
            _cancel.gameObject.SetActive(pActive);
            if (!pActive) {
                _isActive = false;
                _pannel.SetActive(false);
            }
        }
       //==================================================||Unity 
        private void Awake() {
            _confirmButton.onClick.AddListener(ConfirmOption);
            
            _add.onClick.AddListener(() => RouletteManager.AddHandSize(1, _rouletteResult));
            _add.onClick.AddListener(() => SetActiveApplyButtons(false));
            _cancel.onClick.AddListener(() => RouletteManager.AddHandSize(1));
            _cancel.onClick.AddListener(() => SetActiveApplyButtons(false));
        }

        private void Update() {
            if (!_isActive && _level != PlayerData.Level) {
                _level++;
                TurnOn();
            }
        }
    }
}