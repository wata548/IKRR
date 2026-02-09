using Data;
using Data.Map;
using UnityEngine;

namespace UI.Map {
    public partial class Map: MonoBehaviour {
        public void SelectStage(Vector2Int pNextStage, bool pLoad = false) {
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
            var targetNode = _mapNodes[_curStage.y][_curStage.x];
            targetNode.SetActive(true);
            if(!pLoad)
                OnSelect(targetNode.Type);
        }
        
        private void OnSelect(Stage pType) {
            switch (pType) {
                case Stage.Battle:
                    GameManager.SetEnemy();
                    break;
                case Stage.Event:
                    GameManager.SetEvent();
                    break;
                default:
                    GameManager.SetEnemy();
                    break;
            }
        }
    }
}