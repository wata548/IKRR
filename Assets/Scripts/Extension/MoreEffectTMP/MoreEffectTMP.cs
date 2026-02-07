using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Extension {
    public partial class MoreEffectTMP: MonoBehaviour {
        
        //==================================================||Fields 
        private const float X_MOVE_RANGE = 0.08f;
        [SerializeField] private float _rowInterval = 0.3f;
        [SerializeField] private TMP_Text _text;
        private List<TagPoint> _tagPoints = new();
        private bool _preProcess = false;
        private float _timer = 0;
        private string _prevValue = "";
        
        private void Apply() {

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
                    for (int vertex = 0; vertex < 4; vertex++) {
                                                             
                        var vertexIdx = charInfos[idx].vertexIndex + vertex;
                        var func = _changePosFunc[tag.Type];
                        var prev = Vector3.zero;
                        if (!_preProcess)
                            prev = func(prevTime, idx, tag.Arg);
                                         
                        var ratio = func.Invoke(_timer, idx, tag.Arg) - prev;
                        var delta = ratio;
                        delta.y *= charInfo.pointSize * _rowInterval;
                        delta.x *= charInfo.pointSize * charInfo.aspectRatio * X_MOVE_RANGE;
                        vertices[vertexIdx] += delta;
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

        private void Start() {
            _text.textWrappingMode = TextWrappingModes.NoWrap;
        }
    }
    
}