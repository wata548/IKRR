namespace LanguageEmbed {
    public interface ILanguageEmbed {
        T Invoke<T>(string pCode, string pFuncName, object[] pArgs = null);
        void Update();
    }
}