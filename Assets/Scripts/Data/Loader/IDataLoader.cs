using System.Collections.Generic;
using Data;

namespace Symbol {
    public interface IDataLoader<T> {
        public Dictionary<int, T> Load();
    }
}