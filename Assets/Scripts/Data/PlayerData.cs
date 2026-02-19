
using Roulette;
using UnityEngine;

namespace Data {
    public static class PlayerData {
        public static int Money { get; private set; } = 10;
        public static int Level { get; private set; } = 1;
        public static int NeedExp { get; private set; }
        public static int CurExp { get; private set; } = 0;
        public static int LastUpdate { get; private set; } = 0;
        
        public static void Init(int pMoney = 0, int pLevel = 1, int pExp = 0) {
            Money = pMoney;
            Level = pLevel;
            CurExp = pExp;
            NeedExp = GetNeedExp();
        }

        public static void GetMoney(int pAmount) {
            Money += pAmount;
            LastUpdate = Time.frameCount;
        }
        
        public static void GetExp(int pAmount) {
            CurExp += pAmount;
            LastUpdate = Time.frameCount;
            if (CurExp < NeedExp)
                return;
            
            CurExp -= NeedExp;
            Level++;
            NeedExp = GetNeedExp();
            GetExp(0);
        }

        private static int GetNeedExp() => (int)Mathf.Ceil(0.914f * Mathf.Pow(Level, 1.4f) + 7); 
    }
}