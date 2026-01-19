using System.Collections.Generic;
using Extension;

namespace Data.EnemyAppearance {
    public class EnemyAppearanceDB: IDB<int, List<List<int>>> {
        private Dictionary<int, List<List<int>>> _datas;
        
        public void LoadData(IDataLoader<int, List<List<int>>> pLoader) =>
            _datas = pLoader
                .Load()
                .ToDictionary();

        public List<List<int>> GetData(int pNumber) =>
            _datas[pNumber];
    }
}