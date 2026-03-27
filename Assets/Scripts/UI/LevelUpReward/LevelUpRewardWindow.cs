using Data;
using Roulette;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

namespace UI.LevelUpReward {
    public class LevelUpRewardWindow: MonoBehaviour {
        
        //==================================================||Fields 
        [SerializeField] private GameObject _pannel;
        [SerializeField] private VisualEffect _effect;
        [SerializeField] private SymbolShower _shower;
        [SerializeField] private TMP_Text _levelShower;
        [SerializeField] private Button _close;
        private int _level = 1;
        
        //==================================================||Properties 
        public bool IsActive { get; private set; } = false;
        
        public bool NeedUpdate => _level != PlayerData.Level;

        public bool TurnOn() {

            if (IsActive || !NeedUpdate)
                return false;

            Tutorial.Tutorial.Instance.Set("LevelUp");
            var item = PlayerData.Job.LevelUpReward.GetReward(++_level);
            _levelShower.text = _level.ToString();
            RouletteManager.AddHandSize(1, item);
            _effect.Reinit();
            _effect.Play();
            _shower.Set(item);
            IsActive = true;
            _pannel.SetActive(true);
            return true;
        }

        private void TurnOff() {
            IsActive = false;
            _pannel.SetActive(false);
        }  
        
        //==================================================||Unity 
        private void Awake() {
            _level = PlayerData.Level;
            _close.onClick.AddListener(TurnOff);
        }
    }
}