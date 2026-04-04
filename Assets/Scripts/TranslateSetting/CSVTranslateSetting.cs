using System;
using System.IO;
using System.Linq;
using System.Text;
using CSVData;
using CSVData.Extensions;
using Extension.Test;
using Lang;
using UnityEngine;

namespace Font {
    public class CSVTranslateSetting: MonoBehaviour {
        [SerializeField] private FontSetting _fontSetting;
        private bool _isInited = false;
#if UNITY_EDITOR
        [SerializeField] private string _apiKey;
        [SerializeField] private string _path;
        [SerializeField] private string[] _sheets;
        [SerializeField] private bool _localMode = false;

        [TestMethod]
        private void ChangeLanguage(Language pLanguage) {
            LanguageManager.LangPack = pLanguage;
        }

        private void Load() {
            if (_localMode) {
                LocalLoad();
                return;
            }
            var pack = new ColumnCSVPack(Language.Korean);
            foreach (var sheet in _sheets) {
                var data = SpreadSheet.LoadData(_path, sheet, _apiKey);
                pack.Add(data);
            }
            Debug.Log("Translate Pack completely loaded");
            
            LanguageManager.Table = pack;
            LanguageManager.LangPack = Language.Korean;
        }
#endif
        private void LocalLoad() {
            
            var pack = new ColumnCSVPack(Language.Korean);
            var path = Path.Combine(Application.streamingAssetsPath, "Translates");
            var files = Directory.GetFiles(path).Where(file => file.EndsWith(".csv"));
            foreach (var file in files) {
                pack.Add(CSV.Parse(File.ReadAllText(file)));
                Debug.Log($"{file} pack Added");
            }
            
            Debug.Log("Translate Pack completely loaded");
            LanguageManager.Table = pack;
            LanguageManager.LangPack = Language.Korean;
        }
        
        private void Awake() {
            if (_isInited)
                return;
            
            _fontSetting.Init();
            
#if UNITY_EDITOR
            Load();
#else
            LocalLoad();
#endif
            _isInited = true;
        }
    }
}