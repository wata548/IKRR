using System;
using System.Collections.Generic;
using Data;
using Lang;
using Roulette;
using Symbol;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Test {
    public class SymbolSearch: MonoBehaviour {
        [SerializeField] private Button _searchButton;
        [SerializeField] private TMP_InputField _input;
        [SerializeField] private TMP_LangText _name;
        [SerializeField] private TMP_LangText _description;
        [SerializeField] private TMP_LangText _condition;
        [SerializeField] private TMP_Text _usable;

        private void Search() {
            var targetSymbol = int.Parse(_input.text);
            var data = DataManager.SymbolDB.GetSymbolData(targetSymbol);
            if (data == null) {
                _name.text = "Error";
                _description.text = $"{targetSymbol} isn't exist";
                _condition.text = "";
                return;
            }

            RouletteManager.Change(0, 0, targetSymbol);
            _usable.text = $"Usable: {SymbolExecutor.Instance.IsUsable(0, 0)}";
            _name.text = data.Name;
            _description.text = data.Description;
            _condition.text = data.Condition;
        }
        
        private void Awake() {
            _searchButton.onClick.AddListener(Search);
            RouletteManager.Initialize(new List<int>());
            
        }
    }
}