using System.Collections.Generic;
using Extension;
using System;
using System.Linq;

namespace Data {
    public class Status {

        private Dictionary<TargetStatus, int> _statusValue = new();

        public int GetValue(TargetStatus pStatus) {
            if (!((int)pStatus).IsFlag())
                return -1;
            
            _statusValue.TryAdd(pStatus, 0);
            return _statusValue[pStatus];
        }

        public void SetValue(TargetStatus pStatus, int pValue) {

            if (!((int)pStatus).IsFlag())
                return;
            
            if (!_statusValue.TryAdd(pStatus, pValue))
                _statusValue[pStatus] = pValue;
        }

        public void AddValue(TargetStatus pStatus, int pAmount) {
            foreach (var status in pStatus.Split()) {
                SetValue(status, GetValue(pStatus) + pAmount);
            }
        }
        public void MulValue(TargetStatus pStatus, int pAmount) {
            foreach (var status in pStatus.Split()) {
                SetValue(status, GetValue(pStatus) * pAmount);
            }
        }
    }
}