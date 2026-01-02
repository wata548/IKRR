using System.Collections.Generic;

namespace Data {
    public interface IDataLoader<TKey, TValue> {
        public IEnumerable<KeyValuePair<TKey, TValue>> Load();
    }

    public interface IDataLoader<T> {
        public IEnumerable<T> Load();
    }
}