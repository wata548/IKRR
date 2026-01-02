using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Data.Map;
using DG.Tweening;
using Extension;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace MapGenerator {
    public class MapNode: MonoBehaviour, IEnumerable<Vector2Int> {

        //==================================================||Constant
        private const float animationCycle = 0.75f;
        private const float animationScale = 1.2f;
       
        //==================================================||Properties
        public Vector2Int Position { get; private set; }
        public Stage Stage { get; private set; }

        public Vector2Int this[Index pIdx] => new (_edges[pIdx].NextNode, Position.y + 1);
       
        //==================================================||Serialize Field 
        [SerializeField] private Image _backGround;
       
        //==================================================||Fields 
        private static MapGenerator _generator = null;
        private static Dictionary<Stage, Sprite> mapIcons = null;
        private List<(int NextNode, GameObject Edge)> _edges = new();
        private Tween _animation = null;
        
        //==================================================||Methods 

        public static void Init(MapGenerator pGenerator) =>
            _generator = pGenerator;

        public void Clear() {
            _generator.ClearStage(Position);
        }
       
        public void SetActive(bool pActive = true) {
            transform.localScale = Vector3.one;
            _animation?.Kill();
            if(pActive)
                _animation = transform.DOBreathing(animationCycle, animationScale);
        }

        public void ActiveNextEdges() {
            _edges.ForEach(edge => {
                var image = edge.Edge.GetComponent<Image>();
                image.material = MaterialStore.Get(MapGenerator.MOVABLE_MATERIAL);
            });
        }
       
        public void Add(int pIdx, GameObject pEdge) =>
            _edges.Add((pIdx, pEdge));
        
        public void SetUp(Stage pStage, Vector2Int pPosition) {

            FindIcons();
            
            Stage = pStage;
            _backGround.sprite = mapIcons[pStage];

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
        
        //==================================================||Unity

        private void OnDestroy() {
            _animation?.Kill();
            _edges.ForEach(edge => Destroy(edge.Edge));
        }
    }
}