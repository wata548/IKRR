using System.Collections.Generic;
using Extension;

namespace Data {
    public static class Status {

        private static Dictionary<TargetStatus, int> _statusValue = new();

        public static void Clear() {
            SetValue(TargetStatus.All, 0);
        }
        
        public static int GetValue(TargetStatus pStatus) {
            if (!((int)pStatus).IsFlag())
                return -1;
            
            _statusValue.TryAdd(pStatus, 0);
            return _statusValue[pStatus];
        }

        private static void SetFlagValue(TargetStatus pStatus, int pValue) {

            if (!((int)pStatus).IsFlag())
                return;
            
            if (!_statusValue.TryAdd(pStatus, pValue))
                _statusValue[pStatus] = pValue;
        }
        
        public static void SetValue(TargetStatus pStatus, int pAmount) {
            foreach (var status in pStatus.Split()) {
                SetFlagValue(status, pAmount);
            }
        }

        public static void AddValue(TargetStatus pStatus, int pAmount) {
            foreach (var status in pStatus.Split()) {
                SetFlagValue(status, GetValue(pStatus) + pAmount);
            }
        }
        public static void MulValue(TargetStatus pStatus, int pAmount) {
            foreach (var status in pStatus.Split()) {
                SetFlagValue(status, GetValue(pStatus) * pAmount);
            }
        }
        public static void DivValue(TargetStatus pStatus, int pAmount) {
            foreach (var status in pStatus.Split()) {
                SetFlagValue(status, GetValue(pStatus) / pAmount);
            }
        }
        public static void SubValue(TargetStatus pStatus, int pAmount) {
            foreach (var status in pStatus.Split()) {
                SetFlagValue(status, GetValue(pStatus) - pAmount);
            }
        }
    }
}