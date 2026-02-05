namespace LanguageEmbed {
    public interface ILanguageEmbed {
        void Invoke(string pCode, string pFuncName, object[] pArgs = null);
        T Invoke<T>(string pCode, string pFuncName, object[] pArgs = null);
        void Update();
    }
}