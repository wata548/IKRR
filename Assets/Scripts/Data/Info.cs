using System.Collections.Generic;

namespace Data {
    public class Info {
        public readonly string Name;
        public readonly List<(string Category, string Format, object[] Params)> Contexts;

        public Info(string pName, List<(string, string, object[])> pContext) =>
            (Name, Contexts) = (pName, pContext);
    }
    
}