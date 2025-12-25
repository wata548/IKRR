using System;

namespace Interpret {
    public interface IInterpreterType {
        
        object Value { get; }
        Type ReturnType { get; }
        void Init(string pValue);
        
    }
}