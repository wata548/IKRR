using System.Collections.Generic;
using Data;
using Extension.Scene;
using Lang;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Tutorial {
    public class Tutorial: MonoBehaviour {
        [SerializeField] private Button _next;
        [SerializeField] private TMP_Text _nextText;
        [SerializeField] private Button _prev;
        [SerializeField] private GameObject _pannel;
        [SerializeField] private TMP_LangText _context;
        [SerializeField] private List<string> _texts;
        private int _idx = 0;

        public void TurnOn() {
            _pannel.SetActive(true);
            _context.text = _texts[0];
        }

        private void Prev() {
            if (_idx == 0)
                return;
            _context.text = _texts[--_idx];
            _nextText.text = "Next";
        }
        
        private void Next() {
            if (_idx == _texts.Count - 1) {
                SaveSystem.GameStart();
                SceneManager.LoadScene(Scene.Main);
                return;
            }
            
            _context.text = _texts[++_idx];
            _nextText.text = _idx == _texts.Count - 1 ? "Start" : "Next";
        }

        private void Awake() {
            _next.onClick.AddListener(Next);
            _prev.onClick.AddListener(Prev);
        }
    }
}