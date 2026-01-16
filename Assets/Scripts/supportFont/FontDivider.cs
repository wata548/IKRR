#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CSVData.Extensions;
using Lang;
using UnityEditor;
using UnityEngine;

namespace Data.supportFont {
    public static class FontDivider {
        [MenuItem("FontHelper/CreateTargetFontFiles")]
        private static void ExtractContext() {
            
            const string DIRECTORY_NAME = "IncludedText";
            const string API_KEY = "AIzaSyD9TQEfDm8OY3DtfdvZayoZi96QHLl_SA0";
            const string PATH = "1Dq18pC6_x2gOk0Vfc-HkwyfAMEFPzJO7ETpO0Mp0uPQ";
            string[] sheets = { "Symbol", "Condition", "Effect", "UI", "Evolve", "Enemy", "EnemySkill" };
                
            var directoryPath = Path.Combine(Application.streamingAssetsPath, DIRECTORY_NAME);
            Directory.CreateDirectory(directoryPath);
            var korean = new StringBuilder();
            var japanese = new StringBuilder();
            var chinese = new StringBuilder();
            var various = new StringBuilder();
            
            foreach (var sheet in sheets) {
                var data = SpreadSheet.LoadData(PATH, sheet, API_KEY);
                Add(data);
            }
            
            File.WriteAllText(Path.Combine(directoryPath, "Korea.txt"), new string(korean.ToString().Distinct().OrderBy(c => c).ToArray()));
            File.WriteAllText(Path.Combine(directoryPath, "Japanese.txt"), new string(japanese.ToString().Distinct().OrderBy(c => c).ToArray()));
            File.WriteAllText(Path.Combine(directoryPath, "Chinese.txt"), new string(chinese.ToString().Distinct().OrderBy(c => c).ToArray()));
            File.WriteAllText(Path.Combine(directoryPath, "Various.txt"), new string(various.ToString().Distinct().OrderBy(c => c).ToArray()));
            Debug.Log("Complete Save");
            ;
            return;
            
            void Add(List<List<string>> pContext) {
                var packs = pContext.Where(row => !string.IsNullOrWhiteSpace(row[0]))
                    .Select(row => ((Language)Enum.Parse(typeof(Language), row[0]), row.Skip(1)));
                
                foreach (var pack in packs) {
                    var target = pack.Item1 switch {
                        Language.Korean => korean,
                        Language.Japanese => japanese,
                        Language.Chinese => chinese,
                        _ => various
                    };
                    target.AppendJoin("", pack.Item2);
                }
            }
        }
        
    }
}
#endif