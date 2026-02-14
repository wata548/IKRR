using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Map;
using Extension;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
using SaveFile = Data.SaveFile;

namespace UI.Map {
    
    public partial class Map: MonoBehaviour {

        //==================================================||Constant
        private const int TOP_INTERVAL = 2;
        private const float BOSS_ROOM_SIZE = 4;
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

        [Space] 
        [Header("Node Info")] 
        [SerializeField] private List<SerializableKVP<Stage, InfoSO>> _infos;
        
        //==================================================||Fields 
        private List<List<MapNode>> _mapNodes = new();
#if UNITY_EDITOR
        private IDataLoader<StageWidth> _stageWidthFrequency = new SpreadSheetStageWidthLoader();
        private IDataLoader<Stage, int> _stageTypeFrequency = new SpreadSheetStageFrequencyLoader();
#else
        private IDataLoader<StageWidth> _stageWidthFrequency = new CSVStageWidthLoader();
        private IDataLoader<Stage, int> _stageTypeFrequency = new CSVFrequencyLoader();
#endif
        
        private List<StageWidth> _stageWidthInfos;
        private Vector2Int _curStage = -Vector2Int.one;

        private Random _random;
        
        //==================================================||Properties 
        public int Height { get; private set; }
        public static List<Vector2Int> ClearStages { get; set; } = new();
        public IReadOnlyDictionary<Stage, InfoSO> MatchInfo;
            
        //==================================================||Methods 
        public void ClearChapter() => ClearStages.Clear();
        public void ClearStage(bool pSave) {
            Height++;
            
            _mapNodes[_curStage.y][_curStage.x].SetActive(false);
            _mapNodes[_curStage.y][_curStage.x].SetMaterialVisited();
               
            _mapNodes[_curStage.y][_curStage.x].SetActiveNextEdges(true);
            foreach (var nextNode in _mapNodes[_curStage.y][_curStage.x]) {
                _mapNodes[nextNode.y][nextNode.x].SetActive(true);
            }

            if (pSave) {
                ClearStages.Add(_curStage);
                var save = SaveFile.Save();
                save.Save();
                SetActive(true);
            }
        }
        
        public void SetActive(bool pActive) =>  
            _mapPannel.SetActive(pActive);
        public void SetActiveBySwitch() =>
            _mapPannel.SetActive(!_mapPannel.activeSelf);

        private void Load() {
            if (ClearStages.Count == 0) {
                foreach (var node in _mapNodes[0]) {
                    node.SetActive(true);
                }
                return;
            }

            foreach (var stage in ClearStages) {
                SelectStage(stage, true);
                ClearStage(false);
            }
        }
        
        public void GenerateMap(int pSeed, int pChapter) {
            
            var mapRect = _map.transform as RectTransform;
            _random = new(pSeed + pChapter);
            
            foreach (var floor in _mapNodes) {
                floor.ForEach(node => Destroy(node.gameObject));
                floor.Clear();
            }
            _mapNodes.Clear();

            var interval = 1f / (_roundCount + 1.5f + TOP_INTERVAL);
            for (int i = 0; i < _roundCount; i++) {
                GenerateRound(mapRect, interval * (i + 1.5f), i != 0);
                GenerateEdges();
            }
            GenerateBossRoom(mapRect, interval);
            GenerateEdges();
                            
            var roundIdx = 0;
            foreach (var round in _mapNodes) {
                var idx = 0;
                foreach (var node in round) {
                    var type = roundIdx switch {
                        0 => Stage.Battle,
                        _ when roundIdx == _mapNodes.Count - 2 => Stage.Rest,
                        _ when roundIdx == _mapNodes.Count - 1 => Stage.Boss,
                        _ => StageTypeFrequency.Random(_random)
                    };
                    
                    node.SetUp(type, new(idx++, roundIdx));
                }
                roundIdx++;
            }
            Load();
        }

        private void GenerateBossRoom(RectTransform pMap, float pInterval) {
            var newIcon = Instantiate(_roundSymbol, _map.transform);
            _mapNodes.Add(new(){newIcon});
                        
            var rect = (newIcon.transform as RectTransform)!;
            rect.localScale *= BOSS_ROOM_SIZE;
            //set position
            rect.SetLocalPositionX(pMap, PivotLocation.Down, 0.5f); 
            rect.SetLocalPositionY(pMap, PivotLocation.Middle, pInterval * (_roundCount + 0.5f + TOP_INTERVAL / 2));
            foreach (var node in _mapNodes[^2]) {
                GenerateEdge(node, newIcon, 0);
            }
        }
        
        private void GenerateRound(RectTransform pMapRect, float pHeight, bool pApplyNoise) {

            _mapNodes.Add(new());
            
            var width = GetWidthSize() + 1;
            if (_mapNodes.Count >= 2 && _mapNodes[^2].Count == width - 1)
                width++;
            
            var interval = 1f / width;

            for (int i = 1; i < width; i++ ) {
                var newIcon = Instantiate(_roundSymbol, _map.transform);

                var rect = (newIcon.transform as RectTransform)!;
                
                //set position
                rect.SetLocalPositionX(pMapRect, PivotLocation.Down, i * interval); 
                rect.SetLocalPositionY(pMapRect, PivotLocation.Middle, pHeight);
                if(pApplyNoise)
                    rect.localPosition += RandomNoise(rect.sizeDelta);
                
                _mapNodes[^1].Add(newIcon);
            }

            _mapNodes[^1] = _mapNodes[^1]
                .OrderBy(node => node.transform.localPosition.x)
                .ToList();
        } 
        private int GetWidthSize() {
            var count = _stageWidthInfos.Sum(floor => floor.Frequency);
            var random = _random.Next() % count;

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
            var x = _random.Range(-maxSize.x, maxSize.x);
            var y = _random.Range(-maxSize.y, maxSize.y);
            return new(x, y);
        }
        private void GenerateEdges() {
            if (_mapNodes.Count <= 1)
                return;

            var prev = _mapNodes[^2];
            var cur = _mapNodes[^1];
            var isCurBig = cur.Count > prev.Count;
            var count = isCurBig ? prev.Count : cur.Count;
            var visit = new int[count];
            var idx1 = 0;
            foreach (var node in isCurBig ? cur : prev) {
                int target = 0;
                var distance = float.MaxValue;
                var idx2 = 0;
                foreach (var otherNode in isCurBig ? prev : cur) {

                    var curDistance = (node.transform.localPosition - otherNode.transform.localPosition).magnitude;
                    if (distance >= curDistance) {
                        target = idx2;
                        distance = curDistance;
                    }

                    idx2++;
                }

                if (isCurBig)
                    GenerateEdge(prev[target], node, idx1);
                else
                    GenerateEdge(node, cur[target], target);

                visit[target] = idx1;
                idx1++;
            }
            
            for (int i = 0; i < count; i++) {
                if(visit[i] != 0)
                    continue;

                if (i == 0)
                    GenerateEdge(prev[0], cur[0], 0);
                else if (i == count - 1)
                    GenerateEdge(prev[^1], cur[^1], cur.Count - 1);
                else if (isCurBig) {
                    var target = prev[i - 1][^1].x;
                    GenerateEdge(prev[i], cur[target], target);
                }
                else {
                    var target = visit[i - 1];
                    visit[i] = target;
                    GenerateEdge(prev[target], cur[i], i);
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
            MatchInfo = _infos.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            
            _stageWidthInfos = _stageWidthFrequency.Load().ToList();
            var temp = _stageTypeFrequency.Load();
            StageTypeFrequency.LoadData(temp);
        
            MapNode.Init(this);
        }
    }
}