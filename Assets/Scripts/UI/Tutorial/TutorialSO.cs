using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Tutorial {

    [Serializable]
    public class TurorialData {
        public Vector2 Direction = Vector2.up;
        public Vector2 Pos;
        public Vector2 Size;
        public string Context;
    }
    
    [CreateAssetMenu]
    public class TutorialSO: ScriptableObject {
        public List<TurorialData> Datas;
    }
}