using System.Collections.Generic;
using UnityEngine;

namespace Data {
    
    [CreateAssetMenu(menuName = "Info")]
    public class InfoSO : ScriptableObject {
        [field: SerializeField]
        public string Name { get; private set; }
        [field: SerializeField]
        public List<InfoDetail> Details{ get; private set; }

        private Info _info = null;

        public Info GetInfo() =>
            _info ??= new(Name,Details);

        public static implicit operator Info(InfoSO pTarget) =>
            pTarget.GetInfo();
    }

}