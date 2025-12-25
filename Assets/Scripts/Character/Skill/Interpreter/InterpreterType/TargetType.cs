using System;
using Data;
using System.Collections.Generic;
using Extension;

namespace Interpret {
    public class TargetType: IInterpreterType {

       //==================================================||Fields 
        private IEnumerable<Targets> _targets;
        
       //==================================================||Properties 
        public object Value => _targets;
        public Type ReturnType => typeof(Targets);

       //==================================================||Methods 
        public void Init(string pValue) =>
            _targets = ((Targets)Enum.Parse(typeof(Targets), pValue.Replace('|', ','))).Split();
    }
}