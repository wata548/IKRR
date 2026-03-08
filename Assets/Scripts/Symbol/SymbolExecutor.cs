using System.IO;
using Character.Skill;
using Data;
using LanguageEmbed;
using Roulette;
using UnityEngine;

namespace Symbol {
    public static class SymbolExecutor {

        private static string _symbolFuncFormat = null;
        private static string _eventFuncFormat = null;

        private static void SetUp() {
            _symbolFuncFormat ??= File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "LuaSymbolFuncFormat.txt"));
        }
        
        public static bool IsUsable(int pColumn, int pRow) {
            SetUp();
            var targetItem = RouletteManager.Get(pColumn, pRow);
            var condition = DataManager.Symbol.GetData(targetItem).ConditionCode;

            var code = string.Format(_symbolFuncFormat, nameof(IsUsable), condition);
            return LanguageExecutor.Executor.Invoke<bool>(code, nameof(IsUsable), new object[]{pColumn, pRow}); } 
        public static ISkill Evolution(int pColumn, int pRow) {
            var targetItem = RouletteManager.Get(pColumn, pRow);
            var context = DataManager.Symbol.GetData(targetItem).EvolveCondition;
            return GetSkillByOtherLanguage(nameof(Evolution), context, pColumn, pRow);
        }

        public static ISkill GetSkill(int pColumn, int pRow) {
            var targetItem = RouletteManager.Get(pColumn, pRow);
            var context = DataManager.Symbol.GetData(targetItem).EffectCode;
            return GetSkillByOtherLanguage(nameof(GetSkill), context, pColumn, pRow);
        }

        private static ISkill GetSkillByOtherLanguage(string pFuncName, string pContext, int pColumn, int pRow) {
            SetUp();
            
            var code = string.Format(_symbolFuncFormat, pFuncName, pContext);
            var dsl = LanguageExecutor.Executor.Invoke<string>(code, pFuncName, new object[]{pColumn, pRow});
            return SkillInterpreter.Interpret(dsl);
        }
    }
}