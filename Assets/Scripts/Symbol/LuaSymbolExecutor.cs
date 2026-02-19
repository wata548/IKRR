using System.IO;
using Character.Skill;
using Data;
using LanguageEmbed;
using Roulette;
using UnityEngine;

namespace Symbol {
    public class LuaSymbolExecutor: ISymbolExecutor {

        private ILanguageEmbed _language = null;
        private string _symbolFuncFormat = null;
        private string _eventFuncFormat = null;

        public LuaSymbolExecutor() {
            _language = new LuaEmbed(new() {
                
            });
        }

        public void Update() {
            _language.Update();
        }

        private void SetUp() {
            _symbolFuncFormat ??= File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "LuaSymbolFuncFormat.txt"));
        }
        
        public bool IsUsable(int pColumn, int pRow) {
            SetUp();
            var targetItem = RouletteManager.Get(pColumn, pRow);
            var condition = DataManager.Symbol.GetData(targetItem).ConditionCode;

            var code = string.Format(_symbolFuncFormat, nameof(IsUsable), condition);
            return _language.Invoke<bool>(code, nameof(IsUsable), new object[]{pColumn, pRow});
        }

        public ISkill Evolution(int pColumn, int pRow) {
            var targetItem = RouletteManager.Get(pColumn, pRow);
            var context = DataManager.Symbol.GetData(targetItem).EvolveCondition;
            return GetSkillOnLua(nameof(Evolution), context, pColumn, pRow);
        }

        public ISkill GetSkill(int pColumn, int pRow) {
            var targetItem = RouletteManager.Get(pColumn, pRow);
            var context = DataManager.Symbol.GetData(targetItem).EffectCode;
            return GetSkillOnLua(nameof(GetSkill), context, pColumn, pRow);
        }

        private ISkill GetSkillOnLua(string pFuncName, string pContext, int pColumn, int pRow) {
            SetUp();
            
            var code = string.Format(_symbolFuncFormat, pFuncName, pContext);
            var dsl = _language.Invoke<string>(code, pFuncName, new object[]{pColumn, pRow});
            return SkillInterpreter.Interpret(dsl);
        }
    }
}