using Data;

namespace Symbol {
    public interface IDB<T> {
        public void LoadData(IDataLoader<T> pLoader);
        public T GetSymbolData(int pNumber);
    }
}