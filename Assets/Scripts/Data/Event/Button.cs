using System.Text.RegularExpressions;

namespace Data.Event {

    public class Button {
        public readonly string Option;
        public readonly string FuncContext;

        public Button(string pContext) {
            const string PATTERN = @"(?<Option>(?:.|\n)*)\[\{(?<Func>(?:.|\n)*)\}\]";
            var match = Regex.Match(pContext, PATTERN).Groups;
            Option = match["Option"].Value;
            FuncContext = match["Func"].Value;
        }

        public override string ToString() {
            return $"{Option}: {FuncContext}";
        }
    }
}