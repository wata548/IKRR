using System;
using System.Collections.Generic;
using UnityEngine;

namespace Extension {
    [Serializable]
    public class SerializableKVP<TK, TV> {
        [field: SerializeField]public TK Key { get; private set; }
        [field: SerializeField]public TV Value { get; private set; }
        
        public static implicit operator KeyValuePair<TK, TV>(SerializableKVP<TK, TV> pValue) {
            return new(pValue.Key, pValue.Value);
        }
    }
}