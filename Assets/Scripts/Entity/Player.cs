using System;
using Data;
using Extension.Scene;
using UI;

namespace Character {
    public class Player: IEntity {
        
        //==================================================||Properties 
        public int MaxHp { get; private set; }
        public int Hp { get; private set; }
        public bool IsAlive { get; private set; }
        public Positions Position { get; }
        //==================================================||Constructor
        public Player() {
            MaxHp = 911;
            Hp = MaxHp;
            IsAlive = true;
            Position = Positions.Player;
        }
        
        //==================================================||Methods 
        public void OnAttack() {}
        public void ReceiveDamage(int pAmount, Action pOnComplete) {
            if (!IsAlive) {
                pOnComplete!.Invoke();
                return;
            }
                        
            Hp = Math.Max(0, Hp - pAmount);
            if (Hp == 0) {
                IsAlive = false;
             
                UIManager.Instance.Entity[Positions.Player].
                    OnDeath(this, pAmount, pOnComplete);
                
                return;
            }
             
            UIManager.Instance.Entity[Positions.Player].
                OnReceiveDamage(this, pAmount, pOnComplete);
        }

        public void Heal(int pAmount, Action pOnComplete) {
            if (!IsAlive) {
                pOnComplete!.Invoke();
                return;
            }
            
            Hp = Math.Min(MaxHp, Hp + pAmount);
             
            UIManager.Instance.Entity[Positions.Player].
                OnHeal(this, pAmount, pOnComplete);
        }

        public void Kill() {
            SceneManager.LoadScene(Scene.GameOver);
        }

    }
}