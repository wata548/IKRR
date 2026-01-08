using System;
using System.Collections.Generic;
using System.Linq;
using Extension.Test;
using TMPro;
using UnityEngine;
using Random = System.Random;

namespace Test {
    
    [RequireComponent(typeof(TMP_Text))]
    public class TMP_Test: MonoBehaviour {
        private TMP_Text _text;

        [TestMethod]
        private List<int> Split() {
            var textInfo = _text.textInfo;
            var result = new List<int>();
            var idx = 0;
            foreach(var lineInfo in textInfo.lineInfo) {
                result.Add(idx += lineInfo.characterCount);
                Debug.Log(idx);
            }

            return result;
        }
        
        [TestMethod]
        private void Awake() {
            _text = GetComponent<TMP_Text>();

            var tInfo = _text.textInfo;
            var cInfos = tInfo.characterInfo;
            var mInfos = tInfo.meshInfo;

            var startVertices = mInfos[cInfos[0].materialReferenceIndex].vertices;
            var endVertices = mInfos[cInfos[^1].materialReferenceIndex].vertices;
            _text.fontSize = 1f;

            var length = endVertices[cInfos[^1].vertexIndex + 3].x - startVertices[cInfos[0].vertexIndex].x;
            //var GetSize()
        }

        [TestMethod]
        private float GetSize() {
            var size = (_text.transform as RectTransform)!.sizeDelta;
            var tInfo = _text.textInfo;
            var cInfo = tInfo.characterInfo[0];
            var face = cInfo.fontAsset.faceInfo;
            var height = face.lineHeight * _text.fontSize / face.pointSize;
            
            var result = size.x * (int)Math.Round(size.y / height);
            return result;
        }
        
        private void OnDrawGizmos() {
            
            _text = GetComponent<TMP_Text>();

            var tInfo = _text.textInfo;
            var cInfos = tInfo.characterInfo;
            var mInfos = tInfo.meshInfo;

            var length = 0f;
            var r = new Random();
            var pos = transform.position;
            
            
            var startVertices = mInfos[cInfos[0].materialReferenceIndex].vertices;
            var endVertices = mInfos[cInfos[^1].materialReferenceIndex].vertices;
            _text.fontSize = 1f;
            
            Gizmos.DrawLine(pos + endVertices[cInfos[^1].vertexIndex + 3], pos + startVertices[cInfos[0].vertexIndex]);

        }
    }
}