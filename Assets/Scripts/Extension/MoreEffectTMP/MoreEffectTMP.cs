using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Extension {
    [RequireComponent(typeof(TMP_Text))]
    public partial class MoreEffectTMP: MonoBehaviour {

        public TMP_Text TMP => _text;
        
        //==================================================||Fields 
        private const float X_MOVE_RANGE = 0.08f;
        [SerializeField] private float _rowInterval = 0.3f;
        private TMP_Text _text;
        private List<TagPoint> _tagPoints = new();
        private List<FixPoint> _fixPoints = new();
        private bool _preProcess = false;
        private float _timer = 0;
        private string _prevValue = "";

        public IEnumerator Typing(string context, float interval, Action callback = null,
            Func<bool> breakCondition = null) {

            var cur = _text.text;
            var originCnt = cur.Length;
            
            //GetFixPoints
            _text.text += context;
            Update();
            var fixPoints = _fixPoints.ToList();
            
            var idx = -1;
            var fixPointIdx = 0;
            foreach (var charInfo in _text.textInfo.characterInfo.Skip(originCnt)) {
                if (breakCondition?.Invoke() ?? false) {
                    _text.text = context;
                    callback?.Invoke();
                    yield break;
                }
                
                idx++;
                cur += charInfo.character;
                var remainFixPoint = fixPoints.Count > fixPointIdx;
                if (remainFixPoint && idx >= fixPoints[fixPointIdx].Start && idx < fixPoints[fixPointIdx].End) {
                    continue;
                }
                if(!charInfo.isVisible)
                    continue;

                _text.text = cur;
                Update();
                yield return new WaitForSeconds(interval);
            }

            callback?.Invoke();
        }

        private void Apply() {

            if (_tagPoints is { Count: < 1 }) 
                return;
            
            var prevTime = _timer;
            _timer += Time.deltaTime / Time.timeScale;
            
            var pTextInfo = _text.textInfo;
            var idx = _tagPoints[0].Start;

            var charInfos = pTextInfo.characterInfo;
            foreach (var tag in _tagPoints) {
                for (; idx <= tag.End; idx++) {
                    var charInfo = charInfos[idx];

                    if (!charInfo.isVisible)
                        continue;
                        
                    var vertices = pTextInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
                    var func = _changePosFunc[tag.Type];
                    var prevPos = Vector3.zero;
                    var prevRotation = Vector3.zero;
                    if (!_preProcess)
                        (prevPos, prevRotation) = func(prevTime, idx, tag.Arg);
                    var (pos, rotation) = func.Invoke(_timer, idx, tag.Arg);
                    pos -= prevPos;
                    rotation -= prevRotation;
                    pos.y *= charInfo.pointSize * _rowInterval;
                    pos.x *= charInfo.pointSize * charInfo.aspectRatio * X_MOVE_RANGE;
                    
                    for (int vertex = 0; vertex < 4; vertex++) {
                        var vertexIdx = charInfos[idx].vertexIndex + vertex;
                        vertices[vertexIdx] += pos;
                    }   
                }
            }
            
            _preProcess = false;
            TMPUpdate();
        }

       //==================================================||Unity 
        private void Update() {
            if (_text.text != _prevValue) {
                _prevValue = _text.text;
                _text.ForceMeshUpdate();
                Setting();
            }
            
            Apply();
        }

        private void OnEnable() {
            _prevValue = "";
        }

        private void Awake() {
            _text = GetComponent<TMP_Text>();
            _text.textWrappingMode = TextWrappingModes.NoWrap;
        }
    }
    
}