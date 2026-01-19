using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Lang;
using UI.Icon;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class InfoShower: MonoBehaviour {

        [SerializeField] private GameObject _board;
        [SerializeField] private Image _image;
        [SerializeField] private TMP_LangText _name;
        [SerializeField] private TMP_LangText _context;
        [SerializeField] private List<InfoShowerOption> _options;
        private List<(string, string)> _curOptions;
        private int _idx;
        
        public void Set(Info pInfo) {
            _board.SetActive(true);
            _image.sprite = pInfo.SerialNumber.GetIcon();
            _name.text = pInfo.Name;
            _curOptions = pInfo.Contexts;
            _idx = 0;
            _context.text = pInfo.Contexts[0].Item2;

            for (int i = 0; i < _options.Count; i++) {
                if (pInfo.Contexts.Count > i) {
                    _options[i].Set(pInfo.Contexts[i].Item1);
                    _options[i].SetActive(i == 0);
                    continue;
                }

                _options[i].Set("");
                _options[i].SetActive(false);
            }
        }

        public void Hide() {
            _board.SetActive(false);
        }

        private void Update() {
            if (!_board.activeSelf)
                return;

            var pos = Input.mousePosition;
            pos.z = Camera.main!.transform.position.z + transform.position.z;
            pos = Camera.main!.ScreenToWorldPoint(pos);
            _board.transform.position = pos;
            if (Input.mouseScrollDelta.y != 0) {
                _options[_idx].SetActive(false);
                _idx += Input.mouseScrollDelta.y > 0 ? -1 : 1;
                if (_idx == -1) _idx = _curOptions.Count - 1;
                if (_idx == _curOptions.Count) _idx = 0;
                _options[_idx].SetActive(true);
                _context.text = _curOptions[_idx].Item2;
            }
                
        }
    }
}