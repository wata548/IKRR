using System.Collections.Generic;
using System.Linq;
using CSVData;
using Symbol;
using UnityEngine;

namespace Data {
    public class EnemyDB : IDB<IEnemyData> {
       
        private IReadOnlyDictionary<int, IEnemyData> _enemyBySerialNumber = null;
        public void LoadData(IDataLoader<IEnemyData> pLoader) {
            _enemyBySerialNumber = pLoader.Load();
        }

        public IEnemyData GetSymbolData(int pNumber) {
            if(_enemyBySerialNumber.TryGetValue(pNumber, out var data))
                return data;

            Debug.LogError($"This {pNumber}Number isn't symbol number. checkAgain");
            return null;
        }
    }
}