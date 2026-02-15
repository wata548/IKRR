using System.Text.RegularExpressions;

namespace Data.Event {

    public class Button {
        public readonly string Condition;
        public readonly string Option;
        public readonly string FuncContext;

        public Button(string pContext) {
            const string PATTERN = @"(?<Option>(?:.|\n)*)\[\{(?<Func>(?:.|\n)*)\}\]";
            var match = Regex.Match(pContext, PATTERN).Groups;
            var temp = match["Option"].Value;
            if (temp.StartsWith("<[")) {
                var tempArray = temp.Split("]>");
                Condition = tempArray[0][2..];
                Option = tempArray[1];
            }
            else
                Option = temp;
            
            FuncContext = match["Func"].Value;
        }

        public override string ToString() {
            return $"{Option}: {FuncContext}";
        }
    }
}