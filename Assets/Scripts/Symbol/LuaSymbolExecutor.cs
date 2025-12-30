using System.IO;
using Data;
using LanguageEmbed;
using Roulette;
using UnityEngine;

namespace Symbol {
    public class LuaSymbolExecutor: ISymbolExecutor {

        private ILanguageEmbed _language = null;
        private string _luaFuncFormat = null;

        public LuaSymbolExecutor() {
            _language = new LuaEmbed(new() {
                
            });
        }

        public void Update() {
            _language.Update();
        }

        private void SetUp() =>
            _luaFuncFormat ??= File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "LuaFuncFormat.txt"));
        
        public bool IsUsable(int pColumn, int pRow) {
            SetUp();
            var targetItem = RouletteManager.Get(pColumn, pRow);
            var condition = DataManager.SymbolDB.GetSymbolData(targetItem).Condition;

            var code = string.Format(_luaFuncFormat, nameof(IsUsable), condition);
            return _language.Invoke<bool>(code, nameof(IsUsable), new object[]{pColumn, pRow});
        }

        public void Evolution(int pColumn, int pRow) {
            SetUp();
            var targetItem = RouletteManager.Get(pColumn, pRow);
            var evolveCondition = DataManager.SymbolDB.GetSymbolData(targetItem).EvolveCondition;

            var code = string.Format(_luaFuncFormat, nameof(Evolution), evolveCondition);
            var result = _language.Invoke<int>(code, nameof(Evolution), new object[]{pColumn, pRow});
            if (result == targetItem)
                return;

            RouletteManager.Change(pColumn, pRow, result);
        }

        public void Execute(int pColumn, int pRow) {
            SetUp();
            SetUp();
            var targetItem = RouletteManager.Get(pColumn, pRow);
            var effectCode = DataManager.SymbolDB.GetSymbolData(targetItem).EffectCode;

            var code = string.Format(_luaFuncFormat, nameof(Execute), effectCode);
            _language.Invoke<object>(code, nameof(Execute));
        }
    }
}