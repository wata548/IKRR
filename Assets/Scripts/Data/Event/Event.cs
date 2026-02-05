using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Data.Event {
    public class Event {

        private readonly Dictionary<int, SingleScript> _event = new();
        public Event(string pContext) {
            const string PATTERN = @"@(?<Label>\d+):(?<Context>[^@]*)";
            var matches = Regex.Matches(pContext, PATTERN);
            var data = matches;
            foreach (Match match in matches) {
                var script = new SingleScript(match.Groups["Context"].Value);
                var label = int.Parse(match.Groups["Label"].Value);
                _event.Add(label, script);
            }
        }

        public SingleScript Goto(int pIdx) =>
            _event[pIdx];

        public static Event Parse(string pContext) =>
            new Event(pContext);
    }
}