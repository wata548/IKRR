using System.Collections.Generic;
using System.IO;
using System.Linq;
using Character;
using Newtonsoft.Json;
using Roulette;
using UnityEngine;

namespace Data {
    public class SaveSystem {
       //==================================================||Properties 
        public int Seed{ get; set; }
        public int Chapter{ get; set; }
        
        public JsonPlayerData Player { get; set; } 
        public int[] Hand { get; set; } 
        public int Width { get; set; }
        public int Height { get; set; }
        
        public int Level{ get; set; }
        public int CurExp{ get; set; }
        public int Money{ get; set; }

        public List<Vector2Int> ClearStages { get; set; }
        
       //==================================================||Constructor 
       public SaveSystem(){}
        public static SaveSystem Save() {
            var data = new SaveSystem();
            data.Seed = GameManager.Seed;
            data.Chapter = GameManager.Chapter;
            data.Hand = RouletteManager.Hand.ToArray();
            data.Width = RouletteManager.Width;
            data.Height = RouletteManager.Height;
            data.ClearStages = UI.Map.Map.ClearStages;
            data.Level = PlayerData.Level;
            data.CurExp = PlayerData.CurExp;
            data.Money = PlayerData.Money;
            data.Player = new(CharactersManager.Player);
            return data;
        }

        public static void GameStart(string pJop = "Test") {
            UI.Map.Map.ClearStages.Clear();

            var path = Path.Combine(Application.streamingAssetsPath, $"Start-{pJop}.txt");
            var data = File.ReadLines(path).Select(int.Parse).ToArray();
            RouletteManager.Init(data[2..]);
            PlayerData.Init(data[1]);
            CharactersManager.Init(new Player(data[0]));
            GameManager.Init();
        } 
        
        public static void Load(string pPath) {
            var setting = new JsonSerializerSettings();
            setting.TypeNameHandling = TypeNameHandling.All;
            
            var context = File.ReadAllText(pPath);
            var data = JsonConvert.DeserializeObject<SaveSystem>(context, setting);
            PlayerData.Init(data.Money, data.Level, data.CurExp);
            UI.Map.Map.ClearStages = data.ClearStages;
            RouletteManager.Init(data.Width, data.Height, data.Hand.Length, data.Hand);
            CharactersManager.Init(new(data.Player));
            GameManager.Init(data.Seed, data.Chapter);
        }
        
        public void Save(string pPath = "Save.json") {
            var setting = new JsonSerializerSettings();
            setting.TypeNameHandling = TypeNameHandling.All;
            var json = JsonConvert.SerializeObject(this, Formatting.None, setting);
            var path = Path.Combine(Application.streamingAssetsPath, pPath);
            File.WriteAllText(path, json);
        }
    }
}