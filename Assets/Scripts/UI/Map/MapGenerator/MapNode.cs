using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Data.Map;
using DG.Tweening;
using Extension;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MapGenerator {
    public class MapNode: MonoBehaviour, IEnumerable<Vector2Int> {

        //==================================================||Constant
        private const float animationCycle = 0.75f;
        private const float animationScale = 1.2f;

        
        //==================================================||Properties
        public Vector2Int Position { get; private set; }
        public Stage Stage { get; private set; }
        public bool IsActive { get; private set; } = false;

        public Vector2Int this[Index pIdx] => new (_edges[pIdx].NextNode, Position.y + 1);
       
        //==================================================||Serialize Field 
        [SerializeField] private Image _icon;
        [SerializeField] private Button _button;
       
        //==================================================||Fields 
        private static MapGenerator _generator = null;
        private static Dictionary<Stage, Sprite> mapIcons = null;

        private Tween _animation;
        private List<(int NextNode, Image Edge)> _edges = new();
        
        //==================================================||Methods 

        public static void Init(MapGenerator pGenerator) =>
            _generator = pGenerator;

        private bool IsConnected(Vector2Int pTarget) {
            if (pTarget.y != Position.y + 1)
                return false;
            return _edges.Any(data => data.NextNode == pTarget.x);
        }

        public void SetMaterialVisited() => _icon.material = null;
        
        public void SetActive(bool pActive) {
            IsActive = pActive;
            _icon.transform.localScale = Vector3.one;
            _icon.material = pActive ? null : MaterialStore.Get("GrayScale");
            
            _animation?.Kill();
            if(pActive)
                _animation = _icon.transform.DOBreathing(animationCycle, animationScale);
        }

        public bool SelectNextStage(Vector2Int pNextStage) {
            if (!IsConnected(pNextStage))
                return false;
            
            SetActiveNextEdges(false);
            var target = _edges.First(edge => edge.NextNode == pNextStage.x).Edge;
            target.material = MaterialStore.Get(MapGenerator.MOVED_MGTERIAL);
            return true;
        }

        public void SetActiveNextEdges(bool pActive) {
            var material = pActive
                ? MapGenerator.MOVABLE_MATERIAL
                : MapGenerator.NOT_MOVABLE_MATERIAL;
            
            _edges.ForEach(edge => {
                edge.Edge.material = MaterialStore.Get(material);
            });
        }
       
        public void Add(int pIdx, Image pEdge) =>
            _edges.Add((pIdx, pEdge));
        
        public void SetUp(Stage pStage, Vector2Int pPosition) {

            FindIcons();
            
            Stage = pStage;
            _icon.sprite = mapIcons[pStage];

            Position = pPosition;
        }
        
        private static void FindIcons() =>
            mapIcons ??= Resources
                .LoadAll<Sprite>("StageIcons")
                .ToDictionary(
                    sprite => {
                        var name = Regex.Match(sprite.name, @"(.+)_\d").Groups[1].Value;
                        var stage = Enum.Parse(typeof(Stage), name);
                        return (Stage)stage;
                    },
                    sprite => sprite
                );

        public IEnumerator<Vector2Int> GetEnumerator() =>
            _edges
                .Select(edge => new Vector2Int(edge.NextNode, Position.y + 1))
                .GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public void OnClick() {
            _generator.SelectStage(Position);           
        }
        
        //==================================================||Unity
        private void OnDestroy() {
            _animation?.Kill();
            _edges.ForEach(edge => Destroy(edge.Edge));
        }
    }
}