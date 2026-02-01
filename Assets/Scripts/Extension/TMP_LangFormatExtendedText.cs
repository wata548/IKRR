using System.Collections.Generic;
using Extension.CustomFormatString;

namespace Lang {
    public class TMP_LangFormatExtendedText: TMP_LangText {
    
        //==================================================||Fields 
        private Dictionary<string, object> _params;
    
        //==================================================||Methods 
        private void Apply() => Apply(_params);

        public void Apply(Dictionary<string, object> pParams) {
            if (pParams == null || !text.Contains('{'))
                return;
            _params = pParams;
            text = text.ExtendFormat(_params);
        }
    
        protected override void Refresh() {
            base.Refresh(); 
            Apply();
        }  
    }
}