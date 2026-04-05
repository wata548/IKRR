using System.Collections.Generic;
using System.Linq;
using Extension;
using UnityEngine;

namespace Data {
    public static class UseInfo {
        private static HashSet<int> _get = new();
        private static HashSet<int> _evolve = new();


        public static void Clear() {
            _get.Clear();
            _evolve.Clear();
        }
        public static int GetRandomGetInfo() {
            var temp = _get.Shuffle();
            return temp[Random.Range(0, temp.Count)];
        }
        
        public static void Evolve(int pCode) =>
            _evolve.Add(pCode);

        public static void Get(int pCode) =>
            _get.Add(pCode);

        public static bool GetEvolve(int pCode) =>
            _evolve.Contains(pCode);

        public static bool GetGetInfo(int pCode) =>
            _get.Contains(pCode);
    }
}