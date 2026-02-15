using System;
using System.IO;
using Data.Event;
using Extension.StaticUpdate;
using LanguageEmbed;
using Symbol;
using TMPro;
using UnityEngine;
using File = System.IO.File;

namespace UI.Event {
    public class EventButton: MonoBehaviour {

        [SerializeField] private UnityEngine.UI.Button _button;
        [SerializeField] private TMP_Text _context;
        private string _funcContext;
        public static string LastClickButton { get; private set; }
        private static ILanguageEmbed _executor = new LuaEmbed();
        
        public void SetData(Button pData) {
            _context.text = pData.Option;
            _funcContext = pData.FuncContext;
            if (string.IsNullOrWhiteSpace(pData.Condition))
                _button.interactable = true;
            else {
                _button.interactable = _executor.Invoke<bool>(GetFunc(pData.Condition), "Invoke");
            }
        }
        
        private void Invoke() {
            LastClickButton = _context.text;
            _executor.Invoke(GetFunc(_funcContext), "Invoke");
        }

        private void Awake() {
            _button.onClick.AddListener(Invoke);
        }

        [StaticUpdate]
        private static void EmbedLanguageUpdate() {
            _executor.Update();
        }

        private string _eventFuncFormat = null;
        
        private string GetFunc(string pContext) {
            _eventFuncFormat ??= File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "LuaEventFuncFormat.txt"));
            return string.Format(_eventFuncFormat, "Invoke", pContext);
        } 
    }
}