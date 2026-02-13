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

       //==================================================||Fields 
        [SerializeField] private Image _board;
        [SerializeField] private TMP_LangText _name;
        [SerializeField] private TMP_LangFormatExtendedText _context;
        [SerializeField] private List<InfoShowerOption> _options;
        private List<InfoDetail> _curOptions;
        private int _idx;
        private Info _info;

        private const KeyCode ACTIVE_KEY = KeyCode.Mouse1;
        //==================================================||Properties
        public bool IsActive => _board.gameObject.activeSelf;
        
        //==================================================||Methods 
        public void SetInfo(Info pInfo) {
            _info = pInfo;
            
            if(Input.GetKey(ACTIVE_KEY))
                SetData();
        }
        
        private void SetData() {
            const string PATH = "InfoShower/InfoShower_{0}";
            
            _board.gameObject.SetActive(true);
            var rarity = _info.Rarity == Rarity.Evolution 
                ? Rarity.Etc 
                : _info.Rarity;
            _board.sprite = Resources.Load<Sprite>(string.Format(PATH, rarity));
            _name.text = _info.Name;
            _curOptions = _info.Contexts;
            _idx = 0;
            _context.text = _curOptions[0].Context;
            _context.Apply(_info.Params);

            for (int i = 0; i < _options.Count; i++) {
                if (_info.Contexts.Count > i) {
                    _options[i].Set(_info.Contexts[i].Category);
                    _options[i].SetActive(i == 0);
                    continue;
                }

                _options[i].Set("");
                _options[i].SetActive(false);
            }
        }

        public void Hide() {
            _info = null;
            _board.gameObject.SetActive(false);
        }

        private void PositionUpdate() {
            var cam = Camera.main!;
            var pos = Input.mousePosition;
            pos.z = cam.transform.position.z + transform.position.z;
            pos = cam.ScreenToWorldPoint(pos);
            var rect = (_board.transform as RectTransform)!;
            var size = new Vector3(rect.rect.width * rect.lossyScale.x, rect.rect.height * rect.lossyScale.y);
            
            //90 is default canvas z pos
            var dist = Math.Abs(90 - cam.transform.position.z);
            var cameraHeight = dist * Mathf.Tan(cam.fieldOfView / 2 * Mathf.Rad2Deg);
            var limit = new Vector3(cameraHeight * cam.aspect, cameraHeight);
            
            if (Mathf.Floor(pos.x + size.x) > limit.x)
                pos.x -= size.x;
            if (Mathf.Floor(pos.y + size.y) > limit.y)
                pos.y -= size.y;
                        
            _board.transform.position = pos;
        }

        private void CategoryChange() {
            _options[_idx].SetActive(false);
            _idx += Input.mouseScrollDelta.y > 0 ? -1 : 1;
            if (_idx == -1) _idx = _curOptions.Count - 1;
            if (_idx == _curOptions.Count) _idx = 0;
            _options[_idx].SetActive(true);
            _context.text = _curOptions[_idx].Context;
            _context.Apply(_info.Params);
        }
        
       //==================================================||Unity 
        private void Update() {
            if(_info != null && Input.GetKeyDown(ACTIVE_KEY))
                SetData();
            
            if (!_board.gameObject.activeSelf)
                return;
            
            if(Input.GetKeyUp(ACTIVE_KEY))
                _board.gameObject.SetActive(false);

            PositionUpdate();
            if (Input.mouseScrollDelta.y != 0) {
                CategoryChange();
            }
                
        }
    }
}