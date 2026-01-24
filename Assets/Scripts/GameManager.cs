using System;
using System.Collections.Generic;
using System.Linq;
using Character;
using FSM;
using Roulette;
using UI;
using UnityEngine;

namespace Data {
    public class GameManager: MonoBehaviour {

        //==================================================||Fields 
        private static int _curChapter = 0;
        
        //==================================================||Properties 
        public static int Seed { get; private set; } = 987654321;
        
        //==================================================||Methods 
        public static void SetEnemy() {
            Fsm.Instance.StartBattle();
            
            var position = Positions.Middle;
            var enemies = DataManager.GetStageEnemy(_curChapter);
            foreach (var enemy in enemies) {
                
                var newEnemy = new Enemy(position, enemy);
                CharactersManager.SetEntity(position, newEnemy);
                position = (Positions)((int)position << 1);
            }
            Fsm.Instance.Change(State.Rolling);
        }
        
        public static void StartGame() {

            Status.Clear();
            CharactersManager.Init();
            
            var list = new List<int>();
            list.AddRange(Enumerable.Repeat(1001, 7));
            list.AddRange(Enumerable.Repeat(1003, 5));
            list.AddRange(Enumerable.Repeat(1004, 1));
            list.AddRange(Enumerable.Repeat(1005, 5));
            list.AddRange(Enumerable.Repeat(1007, 7));
            RouletteManager.Init(list);
        }

        public void TurnEnd() {
            if(Fsm.Instance.State == State.PlayerTurn)
                Fsm.Instance.Change(State.EnemyTurn);
        }

        //==================================================||Unity 
        private void Awake() {
            StartGame();
            PlayerData.Init();
        }

        private void Start() {
            UIManager.Instance.Map.GenerateMap(Seed);
        }

        private void Update() {

            if (!CharactersManager.IsFighting)
                return;
            
            var target = CharactersManager.TargetUpdate();
            UIManager.Instance.Entity.SetTarget(target);
        }

        private void OnApplicationQuit() {
            Time.timeScale = 1;
            MaterialStore.Update();
        }
    }
}