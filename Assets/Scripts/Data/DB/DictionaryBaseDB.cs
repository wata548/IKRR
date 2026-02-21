using System;
using System.Collections.Generic;
using Extension;
using UnityEngine;

namespace Data {
    public class DictionaryBaseDB<TKey, TValue> : IDB<TKey, TValue> where TValue: class {
       
       //==================================================||Fields 
        protected IReadOnlyDictionary<TKey, TValue> _matchToSerialNumber = null;

        protected Action<TValue> _postProcessing = null;
       //==================================================||Constructors
       public DictionaryBaseDB(Action<TValue> pPostProcessing = null) =>
           _postProcessing = pPostProcessing;
       
       //==================================================||Methods 
        public void LoadData(IDataLoader<TKey, TValue> pLoader) {
            _matchToSerialNumber = pLoader.Load().ToDictionary();
            if (_postProcessing == null)
                return;
            
            foreach(var (_, element) in _matchToSerialNumber)
                _postProcessing.Invoke(element);
        }

        public TValue GetData(TKey pNumber) {
            if(_matchToSerialNumber.TryGetValue(pNumber, out var data))
                return data;

            Debug.LogError($"This {pNumber}Number isn't symbol number. checkAgain");
            return null;
        }

        public IEnumerable<TKey> Keys => _matchToSerialNumber.Keys;
        
#if UNITY_EDITOR
        public IReadOnlyDictionary<TKey, TValue> GetData() => _matchToSerialNumber;
#endif
    }
}