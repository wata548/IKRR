namespace Data {
    public interface IDB<TKey, TValue> {
        public void LoadData(IDataLoader<TKey, TValue> pLoader);
        public TValue GetSymbolData(TKey pNumber);
    }
}