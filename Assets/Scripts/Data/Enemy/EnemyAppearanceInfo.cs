using System.Collections.Generic;
using System.Linq;

namespace Data.EnemyAppearance {
    public class EnemyAppearanceInfo {
        public int Chapter { get; private set; }
        public string Enemies { get; private set; }
        private List<int> _targetEnemies;

        public List<int> GetEnemies() =>
            _targetEnemies ??= Enemies.Split(',').Select(int.Parse).ToList();
    }
}