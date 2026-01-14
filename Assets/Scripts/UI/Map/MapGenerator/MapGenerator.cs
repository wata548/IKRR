using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Map;
using Extension;
using Extension.Test;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MapGenerator {
    
    public class MapGenerator: MonoBehaviour {

       //==================================================||Constant
       private const int topIntval = 2;
       public const string NOT_MOVABLE_MATERIAL = "NotMovableEdge";
       public const string MOVABLE_MATERIAL = "MovableEdge";
       public const string MOVED_MGTERIAL = "MovedEdge";
        
        //==================================================||SerializeFields 
        [Space]
        [Header("Target")]
        [SerializeField] private GameObject _map;
        [SerializeField] private GameObject _mapPannel;
        
        [Space]
        [Header("Position")]
        [SerializeField] private Vector2 _randomNoise = new(0.8f, 0.8f);
        
        [Space]
        [Header("Round Info")]
        [SerializeField] private int _roundCount;
        [SerializeField] private MapNode _roundSymbol;

        [Space] 
        [Header("Prefabs")] 
        [SerializeField] private Image _edge;
        
        //==================================================||Fields 
        private List<List<MapNode>> _mapNodes = new();
        private IDataLoader<StageWidth> _stageWidthFrequency = new SpreadSheetStageWidthLoader();
        private IDataLoader<Stage, int> _stageTypeFrequency = new SpreadSheetStageFrequencyLoader();
        private List<StageWidth> _stageWidthInfos;
        private Vector2Int _curStage = -Vector2Int.one;
        
       //==================================================||Properties 
       public int Height { get; private set; }
        
        //==================================================||Methods 
        public void SelectStage(Vector2Int pNextStage) {
            if (_curStage.y != -1) {
                var cur = _mapNodes[_curStage.y][_curStage.x];
                if (cur.IsActive || !cur.SelectNextStage(pNextStage))
                    return;
               foreach (var nextPos in cur) {
                   _mapNodes[nextPos.y][nextPos.x].SetActive(false);
               } 
            }
            else {
                if (pNextStage.y != 0)
                    return;
                foreach (var node in _mapNodes[0]) {
                    node.SetActive(false);
                }
            }
            
            _curStage = pNextStage;
            _mapNodes[_curStage.y][_curStage.x].SetActive(true);
            OnSelect();

            void OnSelect() {
                GameManager.SetEnemy();
            }
        }
        public void ClearStage() {
            Height++;
            
            _mapNodes[_curStage.y][_curStage.x].SetActive(false);
            _mapNodes[_curStage.y][_curStage.x].SetMaterialVisited();
               
            _mapNodes[_curStage.y][_curStage.x].SetActiveNextEdges(true);
            foreach (var nextNode in _mapNodes[_curStage.y][_curStage.x]) {
                _mapNodes[nextNode.y][nextNode.x].SetActive(true);
            }
        }
        
        public void SetActive(bool pActive) =>  
            _mapPannel.SetActive(pActive);
        public void SetActiveBySwitch() =>
            _mapPannel.SetActive(!_mapPannel.activeSelf);
        
        private void GenerateMap() {
            
            foreach (var floor in _mapNodes) {
                floor.ForEach(node => Destroy(node.gameObject));
                floor.Clear();
            }
            _mapNodes.Clear();

            var interval = 1f / (_roundCount + 1 + topIntval);
            for (int i = 0; i < _roundCount; i++) {
                GenerateRound(interval * (i + 1));
                GenerateEdges();
            }
        }
        private void GenerateRound(float pHeight) {

            _mapNodes.Add(new());
            
            var width = GetWidthSize() + 1;
            if (_mapNodes.Count >= 2 && _mapNodes[^2].Count == width - 1)
                width++;
            
            var interval = 1f / width;

            var mapRect = _map.transform as RectTransform;
            for (int i = 1; i < width; i++ ) {
                var newIcon = Instantiate(_roundSymbol, _map.transform);

                var rect = (newIcon.transform as RectTransform)!;
                
                //set position
                rect.SetLocalPositionX(mapRect, PivotLocation.Down, i * interval); 
                rect.SetLocalPositionY(mapRect, PivotLocation.Middle, pHeight);
                rect.localPosition += RandomNoise(rect.sizeDelta);
                
                _mapNodes[^1].Add(newIcon);
            }

            _mapNodes[^1] = _mapNodes[^1]
                .OrderBy(node => node.transform.localPosition.x)
                .ToList();

            var idx = 0;
            foreach (var node in _mapNodes[^1]) {
                var type = StageTypeFrequency.Random();
                node.SetUp(type, new(idx++, _mapNodes.Count - 1));
            }
        } 
        private int GetWidthSize() {
            var count = _stageWidthInfos.Sum(floor => floor.Frequency);
            var random = Random.Range(0, count);

            var idx = 0;
            foreach (var floor in _stageWidthInfos) {
                idx += floor.Frequency;
                if (idx >= random)
                    return floor.Width;
            }

            throw new ArgumentOutOfRangeException();
        }
        private Vector3 RandomNoise(Vector2 pSize) {
                    
            var maxSize = pSize * _randomNoise;
            var x = Random.Range(-maxSize.x, maxSize.x);
            var y = Random.Range(-maxSize.y, maxSize.y);
            return new(x, y);
        }
        private void GenerateEdges() {
            if (_mapNodes.Count <= 1)
                return;

            var isAfter = _mapNodes[^1].Count > _mapNodes[^2].Count;
            var count = isAfter ? _mapNodes[^2].Count : _mapNodes[^1].Count;
            var visit = new int[count];
            var idx1 = 0;
            foreach (var node in isAfter ? _mapNodes[^1] : _mapNodes[^2]) {
                int target = 0;
                var distance = float.MaxValue;
                var idx2 = 0;
                foreach (var otherNode in isAfter ? _mapNodes[^2] : _mapNodes[^1]) {

                    var curDistance = (node.transform.localPosition - otherNode.transform.localPosition).magnitude;
                    if (distance >= curDistance) {
                        target = idx2;
                        distance = curDistance;
                    }

                    idx2++;
                }

                if (isAfter) {
                    GenerateEdge(_mapNodes[^2][target], node, idx1);
                }
                else {
                    GenerateEdge(node, _mapNodes[^1][target], target);
                }

                visit[target] = idx1;
                
                idx1++;
            }
            
            for (int i = 0; i < count; i++) {
                if(visit[i] != 0)
                    continue;

                if (i == 0) {
                    GenerateEdge(_mapNodes[^2][0], _mapNodes[^1][0], 0);
                    continue;
                }

                if (i == count - 1) {
                    GenerateEdge(_mapNodes[^2][^1], _mapNodes[^1][^1], _mapNodes[^1].Count - 1);
                    continue;
                }

                if (isAfter) {
                    var target = _mapNodes[^2][i - 1][^1].x;
                    GenerateEdge(_mapNodes[^2][i], _mapNodes[^1][target], target);
                }
                else {
                    var target = visit[i - 1];
                    visit[i] = target;
                    GenerateEdge(_mapNodes[^2][target], _mapNodes[^1][i], i);
                }
            }
        }
        private void GenerateEdge(MapNode pStart, MapNode pEnd, int pEndIdx) {
            var edge = Instantiate(_edge, _map.transform);

            var delta = pEnd.transform.localPosition - pStart.transform.localPosition;
            edge.transform.localPosition = pStart.transform.localPosition;
            pStart.Add(pEndIdx, edge);
                 
            //Edge setting
            var size = edge.rectTransform.sizeDelta;
            size.y = delta.magnitude;
            
            edge.rectTransform.sizeDelta = size;
            var direction = Mathf.Atan2(delta.y, delta.x);
            edge.transform.rotation = Quaternion.Euler(0, 0, direction * Mathf.Rad2Deg - 90);
            edge.rectTransform.SetSiblingIndex(0);
        }
        
        //==================================================||Unity

        private void Awake() {
            _stageWidthInfos = _stageWidthFrequency.Load().ToList();
            var temp = _stageTypeFrequency.Load();
            StageTypeFrequency.LoadData(temp);
        
            MapNode.Init(this);
            GenerateMap();

            foreach (var node in _mapNodes[0]) {
                node.SetActive(true);
            }
        }
        
        #region Test
        [TestMethod(pRuntimeOnly: true)]
        private void Generate() {
            GenerateMap();
        }
        #endregion
    }
}