using System.Collections.Generic;
using Extension;
using UnityEngine;

namespace Data {
    public class EnemyDB : IDB<int, EnemyData> {
       
        private IReadOnlyDictionary<int, EnemyData> _enemyBySerialNumber = null;
        public void LoadData(IDataLoader<int, EnemyData> pLoader) {
            _enemyBySerialNumber = pLoader.Load().ToDictionary();
        }

        public EnemyData GetData(int pNumber) {
            if(_enemyBySerialNumber.TryGetValue(pNumber, out var data))
                return data;

            Debug.LogError($"This {pNumber}Number isn't symbol number. checkAgain");
            return null;
        }
    }
}