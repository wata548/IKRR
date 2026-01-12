using System;
using UnityEngine;

namespace Test {
    public class TestBehaviour: MonoBehaviour {
        [SerializeField] private Material _mat;
        private float _time = 0;
        private void Update() {
            Debug.Log(_mat.GetTexture("_After"));
        }
    }
}