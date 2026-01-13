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

        public static void SetEnemy() {
            new Enemy(Positions.Middle, 2001);
            new Enemy(Positions.Left, 2002);
            new Enemy(Positions.Right, 2003);
            Fsm.Instance.Change(State.Rolling);
        }
        
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

        public void TurnEnd() {
            Fsm.Instance.Change(State.EnemyTurn);
        }

        private void Awake() {
            StartGame();
        }

        private void Update() {

            if (!CharactersManager.IsFighting)
                return;
            
            var target = CharactersManager.TargetUpdate();
            UIManager.Instance.Entity.SetTarget(target);
        }
    }
}