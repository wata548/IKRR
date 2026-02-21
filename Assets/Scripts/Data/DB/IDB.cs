using System.Collections.Generic;

namespace Data {
    public interface IDB<TKey, TValue> {
        public void LoadData(IDataLoader<TKey, TValue> pLoader);
        public TValue GetData(TKey pNumber);
        public IEnumerable<TKey> Keys { get; }
    }
    
    public interface IQueryDB<TKey, TValue, TQueryArgs> : IDB<TKey, TValue> {
        public List<TKey> Query(TQueryArgs pArgs);
        public List<TKey> SubQuery(List<TKey> pTarget, TQueryArgs pArgs);
    }
}