using FSM;
using UI;
using UnityEngine;
using Random = System.Random;

namespace Data {
    public class GameManager: MonoBehaviour {
        
        //==================================================||Fields 
        private static bool _isInit = false;
        //==================================================||Properties 
        public static int Seed { get; private set; } = 987654321;
        public static int Chapter{ get; private set; } = 0;
        
        //==================================================||Methods 
        public static void Init() {
            var random = new Random();
            Init(random.Next(int.MinValue, int.MaxValue), 0);
        } 
        
        public static void Init(int pSeed, int pChapter) {
            Seed = pSeed;
            Chapter = pChapter;
            _isInit = true;
        }
        
        public static void StartBattle() {
            Fsm.Instance.StartBattle();
            Fsm.Instance.Change(State.Rolling);
        }
        
        public static void SetEnemy() {
            var position = Positions.Middle;
            var enemies = DataManager.GetStageEnemy(Chapter);
            foreach (var enemyCode in enemies) {
                
                CharactersManager.SetEnemy(enemyCode, position);
                position = (Positions)((int)position << 1);
            }
            StartBattle();
        }

        public static void SetEvent() {
            var eventData = DataManager.GetEvent(Chapter);
            UIManager.Instance.Event.SetEvent(eventData.Name, eventData.Event);    
            UIManager.Instance.Map.SetActive(false);
        }
        
        private static void TargetUpdate() {
            var target = CharactersManager.TargetUpdate();
            UIManager.Instance.Entity.SetTarget(target);    
        }
        
        public static void TurnEnd() {
            if(Fsm.Instance.State == State.PlayerTurn)
                Fsm.Instance.Change(State.EnemyTurn);
        }

        //==================================================||Unity 
        private void Awake() {
            if (!_isInit) {
                SaveFile.Start();
                Debug.Log("Skip title scene");
            }

            _isInit = false;
            Status.Clear();
        }

        private void Start() {
            UIManager.Instance.Map.GenerateMap(Seed, Chapter);
            var player = CharactersManager.Player;
            UIManager.Instance.Entity.Player.Refresh(player);
            //TestCode
            CharactersManager.Player.AddEffect(new Burn(new(30)));
        }

        private void Update() {
            if (!CharactersManager.IsFighting)
                return;
            TargetUpdate();
        }

        private void OnApplicationQuit() {
            Time.timeScale = 1;
            MaterialStore.Update();
        }
    }
}