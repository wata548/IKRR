using Character.Skill;
using Extension.Test;
using Lang;
using UnityEngine;

namespace Test {
    public static class Test_Lang {
        [TestMethod]
        public static void Change(Language pLang) {
            
            LanguageManager.LangPack = pLang;
            Debug.Log(pLang);
            TMP_LangText.SetFont(Language.Arabic, new());
        }
    }
}