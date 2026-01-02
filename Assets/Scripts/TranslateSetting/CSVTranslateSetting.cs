using System;
using CSVData.Extensions;
using Extension.Test;
using Lang;
using UnityEngine;

namespace Font {
    public class CSVTranslateSetting: MonoBehaviour {
        [SerializeField] private FontSetting _fontSetting;
        [SerializeField] private string _apiKey;
        [SerializeField] private string _path;
        [SerializeField] private string[] _sheets;

        [TestMethod]
        private void ChangeLanguage(Language pLanguage) {
            LanguageManager.LangPack = pLanguage;
        }
        
        private void Awake() {
            
            _fontSetting.Init();

            var pack = new CSVPack(Language.Korean);
            foreach (var sheet in _sheets) {
                var data = SpreadSheet.LoadData(_path, sheet, _apiKey);
                pack.Add(data);
            }
            Debug.Log("Translate Pack completely loaded");

            LanguageManager.Table = pack;
        }
    }
}