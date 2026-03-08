using Extension.StaticUpdate;

namespace LanguageEmbed {
    public static class LanguageExecutor {
        public static readonly ILanguageEmbed Executor = new LuaEmbed();
        [StaticUpdate]
        private static void Update() {
            Executor.Update();
        }
    }
}