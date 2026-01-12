using System;
using Data;
using Extension.Scene;

namespace Character {
    public class Player: IEntity {
        
        //==================================================||Properties 
        public int MaxHp { get; private set; }
        public int Hp { get; private set; }
        public bool IsAlive { get; private set; }
        public Positions Position { get; }
        //==================================================||Constructor
        public Player() {
            MaxHp = 10;
            Hp = MaxHp;
            IsAlive = true;
            Position = Positions.Player;
        }
        
        //==================================================||Methods 
        public void ReceiveDamage(int pAmount, Action pOnComplete) {
            if (!IsAlive) {
                pOnComplete!.Invoke();
                return;
            }
                        
            Hp = Math.Max(0, Hp - pAmount);
            if (Hp == 0) {
                IsAlive = false;
                OnDeath();
                return;
            }
                        
            OnReceiveDamage(pAmount, pOnComplete);
        }

        public void Heal(int pAmount, Action pOnComplete) {
            if (!IsAlive) {
                pOnComplete!.Invoke();
                return;
            }
            
            Hp = Math.Min(MaxHp, Hp + pAmount);
            OnHeal(pAmount, pOnComplete);
        }

        public void Kill() {
            OnDeath();
        }

        private void OnDeath() {
            SceneManager.LoadScene(Scene.GameOver);
        }

        private void OnReceiveDamage(int pAmount, Action pOnComplete) {
            
        }
        
        private void OnHeal(int pAmount, Action pOnComplete) {
            
        }
    }
}