using System.Collections.Generic;

namespace Data {
    public static class UseInfo {
        private static HashSet<int> _evolve = new();

        public static void Evolve(int pCode) =>
            _evolve.Add(pCode);

        public static bool Get(int pCode) =>
            _evolve.Contains(pCode);
    }
}