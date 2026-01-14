using System.Collections.Generic;

namespace Data {
    public class Info {
        public readonly int SerialNumber;
        public readonly string Name;
        public readonly List<(string, string)> Contexts;

        public Info(int pSerialNumber, string pName, List<(string, string)> pContext) =>
            (SerialNumber, Name, Contexts) = (pSerialNumber, pName, pContext);
    }
    
}