using System.Collections.Generic;
using Extension;

namespace Data {
    public static class Status {

        private static Dictionary<TargetStatus, int> _statusValue = new();

        public static int GetValue(TargetStatus pStatus) {
            if (!((int)pStatus).IsFlag())
                return -1;
            
            _statusValue.TryAdd(pStatus, 0);
            return _statusValue[pStatus];
        }

        public static void SetValue(TargetStatus pStatus, int pValue) {

            if (!((int)pStatus).IsFlag())
                return;
            
            if (!_statusValue.TryAdd(pStatus, pValue))
                _statusValue[pStatus] = pValue;
        }

        public static void AddValue(TargetStatus pStatus, int pAmount) {
            foreach (var status in pStatus.Split()) {
                SetValue(status, GetValue(pStatus) + pAmount);
            }
        }
        public static void MulValue(TargetStatus pStatus, int pAmount) {
            foreach (var status in pStatus.Split()) {
                SetValue(status, GetValue(pStatus) * pAmount);
            }
        }
    }
}