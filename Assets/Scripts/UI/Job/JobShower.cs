using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Extension;
using Extension.Scene;
using Lang;
using TMPro;
using UI.ContainSymbol;
using UI.Icon;
using UnityEngine;
using UnityEngine.UI;
using SceneManager = Extension.Scene.SceneManager;

namespace UI.Job {
    public class JobShower: MonoBehaviour {
        [SerializeField] private Image _shower;
        [SerializeField] private TMP_LangText _name;
        [SerializeField] private TMP_LangText _desc;
        [SerializeField] private TMP_Text _hp;
        [SerializeField] private TMP_Text _money;
        
        [Space, Header("Symbol Table")]
        [SerializeField] private RectTransform _symbolList;
        [SerializeField] private Vector2Int _symbolTableSize;
        [SerializeField] private SymbolAmountShower _prefab;
        [SerializeField] private Button _startButton; 
        private List<SymbolAmountShower> _symbols = new();
        private int _code = 0;

        

        public void Update() {
            if (_code == JobButton.SelectedJob)
                return;
            
            _code = JobButton.SelectedJob;
            var data = DataManager.Job.GetData(_code);
            _shower.sprite = _code.GetIcon();
            _name.text = data.Name;
            _desc.text = data.Desc;
            _hp.text = data.MaxHp.ToString();
            _money.text = data.Money.ToString();

            var symbols = data.StartItem.ToList();
            var args = new PlaceArgs<SymbolAmountShower>(
                Vector2.zero, 
                symbols.Count,
                _symbolTableSize,
                _prefab,
                null,
                (element, idx) => element.Set(symbols[idx].Code, symbols[idx].Amount)
            );
            _symbolList.Place(_symbols, args);
        }

        private void Awake() {
            _startButton.onClick.AddListener(() => {
               SaveSystem.GameStart(JobButton.SelectedJob);
               SceneManager.LoadScene(Scene.Main);
            });
        }
    }
}