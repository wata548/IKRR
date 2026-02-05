using Data.Event;
using Symbol;
using TMPro;
using UnityEngine;

namespace UI.Event {
    public class EventButton: MonoBehaviour {

        [SerializeField] private UnityEngine.UI.Button _button;
        [SerializeField] private TMP_Text _context;
        private string _funcContext;
        public static string LastClickButton { get; private set; }
        
        public void SetData(Button pData) {
            _context.text = pData.Option;
            _funcContext = pData.FuncContext;
        }
        
        private void Invoke() {
            LastClickButton = _context.text;
            SymbolExecutor.Invoke(_funcContext);
        }

        private void Awake() {
            _button.onClick.AddListener(Invoke);
        }
    }
}