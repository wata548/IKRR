using System.Collections.Generic;
using Extension;
using UnityEngine;

namespace Data {
    public class DictionaryBaseDB<T> : IDB<int, T> where T: class {
       
        private IReadOnlyDictionary<int, T> _matchToSerialNumber = null;
        public void LoadData(IDataLoader<int, T> pLoader) {
            _matchToSerialNumber = pLoader.Load().ToDictionary();
        }

        public T GetData(int pNumber) {
            if(_matchToSerialNumber.TryGetValue(pNumber, out var data))
                return data;

            Debug.LogError($"This {pNumber}Number isn't symbol number. checkAgain");
            return null;
        }
    }
}