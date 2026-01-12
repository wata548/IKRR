using System.Collections.Generic;
using System.Linq;
using Roulette;
using UnityEngine;

namespace Data {
    public class GameManager: MonoBehaviour {
        
        public static void StartGame() {

            Status.Clear();
            CharactersManager.Init();
            
            var list = new List<int>();
            list.AddRange(Enumerable.Repeat(1001, 10));
            list.AddRange(Enumerable.Repeat(1003, 9));
            list.AddRange(Enumerable.Repeat(1004, 3));
            list.AddRange(Enumerable.Repeat(1005, 3));
            RouletteManager.Init(list);
        }

        private void Awake() {
            StartGame();
        }
    }
}