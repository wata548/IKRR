using System;
using System.Text.RegularExpressions;

namespace Interpret {
    public class RangeValue: IInterpreterType {

        private const string PATTERN = @"(?<Min>\d+)\s*\~\s*(?<Max>\d+)";
        
        //==================================================||Fields 
        public int _minValue;
        public int _maxValue;
        
        //==================================================||Fields 
        public object Value => UnityEngine.Random.Range(_minValue, _maxValue);
        public Type ReturnType => typeof(int);
       
        public void Init(string pValue) {

            var match = Regex.Match(pValue, PATTERN);
            if (!match.Success) {
                _minValue = int.Parse(pValue);
                _maxValue = _minValue;
                return;
            }

            _minValue = int.Parse(match.Groups["Min"].Value);
            _maxValue = int.Parse(match.Groups["Max"].Value);
        }
    }
}