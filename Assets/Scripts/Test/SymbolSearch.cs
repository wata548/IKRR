using Data;
using Lang;
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

        private void Search() {
            var targetSymbol = int.Parse(_input.text);
            var data = DataManager.SymbolDB.GetSymbolData(targetSymbol);
            if (data == null) {
                _name.text = "Error";
                _description.text = $"{targetSymbol} isn't exist";
                _condition.text = "";
                return;
            }

            _name.text = data.Name;
            _description.text = data.Description;
            _condition.text = data.Condition;
        }
        
        private void Awake() {
            _searchButton.onClick.AddListener(Search);
        }
    }
}