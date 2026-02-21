#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CSVData;
using CSVData.Extensions;
using Lang;
using UnityEditor;
using UnityEngine;

namespace Data.supportFont {
    public static class DataSupport {
        const string DIRECTORY_NAME = "IncludedText";
        const string API_KEY = "AIzaSyD9TQEfDm8OY3DtfdvZayoZi96QHLl_SA0";
        const string DB_PATH = "1BLW__Va3NhcIJzwd6oblRmFck1DOADGoB3fww_RGqpo";
        const string TRANSLATE_PATH = "1SuglC23QW-DD4pIAWioJG04knTjuR0PAWVBaI4WgC60";

        static readonly string[] DB_ITEMS = new [] {
            "Tips",
            "Effect",
            "Symbol",
            "Event",
            "StageWidth",
            "StageType",
            "Enemy",
            "StageEnemy",
            "LevelUpAppearance",
        };
        static readonly string[] TRANSLATE_ITEMS = new [] {
            "UI",
            "UIWord",
            "Event",
            "Tutorial",
            "EnemySkill",
            "Tips",
            "EffectDesc",
            "Effect",
            "EnemyDesc",
            "Enemy",
            "Evolve",
            "SymbolEffect",
            "Condition",
            "Symbol",
        };

        [MenuItem("DataManager/GetEventText")]
        private static void GetEventText() {
            var db = new DictionaryBaseDB<int, List<EventData>>();
            db.LoadData(new SSGroupLoader<EventData, EventData>(
                    "Event",
                    data => data.Chapter
            ));
            var result = new StringBuilder();
            result.Append("<noparse>");
            foreach (var chapter in db.GetData()) {
                foreach (var @event in chapter.Value) {
                    result.AppendLine($@"""{@event.Name}""");
                    foreach (var script in @event.Event.Scripts) {
                        result.AppendLine($@"""{script.Context}""");
                        foreach (var button in script.Buttons) {
                            result.AppendLine($@"""{button.Option}""");
                        }
                    }
                }
            }
            Debug.Log(result.ToString());
        }
        
        [MenuItem("DataManager/SaveSpreadSheetToCSV")]
        private static void SaveSpreadSheetToCSV() {
            SaveSpreadSheetToCSV("Datas", DB_PATH, DB_ITEMS);
            SaveSpreadSheetToCSV("Translates", TRANSLATE_PATH, TRANSLATE_ITEMS);
        }

        private static void SaveSpreadSheetToCSV(string pFolder, string pPath, string[] pElements) {
            foreach (var element in pElements) {
                var data = SpreadSheet.LoadData(pPath,element, API_KEY);
                var path = Path.Combine(Application.streamingAssetsPath, pFolder, element + ".csv");
                var csv = ToCSV(data);
                File.WriteAllText(path, csv);
                Debug.Log($"{element} loaded");
            }
                        
            string ToCSV(List<List<string>> pData) {
                            
                var builder = new StringBuilder();
                for (int i = 0; i < pData.Count; i++) {
                    for (int j = 0; j < pData[i].Count; j++) {
                        pData[i][j] = '"' + pData[i][j].Replace("\"", "\"\"") + '"';
                    }
            
                    builder.Append(string.Join(',', pData[i]));
                    builder.AppendLine();
                }
                return builder.ToString();
            }
        }
        
        [MenuItem("DataManager/CreateTargetFontFiles")]
        private static void ExtractContext() {
            
            
            var directoryPath = Path.Combine(Application.streamingAssetsPath, DIRECTORY_NAME);
            Directory.CreateDirectory(directoryPath);
            var csvPath = Path.Combine(Application.streamingAssetsPath, "Translates");
            
            var korean = new StringBuilder();
            var japanese = new StringBuilder();
            var chinese = new StringBuilder();
            var various = new StringBuilder();
            
            foreach (var sheet in Directory.GetFiles(csvPath).Where(path => path.EndsWith(".csv"))) {
                var data = CSV.Parse(File.ReadAllText(sheet));
                Add(data);
                Debug.Log($"{sheet} loaded");
            }
            
            File.WriteAllText(Path.Combine(directoryPath, "Korea.txt"), new string(korean.ToString().Distinct().OrderBy(c => c).ToArray()));
            File.WriteAllText(Path.Combine(directoryPath, "Japanese.txt"), new string(japanese.ToString().Distinct().OrderBy(c => c).ToArray()));
            File.WriteAllText(Path.Combine(directoryPath, "Chinese.txt"), new string(chinese.ToString().Distinct().OrderBy(c => c).ToArray()));
            File.WriteAllText(Path.Combine(directoryPath, "Various.txt"), new string(various.ToString().Distinct().OrderBy(c => c).ToArray()));
            Debug.Log("Complete Save");
            return;
            
            void Add(List<List<string>> pContext) {
                var language = pContext[0]
                    .Select(cell => (Language)Enum.Parse(typeof(Language), cell))
                    .ToList();
                
                foreach (var row in pContext.Skip(1)) {
                    var idx = 0;
                    foreach (var cell in row) {
                        var target = language[idx] switch {
                            Language.Korean => korean,
                            Language.Japanese => japanese,
                            Language.Chinese => chinese,
                            _ => various
                        };
                        target.Append(cell);
                        idx++;
                    }
                    
                }
            }
        }
        
    }
}
#endif