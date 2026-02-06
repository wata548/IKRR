using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Extension.Test;
using TMPro;
using UnityEngine;

namespace Extension {
    public class MoreEffectTMP: MonoBehaviour {
        
        //==================================================||Fields 
        [SerializeField] private float _rowInterval = 0.3f;
        [SerializeField] private TMP_Text _text;
        private List<TagPoint> _tagPoints = new();
        const char TAG_OPEN = '<';
        const char TAG_CLOSE = '>';
        const char TAG_END_IDENTIFIER = '/';
        const string PATTERN = @"(?<Tag>.+)=(?<Arg>.*)";

        private void ChangeValue(string pContext) {
            _text.text = pContext;
            
            _text.ForceMeshUpdate();
            Setting();
        }
        
        [TestMethod]
        private void Setting() {
            var textInfo = _text.textInfo;

            var tagStartPosition = new Stack<(int Idx, Vector3 Pos)>();
            var tagStack = new Stack<TagPoint>();
            var curTag = "";
            var idx = -1;
            var fixPoints = new Queue<FixPoint>();
            var tempTagPoint = new List<TagPoint>();
            
            var prev = new TMP_CharacterInfo();
            var canTag = true;
            foreach (var charInfo in textInfo.characterInfo) {
                idx++;
                
                if(!charInfo.isVisible)
                    continue;

                if (prev.character != TAG_CLOSE && tagStartPosition.Count > 0) {
                    if (charInfo.character != TAG_CLOSE) {
                        curTag += charInfo.character;
                    }
                    else {
                        try {
                            if (curTag.Length == 0)
                                //it will catch
                                throw new ArgumentException("It isn't tag");

                            if (curTag[0] == TAG_END_IDENTIFIER) {
                                var tag = Parse<TMP_EffectType>(curTag[1..]);
                                if (tag == tagStack.Peek().Type) {
                                    var data = tagStack.Pop();
                                    data.End = tagStartPosition.Peek().Idx - 1;
                                    tempTagPoint.Add(data);       
                                }
                                //else throw
                            }
                            else {

                                var match = Regex.Match(curTag, PATTERN);
                                var tag = TMP_EffectType.Error;
                                var arg = "";
                                if (match.Success) {
                                    tag = Parse<TMP_EffectType>(match.Groups["Tag"].Value);
                                    arg = match.Groups["Arg"].Value;
                                }
                                else
                                    tag = Parse<TMP_EffectType>(curTag);

                                tagStack.Push(new(idx + 1, 0, tag, arg));
                            }
                        }
                        catch (ArgumentException) {
                            //parsing error. 
                            //this angle bracket is just text, not tag 
                            canTag = false;
                        }
                        finally {
                            curTag = "";
                        }
                        
                    }
                }
                
                var vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
                
                if (tagStartPosition.Count > 0 && prev.character == TAG_CLOSE) {
                    if (canTag) {
                        var (start, pos) = tagStartPosition.Pop();
                        fixPoints.Enqueue(new(start, idx, pos));
                    }
                    else {
                        tagStartPosition.Pop();
                        canTag = true;
                    }
                }
                if (charInfo.character == TAG_OPEN) {
                    tagStartPosition.Push((idx,vertices[charInfo.vertexIndex]));
                }
                
                prev = charInfo;
                
            }

            while (tagStack.Count > 0) {
                var data = tagStack.Pop();
                data.End = int.MaxValue;
                tempTagPoint.Add(data);
            }
            
            _tagPoints = tempTagPoint.OrderBy(point => point.Start).ToList();
            RemoveTagAndDivideRow(fixPoints);
        }

        private void RemoveTagAndDivideRow(Queue<FixPoint> pFixPoints) {
            const float DEFAULT_UNITY_ROW_INTERVAL = 0.45f;   
            var pTextInfo = _text.textInfo;
            var idx = -1;
            var fix = Vector3.zero;
            var width = (transform as RectTransform).sizeDelta.x;
            var startPos = 0f;
            var newLineCnt = 0;
            var needLineCnt = 0;
            var line = 0;
            var pad = 0f;
            foreach (var charInfo in pTextInfo.characterInfo) {
                idx++;

                if (idx == 0) {
                    startPos = pTextInfo.meshInfo[charInfo.materialReferenceIndex].vertices[charInfo.vertexIndex].x;
                }

                if (!charInfo.isVisible) {
                    if (charInfo.character == '\n') {
                        needLineCnt = line - needLineCnt;
                        newLineCnt++;
                        fix.x = 0;
                    }
                    continue;
                }
            
                var vertices = pTextInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            
                var remainSingleTag = pFixPoints.Count > 0;
                var peek = new FixPoint(0, 0, Vector3.zero);
                if (remainSingleTag) {
                    peek = pFixPoints.Peek();
                    if (idx >= peek.End) {
                        pFixPoints.Dequeue();
                        fix.x += peek.Pos.x - vertices[charInfo.vertexIndex].x;
                    }    
                }

                var lineFix = Vector3.zero;
                for (int vertex = 0; vertex < 4; vertex++) {
                                            
                    var vertexIdx = charInfo.vertexIndex + vertex;
                            
                    if (remainSingleTag && peek.Start <= idx && idx < peek.End) {
                        vertices[vertexIdx] = peek.Pos;
                    }
                    else {
                        if (vertex == 0) {
                            var prevLine = line;
                            var temp = vertices[vertexIdx] + fix;
                            var dist = Mathf.Max(0, temp.x - startPos);
                            line = Mathf.FloorToInt(dist / width) + newLineCnt + needLineCnt;
                            var lineFixX = dist % width - dist;
                            if (line != prevLine) {
                                //Set to equal start point
                                pad = startPos - lineFixX - temp.x;
                            }

                            lineFix.x = lineFixX + pad; 
                            lineFix.y = (newLineCnt * (1 + DEFAULT_UNITY_ROW_INTERVAL) - line * (1 + _rowInterval)) * charInfo.pointSize;
                            
                            vertices[vertexIdx] = temp + lineFix;
                        }
                        else
                            vertices[vertexIdx] += fix + lineFix;
                    }
                    
                }
            }
            TMPUpdate();   
        }

        private void Apply() {
                        
            var pTextInfo = _text.textInfo;
            var idx = -1;
            var tagIdx = 0;
            foreach (var charInfo in pTextInfo.characterInfo) {
                idx++;
                            
                if(!charInfo.isVisible)
                    continue;
            
                var vertices = pTextInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            
                var remainTag = _tagPoints.Count > tagIdx;
                while (remainTag && idx > _tagPoints[tagIdx].End) {
                    tagIdx++;
                    remainTag = _tagPoints.Count > tagIdx;
                }
                            
                for (int vertex = 0; vertex < 4; vertex++) {
                                            
                    var vertexIdx = charInfo.vertexIndex + vertex;
                    if (remainTag && _tagPoints[tagIdx].Start < idx) {
                        vertices[vertexIdx] += Vector3.zero;//TODO: apply value
                    }
                }
                TMPUpdate();
            }
        }

        private void TMPUpdate() {
            var textInfo = _text.textInfo;
            foreach (var mesh in textInfo.meshInfo) {
                mesh.mesh.vertices = mesh.vertices;
                mesh.mesh.RecalculateBounds();
            }
            _text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        }
        
        private T Parse<T>(string pContext) =>
            (T)Enum.Parse(typeof(T), pContext);

       //==================================================||Unity 
        private void Update() {
            Apply();
        }

        private void Start() {
            _text.textWrappingMode = TextWrappingModes.NoWrap;
            ChangeValue(_text.text);
        }

        //==================================================||SubType 
        private class FixPoint {
            public int Start;
            public int End;
            public Vector3 Pos;

            public FixPoint(int pStart, int pEnd, Vector3 pPos) =>
                (Start, End, Pos) = (pStart, pEnd, pPos);
        }

        private class TagPoint {
            public int Start;
            public int End;
            public TMP_EffectType Type;
            public string Arg;

            public TagPoint(int pStart, int pEnd, TMP_EffectType pType, string pArg) =>
                (Start, End, Type, Arg) = (pStart, pEnd, pType, pArg);
        }
        private enum TMP_EffectType {
            Error = 0,
            Flow,
            Shake
        }
    }
    
}