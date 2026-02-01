using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Extension {
    public record PlaceArgs<T>(
        in Vector2 Padding,
        in int Amount,
        in Vector2Int TableSize,
        in T Prefab,
        in Action<T, int> OnGenerate
    );
    public static class ExRect2 {

        public static void Place<T>(this RectTransform pRect, List<T> pContainer, PlaceArgs<T> pArgs) where T: MonoBehaviour {

            var prefabSize = (pArgs.Prefab.transform as RectTransform)!.sizeDelta;
            
            var prefabRatio = prefabSize / pRect.sizeDelta;
            var interval =  (Vector2.one - prefabRatio) / (pArgs.TableSize- Vector2.one);
            if (pArgs.TableSize.x <= 1)
                interval.x = 0;
            if (pArgs.TableSize.y <= 1)
                interval.y = 0;
            var initPos = pArgs.Padding / 2;
            initPos.x += prefabRatio.x / 2f;
            initPos.y -= prefabRatio.y / 2f;

            if (pArgs.TableSize.x <= 1)
                initPos.x = (1f - prefabRatio.x) / 2f;
            if (pArgs.TableSize.y <= 1)
                initPos.y -= (1f - prefabRatio.y) / 2f;
            
            var pivot = new Pivot(PivotLocation.Down, PivotLocation.Up);
            
            while (pContainer.Count != pArgs.Amount) {
                if (pContainer.Count < pArgs.Amount) {
                    var pos = initPos;
                    pos.y -= (pContainer.Count / pArgs.TableSize.x) * interval.y;
                    pos.x += pContainer.Count % pArgs.TableSize.x * interval.x;
            
                    var symbol = Object.Instantiate(pArgs.Prefab, pRect);
                    pArgs.OnGenerate?.Invoke(symbol, pContainer.Count);
                    
                    var symbolRect = (symbol.transform as RectTransform)!;
                    symbolRect.SetLocalPosition(pRect, pivot, pos);
                    pContainer.Add(symbol);
                }else{
                    Object.Destroy(pContainer[^1].gameObject);
                    pContainer.RemoveAt(pContainer.Count - 1);
                }
            }

        }
    }
}