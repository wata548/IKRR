using System;
using UnityEngine;

namespace Extension {
    [Serializable]
    public class SerializableKVP<TK, TV> {
        [field: SerializeField]public TK Key { get; private set; }
        [field: SerializeField]public TV Value { get; private set; }
    }
}