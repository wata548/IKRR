using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Extension {
    public struct PlaceArgs<T>{
        public Vector2 Padding;
        public int Amount;
        public Vector2Int TableSize;
        public T Prefab;
        public Action<T, int> OnGenerate;
        public Action<T, int> Foreach;

        public PlaceArgs(Vector2 pPadding, int pAmount, Vector2Int pTableSize, T pPrefab, Action<T, int> pOnGenerate = null, Action<T, int> pForeach = null) {
            Padding = pPadding;
            Amount = pAmount;
            TableSize = pTableSize;
            Prefab = pPrefab;
            OnGenerate = pOnGenerate;
            Foreach = pForeach;
        }
    };
    public static class ExRect2 {

        public static void Place<T>(this RectTransform pRect, List<T> pContainer, PlaceArgs<T> pArgs) where T: MonoBehaviour {

            while (pContainer.Count != pArgs.Amount) {
                if (pContainer.Count < pArgs.Amount) {
                    var newElement = Object.Instantiate(pArgs.Prefab, pRect);
                    pArgs.OnGenerate?.Invoke(newElement, pContainer.Count);
                    pContainer.Add(newElement);
                }else{
                    Object.Destroy(pContainer[^1].gameObject);
                    pContainer.RemoveAt(pContainer.Count - 1);
                }
            }
            
            var prefabSize = (pArgs.Prefab.transform as RectTransform)!.sizeDelta;
            
            var prefabRatio = prefabSize / pRect.sizeDelta;
            var interval =  (Vector2.one - prefabRatio - pArgs.Padding) / (pArgs.TableSize- Vector2.one);
            if (pArgs.TableSize.x <= 1)
                interval.x = 0;
            if (pArgs.TableSize.y <= 1)
                interval.y = 0;
            var initPos = pArgs.Padding / 2;
            initPos += prefabRatio / 2f;
            initPos.y *= -1;

            if (pArgs.TableSize.x <= 1)
                initPos.x += (1f - prefabRatio.x) / 2f;
            if (pArgs.TableSize.y <= 1)
                initPos.y -= (1f - prefabRatio.y) / 2f;
            
            var pivot = new Pivot(PivotLocation.Down, PivotLocation.Up);

            var idx = 0;
            foreach (var element in pContainer) {
                var pos = initPos;
                pos.y -= (idx / pArgs.TableSize.x) * interval.y;
                pos.x += idx % pArgs.TableSize.x * interval.x;
                            
                pArgs.Foreach?.Invoke(element, idx);
                                    
                var elementRect = (element.transform as RectTransform)!;
                elementRect.SetLocalPosition(pRect, pivot, pos);
                idx++;
            }
        }
    }
}